using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using FarmIT_Api.Models;

namespace FarmIT_Api.database_accesslayer
{
    public interface ISettingDB
    {
        DataTable Get_Roles();
        DataSet GetModulePagePermissions(int Role_Id);
        string UpdateRolePermission(IEnumerable<RolePermissionsStatus> rollPermission);
        string UpdateUserLocation(IEnumerable<UserLocations> userLocations);
        string UpdateSystemUser(int User_Id,string Location,int sub_location_id);
        DataTable GetPagePermissionsForUser(int Role_id,int User_id);
        DataSet GetUser_Details(int User_Id,int Company_Id);
        string UpdateUserRolePermission(IEnumerable<UserPermissionsStatus> ups, string Created_By);
    }
    public class SettingDB:ISettingDB
    {
        public readonly string constr;

        private SqlConnection con;
        public SettingDB(string connectionString)
        {
            constr = AesOperation.DecryptString(connectionString);
        }
        private void connection()
        {
            con = new SqlConnection(constr);
        }
        public DataTable Get_Roles()
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_ROLES", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return dt;
        }
        public DataSet GetModulePagePermissions(int Role_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_GetPagePermissions", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ROLE_ID",Role_Id);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public string UpdateRolePermission(IEnumerable<RolePermissionsStatus> rollPermission)
        {
            string res = "";
            try
            {
                DataTable dtups = new DataTable();
                dtups.Columns.Add("ROLE_ID");
                dtups.Columns.Add("ROLE_PERMISSION_ID");
                dtups.Columns.Add("CHECKED_STATUS");

                foreach (RolePermissionsStatus ps in rollPermission)
                {
                    DataRow dr = dtups.NewRow();
                    dr["ROLE_ID"] = ps.ROLE_ID;
                    dr["ROLE_PERMISSION_ID"] = ps.ROLE_PERMISSION_ID;
                    dr["CHECKED_STATUS"] = ps.CHECKED_STATUS;
                    dtups.Rows.Add(dr);
                }

                connection();                
                using (SqlCommand cmd = new SqlCommand("USP_UpdateRolePermission", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ROLE_PERMISSION_STATUS",dtups);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@Message";
                    outData.SqlDbType = SqlDbType.VarChar;
                    outData.Size = 100;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    res = cmd.ExecuteNonQuery().ToString();
                    res = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        public string UpdateUserLocation(IEnumerable<UserLocations> userLocations)
        {
            string res = "";
            try
            {
                DataTable dtups = new DataTable();
                dtups.Columns.Add("USER_ID");
                dtups.Columns.Add("LOCATION");
                dtups.Columns.Add("SUB_LOCATION_ID");

                foreach (UserLocations ps in userLocations)
                {
                    DataRow dr = dtups.NewRow();
                    dr["USER_ID"] = ps.user_id;
                    dr["LOCATION"] = ps.location;
                    dr["SUB_LOCATION_ID"] = ps.sub_location_id;
                    dtups.Rows.Add(dr);
                }

                connection();
                using (SqlCommand cmd = new SqlCommand("USP_UUSER_LOCATION", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tbl_user_location", dtups);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@Message";
                    outData.SqlDbType = SqlDbType.VarChar;
                    outData.Size = 100;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    res = cmd.ExecuteNonQuery().ToString();
                    res = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        public DataTable GetPagePermissionsForUser(int Role_Id,int User_Id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_GetPagePermissionsForUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ROLE_ID", Role_Id);
                    cmd.Parameters.AddWithValue("@USER_ID", User_Id);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return dt;
        }
        public DataSet GetUser_Details(int User_Id, int Company_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_USER_DETAILS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@USER_ID", User_Id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public string UpdateSystemUser(int User_Id,string Location,int sub_location_id)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_USYSTEM_USER", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@USER_ID", User_Id);
                    cmd.Parameters.AddWithValue("@LOCATION", Location);
                    cmd.Parameters.AddWithValue("@SUB_LOCATION_ID", sub_location_id);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
                    outData.SqlDbType = SqlDbType.VarChar;
                    outData.Size = 50;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    res = cmd.ExecuteNonQuery().ToString();
                    res = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        public string UpdateUserRolePermission(IEnumerable<UserPermissionsStatus> ups, string Created_By)
        {
            string res = "";
            try
            {
                DataTable dtups = new DataTable();
                dtups.Columns.Add("USER_ID");
                dtups.Columns.Add("ROLE_PERMISSION_ID");
                dtups.Columns.Add("ROLE_ID");
                dtups.Columns.Add("CHECKED_STATUS");

                foreach (UserPermissionsStatus ps in ups)
                {
                    DataRow dr = dtups.NewRow();
                    dr["USER_ID"] = ps.USER_ID;
                    dr["ROLE_PERMISSION_ID"] = ps.ROLE_PERMISSION_ID;
                    dr["ROLE_ID"] = ps.ROLE_ID;
                    dr["CHECKED_STATUS"] = ps.CHECKED_STATUS;
                    dtups.Rows.Add(dr);
                }

                connection();
                using (SqlCommand cmd = new SqlCommand("USP_SaveUpdateUserPermission", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@USER_PERMISSION_STATUS",dtups);
                    cmd.Parameters.AddWithValue("@CREATED_BY", Created_By);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    cmd.CommandTimeout = 500;
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

    }
}
