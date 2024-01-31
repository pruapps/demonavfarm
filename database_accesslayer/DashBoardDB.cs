using FarmIT_Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FarmIT_Api.database_accesslayer
{
    public interface IDashBoardDB
    {
        DataSet MONTH_WISE_REGISTRATION();
        DataSet MONTH_WISE_CANCELLATION_REFUND();
        DataSet ACTIVE_INACTIVE_USERS();
        DataSet TRAIL_PAID_USERS();
        DataSet ACTIVE_INACTIVE_USERS_DETAILS();
        DataSet TRAIL_PAID_USERS_DETAILS();
        DataSet USERS_DETAILS(int user_id);
        string Insert_FeedBack(int User_ID, string Remarks);
    }
    public class DashBoardDB : IDashBoardDB
    {
        public readonly string constr;

        private SqlConnection con;

        public DashBoardDB(string connectionString)
        {
            constr = AesOperation.DecryptString(connectionString);
        }
        private void connection()
        {
            con = new SqlConnection(constr);
        }
        public DataSet MONTH_WISE_REGISTRATION()
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("GET_MONTH_WISE_REGISTRATION", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataSet MONTH_WISE_CANCELLATION_REFUND()
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("GET_MONTH_WISE_CANCELLATION_REFUND", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataSet ACTIVE_INACTIVE_USERS()
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("GET_ACTIVE_INACTIVE_USERS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataSet TRAIL_PAID_USERS()
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("GET_TRAIL_PAID_USERS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataSet ACTIVE_INACTIVE_USERS_DETAILS()
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("GET_ACTIVE_INACTIVE_USERS_DETAILS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataSet TRAIL_PAID_USERS_DETAILS()
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("GET_TRAIL_PAID_USERS_DETAILS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }

        public DataSet USERS_DETAILS(int User_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("GET_USERS_DETAILS_BY_ID", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@User_ID", User_id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }

        public string Insert_FeedBack(int User_ID, string Remarks)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("INSERT_FEEDBACK", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@User_ID", User_ID);
                    cmd.Parameters.AddWithValue("@Remarks", Remarks);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@Message";
                    outData.SqlDbType = SqlDbType.NVarChar;
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
    }
}
