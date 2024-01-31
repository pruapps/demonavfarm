using FarmIT_Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FarmIT_Api.database_accesslayer
{
    public interface IDataentryDB{
        
        string Insert_DataEntry(DataTable Header, DataTable Line,DataTable Livestock);
        string Insert_Transfer(DataTable Header, DataTable Line, DataTable livestock);
        string Update_DataEntry(DataTable Line,DataTable LiveStock);
        string Insert_DataEntry_Adjustment(DataTable Line, DataTable LiveStock);
        DataTable Get_DataEntry_Summary(string Nature_Id,int Company_Id,string Location_Id);
        DataTable Get_Transfer_Summary(int Company_Id, string Location_Id,string status);
        DataTable Get_BPIL_UOM_List();
        DataTable Get_DataEntry_History(int Company_Id, int NOB_Id, int LOB_Id, int Batch_Id, string Month_Year);
        DataSet Get_DataEntry_Details(int Batch_Id);
        DataSet Get_Livestock_Details(int Batch_Id);
        DataSet Get_Transfer_Details(int Transfer_id);
        DataSet Get_DataEntry_Details_JSON(int Batch_Id);
        DataSet Get_DataEntry_Details_All(int Comp_id);
        DataSet Get_DataEntry_History_Details(int Dataentry_id);
        DataSet Get_DataEntry_Adjustment_Details(int Dataentry_id);

        DataSet Get_DataEntry_Poultry_Details_Bulk(int company_id, int lob_id, int location_id, string posting_date);
        string Validate_Dataentry(int dataentry_id,int batch_id,int company_id,int user_id,string status,DateTime posting_date);
        decimal Remaining_Qty(int Batch_Id);
        
        string GetBatch_LocationCode(int Batch_Id);

        void InsertDataentryTransfer(int dataentry_id);

    }
    public class DataentryDB:IDataentryDB
    {
        public readonly string constr;

        private SqlConnection con;
        
        public DataentryDB(string connectionString)
        {
            constr = AesOperation.DecryptString(connectionString);
        }
        private void connection()
        {
            con = new SqlConnection(constr);
        }

        public string Validate_Dataentry(int dataentry_id,int batch_id, int company_id, int user_id, string status, DateTime posting_date)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_VALIDATE_DATAENTRY", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@DATAENTRY_ID", dataentry_id);
                    cmd.Parameters.AddWithValue("@BATCH_ID", batch_id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", company_id);
                    cmd.Parameters.AddWithValue("@USER_ID", user_id);
                    cmd.Parameters.AddWithValue("@STATUS", status);
                    cmd.Parameters.AddWithValue("@POSTING_DATE", posting_date);

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

        public string Insert_DataEntry(DataTable dt_header, DataTable dt_line,DataTable dt_livestock)
        {
            string res = "";
            int dataEntryID = 0;
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_INSERT_DATAENTRY", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@tbl_header", dt_header);
                    cmd.Parameters.AddWithValue("@tbl_line", dt_line);
                    cmd.Parameters.AddWithValue("@tbl_livestock", dt_livestock);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@Message";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 200;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);

                    SqlParameter outData1 = new SqlParameter();
                    outData1.ParameterName = "@DATAENTRY_ID_OUT";
                    outData1.SqlDbType = SqlDbType.Int;
                    outData1.Size = 200;
                    outData1.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData1);

                    con.Open();
                    res = cmd.ExecuteNonQuery().ToString();
                    res = Convert.ToString(outData.Value);
                    dataEntryID = Convert.ToInt32(outData1.Value);
                    con.Close();
                }
                if (dt_header.Rows[0]["STATUS"].ToString() == "posted")
                {
                    InsertDataentryTransfer(dataEntryID);
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public void InsertDataentryTransfer(int dataentry_id)
        {
            try
            {
                DataSet ds = new DataSet();
                connection();
                using (SqlCommand cmd = new SqlCommand("GET_DATAENTRY_STOCK", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@DATAENTRY_ID", dataentry_id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }

                double qty = 0; double remqty = 0;
                DataTable headerTblConsuption = new DataTable("TBL_TRANSFER_HEADER");
                headerTblConsuption.Columns.Add("TRANSFER_ID", typeof(int));
                headerTblConsuption.Columns.Add("TRANSFER_TYPE", typeof(string));
                headerTblConsuption.Columns.Add("TRANSFER_DATE", typeof(DateTime));
                headerTblConsuption.Columns.Add("LOCATION_ID", typeof(int));
                headerTblConsuption.Columns.Add("LOCATION_FROM", typeof(int));
                headerTblConsuption.Columns.Add("LOCATION_TO", typeof(int));
                headerTblConsuption.Columns.Add("BATCH_FROM", typeof(int));
                headerTblConsuption.Columns.Add("BATCH_TO", typeof(int));
                headerTblConsuption.Columns.Add("REMARKS", typeof(string));
                headerTblConsuption.Columns.Add("COMPANY_ID", typeof(int));
                headerTblConsuption.Columns.Add("CREATED_BY", typeof(int));
                headerTblConsuption.Columns.Add("DATAENTRY_ID", typeof(int));

                DataTable headerTblIN = new DataTable("TBL_TRANSFER_HEADER");
                headerTblIN.Columns.Add("TRANSFER_ID", typeof(int));
                headerTblIN.Columns.Add("TRANSFER_TYPE", typeof(string));
                headerTblIN.Columns.Add("TRANSFER_DATE", typeof(DateTime));
                headerTblIN.Columns.Add("LOCATION_ID", typeof(int));
                headerTblIN.Columns.Add("LOCATION_FROM", typeof(int));
                headerTblIN.Columns.Add("LOCATION_TO", typeof(int));
                headerTblIN.Columns.Add("BATCH_FROM", typeof(int));
                headerTblIN.Columns.Add("BATCH_TO", typeof(int));
                headerTblIN.Columns.Add("REMARKS", typeof(string));
                headerTblIN.Columns.Add("COMPANY_ID", typeof(int));
                headerTblIN.Columns.Add("CREATED_BY", typeof(int));
                headerTblIN.Columns.Add("DATAENTRY_ID", typeof(int));

                var comp_id = 0; var data_entryID = 0;
                DataRow dr = null; DataRow dr1 = null;
                dr = headerTblConsuption.NewRow();
                dr1 = headerTblIN.NewRow();
              
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow drh = ds.Tables[0].Rows[0];
                    comp_id =  Convert.ToInt32(drh["COMPANY_ID"]);
                    data_entryID = Convert.ToInt32(drh["DATAENTRY_ID"]);
                    dr["TRANSFER_ID"] = 0;
                    dr["COMPANY_ID"] = drh["COMPANY_ID"];
                    dr["LOCATION_ID"] = drh["USER_LOCATION"];
                    dr["LOCATION_FROM"] = drh["LOCATION_ID"];
                    dr["LOCATION_TO"] = drh["LOCATION_ID"];
                    dr["TRANSFER_TYPE"] = "CONSUMPTION";
                    dr["BATCH_FROM"] = drh["BATCH_ID"];
                    dr["BATCH_TO"] = 0;
                    dr["TRANSFER_DATE"] = drh["POSTING_DATE"];
                    dr["CREATED_BY"] = drh["CREATED_BY"];
                    dr["REMARKS"] = drh["REMARK"];
                    dr["DATAENTRY_ID"] = drh["DATAENTRY_ID"];
                    headerTblConsuption.Rows.Add(dr);


                    dr1["TRANSFER_ID"] = 0;
                    dr1["COMPANY_ID"] = drh["COMPANY_ID"];
                    dr1["LOCATION_ID"] = drh["USER_LOCATION"];
                    dr1["LOCATION_FROM"] = drh["LOCATION_ID"];
                    dr1["LOCATION_TO"] = drh["LOCATION_ID"];
                    dr1["TRANSFER_TYPE"] = "IN";
                    dr1["BATCH_FROM"] = drh["BATCH_ID"];
                    dr1["BATCH_TO"] = 0;
                    dr1["TRANSFER_DATE"] = drh["POSTING_DATE"];
                    dr1["CREATED_BY"] = drh["CREATED_BY"];
                    dr1["REMARKS"] = drh["REMARK"];
                    dr1["DATAENTRY_ID"] = drh["DATAENTRY_ID"];
                    headerTblIN.Rows.Add(dr1);

                    qty = Convert.ToDouble(drh["OPENING_QUANTITY"].ToString());
                    remqty = Convert.ToDouble(drh["REMAINING_QTY"].ToString());

                    //////////////////////Line///////////////////////////

                    DataTable lineTblConsuption = new DataTable("TBL_TRANSFER_LINE");
                    lineTblConsuption.Columns.Add("LINE_ID", typeof(int));
                    lineTblConsuption.Columns.Add("TRANSFER_ID", typeof(int));
                    lineTblConsuption.Columns.Add("ITEM_ID", typeof(string));
                    lineTblConsuption.Columns.Add("UOM", typeof(string));
                    lineTblConsuption.Columns.Add("QUANTITY", typeof(decimal));
                    lineTblConsuption.Columns.Add("UNIT_COST", typeof(decimal));
                    lineTblConsuption.Columns.Add("REMAINING_QTY", typeof(decimal));
                    lineTblConsuption.Columns.Add("DATAENTRY_ID", typeof(int));
                    lineTblConsuption.Columns.Add("DATAENTRY_LINE_ID", typeof(int));
                    lineTblConsuption.Columns.Add("DEAD_ON_ARRIVAL", typeof(decimal));
                    lineTblConsuption.Columns.Add("WEIGH_SCALE", typeof(decimal));
                    lineTblConsuption.Columns.Add("ADJ_LINE_ID", typeof(int));
                    lineTblConsuption.Columns.Add("BATCH_FROM", typeof(int));
                    lineTblConsuption.Columns.Add("BATCH_TO", typeof(int));
                    lineTblConsuption.Columns.Add("LOT_NO", typeof(string));

                    DataTable lineTblIN = new DataTable("TBL_TRANSFER_LINE");
                    lineTblIN.Columns.Add("LINE_ID", typeof(int));
                    lineTblIN.Columns.Add("TRANSFER_ID", typeof(int));
                    lineTblIN.Columns.Add("ITEM_ID", typeof(string));
                    lineTblIN.Columns.Add("UOM", typeof(string));
                    lineTblIN.Columns.Add("QUANTITY", typeof(decimal));
                    lineTblIN.Columns.Add("UNIT_COST", typeof(decimal));
                    lineTblIN.Columns.Add("REMAINING_QTY", typeof(decimal));
                    lineTblIN.Columns.Add("DATAENTRY_ID", typeof(int));
                    lineTblIN.Columns.Add("DATAENTRY_LINE_ID", typeof(int));
                    lineTblIN.Columns.Add("DEAD_ON_ARRIVAL", typeof(decimal));
                    lineTblIN.Columns.Add("WEIGH_SCALE", typeof(decimal));
                    lineTblIN.Columns.Add("ADJ_LINE_ID", typeof(int));
                    lineTblIN.Columns.Add("BATCH_FROM", typeof(int));
                    lineTblIN.Columns.Add("BATCH_TO", typeof(int));
                    lineTblIN.Columns.Add("LOT_NO", typeof(string));

                    DataRow drline = null;
                    DataTable dtstock = ds.Tables[2];
                    DataRow[] dtConsuption = ds.Tables[1].Select("PARAMETER_TYPE_ID='1'");
                    DataRow[] dtIN = ds.Tables[1].Select("PARAMETER_TYPE_ID<>'1'");
                    foreach (DataRow pr in dtConsuption)
                    {
                        DataRow[] stkrow = dtstock.Select("LINE_ID = '" + pr["LINE_ID"].ToString() + "'");
                        if (stkrow.Length > 0)
                        {
                            double getActualValue = Convert.ToDouble(pr["ACTUAL_VALUE"].ToString());
                            int flag = 1;
                            foreach (DataRow stk in stkrow)
                            {
                                /*
                                if (flag == 1 && getActualValue <= (Convert.ToDouble(stk["Remaining_Qty"].ToString())))
                                {
                                    flag = 0;
                                    // rem=5000,actual=4000then remailqtyline=1000 or qty=0
                                    drline = lineTblConsuption.NewRow();
                                    drline["LINE_ID"] = 0;
                                    drline["TRANSFER_ID"] = 0;
                                    drline["ITEM_ID"] = pr["ITEM_ID"];
                                    drline["UOM"] = pr["UOM"];
                                    drline["QUANTITY"] = getActualValue;
                                    drline["UNIT_COST"] = stk["Unit_Cost"].ToString();
                                    drline["REMAINING_QTY"] = (Convert.ToDouble(stk["Remaining_Qty"].ToString()))- getActualValue;
                                    drline["DATAENTRY_ID"] = pr["DATAENTRY_ID"];
                                    drline["DATAENTRY_LINE_ID"] = pr["LINE_ID"];
                                    lineTblConsuption.Rows.Add(drline);
                                }
                                else if (flag == 1)
                                {
                                    // rem=5000,actual=6000then remailqtyline=1000 or qty=0
                                    getActualValue = getActualValue - (Convert.ToDouble(stk["Remaining_Qty"].ToString()));
                                    drline = lineTblConsuption.NewRow();
                                    drline["LINE_ID"] = 0;
                                    drline["TRANSFER_ID"] = 0;
                                    drline["ITEM_ID"] = pr["ITEM_ID"];
                                    drline["UOM"] = pr["UOM"];
                                    drline["QUANTITY"] = (Convert.ToDouble(stk["Remaining_Qty"].ToString()));
                                    drline["UNIT_COST"] = stk["Unit_Cost"].ToString();
                                    drline["REMAINING_QTY"] = 0;
                                    drline["DATAENTRY_ID"] = pr["DATAENTRY_ID"];
                                    drline["DATAENTRY_LINE_ID"] = pr["LINE_ID"];
                                    lineTblConsuption.Rows.Add(drline);
                                }
                                */
                                drline = lineTblConsuption.NewRow();
                                drline["LINE_ID"] = 0;
                                drline["TRANSFER_ID"] = 0;
                                drline["ITEM_ID"] = pr["ITEM_ID"];
                                drline["UOM"] = pr["UOM"];
                                drline["QUANTITY"] = getActualValue;
                                drline["UNIT_COST"] = stk["Unit_Cost"].ToString();
                                drline["REMAINING_QTY"] = 0;
                                //drline["REMAINING_QTY"] = (Convert.ToDouble(stk["Remaining_Qty"].ToString())) - getActualValue;
                                drline["DATAENTRY_ID"] = pr["DATAENTRY_ID"];
                                drline["DATAENTRY_LINE_ID"] = pr["LINE_ID"];
                                lineTblConsuption.Rows.Add(drline);
                            }
                        }
                        //else
                        //{
                        //    drline = lineTblConsuption.NewRow();
                        //    drline["LINE_ID"] = 0;
                        //    drline["TRANSFER_ID"] = 0;
                        //    drline["ITEM_ID"] = pr["ITEM_ID"];
                        //    drline["UOM"] = pr["UOM"];
                        //    drline["QUANTITY"] = qty;
                        //    drline["UNIT_COST"] = pr["UNIT_COST"].ToString();
                        //    drline["REMAINING_QTY"] = remqty;
                        //    drline["DATAENTRY_ID"] = pr["DATAENTRY_ID"];
                        //    drline["DATAENTRY_LINE_ID"] = pr["LINE_ID"];
                        //    lineTblConsuption.Rows.Add(drline);

                        //}



                    }

                    foreach (DataRow pr in dtIN)
                    {
                        DataRow[] stkrow = dtstock.Select("LINE_ID = '" + pr["LINE_ID"].ToString() + "'");
                        if (stkrow.Length > 0)
                        {
                            double getActualValue = Convert.ToDouble(pr["ACTUAL_VALUE"].ToString());
                            int flag = 1;
                            foreach (DataRow stk in stkrow)
                            {


                                //if (flag == 1 && getActualValue <= (Convert.ToDouble(stk["Remaining_Qty"].ToString())))
                                //{
                                //    flag = 0;

                                //    drline = lineTblIN.NewRow();
                                //    drline["LINE_ID"] = 0;
                                //    drline["TRANSFER_ID"] = 0;
                                //    drline["ITEM_ID"] = pr["ITEM_ID"];
                                //    drline["UOM"] = pr["UOM"];
                                //    drline["QUANTITY"] = getActualValue;
                                //    drline["UNIT_COST"] = stk["Unit_Cost"].ToString();
                                //    drline["REMAINING_QTY"] = (Convert.ToDouble(stk["Remaining_Qty"].ToString())) - getActualValue;
                                //    drline["DATAENTRY_ID"] = pr["DATAENTRY_ID"];
                                //    drline["DATAENTRY_LINE_ID"] = pr["LINE_ID"];
                                //    lineTblIN.Rows.Add(drline);
                                //}
                                //else if (flag == 1)
                                //{


                                //    getActualValue = (getActualValue - (Convert.ToDouble(stk["Remaining_Qty"].ToString())));

                                //    drline = lineTblIN.NewRow();
                                //    drline["LINE_ID"] = 0;
                                //    drline["TRANSFER_ID"] = 0;
                                //    drline["ITEM_ID"] = pr["ITEM_ID"];
                                //    drline["UOM"] = pr["UOM"];
                                //    drline["QUANTITY"] = (Convert.ToDouble(stk["Remaining_Qty"].ToString()));
                                //    drline["UNIT_COST"] = stk["Unit_Cost"].ToString();
                                //    drline["REMAINING_QTY"] = 0;
                                //    drline["DATAENTRY_ID"] = pr["DATAENTRY_ID"];
                                //    drline["DATAENTRY_LINE_ID"] = pr["LINE_ID"];
                                //    lineTblIN.Rows.Add(drline);
                                //}

                                drline = lineTblIN.NewRow();
                                drline["LINE_ID"] = 0;
                                drline["TRANSFER_ID"] = 0;
                                drline["ITEM_ID"] = pr["ITEM_ID"];
                                drline["UOM"] = pr["UOM"];
                                drline["QUANTITY"] = getActualValue;
                                drline["UNIT_COST"] = stk["Unit_Cost"].ToString();
                                drline["REMAINING_QTY"] = 0;
                               // drline["REMAINING_QTY"] = (Convert.ToDouble(stk["Remaining_Qty"].ToString())) - getActualValue;
                                drline["DATAENTRY_ID"] = pr["DATAENTRY_ID"];
                                drline["DATAENTRY_LINE_ID"] = pr["LINE_ID"];
                                lineTblIN.Rows.Add(drline);
                            }
                        }
                        //else
                        //{

                        //    drline = lineTblIN.NewRow();
                        //    drline["LINE_ID"] = 0;
                        //    drline["TRANSFER_ID"] = 0;
                        //    drline["ITEM_ID"] = pr["ITEM_ID"];
                        //    drline["UOM"] = pr["UOM"];
                        //    drline["QUANTITY"] = qty;
                        //    drline["UNIT_COST"] = pr["UNIT_COST"].ToString();
                        //    drline["REMAINING_QTY"] = remqty;
                        //    drline["DATAENTRY_ID"] = pr["DATAENTRY_ID"];
                        //    drline["DATAENTRY_LINE_ID"] = pr["LINE_ID"];
                        //    lineTblIN.Rows.Add(drline);
                        //}



                    }

                    DataTable livestockTable = new DataTable("TBL_TRANSFER_LINE_Livestock");
                    livestockTable.Columns.Add("Livestock_No", typeof(string));
                    livestockTable.Columns.Add("ITEM_ID", typeof(int));
                    livestockTable.Columns.Add("Total_Units", typeof(decimal));

                    if (lineTblConsuption.Rows.Count > 0)
                    {
                        string r1 = (Insert_Transfer(headerTblConsuption, lineTblConsuption, livestockTable));
                    }
                    if (lineTblIN.Rows.Count > 0)
                    {
                        string r2 = (Insert_Transfer(headerTblIN, lineTblIN, livestockTable));
                    }

                    try
                    {
                        if (lineTblConsuption.Rows.Count > 0 || lineTblIN.Rows.Count > 0)
                        {
                            soapApiInitialize soa = new soapApiInitialize();
                         //   soa.Call_postitem(data_entryID, comp_id);

                        }
                    }
                    catch {}
                   

                    ////////////////////end Line/////////////////////////

                }



            }

            catch { }

        }

        public void InsertDataentryTransfer_Adjustment(int dataentry_id)
        {
            //dataentry_id this dataentryAdjustment id
            try
            {
                DataSet ds = new DataSet();
                connection();
                using (SqlCommand cmd = new SqlCommand("GET_DATAENTRY_STOCK_ADJUSTMENT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@DATAENTRY_ADJ_ID", dataentry_id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }

                double qty = 0; double remqty = 0;
                DataTable headerTblConsuption = new DataTable("TBL_TRANSFER_HEADER");
                headerTblConsuption.Columns.Add("TRANSFER_ID", typeof(int));
                headerTblConsuption.Columns.Add("TRANSFER_TYPE", typeof(string));
                headerTblConsuption.Columns.Add("TRANSFER_DATE", typeof(DateTime));
                headerTblConsuption.Columns.Add("LOCATION_ID", typeof(int));
                headerTblConsuption.Columns.Add("LOCATION_FROM", typeof(int));
                headerTblConsuption.Columns.Add("LOCATION_TO", typeof(int));
                headerTblConsuption.Columns.Add("BATCH_FROM", typeof(int));
                headerTblConsuption.Columns.Add("BATCH_TO", typeof(int));
                headerTblConsuption.Columns.Add("REMARKS", typeof(string));
                headerTblConsuption.Columns.Add("COMPANY_ID", typeof(int));
                headerTblConsuption.Columns.Add("CREATED_BY", typeof(int));
                headerTblConsuption.Columns.Add("DATAENTRY_ID", typeof(int));

                DataTable headerTblIN = new DataTable("TBL_TRANSFER_HEADER");
                headerTblIN.Columns.Add("TRANSFER_ID", typeof(int));
                headerTblIN.Columns.Add("TRANSFER_TYPE", typeof(string));
                headerTblIN.Columns.Add("TRANSFER_DATE", typeof(DateTime));
                headerTblIN.Columns.Add("LOCATION_ID", typeof(int));
                headerTblIN.Columns.Add("LOCATION_FROM", typeof(int));
                headerTblIN.Columns.Add("LOCATION_TO", typeof(int));
                headerTblIN.Columns.Add("BATCH_FROM", typeof(int));
                headerTblIN.Columns.Add("BATCH_TO", typeof(int));
                headerTblIN.Columns.Add("REMARKS", typeof(string));
                headerTblIN.Columns.Add("COMPANY_ID", typeof(int));
                headerTblIN.Columns.Add("CREATED_BY", typeof(int));
                headerTblIN.Columns.Add("DATAENTRY_ID", typeof(int));

                var comp_id = 0; 
                DataRow dr = null; DataRow dr1 = null;
                dr = headerTblConsuption.NewRow();
                dr1 = headerTblIN.NewRow();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow drh = ds.Tables[0].Rows[0];
                    comp_id = Convert.ToInt32(drh["COMPANY_ID"]);
                  //  data_entryID = Convert.ToInt32(drh["DATAENTRY_ID"]);
                    dr["TRANSFER_ID"] = 0;
                    dr["COMPANY_ID"] = drh["COMPANY_ID"];
                    dr["LOCATION_ID"] = drh["USER_LOCATION"];
                    dr["LOCATION_FROM"] = drh["LOCATION_ID"];
                    dr["LOCATION_TO"] = drh["LOCATION_ID"];
                    dr["TRANSFER_TYPE"] = "CONSUMPTION";
                    dr["BATCH_FROM"] = drh["BATCH_ID"];
                    dr["BATCH_TO"] = 0;
                    dr["TRANSFER_DATE"] = drh["POSTING_DATE"];
                    dr["CREATED_BY"] = drh["CREATED_BY"];
                    dr["REMARKS"] = drh["REMARK"];
                    dr["DATAENTRY_ID"] = drh["DATAENTRY_ID"];
                    headerTblConsuption.Rows.Add(dr);


                    dr1["TRANSFER_ID"] = 0;
                    dr1["COMPANY_ID"] = drh["COMPANY_ID"];
                    dr1["LOCATION_ID"] = drh["USER_LOCATION"];
                    dr1["LOCATION_FROM"] = drh["LOCATION_ID"];
                    dr1["LOCATION_TO"] = drh["LOCATION_ID"];
                    dr1["TRANSFER_TYPE"] = "IN";
                    dr1["BATCH_FROM"] = drh["BATCH_ID"];
                    dr1["BATCH_TO"] = 0;
                    dr1["TRANSFER_DATE"] = drh["POSTING_DATE"];
                    dr1["CREATED_BY"] = drh["CREATED_BY"];
                    dr1["REMARKS"] = drh["REMARK"];
                    dr1["DATAENTRY_ID"] = drh["DATAENTRY_ID"];
                    headerTblIN.Rows.Add(dr1);

                    qty = Convert.ToDouble(drh["OPENING_QUANTITY"].ToString());
                    remqty = Convert.ToDouble(drh["REMAINING_QTY"].ToString());

                    //////////////////////Line///////////////////////////

                    DataTable lineTblConsuption = new DataTable("TBL_TRANSFER_LINE");
                    lineTblConsuption.Columns.Add("LINE_ID", typeof(int));
                    lineTblConsuption.Columns.Add("TRANSFER_ID", typeof(int));
                    lineTblConsuption.Columns.Add("ITEM_ID", typeof(string));
                    lineTblConsuption.Columns.Add("UOM", typeof(string));
                    lineTblConsuption.Columns.Add("QUANTITY", typeof(decimal));
                    lineTblConsuption.Columns.Add("UNIT_COST", typeof(decimal));
                    lineTblConsuption.Columns.Add("REMAINING_QTY", typeof(decimal));
                    lineTblConsuption.Columns.Add("DATAENTRY_ID", typeof(int));
                    lineTblConsuption.Columns.Add("DATAENTRY_LINE_ID", typeof(int));
                    lineTblConsuption.Columns.Add("DEAD_ON_ARRIVAL", typeof(decimal));
                    lineTblConsuption.Columns.Add("WEIGH_SCALE", typeof(decimal));
                    lineTblConsuption.Columns.Add("ADJ_LINE_ID", typeof(int));
                    lineTblConsuption.Columns.Add("BATCH_FROM", typeof(int));
                    lineTblConsuption.Columns.Add("BATCH_TO", typeof(int));
                    lineTblConsuption.Columns.Add("LOT_NO", typeof(string));

                    DataTable lineTblIN = new DataTable("TBL_TRANSFER_LINE");
                    lineTblIN.Columns.Add("LINE_ID", typeof(int));
                    lineTblIN.Columns.Add("TRANSFER_ID", typeof(int));
                    lineTblIN.Columns.Add("ITEM_ID", typeof(string));
                    lineTblIN.Columns.Add("UOM", typeof(string));
                    lineTblIN.Columns.Add("QUANTITY", typeof(decimal));
                    lineTblIN.Columns.Add("UNIT_COST", typeof(decimal));
                    lineTblIN.Columns.Add("REMAINING_QTY", typeof(decimal));
                    lineTblIN.Columns.Add("DATAENTRY_ID", typeof(int));
                    lineTblIN.Columns.Add("DATAENTRY_LINE_ID", typeof(int));
                    lineTblIN.Columns.Add("DEAD_ON_ARRIVAL", typeof(decimal));
                    lineTblIN.Columns.Add("WEIGH_SCALE", typeof(decimal));
                    lineTblIN.Columns.Add("ADJ_LINE_ID", typeof(int));
                    lineTblIN.Columns.Add("BATCH_FROM", typeof(int));
                    lineTblIN.Columns.Add("BATCH_TO", typeof(int));
                    lineTblIN.Columns.Add("LOT_NO", typeof(string));

                    DataRow drline = null;
                    DataTable dtstock = ds.Tables[2];
                    DataRow[] dtConsuption = ds.Tables[1].Select("PARAMETER_TYPE_ID='1'");
                    DataRow[] dtIN = ds.Tables[1].Select("PARAMETER_TYPE_ID<>'1'");
                    foreach (DataRow pr in dtConsuption)
                    {
                        DataRow[] stkrow = dtstock.Select("LINE_ID = '" + pr["LINE_ID"].ToString() + "'");
                        if (stkrow.Length > 0)
                        {
                            double getActualValue = Convert.ToDouble(pr["ACTUAL_VALUE"].ToString());
                            int flag = 1;
                            foreach (DataRow stk in stkrow)
                            {
                                if (flag == 1 && getActualValue <= (Convert.ToDouble(stk["Remaining_Qty"].ToString())))
                                {
                                    flag = 0;
                                    // rem=5000,actual=4000then remailqtyline=1000 or qty=0
                                    drline = lineTblConsuption.NewRow();
                                    drline["LINE_ID"] = 0;
                                    drline["TRANSFER_ID"] = 0;
                                    drline["ITEM_ID"] = pr["ITEM_ID"];
                                    drline["UOM"] = pr["UOM"];
                                    drline["QUANTITY"] = getActualValue;
                                    drline["UNIT_COST"] = stk["Unit_Cost"].ToString();
                                    drline["REMAINING_QTY"] = (Convert.ToDouble(stk["Remaining_Qty"].ToString())) - getActualValue;
                                    drline["DATAENTRY_ID"] = pr["DATAENTRY_ID"];
                                    drline["DATAENTRY_LINE_ID"] = pr["DATAENTRY_LINE_ID"];
                                    drline["ADJ_LINE_ID"] = pr["LINE_ID"];
                                    lineTblConsuption.Rows.Add(drline);
                                }
                                else if (flag == 1)
                                {
                                    // rem=5000,actual=6000then remailqtyline=1000 or qty=0
                                    getActualValue = getActualValue - (Convert.ToDouble(stk["Remaining_Qty"].ToString()));
                                    drline = lineTblConsuption.NewRow();
                                    drline["LINE_ID"] = 0;
                                    drline["TRANSFER_ID"] = 0;
                                    drline["ITEM_ID"] = pr["ITEM_ID"];
                                    drline["UOM"] = pr["UOM"];
                                    drline["QUANTITY"] = (Convert.ToDouble(stk["Remaining_Qty"].ToString()));
                                    drline["UNIT_COST"] = stk["Unit_Cost"].ToString();
                                    drline["REMAINING_QTY"] = 0;
                                    drline["DATAENTRY_ID"] = pr["DATAENTRY_ID"];
                                    drline["DATAENTRY_LINE_ID"] = pr["DATAENTRY_LINE_ID"];
                                    drline["ADJ_LINE_ID"] = pr["LINE_ID"];
                                    lineTblConsuption.Rows.Add(drline);
                                }
                            }
                        }
                        //else
                        //{
                        //    drline = lineTblConsuption.NewRow();
                        //    drline["LINE_ID"] = 0;
                        //    drline["TRANSFER_ID"] = 0;
                        //    drline["ITEM_ID"] = pr["ITEM_ID"];
                        //    drline["UOM"] = pr["UOM"];
                        //    drline["QUANTITY"] = qty;
                        //    drline["UNIT_COST"] = pr["UNIT_COST"].ToString();
                        //    drline["REMAINING_QTY"] = remqty;
                        //    drline["DATAENTRY_ID"] = pr["DATAENTRY_ID"];
                        //    drline["DATAENTRY_LINE_ID"] = pr["LINE_ID"];
                        //    lineTblConsuption.Rows.Add(drline);

                        //}



                    }

                    foreach (DataRow pr in dtIN)
                    {
                        DataRow[] stkrow = dtstock.Select("LINE_ID = '" + pr["LINE_ID"].ToString() + "'");
                        if (stkrow.Length > 0)
                        {
                            double getActualValue = Convert.ToDouble(pr["ACTUAL_VALUE"].ToString());
                            int flag = 1;
                            foreach (DataRow stk in stkrow)
                            {


                                if (flag == 1 && getActualValue <= (Convert.ToDouble(stk["Remaining_Qty"].ToString())))
                                {
                                    flag = 0;

                                    drline = lineTblIN.NewRow();
                                    drline["LINE_ID"] = 0;
                                    drline["TRANSFER_ID"] = 0;
                                    drline["ITEM_ID"] = pr["ITEM_ID"];
                                    drline["UOM"] = pr["UOM"];
                                    drline["QUANTITY"] = getActualValue;
                                    drline["UNIT_COST"] = stk["Unit_Cost"].ToString();
                                    drline["REMAINING_QTY"] = (Convert.ToDouble(stk["Remaining_Qty"].ToString())) - getActualValue;
                                    drline["DATAENTRY_ID"] = pr["DATAENTRY_ID"];
                                    drline["DATAENTRY_LINE_ID"] = pr["DATAENTRY_LINE_ID"];
                                    drline["ADJ_LINE_ID"] = pr["LINE_ID"];
                                    lineTblIN.Rows.Add(drline);
                                }
                                else if (flag == 1)
                                {


                                    getActualValue = (getActualValue - (Convert.ToDouble(stk["Remaining_Qty"].ToString())));

                                    drline = lineTblIN.NewRow();
                                    drline["LINE_ID"] = 0;
                                    drline["TRANSFER_ID"] = 0;
                                    drline["ITEM_ID"] = pr["ITEM_ID"];
                                    drline["UOM"] = pr["UOM"];
                                    drline["QUANTITY"] = (Convert.ToDouble(stk["Remaining_Qty"].ToString()));
                                    drline["UNIT_COST"] = stk["Unit_Cost"].ToString();
                                    drline["REMAINING_QTY"] = 0;
                                    drline["DATAENTRY_ID"] = pr["DATAENTRY_ID"];
                                    drline["DATAENTRY_LINE_ID"] = pr["DATAENTRY_LINE_ID"];
                                    drline["ADJ_LINE_ID"] = pr["LINE_ID"];
                                    lineTblIN.Rows.Add(drline);
                                }
                            }
                        }
                        //else
                        //{

                        //    drline = lineTblIN.NewRow();
                        //    drline["LINE_ID"] = 0;
                        //    drline["TRANSFER_ID"] = 0;
                        //    drline["ITEM_ID"] = pr["ITEM_ID"];
                        //    drline["UOM"] = pr["UOM"];
                        //    drline["QUANTITY"] = qty;
                        //    drline["UNIT_COST"] = pr["UNIT_COST"].ToString();
                        //    drline["REMAINING_QTY"] = remqty;
                        //    drline["DATAENTRY_ID"] = pr["DATAENTRY_ID"];
                        //    drline["DATAENTRY_LINE_ID"] = pr["LINE_ID"];
                        //    lineTblIN.Rows.Add(drline);
                        //}



                    }

                    DataTable livestockTable = new DataTable("TBL_TRANSFER_LINE_Livestock");
                    livestockTable.Columns.Add("Livestock_No", typeof(string));
                    livestockTable.Columns.Add("ITEM_ID", typeof(int));
                    livestockTable.Columns.Add("Total_Units", typeof(decimal));

                    if (lineTblConsuption.Rows.Count > 0)
                    {
                        string r1 = (Insert_Transfer(headerTblConsuption, lineTblConsuption, livestockTable));
                    }
                    if (lineTblIN.Rows.Count > 0)
                    {
                        string r2 = (Insert_Transfer(headerTblIN, lineTblIN, livestockTable));
                    }

                    try
                    {
                        if (lineTblConsuption.Rows.Count > 0 || lineTblIN.Rows.Count > 0)
                        {
                            soapApiInitialize soa = new soapApiInitialize();
                          //  soa.Call_postitem_adjustment(dataentry_id, comp_id);

                        }
                    }
                    catch { }


                    ////////////////////end Line/////////////////////////

                }



            }

            catch { }

        }
        public string Update_DataEntry(DataTable dt_line,DataTable dt_livestock)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_UPDATE_DATAENTRY", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tbl_line", dt_line);
                    cmd.Parameters.AddWithValue("@tbl_livestock", dt_livestock);
                    cmd.CommandTimeout = 5000;
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

        public string Insert_DataEntry_Adjustment(DataTable dt_line, DataTable dt_livestock)
        {
            string res = "";
            var dataEntryAdjID = 0;
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_INSERT_DATAENTRY_ADJUSTMENT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tbl_line", dt_line);
                    cmd.Parameters.AddWithValue("@tbl_livestock", dt_livestock);
                    cmd.CommandTimeout = 5000;
                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@Message";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 200;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);

                    SqlParameter outData1 = new SqlParameter();
                    outData1.ParameterName = "@DATAENTRY_ADJ_ID_OUT";
                    outData1.SqlDbType = SqlDbType.Int;
                    outData1.Size = 200;
                    outData1.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData1);
                    con.Open();
                    res = cmd.ExecuteNonQuery().ToString();
                    res = Convert.ToString(outData.Value);
                    dataEntryAdjID = Convert.ToInt32(outData1.Value);
                    con.Close();

                    if (dataEntryAdjID > 0)
                    {
                        InsertDataentryTransfer_Adjustment(dataEntryAdjID);
                    }
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        public DataTable Get_DataEntry_Summary(string Nature_Id, int Company_Id, string Location_Id)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_DATAENTRY_SUMMARY", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NATURE_ID", Nature_Id);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@LOCATION_ID", Location_Id);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return dt;
        }

        public DataTable Get_DataEntry_History(int Company_Id, int NOB_Id, int LOB_Id, int Batch_Id, string Month_Year)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_DATAENTRY_HISTORY", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@NOB_ID", NOB_Id);
                    cmd.Parameters.AddWithValue("@LOB_ID", LOB_Id);
                    cmd.Parameters.AddWithValue("@BATCH_ID", Batch_Id);
                    cmd.Parameters.AddWithValue("@MONTH_YEAR", Month_Year);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return dt;
        }

        public DataSet Get_DataEntry_Details(int Batch_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_DATAENTRY_DETAILS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BATCH_ID", Batch_Id);
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

        #region Dataentry Poultry
        public DataSet Get_DataEntry_Poultry_Details_Bulk(int company_id, int lob_id, int location_id, string posting_date)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_DATAENTRY_POLUTRY_DETAILS_BULK", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@company_id", company_id);
                    cmd.Parameters.AddWithValue("@lob_id", lob_id);
                    cmd.Parameters.AddWithValue("@location_id", location_id);
                    cmd.Parameters.AddWithValue("@FILTER_POSTING_DATE", posting_date);
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
        public DataSet Get_Livestock_Details(int Batch_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_GET_LIVESTOCK", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BATCH_ID", Batch_Id);
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
        public DataSet Get_DataEntry_Details_All(int Comp_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_DATAENTRY_DETAILS_ALL", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Comp_id", Comp_id);
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
        public DataSet Get_DataEntry_Details_JSON(int Batch_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_DATAENTRY_HISTORY_PIVOT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BATCH_ID", Batch_Id);
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

        public DataSet Get_DataEntry_History_Details(int Dataentry_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_DATAENTRY_HISTORY_DETAILS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DATAENTRY_ID", Dataentry_id);
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

        public DataSet Get_DataEntry_Adjustment_Details(int Dataentry_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_DATAENTRY_ADJUSTMENT_DETAILS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DATAENTRY_ID", Dataentry_id);
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
        public decimal Remaining_Qty(int Batch_Id)
        {
            decimal remaining_qty = 0;
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_DATAENTRY_HISTORY", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BATCH_ID", Batch_Id);
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while(sdr.Read())
                    {
                        int iOrderID = sdr.GetOrdinal("REMAINING_QTY");
                        remaining_qty = sdr.GetInt32(iOrderID);
                    }                   
                    sdr.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return remaining_qty;
        }

        public string Insert_Transfer(DataTable dt_header, DataTable dt_line, DataTable dt_livestock)
        {
            string res = "";
            int tran_id = 0;
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_INSERT_TRANSFER", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@HEADER", dt_header);
                    cmd.Parameters.AddWithValue("@LINE", dt_line);
                    cmd.Parameters.AddWithValue("@IS_SYSTEM_GENERATED_SR_NO", dt_header.Rows[0]["IS_SYSTEM_GENERATED_SR_NO"]);

                    cmd.Parameters.AddWithValue("@tbl_livestock", dt_livestock);
                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@Message";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 200;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);


                    SqlParameter outData_tran = new SqlParameter();
                    outData_tran.ParameterName = "@out_transfer_id";
                    outData_tran.SqlDbType = SqlDbType.Int;
                    outData_tran.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData_tran);
                    con.Open();
                    res = cmd.ExecuteNonQuery().ToString();
                    res = Convert.ToString(outData.Value);
                    tran_id = Convert.ToInt32(outData_tran.Value);
                    con.Close();

                    try
                    {
                        if (Convert.ToInt32(dt_header.Rows[0]["TRANSFER_ID"]) == 0 &&
                        Convert.ToInt32(dt_header.Rows[0]["COMPANY_ID"]) == 246 &&
                        Convert.ToInt32(dt_header.Rows[0]["DATAENTRY_ID"]) == 0 &&
                        dt_header.Rows[0]["TRANSFER_TYPE"].ToString() == "TRANSFER" && tran_id > 0
                        )
                        {
                            //soapApiInitialize io = new soapApiInitialize();
                            //io.call_PostTransfer_receipt(tran_id, Convert.ToInt32(dt_header.Rows[0]["DATAENTRY_ID"]));
                        }
                    }
                    catch (Exception ex) { }
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        public DataTable Get_Transfer_Summary(int Company_Id, string Location_Id, string status)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_TRANSFER_SUMMARY", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@LOCATION_ID", Location_Id);
                    cmd.Parameters.AddWithValue("@Status", status);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return dt;
        }
        public DataSet Get_Transfer_Details(int Transfer_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_TRANSFER_DETAILS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Transfer_id", Transfer_id);
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

        public DataTable Get_BPIL_UOM_List()
        {
            DataTable dt = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_GET_BPIL_UOM_LIST", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return dt;
        }
        public string GetBatch_LocationCode(int batch_id)
        {
            string str = "";
            soapApiInitialize soa = new soapApiInitialize();
            int company_id = 0;
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("GET_BATCH_LOCATION_CODE", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@batch_id", batch_id);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (Convert.ToInt32(reader["COMPANY_ID"].ToString())==56)
                        {
                            str = reader["LOCATION_CODE"].ToString();
                            company_id = Convert.ToInt32(reader["COMPANY_ID"].ToString());

                        } else if (Convert.ToInt32(reader["COMPANY_ID"].ToString()) == 81)
                        {
                            str = reader["LOCATION_CODE"].ToString();
                            company_id = Convert.ToInt32(reader["COMPANY_ID"].ToString());
                        }

                    }
                    reader.Close();
                    con.Close();
                }

                try
                {   // 56 is for unnat & 81 is for bpil
                    if ((company_id == 56) || (company_id == 81))
                    {
                        if (str != "" )
                        {
                          //  var returnMsg = soa.Call_StockUpdate(str, company_id);
                        }
                    }
                  
                }
                catch {}
            }
            catch (Exception ex)
            {

            }
            return str;
        }


        

    }
}
