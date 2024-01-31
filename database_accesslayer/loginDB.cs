using System;
using FarmIT_Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Data;

namespace FarmIT_Api.database_accesslayer
{
    public interface IloginDB
    {
        string User_Login(string mobile_no,string password, string Social_Id);
        string Registration(string Social_Name, string Mobile_No, string Social_Id, string Social_Email, string Social_Profile_Pic,int Is_FaceBook,int Is_Gmail,int Is_Apple);
        string ChangePassword(string old_Password, string new_Password, int Emp_id);
        string ForgotPassword(string mobile_no);
        string User_Logout(int User_Id,string Token);
        string User_Logout(int User_Id);
        DataSet Enable_Disable();
        DataTable GetMenu(int User_Id);
        DataTable Get_Dashboard(int Nature_Id,int Company_Id, int user_id);
        DataTable Get_Dashboard_All_Nob(int Company_Id);
        DataTable Get_Dashboard_Summary(int Lob_Id, int Company_Id);
        DataTable Get_Dashboard_Summary_Details(int Batch_Id, int Lob_Id);
        DataTable Get_Dashboard_Summary_Details2(int Batch_Id,string Parameter_Name,string Month_Year);
        DataSet Get_Profile(int Emp_Id, int User_Id);
        DataTable Get_Billing_Info(int Company_Id,int User_Id);
        DataTable UserDetails_ByToken(string Token);
        DataTable Get_Support_Reason();

        DataTable Get_Dashboard_Laying_Monthly_Output(int Company_Id,int Lob_Id, int RType);
        DataTable Get_Dashboard_Laying_Monthly_Output1(int Company_Id);
        DataTable Get_Dashboard_COMM_Monthly_Output(int Company_Id, int Lob_Id, int RType);
        DataTable Get_Dashboard_Laying_Monthly_Output2(int Company_Id, int Lob_Id, int RType);
        DataTable Get_Dashboard_Location_Output(int Company_Id);
        DataSet Get_Dashboard_Laying_Mortality(int Company_Id, int Lob_Id);
        DataSet Get_Dashboard_Female_Laying_Mortality(int Company_Id, int Lob_Id);
        DataSet Get_Dashboard_Dairy_Milk(int Company_Id, int Lob_Id);
        DataSet Get_Dashboard_Hatch_Reject(int Company_Id, int Lob_Id);
        DataSet Get_Dashboard_Hatch_Egg_Graph(int Company_Id, int Lob_Id);   //NEw 21_Feb_2022
        DataSet Get_Dashboard_Hatch_Mortality_Graph(int Company_Id, int Lob_Id);  //NEw 21_Feb_2022
        DataSet Get_Dashboard_Hatch_Output_Graph(int Company_Id, int Lob_Id);  //NEw 21_Feb_2022
        DataSet Get_Dashboard_CommercialBroiler_Output_Graph(int Company_Id, int Lob_Id);  //NEw 21_Feb_2022
        DataSet Get_Dashboard_Least_Performance(int Company_Id, int Lob_Id);
        DataTable Get_Dashboard_LBREEDING_Monthly_Output(int Company_Id, int Lob_Id, int RType);
        DataTable Get_Dashboard_LDairy_Monthly_Output(int Company_Id, int Lob_Id, int RType);
        DataTable Get_Dashboard_Laying_Egg_Output(int Company_Id, int Lob_Id, int RType);
        DataTable Get_Dashboard_Laying_Egg_WT_Output(int Company_Id, int Lob_Id, int RType);
        DataTable Get_Dashboard_Laying_Egg_BATCH_Output(int Batch_id);
        string Update_Profile(DataTable profile_dt,DataTable activeInactive_dt,DataTable user_dt);
        string Insert_User_Token(string Token,int User_Id);
        string Insert_Billing_Info(DataTable billing_info,string plan_type);
        string Insert_Payment_details(Payment_Details pd);
        string Subscription_cancelation(Subscription_CancelModel sc);
        string Subscription_Upgrade(Subscription_UpdateModel sm);
        string Validate_Mobile_No(string Mobile_No);
        string insert_plan(DataTable plan,DataTable plan_description);
        DataTable GetRazorPay_Invoices(string Subscription_Id);
        string Insert_Email_Logs(int user_id,string email_id,string email_type,string doc_no,string message,string status);
        string UpdateSubscriptionByInvoiceDetails(int User_Id, decimal Payment_Amount, string Plan_Id, string Transaction_Id, long Payment_date, int Company_Id, int Licence, string Frequency, string Email, string Phone_No, int Billinfo_Id, string Subscription_Id, string Status, long Start_Date, long End_Date, string Invoice_No, string Invoice_Url,string Currency);
        DataSet Get_Dashboard_locationWiseRunningCostGraph(int Company_Id ,int User_ID, int nature_id);
        DataSet Get_Dashboard_locationWiseOutputGraph(int Company_Id, int User_ID,int nature_id);
    }
    public class loginDB : IloginDB
    {
        public static string constr1 { get; set; } 
        public readonly string constr;
        private SqlConnection con;
        public loginDB(string connectionString)
        {
            constr = AesOperation.DecryptString(connectionString);
            constr1 = constr;
        }
        private void connection()
        {
            con = new SqlConnection(constr);
        }

