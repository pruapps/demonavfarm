using FarmIT_Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FarmIT_Api.database_accesslayer
{
    public interface IReportDB
    {
        DataSet Get_Weekly_Report(int Nob_Id,int Lob_Id,int Company_Id,string From,string To,int Batch_Id);
        DataSet Get_Laying_Report(int Batch_Id);
        DataSet Get_Rearing_Report(int Batch_Id);
        DataSet Get_BreedingLaying_Report(int Batch_Id);
        DataSet Get_BreedingLaying_Report_Part2(int Batch_Id);
        DataSet Get_Hatchery_Report(int Batch_Id);
        DataSet Get_FlockWiseFcr_Report(int Company_id, int Lob_Id, string Batch_Id, int? Location_Id, string from_date, string to_date);
        DataSet Get_Cbf_Report(int Batch_Id);
        DataSet Get_Cbf_Report_Part2(int Batch_Id);
        DataSet Get_feed_Report(int Company_Id,int Lob_Id,int Batch_id);
        DataSet Get_Slaughter_Report(int Company_Id, int Lob_Id,int Batch_id);
        DataSet Get_Farm_Performance_Location_Report(int Company_id,string Batch_id);
        DataSet Get_Farm_Performance_Wk_Report(int Company_Id,string Start_Date,string End_Date);
        DataSet Get_Batch_Output_Performance_Report(int Company_Id, string Start_Date, string End_Date);
        DataSet Get_Cattle_Report(int Batch_Id);
        DataSet Get_Cattle_Dairy_Report(string fromdate, string todate, string Batch_Id);
        DataSet Get_Animal_Drug_Report(int compamy_id,int lob_id,int Month, int Year, string Batch_Id);
        DataSet Get_Feed_Usage_Report(int compamy_id, int lob_id, string fromdate, string todate, string Batch_Id);
        DataSet Get_Item_Stock_Report(int compamy_id, string fromdate, string todate, int item_id, int loc_id);
        DataSet Get_Item_Stock_Report_location_wise(int compamy_id, string fromdate, string todate, int item_id, int loc_id);
        DataSet Get_Aqua_rearing_report(int compamy_id, int lob_id, int batch_id);
        DataSet Get_Aqua_rearing_growth_percentage_fcr_report(int compamy_id, int lob_id, int batch_id);
        DataSet Get_Piggery_Report(int Batch_Id);
        DataSet Get_Piggery_Report_Graph(int Batch_Id,int type);
        DataSet Get_Agri_Report(int Batch_Id);
        DataSet Get_Agri_Summ_Report(int Batch_Id);
        DataSet Get_CBR_KPI_Report(int company_id, int Batch_Id);
        DataSet Get_CBR_Monitring_Report(int company_id, int Batch_Id);
        DataSet get_laying_feed_kpi_report(int company_id, int Batch_Id,int type);
        DataSet get_laying_feed_kpi_report_standard(int company_id, int Batch_Id, int type,int user_id);
        DataSet Get_Aqua_FeedPlanning_report(int compamy_id, int batch_id);
        DataSet Get_Dashboard_Hatch_Output_Graph(int batch);
        DataSet Get_Stock_Valuation_report(int compamy_id, int batch,string from_date,string to_date);
        DataSet Get_slaughtering_chart(int compamy_id, int batch,int type);
        DataSet Get_item_location_stock_report(int compamy_id, string item_id, string location_id);
        DataSet Get_Transfer_Summary_report(int compamy_id, string from_date, string to_date);
        DataSet Get_Inventory_Stock_Report(int compamy_id, string fromdate, string todate, int item_id, string loc_id);
        DataSet Get_Ledger_Stock_Report(int compamy_id);
        DataSet Get_Transacation_Ledger_Report(int compamy_id, int user_id, int nob_id, string location, string batch, string item, string item_category, string transfer_type);
        DataSet Get_feed_forecast_report(int batch_id, int item_id, string from_date, string to_date);
        DataSet Get_Transactions_Apply_Entry_Report(int compamy_id);
        DataSet Get_Trace_Report(int company_id);
        DataSet Get_Item_Stock_Summary_Report(int company_id);
        DataSet Get_BreedAttribute_Report(int company_id);

        #region Aqua Dashboard Api
        DataTable get_aqua_dashbard_feed(int company_id, int lob_id);
        DataTable GET_Aqua_growth_percentage_fcr_dashboard(int company_id);
        DataTable GET_Aqua_dashboard_running_cost(int company_id);
        #endregion
        DataTable GET_Descriptive_report(int batch_id);
        DataTable GET_Inventory_summary_report(int company_id, int location_id, int item_id, string transfer_type,string fromDate,string Todate);


    }
    public class ReportDB : IReportDB
    {
        public readonly string constr;

        private SqlConnection con;

        public ReportDB(string connectionString)
        {
            constr = AesOperation.DecryptString(connectionString);
        }
        private void connection()
        {
            con = new SqlConnection(constr);
        }
        public DataSet Get_Weekly_Report(int Nob_Id, int Lob_Id, int Company_Id, string From, string To,int Batch_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_FLOCK_FCR_REPORT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@NOB", Nob_Id);
                    cmd.Parameters.AddWithValue("@LOB", Lob_Id);
                    cmd.Parameters.AddWithValue("@FROM", From);
                    cmd.Parameters.AddWithValue("@TO", To);
                    cmd.Parameters.AddWithValue("@ID", Batch_Id);
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
        public DataSet Get_Laying_Report(int Batch_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_LAYING_REPORT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BATCH_ID", Batch_Id);
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
        public DataSet Get_Rearing_Report(int Batch_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_REARING_REPORT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BATCH_ID", Batch_Id);
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
        public DataSet Get_BreedingLaying_Report(int Batch_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_LAYING_REPORT2", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BATCH_ID", Batch_Id);
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
        public DataSet Get_BreedingLaying_Report_Part2(int Batch_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_BREEDING_LAYING_REPORT_PART2", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BATCH_ID", Batch_Id);
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
        public DataSet Get_Hatchery_Report(int Batch_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_POULTRY_Hatchery_REPORT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BATCH_ID", Batch_Id);
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
        public DataSet Get_FlockWiseFcr_Report(int Company_id, int Lob_Id, string Batch_Id, int? Location_Id,string from_date,string to_date)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_FLOCKWISE_FCR_REPORT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_id);
                    cmd.Parameters.AddWithValue("@LOB_ID", Lob_Id);
                    cmd.Parameters.AddWithValue("@BATCH_ID", Batch_Id);
                    cmd.Parameters.AddWithValue("@LOC_ID", Location_Id);
                    cmd.Parameters.AddWithValue("@FROM_DATE", from_date);
                    cmd.Parameters.AddWithValue("@TO_DATE", to_date);
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
        public DataSet Get_Cbf_Report(int Batch_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_CBF_REPORT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BATCH_ID", Batch_Id);
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
        public DataSet Get_Cbf_Report_Part2(int Batch_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_CBF_REPORT_PART2", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BATCH_ID", Batch_Id);
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
        public DataSet Get_feed_Report(int Company_Id,int Lob_Id,int Batch_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_POULTRY_FEED_REPORT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@LOB_ID", Lob_Id);
                    cmd.Parameters.AddWithValue("@Batch_ID", Batch_id);
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
        public DataSet Get_Slaughter_Report(int Company_Id, int Lob_Id,int Batch_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_POULTRY_SLAUGHTER_REPORT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@LOB_ID", Lob_Id);
                    cmd.Parameters.AddWithValue("@Batch_ID", Batch_id);
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
        public DataSet Get_Farm_Performance_Location_Report(int Company_Id,string batch_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_FARM_PERFORMANCE_LOCATION_REPORT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@Batch_id", batch_id);
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
        public DataSet Get_Farm_Performance_Wk_Report(int Company_Id,string Start_Date,string End_Date)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_FARM_PREFORMANCE_WK_REPORT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID",Company_Id);
                    cmd.Parameters.AddWithValue("@WeekStartdate", Start_Date);
                    cmd.Parameters.AddWithValue("@Weekenddate", End_Date);
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
        public DataSet Get_Batch_Output_Performance_Report(int Company_Id,string Start_Date, string End_Date)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_BATCH_OUTPUT_PREFORMANCE_REPORT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@WeekStartdate", Start_Date);
                    cmd.Parameters.AddWithValue("@Weekenddate", End_Date);
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
        public DataSet Get_Cattle_Report(int Batch_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_POULTRY_CATTLE_REPORT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BATCH_ID", Batch_Id);
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
        public DataSet Get_Cattle_Dairy_Report(string fromdate,string todate,string Batch_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_POULTRY_CATTLE_DAIRY_REPORT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fromdate", fromdate);
                    cmd.Parameters.AddWithValue("@todate", todate);
                    cmd.Parameters.AddWithValue("@BATCH_ID", Batch_Id);
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
        public DataSet Get_Animal_Drug_Report(int comapny_id,int lob_id,int Month, int Year, string Batch_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_ANIMAL_DRUG_REPORT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@company_id", comapny_id);
                    cmd.Parameters.AddWithValue("@lob_id", lob_id);
                    cmd.Parameters.AddWithValue("@Month", Month);
                    cmd.Parameters.AddWithValue("@Year", Year);
                    cmd.Parameters.AddWithValue("@BATCH_ID", Batch_Id);
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
        public DataSet Get_Feed_Usage_Report(int comapny_id, int lob_id, string fromdate, string todate, string Batch_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_FEED_USAGE_REPORT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@company_id", comapny_id);
                    cmd.Parameters.AddWithValue("@lob_id", lob_id);
                    cmd.Parameters.AddWithValue("@FromDate", fromdate);
                    cmd.Parameters.AddWithValue("@ToDate", todate);
                    cmd.Parameters.AddWithValue("@BATCH_ID", Batch_Id);
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
        
        public DataSet Get_Item_Stock_Report(int comapny_id,  string fromdate, string todate, int item_id,int loc_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("GetItem_Stock_Report", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@company_id", comapny_id);
                    cmd.Parameters.AddWithValue("@item_id", item_id);
                    cmd.Parameters.AddWithValue("@FromDate", fromdate);
                    cmd.Parameters.AddWithValue("@ToDate", todate);
                    cmd.Parameters.AddWithValue("@loc_id", loc_id);
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
        public DataSet Get_Item_Stock_Report_location_wise(int comapny_id, string fromdate, string todate, int item_id, int loc_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("GetItem_Stock_Report_LocationWise", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@company_id", comapny_id);
                    cmd.Parameters.AddWithValue("@item_id", item_id);
                    cmd.Parameters.AddWithValue("@FromDate", fromdate);
                    cmd.Parameters.AddWithValue("@ToDate", todate);
                    cmd.Parameters.AddWithValue("@loc_id", loc_id);
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
        public DataSet Get_Aqua_rearing_report(int comapny_id, int lob_id, int batch_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_AQUA_REARING_REPORT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@company_id", comapny_id);
                    cmd.Parameters.AddWithValue("@lob_id", lob_id);
                    cmd.Parameters.AddWithValue("@batch_id", batch_id);
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
        public DataSet Get_Aqua_rearing_growth_percentage_fcr_report(int comapny_id, int lob_id, int batch_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_Aqua_Rearing_Growth_pr_fcr", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@company_id", comapny_id);
                    cmd.Parameters.AddWithValue("@lob_id", lob_id);
                    cmd.Parameters.AddWithValue("@batch_id", batch_id);
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
        public DataSet Get_Aqua_FeedPlanning_report(int comapny_id, int batch_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_AQUA_FEED_FORECAST_REPORT ", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_id", comapny_id);
                    cmd.Parameters.AddWithValue("@Batch_ID", batch_id);
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
        public DataSet Get_Piggery_Report(int Batch_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_PIGGERY_REPORT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BATCH_ID", Batch_Id);
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

        public DataSet Get_Piggery_Report_Graph(int Batch_Id, int type)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_PIGGERY_REPORT_Graph", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BATCH_ID", Batch_Id);
                    cmd.Parameters.AddWithValue("@TYPE", type);
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
        public DataSet Get_Agri_Report(int Batch_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_AGRI_REPORT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BATCH_ID", Batch_Id);
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
        public DataSet Get_Agri_Summ_Report(int Batch_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_AGRI_SUMMARY", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Batch_Id);
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

        public DataSet Get_CBR_KPI_Report(int company_id, int Batch_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_CBR_KPI_GRAPH", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Batch_ID", Batch_Id);
                    cmd.Parameters.AddWithValue("@Company_ID", company_id);
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
        public DataSet Get_CBR_Monitring_Report(int company_id, int Batch_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_CBF_MONITORING_GRAPH", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Batch_ID", Batch_Id);
                    cmd.Parameters.AddWithValue("@Company_ID", company_id);
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
        public DataSet get_laying_feed_kpi_report(int company_id, int Batch_Id, int type)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();

                if (type==1 || type == 2|| type == 3 || type == 4)
                {
                    string proc_name = "";
                    if (type == 1){ proc_name = "USP_Laying_FEED_KPI_GRAPH";}  // for feed line graph
                    else if (type == 2) { proc_name = "USP_Laying_AW_KPI_GRAPH"; } // for avg wt line graph
                    else if (type == 3) { proc_name = "PRC_Report_LAYING__Mortality_GRAPH"; } // for mortality bar graph
                    else if (type == 4) { proc_name = "USP_Laying_OnsetLay_Graph"; } // for OnSet of Lay graph
                    using (SqlCommand cmd = new SqlCommand(proc_name, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Batch_ID", Batch_Id);
                        cmd.Parameters.AddWithValue("@Company_ID", company_id);
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);
                        con.Close();
                    }
                }

              
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataSet get_laying_feed_kpi_report_standard(int company_id, int Batch_Id, int type, int user_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();

                if (type == 1 || type == 2 || type == 3)
                {
                    string proc_name = "";
                    if (type == 1) { proc_name = "USP_Laying_FEED_KPI_GRAPH"; }  // for feed line graph
                    else if (type == 2) { proc_name = "USP_Laying_AW_KPI_GRAPH_Standard"; } // for avg wt line graph
                    else if (type == 3) { proc_name = "PRC_Report_LAYING__Mortality_GRAPH"; } // for mortality bar graph
                    using (SqlCommand cmd = new SqlCommand(proc_name, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Batch_ID", Batch_Id);
                        cmd.Parameters.AddWithValue("@Company_ID", company_id);
                        if (type == 2)
                            cmd.Parameters.AddWithValue("@User_id", user_id);
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);
                        con.Close();
                    }
                }


            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }


        public DataTable get_aqua_dashbard_feed(int compay_id, int lob_id)
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("GET_AQUA_DASHBOARD_FEED", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@company_id", compay_id);
                    cmd.Parameters.AddWithValue("@lob_id", lob_id);
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

        public DataTable GET_Aqua_growth_percentage_fcr_dashboard(int compay_id)
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_AQUA_REARING_FCR_GRAPH", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_id", compay_id);
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
        public DataTable GET_Aqua_dashboard_running_cost(int compay_id)
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_DASHBOARD_AQUA_RUNNING_COST", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_id", compay_id);
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
        public DataSet Get_Dashboard_Hatch_Output_Graph(int batch_id)
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


        public DataTable GET_Descriptive_report(int batch_id)
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_DATAENTRY_Descriptive_report", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BATCH_ID", batch_id);
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

        public DataTable GET_Inventory_summary_report(int company_id, int location_id, int item_id,string transfer_type,string fromDate,string toDate)
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_INVENTORY_SUMMARY_REPORT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@company_id", company_id);
                    cmd.Parameters.AddWithValue("@item_id", item_id);
                    cmd.Parameters.AddWithValue("@location_id", location_id);
                    cmd.Parameters.AddWithValue("@transfer_type", transfer_type);
                    cmd.Parameters.AddWithValue("@from_date", fromDate);
                    cmd.Parameters.AddWithValue("@to_date", toDate);
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

        public DataSet Get_Stock_Valuation_report(int compamy_id, int batch_id, string from_date, string to_date)
        {
            DataSet dt = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_Get_Stock_Valuation_report", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@company_id", compamy_id);
                    cmd.Parameters.AddWithValue("@Batch_id", batch_id);
                    cmd.Parameters.AddWithValue("@start_date", from_date);
                    cmd.Parameters.AddWithValue("@end_date", to_date);
                    cmd.CommandTimeout = 160;
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

        public DataSet Get_slaughtering_chart(int compamy_id, int batch_id, int type)
        {
            DataSet dt = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_POULTRY_SLAUGHTER_CHART", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@company_id", compamy_id);
                    cmd.Parameters.AddWithValue("@Batch_id", batch_id);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.CommandTimeout = 160;
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

        public DataSet Get_item_location_stock_report(int compamy_id, string item_id, string location_id)
        {
            DataSet dt = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("prc_get_item_loc_stock_report", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@company_id", compamy_id);
                 
                    cmd.Parameters.AddWithValue("@item_id", item_id);
                    cmd.Parameters.AddWithValue("@location_id", location_id);
                    cmd.CommandTimeout = 160;
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
        public DataSet Get_Transfer_Summary_report(int compamy_id, string from_date, string to_Date)
        {
            DataSet dt = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_GET_TRANSFER_SUMMARY_REPORT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@company_id", compamy_id);

                    cmd.Parameters.AddWithValue("@from_date", from_date);
                    cmd.Parameters.AddWithValue("@to_date", to_Date);
                    cmd.CommandTimeout = 160;
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
        
        public DataSet Get_Inventory_Stock_Report(int comapny_id, string fromdate, string todate, int item_id, string loc_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("GetInventory_Stock_Report", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@company_id", comapny_id);
                    cmd.Parameters.AddWithValue("@item_id", item_id);
                    cmd.Parameters.AddWithValue("@FromDate", fromdate);
                    cmd.Parameters.AddWithValue("@ToDate", todate);
                    cmd.Parameters.AddWithValue("@loc_id", loc_id);
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
        public DataSet Get_Ledger_Stock_Report(int comapny_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("GetLedger_Stock_Report", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@company_id", comapny_id);
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

        public DataSet Get_Transacation_Ledger_Report(int comapny_id ,int user_id, int nob_id, string location, string batch, string item, string item_category, string transfer_type)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("GET_Transacation_Ledger_Report", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@company_id", comapny_id);
                    cmd.Parameters.AddWithValue("@user_id", user_id);
                    cmd.Parameters.AddWithValue("@nature_id", nob_id);
                    cmd.Parameters.AddWithValue("@filter_location", location);
                    cmd.Parameters.AddWithValue("@filter_batch", batch);
                    cmd.Parameters.AddWithValue("@filter_item", item);
                    cmd.Parameters.AddWithValue("@filter_item_category", item_category);
                    cmd.Parameters.AddWithValue("@filter_transfer_type", transfer_type);
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
        public DataSet Get_feed_forecast_report(int batch_id, int item_id, string from_date, string to_date)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_FEED_FORECAST_REPORT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Batch_ID", batch_id);
                    cmd.Parameters.AddWithValue("@Item_ID", item_id);
                    cmd.Parameters.AddWithValue("@From_Date", from_date);
                    cmd.Parameters.AddWithValue("@To_Date", to_date);

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
        public DataSet Get_Transactions_Apply_Entry_Report(int comapny_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_Get_Transactions_apply_entry_report", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@company_id", comapny_id);
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
        public DataSet Get_Trace_Report(int comapny_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_TRACEBILITY_Report", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_id", comapny_id);
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

        public DataSet Get_Item_Stock_Summary_Report(int comapny_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_ITEM_STOCK_SUMMARY", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_id", comapny_id);
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

        public DataSet Get_BreedAttribute_Report(int comapny_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_GET_BreedAttribute_report", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_id", comapny_id);
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

       
        
    }
}
