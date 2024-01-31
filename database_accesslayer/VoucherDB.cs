using FarmIT_Api.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace FarmIT_Api.database_accesslayer
{
    public interface IVoucherDB
    {
        string insert_voucher(string type,int expense_id, DateTime? expense_date,decimal expense_amount,string description,string status,int company_id,int created_by,int batch_id,decimal quantity,int item_id ,decimal unit_cost);
        DataTable get_voucher_summary(int company_id,string type);
        string Delete_Voucher_File(int File_id, string type);
    }
    public class VoucherDB:IVoucherDB
    {
        public readonly string constr;

        private SqlConnection con;
        public VoucherDB(string connectionString)
        {
            constr = AesOperation.DecryptString(connectionString);
        }
        private void connection()
        {
            con = new SqlConnection(constr);
        }

        public string insert_voucher(string type,int expense_id,DateTime? expense_date, decimal expense_amount, string description,string status, int company_id, int created_by,int batch_id,decimal quantity,int item_id ,decimal unit_cost)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_INSERT_VOUCHER", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@expense_id", expense_id);
                    cmd.Parameters.AddWithValue("@expense_date", expense_date);
                    cmd.Parameters.AddWithValue("@expense_amount", expense_amount);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@company_id", company_id);
                    cmd.Parameters.AddWithValue("@created_by", created_by);
                    cmd.Parameters.AddWithValue("@batch_id", batch_id);
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    cmd.Parameters.AddWithValue("@item_id", item_id);
                    cmd.Parameters.AddWithValue("@unit_cost", unit_cost);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@Message";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 200;
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

        public DataTable get_voucher_summary(int company_id,string type)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_VOUCHER_SUMMARY", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", company_id);
                    cmd.Parameters.AddWithValue("@TYPE", type);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        public string Delete_Voucher_File(int File_id, string type)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_DELETE_FILES", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FILE_ID", File_id);
                    cmd.Parameters.AddWithValue("@TYPE", type);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 200;
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