        #region SignIn/SignOut
        public string User_Login(string Mobile_No,string Password,string Social_Id)
        {
            string str="";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_LOGIN", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MOBILE_NO", Mobile_No);
                    cmd.Parameters.AddWithValue("@PASSWORD", Password);
                    cmd.Parameters.AddWithValue("@SOCIAL_ID", Social_Id);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 500;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    str = cmd.ExecuteNonQuery().ToString();
                    str = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }            
            return str;
        }
        public string Registration(string Social_Name,string Mobile_No,string Social_Id,string Social_Email,string Social_Profile_Pic,int Is_FaceBook,int Is_Gmail,int Is_Apple)
        {
            string str = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_REGISTRATION", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SOCIAL_NAME", Social_Name);
                    cmd.Parameters.AddWithValue("@MOBILE_NO", Mobile_No);
                    cmd.Parameters.AddWithValue("@SOCIAL_ID", Social_Id);
                    cmd.Parameters.AddWithValue("@SOCIAL_EMAIL", Social_Email);
                    cmd.Parameters.AddWithValue("@SOCIAL_PROFILE_PIC", Social_Profile_Pic);
                    cmd.Parameters.AddWithValue("@IS_FACEBOOK", Is_FaceBook);
                    cmd.Parameters.AddWithValue("@IS_GMAIL", Is_Gmail);
                    cmd.Parameters.AddWithValue("@Is_Apple", Is_Apple);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 200;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    str = cmd.ExecuteNonQuery().ToString();
                    str = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }
        public string User_Logout(int User_Id, string Token)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_SIGN_OUT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@USER_ID", User_Id);
                    cmd.Parameters.AddWithValue("@TOKEN", Token);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
                    outData.SqlDbType = SqlDbType.NVarChar;
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
        public string User_Logout(int User_Id)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_SIGN_OUT_ALL", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@USER_ID", User_Id);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
                    outData.SqlDbType = SqlDbType.NVarChar;
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
        #endregion

        #region Change/Forgot Password
        public string ChangePassword(string Old_Password, string New_Password, int Emp_id)
        {
            var str = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_CHANGE_PASSWORD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OLD_PWD", Old_Password);
                    cmd.Parameters.AddWithValue("@NEW_PWD", New_Password);
                    cmd.Parameters.AddWithValue("@USER_ID", Emp_id);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 100;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    str = cmd.ExecuteNonQuery().ToString();
                    str = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }
        public string ForgotPassword(string Mobile_No)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_VALIDATE_USER", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MOBILE_NO", Mobile_No);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
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
        #endregion

        #region Menu/Dashboard
        public DataTable GetMenu(int User_Id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_MENU", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@USER_ID", User_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        public DataTable Get_Dashboard(int Nature_Id,int Company_Id,int user_id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_DASHBOARD", con))
               // using (SqlCommand cmd = new SqlCommand("USP_DASHBOARD_COMMON", con)) this for development
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NATURE_ID",Nature_Id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@USER_ID", user_id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public DataTable Get_Dashboard_All_Nob(int Company_Id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_DASHBOARD_ALL_NOB", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        public DataTable Get_Dashboard_Summary(int Lob_Id, int Company_Id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_DASHBOARD_SUMMARY", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LOB_ID", Lob_Id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        public DataTable Get_Dashboard_Summary_Details(int Batch_Id,int Lob_Id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_DASHBOARD_SUMMARY_DETAILS1", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BATCH_ID", Batch_Id);
                    cmd.Parameters.AddWithValue("@LOB_ID", Lob_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        public DataTable Get_Dashboard_Summary_Details2(int Batch_Id, string Parameter_Name, string Month_Year)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_DASHBOARD_SUMMARY_DETAILS2", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BATCH_ID", Batch_Id);
                    cmd.Parameters.AddWithValue("@PARAMETER_NAME", Parameter_Name);
                    cmd.Parameters.AddWithValue("@MONTH_YEAR", Month_Year);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public DataTable Get_Dashboard_Laying_Monthly_Output(int Company_Id,int Lob_Id,int RType )
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_DASHBOARD_LAYING_OUTPUT_GRAPH", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LOB_ID", Lob_Id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@Type", RType);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public DataTable Get_Dashboard_Laying_Monthly_Output1(int Company_Id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_DASHBOARD_LAYING_GRAPH3 ", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        public DataTable Get_Dashboard_COMM_Monthly_Output(int Company_Id, int Lob_Id, int RType)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_DASHBOARD_COMM_OUTPUT_GRAPH", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LOB_ID", Lob_Id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@Type", RType);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public DataTable Get_Dashboard_LBREEDING_Monthly_Output(int Company_Id, int Lob_Id, int RType)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_DASHBOARD_LBREEDING_OUTPUT_GRAPH", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LOB_ID", Lob_Id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@Type", RType);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public DataTable Get_Dashboard_LDairy_Monthly_Output(int Company_Id, int Lob_Id, int RType)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_DASHBOARD_DAIRY_OUTPUT_GRAPH", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LOB_ID", Lob_Id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@Type", RType);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        public DataTable Get_Dashboard_Laying_Monthly_Output2(int Company_Id, int Lob_Id, int RType)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_DASHBOARD_LAYING_OUTPUT_GRAPH2", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LOB_ID", Lob_Id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@Type", RType);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        public DataTable Get_Dashboard_Location_Output(int Company_Id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_DASHBOARD_LOCATION_LIST", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        public DataSet Get_Dashboard_Laying_Mortality(int Company_Id, int Lob_Id)
        {
            DataSet dt = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_DASHBOARD_LAYING_Mortality_GRAPH", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LOB_ID", Lob_Id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public DataSet Get_Dashboard_Female_Laying_Mortality(int Company_Id, int Lob_Id)
        {
            DataSet dt = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_DASHBOARD_LAYING_Female_Mortality_GRAPH", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LOB_ID", Lob_Id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        public DataSet Get_Dashboard_Dairy_Milk(int Company_Id, int Lob_Id)
        {
            DataSet dt = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_DASHBOARD_DAIRY_MILK_GRAPH", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LOB_ID", Lob_Id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public DataSet Get_Dashboard_Hatch_Reject(int Company_Id, int Lob_Id)
        {
            DataSet dt = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_DASHBOARD_HATCH_REJECTED_GRAPH", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LOB_ID", Lob_Id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public DataSet Get_Dashboard_Hatch_Egg_Graph(int Company_Id, int Lob_Id)
        {
            DataSet dt = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_DASHBOARD_HATCH_EGG_GRAPH", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LOB_ID", Lob_Id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        public DataSet Get_Dashboard_Hatch_Mortality_Graph(int Company_Id, int Lob_Id)
        {
            DataSet dt = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_DASHBOARD_HATCH_Mortality_GRAPH", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LOB_ID", Lob_Id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        public DataSet Get_Dashboard_Hatch_Output_Graph(int Company_Id, int Lob_Id)
        {
            DataSet dt = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_DASHBOARD_HATCH_OUTPUT_REPORT_GRAPH", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LOB_ID", Lob_Id);
                    cmd.Parameters.AddWithValue("@Company_id", Company_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        public DataSet Get_Dashboard_Hatch_Output_Graph( int batch_id)
        {
            DataSet dt = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_DASHBOARD_HATCH_OUTPUT_REPORT_GRAPH", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@batch_id", batch_id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        public DataSet Get_Dashboard_CommercialBroiler_Output_Graph(int Company_Id, int Lob_Id)
        {
            DataSet dt = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_DASHBOARD_COMM_OUTPUT_GRAPH2", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LOB_ID", Lob_Id);
                    cmd.Parameters.AddWithValue("@Company_id", Company_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public DataSet Get_Dashboard_Least_Performance(int Company_Id, int Lob_Id)
        {
            DataSet dt = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_BATCH_PERFORMANCE_LEAST_REPORT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LOB_ID", Lob_Id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        public DataTable Get_Dashboard_Laying_Egg_Output(int Company_Id, int Lob_Id, int RType)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_DASHBOARD_LAYING_EGG_GRAPH", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LOB_ID", Lob_Id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@Type", RType);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        public DataTable Get_Dashboard_Laying_Egg_WT_Output(int Company_Id, int Lob_Id, int RType)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_DASHBOARD_LAYING_EGG_WT_GRAPH", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LOB_ID", Lob_Id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@Type", RType);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        public DataTable Get_Dashboard_Laying_Egg_BATCH_Output(int Batch_id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_DASHBOARD_LAYING_BATch_GRAPH", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BATCH_ID", Batch_id);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        #endregion

        #region User Details
        public DataSet Get_Profile(int Company_Id,int User_Id)
        {
            DataSet ds = new DataSet();
            connection();
            using (SqlCommand cmd = new SqlCommand("USP_PROFILE", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                cmd.Parameters.AddWithValue("@USER_ID", User_Id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            return ds;
        }        
        public DataTable Get_Billing_Info(int Company_Id,int User_Id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_BILLING_INFO", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@USERID", User_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        public DataTable Get_Support_Reason()
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_SUPPORT_REASON", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        public string Update_Profile(DataTable profile_dt, DataTable activeInactive_dt, DataTable user_dt)
        {
            string res = "";
            connection();
            using (SqlCommand cmd = new SqlCommand("USP_UCOMPANY_PROFILE", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tbl_config_setup", profile_dt);
                cmd.Parameters.AddWithValue("@tbl_activedeactive", activeInactive_dt);
                cmd.Parameters.AddWithValue("@tbl_user_setup", user_dt);

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
            return res;
        }
        public string Insert_User_Token(string Token, int User_Id)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_USER_TOKEN", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TOKEN", Token);
                    cmd.Parameters.AddWithValue("@USER_ID", User_Id);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
                    outData.SqlDbType = SqlDbType.NVarChar;
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
        public string Validate_Mobile_No(string Mobile_No)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_MOBILE_VALIDATE", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MOBILE_NO", Mobile_No);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
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
        public DataTable UserDetails_ByToken(string Token)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("GET_USER_DETAILS_BYTOKEN", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TOKEN", Token);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        
        #endregion

        #region Payment/Billing Information/Plan
        public string Insert_Billing_Info(DataTable billing_info,string plan_type)
        {
            string RES = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_IUSER_BILLING_INFO", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tbl_billing_info", billing_info);
                    cmd.Parameters.AddWithValue("@PLAN_TYPE", plan_type);
                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 300;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    RES = cmd.ExecuteNonQuery().ToString();
                    RES = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                RES = ex.Message;
            }
            return RES;
        }
        public string Insert_Payment_details(Payment_Details pd)
        {
            string str = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_INSERT_PAYMENT_DETAILS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@BILLING_INFO_ID", pd.billing_info_id);
                    cmd.Parameters.AddWithValue("@EMP_ID", pd.Emp_Id);
                    cmd.Parameters.AddWithValue("@PAYMENT_AMOUNT",pd.Payment_Amount);
                    cmd.Parameters.AddWithValue("@PLAN_ID",pd.Plan_Id);
                    cmd.Parameters.AddWithValue("@TRANSACTION_ID",pd.Transaction_Id);
                    cmd.Parameters.AddWithValue("@PAYMENT_DATE",pd.Payment_Date);
                    cmd.Parameters.AddWithValue("@PAYMENT_STATUS",pd.Payment_Status);
                    cmd.Parameters.AddWithValue("@PLAN_TYPE",pd.Plan_Type);
                    cmd.Parameters.AddWithValue("@LICENCE",pd.licence);
                    cmd.Parameters.AddWithValue("@FREQUENCY", pd.frequency);
                    cmd.Parameters.AddWithValue("@USER_NAME", pd.user_name);
                    cmd.Parameters.AddWithValue("@EMAIL", pd.email);
                    cmd.Parameters.AddWithValue("@PHONE_NO", pd.phone_no);
                    cmd.Parameters.AddWithValue("@SUBSCRIPTION_ID", pd.subscription_id);
                    cmd.Parameters.AddWithValue("@RAZOR_PAY_INVOICE_NO", pd.razor_pay_invoice_no);
                    cmd.Parameters.AddWithValue("@INVOICE_URL", pd.invoice_url);
                    //cmd.Parameters.AddWithValue("@SUBSCRIPTION_START_DATE", pd.subscription_start_date);
                    //cmd.Parameters.AddWithValue("@SUBSCRIPTION_END_DATE", pd.subscription_end_date);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 150;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    str = cmd.ExecuteNonQuery().ToString();
                    str = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }
        public string insert_plan(DataTable plan, DataTable plan_description)
        {
            string str = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_INSERT_PLAN", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@tbl_plan", plan);
                    cmd.Parameters.AddWithValue("@tbl_plan_description", plan_description);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MSG";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 250;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    str = cmd.ExecuteNonQuery().ToString();
                    str = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }
        #endregion

        #region Subscription
        public string Subscription_cancelation(Subscription_CancelModel sc)
        {
            string str = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_USUBSCRIPTION", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@USER_ID", sc.user_id);
                    cmd.Parameters.AddWithValue("@SUBSCRIPTION_ID", sc.subscription_id);
                    cmd.Parameters.AddWithValue("@SUBSCRIPTION_STATUS", sc.subscription_status);
                    cmd.Parameters.AddWithValue("@CANCEL_DATE", sc.cancel_date);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
                    outData.SqlDbType = SqlDbType.VarChar;
                    outData.Size = 400;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    str = cmd.ExecuteNonQuery().ToString();
                    str = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }
        public string Subscription_Upgrade(Subscription_UpdateModel sm)
        {
            string str = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_UPGRADE_SUBSCRIPTION", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@USER_ID", sm.user_id);
                    cmd.Parameters.AddWithValue("@SUBSCRIPTION_ID", sm.subscription_id);
                    cmd.Parameters.AddWithValue("@LICENCE", sm.quantity);                    

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
                    outData.SqlDbType = SqlDbType.VarChar;
                    outData.Size = 300;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    str = cmd.ExecuteNonQuery().ToString();
                    str = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }
        public DataTable GetRazorPay_Invoices(string Subscription_Id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("GET_RAZOR_PAY_INVOICES", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SUBSCRIPTION_ID", Subscription_Id);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public string UpdateSubscriptionByInvoiceDetails(int User_Id, decimal Payment_Amount, string Plan_Id, string Transaction_Id, long Payment_date, int Company_Id, int Licence, string Frequency, string Email, string Phone_No, int Billinfo_Id, string Subscription_Id, string Status, long Start_Date, long End_Date, string Invoice_No, string Invoice_Url,string Currency)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_INSERT_INVOICE_DETAILS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@USER_ID", User_Id);
                    cmd.Parameters.AddWithValue("@PAYMENT_AMOUNT", Payment_Amount);
                    cmd.Parameters.AddWithValue("@PLAN_ID", Plan_Id);
                    cmd.Parameters.AddWithValue("@TRANSACTION_ID", Transaction_Id);
                    cmd.Parameters.AddWithValue("@PAYMENT_DATE", Payment_date);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@LICENCE", Licence);
                    cmd.Parameters.AddWithValue("@FREQUENCY", Frequency);
                    cmd.Parameters.AddWithValue("@EMAIL", Email);
                    cmd.Parameters.AddWithValue("@PHONE_NO", Phone_No);
                    cmd.Parameters.AddWithValue("@BILLING_INFO_ID", Billinfo_Id);
                    cmd.Parameters.AddWithValue("@SUBSCRIPTION_ID", Subscription_Id);
                    cmd.Parameters.AddWithValue("@SUBSCRIPTION_STATUS", Status);
                    cmd.Parameters.AddWithValue("@SUBSCRIPTION_START_DATE", Start_Date);
                    cmd.Parameters.AddWithValue("@SUBSCRIPTION_END_DATE", End_Date);
                    cmd.Parameters.AddWithValue("@RAZOR_PAY_INVOICE_NO", Invoice_No);
                    cmd.Parameters.AddWithValue("@INVOICE_URL", Invoice_Url);
                    cmd.Parameters.AddWithValue("@Currency", Currency);
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
        public string Insert_Email_Logs(int user_id, string email_id, string email_type, string doc_no, string message, string status)
        {
            string res = "success";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_INSERT_EMAIL_LOGS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@USER_ID", user_id);
                    cmd.Parameters.AddWithValue("@EMAIL_ID", email_id);
                    cmd.Parameters.AddWithValue("@EMAIL_TYPE", email_type);
                    cmd.Parameters.AddWithValue("@DOC_NO", doc_no);
                    cmd.Parameters.AddWithValue("@MESSAGE", message);
                    cmd.Parameters.AddWithValue("@STATUS", status);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        #endregion
        public DataSet Enable_Disable()
        {
            DataSet ds = new DataSet();
            connection();
            using (SqlCommand cmd = new SqlCommand("USP_ENABLE_DISABLE", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            return ds;
        }

        public DataSet Get_Dashboard_locationWiseRunningCostGraph(int company_id,int User_ID ,int nature_id)
        {
            DataSet ds = new DataSet();
            connection();
            using (SqlCommand cmd = new SqlCommand("USP_LOCATION_RUNNINGCOST", con)){
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Company_id", company_id);
                cmd.Parameters.AddWithValue("@User_id", User_ID);
                cmd.Parameters.AddWithValue("@nature_id", nature_id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            return ds;
        }
        public DataSet Get_Dashboard_locationWiseOutputGraph(int company_id ,int user_id,int nature_id)
        {
            DataSet ds = new DataSet();
            connection();
            using (SqlCommand cmd = new SqlCommand("USP_LOCATION_OUTPUT", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Company_id", company_id);
                cmd.Parameters.AddWithValue("@User_id", user_id);
                cmd.Parameters.AddWithValue("@nature_id", nature_id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            return ds;
        }
    }

}
