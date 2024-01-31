using FarmIT_Api.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace FarmIT_Api.database_accesslayer
{
    public interface IConfigDB
    {
        DataTable Get_Subscription_Plan();
        DataSet Get_RazorPay_Plan(int User_Id);
        DataSet Get_RazorPay_Plan();
        DataSet Process_Setup();
        DataTable Line_of_business(string Nature_id,string Company_Id);
        string Process_Configuration_Setup(DataTable Config_Setup, DataTable Process_Setup, DataTable User_Setup, int Created_by);
        string Insert_Template_Settings(DataTable Header, DataTable Line, DataTable KPILine, int Created_by, string batch_start_from);
        string insert_user_Setup(DataTable User_Setup);
        DataTable Get_Nature_of_business(int Company_Id);
        DataTable Line_of_business(int Nature_id,int Company_Id);
        DataTable Get_UOM_LIST(int Item_id);
        DataTable Get_Batches(int Lob_id, int Company_Id);
        DataTable Get_Batcheno(int Company_Id,string batchno);
        DataTable Get_OriginalBatcheno(int Company_Id, int batch_id ,int item_id, int location_id,int lob_id,string isbatch_tranfer);
        DataTable Get_Batches_location(int Lob_id, int Company_Id,int loc_id);
        DataTable Get_All_Batches(int Company_Id);
        DataTable Get_Loc_Batches(int Loc_id, int Company_Id);
        DataTable Get_All_Breeds(int nob_id,int Company_Id);
        DataSet Breed_Dataentry_Item(int Nob_Id,int Lob_Id,int Company_Id);
        DataSet Get_Item_List(int Nob_Id, int isDefault, int Company_Id);
        DataSet Get_Item_Details(int Company_Id,int item_id,int loc_id);
        DataSet Get_LOB_Item_Details(int Company_Id, int loc_id,int nob_id);
        DataSet Dataentry_Item(int Company_Id);
        DataTable Parameter_Type();
        DataSet Parameter(int Parameter_Type_Id,int Company_Id,int Lob_Id);
        DataTable Parameter_ALL(int Company_Id);
        DataSet Get_Template_Details(int Tempalte_Id);
        DataSet Get_Template_Details(int Nature_id,int Lob_id);
        DataTable Get_Template_Summary(string Nature_Id,int Company_Id, string Location_Id);

        DataTable Default_template();
        DataTable Get_Batch_Summary(string Nature_Id, int Company_Id, string Location_Id);
        DataTable Get_Batch_Summary_Web(int Company_Id);
        DataSet Get_Batch_Details(int batch_id);
        DataSet Get_Location_Template(int Lob_id,int Company_Id,int Nob_id,int edit_batch_id);
        string Insert_Batch(DataTable dt,DataTable ls,DataTable bi, decimal batch_capacity,DataTable rc);
        string Insert_Batch_files(DataTable dt);
        string Insert_Voucher_files(DataTable dt);
        string Insert_Profile_Picture(string File_name,int User_id);
        string Delete_Batch_File(int File_id,string type);
        DataSet Get_Configuration_Setup(int company_id, string nature_id);
        string Insert_Device_Token(string device_token, string device_type, int user_id);
        string Insert_Notification(string TITLE, string MESSAGE, string SCREEN_NAME = "", string month = "0", int year = 0, int SENDER_EMP_ID = 0, int RECIEVER_EMP_ID = 0);
        DataTable Get_Notifications(int User_Id);
        DataTable Get_CurrentNotifications(int User_Id);
        int Update_Notification_Status(int id, int is_read, int clear, int user_id);
        string Turn_On_Off_Notifications( int user_id,int status);
        DataSet Get_Users_List(int Company_Id);
        string RazorPay_Plan_Update(string plan_id, string status);

        DataTable Machine_List(int Company_Id);//Batch machine list
        DataTable Get_All_Locations(int Company_Id);
        DataTable Get_User_Dashboard_Nob(int User_ID);
    }
    public class ConfigDB : IConfigDB
    {
        public readonly string constr;

        private SqlConnection con;

        public ConfigDB(string connectionString)
        {
            constr = AesOperation.DecryptString(connectionString);
        }

        private void connection()
        {
            con = new SqlConnection(constr);
        }

        #region Plan
        public DataTable Get_Subscription_Plan()
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_GET_PLAN", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex) { }
            return dt;
        }

        public DataSet Get_RazorPay_Plan(int User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_GET_RAZOR_PLAN", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@USER_ID",User_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex) { }
            return ds;
        }
        public DataSet Get_RazorPay_Plan()
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_GET_RAZOR_PLAN_All", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex) { }
            return ds;
        }
        #endregion

        #region Process Configuration Setup
        public DataSet Process_Setup()
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_NATURE_OF_BUSNINESS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception)
            {
            }
            return ds;
        }
        public DataSet Get_Configuration_Setup(int company_id, string nature_id)
        {
            DataSet ds = new DataSet();
            connection();
            using (SqlCommand cmd = new SqlCommand("USP_GET_CONFIGURATION_SETUP", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 5000;
                cmd.Parameters.AddWithValue("@COMPANY_ID", company_id);
                cmd.Parameters.AddWithValue("@NATURE_ID", nature_id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            return ds;
        }
        public string Process_Configuration_Setup(DataTable dtConfig_setup, DataTable dtProcess_setup, DataTable dtUser_setup, int Created_by)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_PROCESS_CONFIGURATION_SETUP", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@tbl_CONFIGURATION_SETUP", dtConfig_setup);
                    cmd.Parameters.AddWithValue("@tbl_PROCESS_SETUP", dtProcess_setup);
                    cmd.Parameters.AddWithValue("@tbl_USER_SETUP", dtUser_setup);
                    cmd.Parameters.AddWithValue("@CREATED_BY", Created_by);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@Message";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 500;
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
        public string insert_user_Setup(DataTable User_setup)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_INSERT_USER_SETUP", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@tbl_USER_SETUP", User_setup);
                 
                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@Message";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 500;
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
        public DataSet Get_Users_List(int Company_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_USERS_LIST", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception)
            {
            }
            return ds;
        }
        public string RazorPay_Plan_Update(string plan_id,string status)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_RAZOR_PLAN_UPDATE", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@plan_id", plan_id);
                    cmd.Parameters.AddWithValue("@status", status);
                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@Message";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 500;
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

        #region Template Setting
        public string Insert_Template_Settings(DataTable Header, DataTable Line, DataTable KPILine, int Created_by, string Batch_Start_From)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_INSERT_TEMPLATE", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@HEADER", Header);
                    cmd.Parameters.AddWithValue("@LINE", Line);
                    cmd.Parameters.AddWithValue("@KPI_LINE", KPILine);
                    cmd.Parameters.AddWithValue("@CREATED_BY", Created_by);
                    cmd.Parameters.AddWithValue("@BATCH_START_FROM", Batch_Start_From);

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

        public DataSet Get_Template_Details(int Tempalte_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_TEMPLATE_DETAILS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TEMPLATE_ID", Tempalte_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception)
            {
            }
            return ds;
        }

        public DataSet Get_Template_Details(int Nature_id, int Lob_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_GET_TEMPLATE_DETAILS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NOB_ID", Nature_id);
                    cmd.Parameters.AddWithValue("@LOB_ID", Lob_id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception)
            {
            }
            return ds;
        }

        public DataTable Get_Template_Summary(string Nature_Id, int Company_Id,string Location_Id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_TEMPLATE_SUMMARY", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NATURE_ID", Nature_Id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@LOCATION_ID", Location_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        public DataTable Default_template()
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_Default_TEMPLATE_SUMMARY", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        public DataSet Get_Location_Template(int Lob_id, int Company_Id,int Nob_id,int edit_batch_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_LOCATION_TEMPLATE", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LOB_ID", Lob_id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@NOB_ID", Nob_id);
                    cmd.Parameters.AddWithValue("@edit_batch_id", edit_batch_id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
            }
            return ds;
        }
        #endregion

        #region Batches
        public string Insert_Batch(DataTable dt,DataTable ls,DataTable bi,decimal batch_capacity,DataTable rc)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_INSERT_BATCH", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tbl_batch", dt);
                    cmd.Parameters.AddWithValue("@tbl_batch_LS", ls);//batch livestock
                    cmd.Parameters.AddWithValue("@tbl_batch_item", bi); //batch item
                    cmd.Parameters.AddWithValue("@tbl_batch_machine", rc);//resourch card(machine)
                    cmd.Parameters.AddWithValue("@batch_capacity", batch_capacity);
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

        public string Insert_Batch_files(DataTable dt)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_UPLOAD_FILES", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tbl_files", dt);
                    cmd.Parameters.AddWithValue("@type", "other");
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

        public DataTable Get_Batch_Summary(string Nature_Id, int Company_Id,string Location_Id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_BATCH_SUMMARY", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NATURE_ID", Nature_Id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@LOCATION_ID", Location_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        public DataTable Get_Batch_Summary_Web(int Company_Id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_BATCH_SUMMARY_WEB", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        public DataSet Get_Batch_Details(int batch_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_BATCH_DETAILS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BATCH_ID", batch_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        public string Delete_Batch_File(int File_id, string type)
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
        public DataTable Get_Batches(int Lob_id, int Company_Id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_GET_BATCHES", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LOB_ID", Lob_id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }
        public DataTable Get_Batcheno(int Company_Id,string batchno)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_GET_BATCH_NO", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Batch_No", batchno);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }
        public DataTable Get_OriginalBatcheno(int Company_Id, int batch_id,int item_id,int location_id,int lob_id,string isbatch_tranfer)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_GET_ORIGINAL_BATCH_NO", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;                    
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@Batch_ID", batch_id);
                    cmd.Parameters.AddWithValue("@item_id", item_id);
                    cmd.Parameters.AddWithValue("@location_id", location_id);
                    cmd.Parameters.AddWithValue("@lob_id", lob_id);
                    cmd.Parameters.AddWithValue("@isbatch_tranfer", isbatch_tranfer);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }


        
        public DataTable Get_UOM_LIST(int Item_id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_GETUOM_BYTYPE", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Item_id", Item_id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }
        public DataTable Get_Batches_location(int Lob_id, int Company_Id,int loc_id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_GET_BATCHES_BY_Location", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LOB_ID", Lob_id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@LOC_ID", loc_id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }
        public DataTable Get_All_Batches(int Company_Id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("GET_BATCHES", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }
        public DataTable Get_Loc_Batches(int Loc_id, int Company_Id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_GET_LOC_BATCHES", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LOC_ID", Loc_id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }
        #endregion

        #region Nob/Lob/Breed/Parameters

        public DataTable Line_of_business(string Nature_id,string Company_Id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_LINE_OF_BUSNINESS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NATURE_ID", Nature_id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        public DataTable Get_Nature_of_business(int Company_Id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_NOB", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        public DataTable Line_of_business(int Nature_id,int Company_Id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_LOB", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NATURE_ID", Nature_id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        public DataSet Breed_Dataentry_Item(int Nob_Id, int Lob_Id, int Company_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_BREED_DATAENTRY_TYPE_ITEM", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NOB_ID", Nob_Id);
                    cmd.Parameters.AddWithValue("@LOB_ID", Lob_Id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
            }
            return ds;
        }


        public DataSet Get_Item_List(int Nob_Id, int isDefault, int Company_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_GET_ITEM_LIST", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NOB_ID", Nob_Id);
                    cmd.Parameters.AddWithValue("@isDefault", isDefault);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
            }
            return ds;
        }
        public DataSet Get_Item_Details(int Company_Id,int item_id,int loc_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_GET_ITEM_DETAILS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("@Item_id", item_id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@Location_ID", loc_id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
            }
            return ds;
        }
        public DataSet Get_LOB_Item_Details(int Company_Id, int loc_id,int nob_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_GET_LOB_ITEM_DETAILS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@Location_ID", loc_id);
                    cmd.Parameters.AddWithValue("@NOB_ID", nob_id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
            }
            return ds;
        }
        public DataSet Dataentry_Item(int Company_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_DATAENTRY_TYPE_ITEM_ALL", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        public DataTable Parameter_Type()
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_PARAMETER_TYPE", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        public DataSet Parameter(int Parameter_Type_Id,int Company_Id,int Lob_Id)
        {
            DataSet dt = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_PARAMETER", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PARAMETER_TYPE_ID", Parameter_Type_Id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@LOB_ID", Lob_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }
        public DataTable Parameter_ALL(int Company_Id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_PARAMETER_ALL", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }
        #endregion

        #region Insert Picture Name
        public string Insert_Profile_Picture(string File_name, int User_id)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_UPLOAD_PROFILE_PICTURE", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@File_Name", File_name);
                    cmd.Parameters.AddWithValue("@User_Id", User_id);

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
        public string Insert_Voucher_files(DataTable dt)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_UPLOAD_FILES", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tbl_files", dt);
                    cmd.Parameters.AddWithValue("@type", "expense");

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
        #endregion

        #region Notification
        public string Insert_Device_Token(string device_token, string device_type, int user_id)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_INSERT_NOTIFICATION_TOKEN", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@USER_ID", user_id);
                    cmd.Parameters.AddWithValue("@DEVICE_TOKEN", device_token);
                    cmd.Parameters.AddWithValue("@DEVICE_TYPE", device_type);

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
            catch(Exception ex)
            {
                res = ex.Message;
            }            
            return res;
        }
        public string Insert_Notification(string TITLE, string MESSAGE, string SCREEN_NAME = "", string month = "0", int year = 0, int SENDER_EMP_ID = 0, int RECIEVER_EMP_ID = 0)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_INSERT_GET_NOTIFICATION", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@TITLE", TITLE);
                    cmd.Parameters.AddWithValue("@BODY", MESSAGE);
                    cmd.Parameters.AddWithValue("@SCREEN_NAME", SCREEN_NAME);
                    cmd.Parameters.AddWithValue("@MONTH", month);
                    cmd.Parameters.AddWithValue("@YEAR", year);
                    cmd.Parameters.AddWithValue("@RECEIVER_EMP_ID", RECIEVER_EMP_ID);
                    cmd.Parameters.AddWithValue("@SENDER_EMP_ID", SENDER_EMP_ID);

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
        public DataTable Get_Notifications(int User_Id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("PRC_GET_NOTIFICATIONS", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@USER_ID", User_Id);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
            }
            catch { }
            return dt;
        }
        public DataTable Get_CurrentNotifications(int User_Id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("PRC_GET_CURRENT_NOTIFICATIONS", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@USER_ID", User_Id);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
            }
            catch { }
            return dt;
        }
        public int Update_Notification_Status(int id, int is_read, int clear, int user_id)
        {
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_UPDATE_NOTIFICATION", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@IS_READ", is_read);
                    cmd.Parameters.AddWithValue("@CLEAR", clear);
                    cmd.Parameters.AddWithValue("@USER_ID", user_id);

                    con.Open();
                    Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();
                    con.Close();
                    return count;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public string Turn_On_Off_Notifications(int user_id,int status)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_UNOTIFICATION_STATUS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@USER_ID", user_id);
                    cmd.Parameters.AddWithValue("@STATUS", status); 

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

        public DataTable Machine_List(int company_id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_Get_MachineList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@company_id", company_id);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
            }
            catch { }
            return dt;
        }

        public DataTable Get_All_Locations(int compay_id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("GET_LOCATIONS", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANY_ID", compay_id);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
            }
            catch { }
            return dt;
        }

        public DataTable Get_All_Breeds(int nob_id,int compay_id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Get_Breeds", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NOB_ID", nob_id);
                cmd.Parameters.AddWithValue("@COMPANY_ID", compay_id);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
            }
            catch { }
            return dt;
        }
        #endregion

        public DataTable Get_User_Dashboard_Nob(int user_id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Get_User_Dashboard_NobID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@USER_ID", user_id);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
            }
            catch { }
            return dt;
        }
    }
}

