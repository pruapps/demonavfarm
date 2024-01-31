using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using FarmIT_Api.database_accesslayer;
using FarmIT_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FarmIT_Api.Controllers
{
    [ApiController]
    [Authorize]
    [RESTAuthorizeAttribute]
    public class DataEntryController : ControllerBase
    {
        private IDataentryDB _dataentryDB;
        public DataEntryController(IDataentryDB dataentryDB)
        {
            _dataentryDB = dataentryDB;
        }

       
        [Route("api/insert_id")]
        [HttpPost]
        public IActionResult Data_post(int id)
        {
            _dataentryDB.InsertDataentryTransfer(id);
            return Ok("");
        }

        [Route("api/insert_dataentry")]
        [HttpPost]
        public IActionResult Data_Entry(DataEntryModel DE_Model)
        {
            

            ResponseModel obj = new ResponseModel();
            try
            {
                var header = DE_Model.header;
                var lines = DE_Model.lines;
                var livestock = DE_Model.livestock;

                if (header.NOB_ID == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Nature of business can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (header.LOB_ID == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Line of business can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (header.TEMPLATE_NAME == null)
                {
                    obj.Status = "failure";
                    obj.Message = "Template name can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (header.BATCH_ID == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Batch can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (header.BREED_NAME == null)
                {
                    obj.Status = "failure";
                    obj.Message = "Breed name can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (header.TEMPLATE_NAME == null)
                {
                    obj.Status = "failure";
                    obj.Message = "Template name can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (header.LOCATION_NAME == null)
                {
                    obj.Status = "failure";
                    obj.Message = "Location name can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (header.POSTING_DATE == null)
                {
                    obj.Status = "failure";
                    obj.Message = "Posting date can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (header.OPENING_QTY == 0  && header.LOB_ID !=16)
                {
                    obj.Status = "failure";
                    obj.Message = "Opening quantity can not zero/blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (header.CREATED_BY == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Emp Id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                //if (header.CURRENT_LOCATION == null || header.CURRENT_LOCATION == "" || header.CURRENT_LOCATION == "null")
                //{
                //    obj.Status = "failure";
                //    obj.Message = "Location details can not blank.";
                //    obj.Data = new { };
                //    return Ok(obj);
                //}
                if (header.STATUS == null)
                {
                    obj.Status = "failure";
                    obj.Message = "Status can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (header.LOCATION == null || header.LOCATION == "null")
                {
                    obj.Status = "failure";
                    obj.Message = "User location can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                if (lines == null)
                {
                    obj.Status = "failure";
                    obj.Message = "Dataentry line can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                if(header.DATAENTRY_ID==0)
                {
                    if (header.STATUS.ToLower() == "draft")
                    {
                        string[] res_msg = (_dataentryDB.Validate_Dataentry(header.DATAENTRY_ID, header.BATCH_ID, header.COMPANY_ID, header.CREATED_BY, header.STATUS, header.POSTING_DATE )).Split(',');
                        if (res_msg[0] == "error")
                        {
                            obj.Status = "failure";
                            obj.Message = res_msg[1];
                            obj.Data = new { };
                            return Ok(obj);
                        }
                    }
                    else if (header.STATUS.ToLower() == "posted")
                    {
                        string[] res_msg = (_dataentryDB.Validate_Dataentry(header.DATAENTRY_ID,header.BATCH_ID, header.COMPANY_ID, header.CREATED_BY, header.STATUS, header.POSTING_DATE )).Split(',');
                        if (res_msg[0] == "error")
                        {
                            obj.Status = "failure";
                            obj.Message = res_msg[1];
                            obj.Data = new { };
                            return Ok(obj);
                        }
                    }
                }
                else
                {
                    string[] res_msg = (_dataentryDB.Validate_Dataentry(header.DATAENTRY_ID,header.BATCH_ID, header.COMPANY_ID, header.CREATED_BY, header.STATUS, header.POSTING_DATE )).Split(',');
                    if (res_msg[0] == "error")
                    {
                        obj.Status = "failure";
                        obj.Message = res_msg[1];
                        obj.Data = new { };
                        return Ok(obj);
                    }
                }

                DataTable headerTable = new DataTable("TBL_DATAENTRY_HEADER");
                headerTable.Columns.Add("DATAENTRY_ID", typeof(int));
                headerTable.Columns.Add("COMPANY_ID", typeof(int));
                headerTable.Columns.Add("NOB_ID", typeof(int));
                headerTable.Columns.Add("LOB_ID", typeof(int));
                headerTable.Columns.Add("BATCH_ID", typeof(int));
                headerTable.Columns.Add("BREED_NAME", typeof(string));
                headerTable.Columns.Add("TEMPLATE_NAME", typeof(string));
                headerTable.Columns.Add("LOCATION_NAME", typeof(string));
                headerTable.Columns.Add("POSTINF_DATE", typeof(DateTime));
                headerTable.Columns.Add("AGE_DAYS", typeof(int));
                headerTable.Columns.Add("AGE_WEEK", typeof(int));
                headerTable.Columns.Add("OPENING_QTY", typeof(int));
                headerTable.Columns.Add("START_DATE", typeof(DateTime));
                headerTable.Columns.Add("RUNNING_COST", typeof(decimal));
                headerTable.Columns.Add("CREATED_BY", typeof(int));
                headerTable.Columns.Add("STATUS", typeof(string));
                headerTable.Columns.Add("CURRENT_LOCATION", typeof(string));
                headerTable.Columns.Add("LOCATION", typeof(string));
                headerTable.Columns.Add("Entry_From", typeof(string));
                headerTable.Columns.Add("CHK_in_lat", typeof(string));
                headerTable.Columns.Add("CHK_in_long", typeof(string));
                headerTable.Columns.Add("REMARK", typeof(string));

                DataRow dr = null;
                dr = headerTable.NewRow();
                dr["DATAENTRY_ID"] = header.DATAENTRY_ID;
                dr["COMPANY_ID"] = header.COMPANY_ID;
                dr["NOB_ID"] = header.NOB_ID;
                dr["LOB_ID"] = header.LOB_ID;
                dr["BATCH_ID"] = header.BATCH_ID;
                dr["BREED_NAME"] = header.BREED_NAME;
                dr["TEMPLATE_NAME"] = header.TEMPLATE_NAME;
                dr["LOCATION_NAME"] = header.LOCATION_NAME;
                dr["POSTINF_DATE"] = header.POSTING_DATE;
                dr["AGE_DAYS"] = header.AGE_DAYS;
                dr["AGE_WEEK"] = header.AGE_WEEK;
                dr["OPENING_QTY"] = header.OPENING_QTY;
                dr["START_DATE"] = header.START_DATE;
                dr["RUNNING_COST"] = header.RUNNING_COST;
                dr["CREATED_BY"] = header.CREATED_BY;
                dr["STATUS"] = header.STATUS;
                dr["CURRENT_LOCATION"] = header.CURRENT_LOCATION;
                dr["LOCATION"] = header.LOCATION;
                dr["Entry_From"] = header.ENTRY_FROM;
                dr["CHK_in_lat"] = header.CHK_in_lat;
                dr["CHK_in_long"] = header.CHK_in_long;
                dr["REMARK"] = header.REMARK;
                headerTable.Rows.Add(dr);

                DataTable lineTable = new DataTable("TBL_DATAENTRY_LINE");
                lineTable.Columns.Add("PARAMETER_TYPE_ID", typeof(int));
                lineTable.Columns.Add("PARAMETER_ID", typeof(int));
                lineTable.Columns.Add("PARAMETER_NAME", typeof(string));
                lineTable.Columns.Add("FORMULA_FLAG", typeof(string));
                lineTable.Columns.Add("ACTUAL_VALUE", typeof(decimal));
                lineTable.Columns.Add("UNIT_COST", typeof(decimal));
                lineTable.Columns.Add("DATAENTRY_TYPE_ID", typeof(int));
                lineTable.Columns.Add("DATAENTRY_UOM", typeof(string));
                lineTable.Columns.Add("OCCURRENCE", typeof(string));
                lineTable.Columns.Add("ITEM_NAME", typeof(string));
                lineTable.Columns.Add("FREQUENCY_START_DATE", typeof(int));
                lineTable.Columns.Add("FREQUENCY_END_DATE", typeof(int));
                lineTable.Columns.Add("LINE_AMOUNT", typeof(decimal));
                lineTable.Columns.Add("ITEM_ID", typeof(int));
                lineTable.Columns.Add("DESCRIPTIVE_INPUT_VALUE", typeof(string));

                DataRow drline = null;
                foreach (var pr in lines)
                {
                    drline = lineTable.NewRow();
                    drline["PARAMETER_TYPE_ID"] = pr.PARAMETER_TYPE_ID;
                    drline["PARAMETER_ID"] = Convert.ToInt32(pr.PARAMETER_ID);
                    drline["PARAMETER_NAME"] = pr.PARAMETER_NAME;
                    drline["FORMULA_FLAG"] = pr.FORMULA_FLAG;
                    drline["ACTUAL_VALUE"] = pr.ACTUAL_VALUE;
                    drline["UNIT_COST"] = pr.UNIT_COST;
                    drline["DATAENTRY_TYPE_ID"] = pr.DATAENTRY_TYPE_ID;
                    drline["DATAENTRY_UOM"] = pr.DATAENTRY_UOM;
                    drline["OCCURRENCE"] = pr.OCCURRENCE;
                    drline["ITEM_NAME"] = pr.ITEM_NAME;
                    drline["FREQUENCY_START_DATE"] = pr.FREQUENCY_START_DATE;
                    drline["FREQUENCY_END_DATE"] = pr.FREQUENCY_END_DATE;
                    drline["LINE_AMOUNT"] = pr.LINE_AMOUNT;
                    drline["ITEM_ID"] = pr.ITEM_ID;
                    drline["DESCRIPTIVE_INPUT_VALUE"] = pr.Parameter_input_value;
                    lineTable.Rows.Add(drline);
                }
                DataTable livestockTable = new DataTable("TBL_DATAENTRY_LINE_Livestock");
                livestockTable.Columns.Add("Livestock_No", typeof(string));
                livestockTable.Columns.Add("PARAMETER_ID", typeof(int));
                livestockTable.Columns.Add("ITEM_ID", typeof(int));
                livestockTable.Columns.Add("Total_Units", typeof(decimal));
                if (livestock != null)
                {
                    foreach (var pr in livestock)
                    {
                        drline = livestockTable.NewRow();
                        drline["Livestock_no"] = pr.Livestock_No;
                        drline["PARAMETER_ID"] = pr.Parameter_id;
                        drline["ITEM_ID"] = pr.item_id;
                        drline["Total_units"] = Convert.ToDecimal(pr.Total_Units);

                        livestockTable.Rows.Add(drline);
                    }
                }

                string[] res = (_dataentryDB.Insert_DataEntry(headerTable, lineTable, livestockTable)).Split(',');

                if (res[0] == "success")
                {
                    obj.Status = "success";
                    obj.Message = res[1];
                    obj.Data = new { };
                }
                else if (res[0] == "update")
                {
                    obj.Status = "success";
                    obj.Message = res[1];
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = res[1];
                    obj.Data = new { };
                }
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }
        [Route("api/update_dataentry")]
        [HttpPost]
        public IActionResult Update_Entry(DataEntryModel dataEntry)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                var lines = dataEntry.lines;
                DataTable lineTable = new DataTable("TBL_DATAENTRY_LINE_UPDATE");
                lineTable.Columns.Add("LINE_ID", typeof(int));
                lineTable.Columns.Add("ACTUAL_VALUE", typeof(decimal));
                lineTable.Columns.Add("LINE_AMOUNT", typeof(decimal));
 
                DataRow drline = null;
                foreach (var pr in lines)
                {
                    drline = lineTable.NewRow();
                    drline["LINE_ID"] = pr.LINE_ID;
                    drline["ACTUAL_VALUE"] = pr.ACTUAL_VALUE;
                    drline["LINE_AMOUNT"] = pr.LINE_AMOUNT;
                    lineTable.Rows.Add(drline);
                }
                var ls_lines = dataEntry.livestock;
                DataTable livestockTable = new DataTable("TBL_DATAENTRY_LINE_Livestock");
                livestockTable.Columns.Add("DATAENTRY_ID", typeof(string));
                livestockTable.Columns.Add("Livestock_No", typeof(string));
                livestockTable.Columns.Add("PARAMETER_ID", typeof(int));
                livestockTable.Columns.Add("ITEM_ID", typeof(int));
                livestockTable.Columns.Add("Total_Units", typeof(decimal));
                livestockTable.Columns.Add("Posting_Date", typeof(DateTime));
                if (ls_lines != null)
                {
                    DataRow drlinels = null;
                    foreach (var pr in ls_lines)
                    {
                        drlinels = livestockTable.NewRow();
                        drlinels["Dataentry_id"] = pr.DataEntry_id;
                        drlinels["Livestock_no"] = pr.Livestock_No;
                        drlinels["PARAMETER_ID"] = pr.Parameter_id;
                        drlinels["ITEM_ID"] = pr.item_id;
                        drlinels["Total_units"] = Convert.ToDecimal(pr.Total_Units);
                        drlinels["Posting_Date"] = Convert.ToDateTime(pr.Posting_date);
                        livestockTable.Rows.Add(drlinels);
                    }
                }

                string[] res = (_dataentryDB.Update_DataEntry(lineTable, livestockTable)).Split(',');

                if (res[0] == "success")
                {
                    obj.Status = "success";
                    obj.Message = res[1];
                    obj.Data = new { };
                }
                else if (res[0] == "update")
                {
                    obj.Status = "success";
                    obj.Message = res[1];
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = res[1];
                    obj.Data = new { };
                }
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }


        [Route("api/insert_dataentry_adjustment")]
        [HttpPost]
        public IActionResult Update_Entry_Adjustment(DataEntryModel dataEntry)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                var lines = dataEntry.lines;
                DataTable lineTable = new DataTable("TBL_DATAENTRY_LINE_UPDATE");
                lineTable.Columns.Add("LINE_ID", typeof(int));
                lineTable.Columns.Add("ACTUAL_VALUE", typeof(decimal));
                lineTable.Columns.Add("LINE_AMOUNT", typeof(decimal));

                DataRow drline = null;
                foreach (var pr in lines)
                {
                    drline = lineTable.NewRow();
                    drline["LINE_ID"] = pr.LINE_ID;
                    drline["ACTUAL_VALUE"] = pr.ACTUAL_VALUE;
                    drline["LINE_AMOUNT"] = pr.LINE_AMOUNT;
                    lineTable.Rows.Add(drline);
                }
                var ls_lines = dataEntry.livestock;
                DataTable livestockTable = new DataTable("TBL_DATAENTRY_LINE_Livestock");
                livestockTable.Columns.Add("DATAENTRY_ID", typeof(string));
                livestockTable.Columns.Add("Livestock_No", typeof(string));
                livestockTable.Columns.Add("PARAMETER_ID", typeof(int));
                livestockTable.Columns.Add("ITEM_ID", typeof(int));
                livestockTable.Columns.Add("Total_Units", typeof(decimal));
                livestockTable.Columns.Add("Posting_Date", typeof(DateTime));
                if (ls_lines != null)
                {
                    DataRow drlinels = null;
                    foreach (var pr in ls_lines)
                    {
                        drlinels = livestockTable.NewRow();
                        drlinels["Dataentry_id"] = pr.DataEntry_id;
                        drlinels["Livestock_no"] = pr.Livestock_No;
                        drlinels["PARAMETER_ID"] = pr.Parameter_id;
                        drlinels["ITEM_ID"] = pr.item_id;
                        drlinels["Total_units"] = Convert.ToDecimal(pr.Total_Units);
                        drlinels["Posting_Date"] = Convert.ToDateTime(pr.Posting_date);
                        livestockTable.Rows.Add(drlinels);
                    }
                }

                string[] res = (_dataentryDB.Insert_DataEntry_Adjustment(lineTable, livestockTable)).Split(',');

                if (res[0] == "success")
                {
                    obj.Status = "success";
                    obj.Message = res[1];
                    obj.Data = new { };
                }
                else if (res[0] == "update")
                {
                    obj.Status = "success";
                    obj.Message = res[1];
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = res[1];
                    obj.Data = new { };
                }
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }

        [Route("api/get_dataentry_summary")]
        [HttpGet]
        public IActionResult DataEntry_Summary(string Nature_Id, int Company_Id,string Location_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<DataEntry_Summary> summarry = new List<DataEntry_Summary>();
            try
            {
                if (Nature_Id == null || Nature_Id == "null")
                {
                    obj.Status = "failure";
                    obj.Message = "Nature of business id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Company_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Location_Id == null || Location_Id == "null")
                {
                    obj.Status = "failure";
                    obj.Message = "Location details can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var dt = _dataentryDB.Get_DataEntry_Summary(Nature_Id, Company_Id,Location_Id);
                if (dt.Rows.Count > 0)
                {
                    string line_of_business = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        DataEntry_Summary bs = new DataEntry_Summary();
                        if (line_of_business != dr["LINE_OF_BUSINESS"].ToString())
                        {
                            line_of_business = dr["LINE_OF_BUSINESS"].ToString();

                            bs.nob_id = dr["NOB_ID"].ToString();
                            bs.nature_of_business = dr["NATURE_OF_BUSNINESS"].ToString();
                            bs.line_of_business = dr["LINE_OF_BUSINESS"].ToString();
                            bs.lob_id = dr["LOB_ID"].ToString();

                            List<DataEntry_Details> details = new List<DataEntry_Details>();
                            foreach (DataRow dr1 in dt.Select("LINE_OF_BUSINESS = '" + line_of_business + "'"))
                            {
                                DataEntry_Details bd = new DataEntry_Details();

                                bd.batch_id = Convert.ToInt32(dr1["BATCH_ID"].ToString());
                                bd.batch_no = dr1["BATCH_NO"].ToString();
                                bd.start_date = dr1["START_DATE"].ToString();
                                bd.opening_stocks = Convert.ToInt32(dr1["OPENING_STOCK"].ToString());
                                bd.remaining_stocks = Convert.ToInt32(dr1["REMAINING_STOCK"].ToString());
                                bd.status = dr1["STATUS"].ToString();
                                bd.last_entry_date = dr1["LAST_ENTRY_DATE"].ToString();
                                bd.remark = dr1["REMARK"].ToString();

                                details.Add(bd);
                            }
                            bs.batches = details;

                            summarry.Add(bs);
                        }
                    }

                    obj.Status = "success";
                    obj.Message = "Batch summary data.";
                    obj.Data = new { summarry };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = "Data is not available.";
                    obj.Data = new { };
                }
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }

        [Route("api/get_dataentry_details")]
        [HttpGet]
        public IActionResult DataEntry_Deatils(int Batch_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<DataEntry_Header> header = new List<DataEntry_Header>();
            List<DataEntry_Line> line = new List<DataEntry_Line>();
         
            List<DataEntry_Line_LiveStock> livestock_new = new List<DataEntry_Line_LiveStock>();

           
            try
            {
                if (Batch_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Batch id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                try
                {

                    //getLocationCode is empty means company id  not 56 in GetBatch_LocationCode
                    // call StockUpdate api in GetBatch_LocationCode
                    var getLocationCode = _dataentryDB.GetBatch_LocationCode(Batch_Id);
                    
                 

                }
                catch {}

               
              

                var ds = _dataentryDB.Get_DataEntry_Details(Batch_Id);
                var ds1 = _dataentryDB.Get_Livestock_Details(Batch_Id);
                if (ds.Tables[0].Rows.Count > 0)
                {                   
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DataEntry_Header dh = new DataEntry_Header();
                        dh.DATAENTRY_ID = Convert.ToInt32(dr["DATAENTRY_ID"].ToString());
                        dh.NOB_ID = Convert.ToInt32(dr["NOB_ID"].ToString());
                        dh.NATURE_OF_BUSINESS = dr["NATURE_OF_BUSINESS"].ToString();
                        dh.LOB_ID = Convert.ToInt32(dr["LOB_ID"].ToString());
                        dh.LINE_OF_BUSINESS = dr["LINE_OF_BUSINESS"].ToString();
                        dh.BATCH_ID = Convert.ToInt32(dr["BATCH_ID"].ToString());
                        dh.BATCH_NO = dr["BATCH_NO"].ToString();
                        dh.BREED_NAME = dr["BREED_NAME"].ToString();
                        dh.TEMPLATE_NAME = dr["TEMPLATE_NAME"].ToString();
                        dh.TEMPLATE_ID = Convert.ToInt32(dr["TEMPLATE_ID"].ToString());
                        dh.LOCATION_NAME = dr["LOCATION_NAME"].ToString();
                        dh.S_DATE = dr["START_DATE"].ToString();
                        dh.P_DATE = dr["POSTING_DATE"].ToString();
                        dh.OPENING_QTY = Convert.ToInt32(dr["OPENING_QUANTITY"].ToString());
                        dh.RUNNING_COST = Convert.ToDecimal(dr["RUNNING_COST"].ToString());
                        dh.AGE_DAYS = Convert.ToInt32(dr["AGE_DAYS"].ToString());
                        dh.AGE_WEEK = Convert.ToInt32(dr["AGE_WEEK"].ToString());
                        dh.STATUS = dr["STATUS"].ToString();
                        dh.REMARK = dr["REMARK"].ToString();
                        header.Add(dh);
                    }
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        DataEntry_Line dl = new DataEntry_Line();
                        dl.PARAMETER_TYPE_ID = Convert.ToInt32(dr["PARAMETER_TYPE_ID"].ToString());
                        dl.PARAMETER_TYPE = dr["PARAMETER_TYPE"].ToString();
                        dl.PARAMETER_ID = dr["PARAMETER_ID"].ToString();
                        dl.PARAMETER_NAME = dr["PARAMETER_NAME"].ToString();
                        dl.FORMULA_FLAG = dr["FORMULA_FLAG"].ToString();
                        dl.DATAENTRY_TYPE_ID = Convert.ToInt32(dr["DATAENTRY_TYPE_ID"].ToString());
                        dl.DATAENTRY_TYPE = dr["DATAENTRY_TYPE"].ToString();
                        dl.DATAENTRY_UOM = dr["UOM"].ToString();
                        dl.OCCURRENCE = dr["OCCURRENCE"].ToString();
                        dl.ITEM_NAME = dr["ITEM_NAME"].ToString();
                        dl.F_START_DATE = dr["FREQUENCY_START_DATE"].ToString();
                        dl.F_END_DATE = dr["FREQUENCY_END_DATE"].ToString();
                        dl.UNIT_COST = Convert.ToDecimal(dr["UNIT_COST"].ToString());
                        dl.ACTUAL_VALUE = Convert.ToDecimal(dr["ACTUAL_VALUE"].ToString());
                        dl.ITEM_ID = Convert.ToInt32(dr["ITEM_ID"].ToString());
                        dl.Livestock_flag = Convert.ToInt32(dr["Livestock_flag"].ToString());
                        dl.Stock = Convert.ToDecimal(dr["Stock"].ToString());
                        dl.Company_id = Convert.ToInt32(dr["Company_id"].ToString());
                        dl.Parameter_input_type = dr["parameter_input_type"].ToString();
                        dl.Parameter_input_format = dr["parameter_input_format"].ToString();
                        dl.Parameter_input_value = dr["parameter_input_value"].ToString();
                        List<DataEntry_Line_LiveStock> livestock = new List<DataEntry_Line_LiveStock>();
                        if (dr["STATUS"].ToString()=="NEW")
                        {
                            var livestockType = dr["LIVESTOCK_TYPE"].ToString();
                            DataRow[] filteredRows;
                            if (livestockType != "")
                            {
                                string condition = "LIVESTOCK_TYPE = '"+ livestockType + "'";
                                 filteredRows = ds.Tables[2].Select(condition);
                            }
                            else
                            {
                                 filteredRows = ds.Tables[2].Select();
                            }

                            // foreach (DataRow dr1 in ds.Tables[2].Rows)
                            foreach (DataRow dr1 in filteredRows)
                            {
                                DataEntry_Line_LiveStock d = new DataEntry_Line_LiveStock();
                                d.Livestock_No = Convert.ToString(dr1["LiveStock_no"]);
                                d.Total_Units = Convert.ToDecimal(dr1["Total_Units"]);
                                d.Parameter_id = Convert.ToInt32(dr["PARAMETER_ID"]);
                                d.item_id = Convert.ToInt32(dr["ITEM_ID"]);
                                livestock.Add(d);
                            }
                            dl.livestock = livestock;
                            line.Add(dl);
                        }
                        else  
                        {
                            foreach (DataRow dr1 in ds.Tables[2].Rows)
                            {
                                DataEntry_Line_LiveStock d = new DataEntry_Line_LiveStock();
                                if (dr["PARAMETER_ID"].ToString() == dr1["L_PARAMETER_ID"].ToString() && dr["ITEM_ID"].ToString() == dr1["L_ITEM_ID"].ToString())
                                {
                                    d.Livestock_No = Convert.ToString(dr1["LiveStock_no"]);
                                    d.Total_Units = Convert.ToDecimal(dr1["Total_Units"]);
                                    d.Parameter_id = Convert.ToInt32(dr1["L_PARAMETER_ID"]);
                                    d.item_id = Convert.ToInt32(dr1["L_ITEM_ID"]);
                                    livestock.Add(d);
                                }
                            }
                            dl.livestock = livestock;
                            line.Add(dl);
                        }
                      
                    
                    }
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr1 in ds1.Tables[0].Rows)
                        {
                            DataEntry_Line_LiveStock d = new DataEntry_Line_LiveStock();
                            d.Livestock_No = Convert.ToString(dr1["LiveStock_no"]);
                            d.Total_Units = Convert.ToDecimal(dr1["Total_Units"]);
                            d.Parameter_id = Convert.ToInt32(0);
                            d.item_id = Convert.ToInt32(0);
                            livestock_new.Add(d);

                        }
                    }
                    obj.Status = "success";
                    obj.Message = "Data entry details data.";
                    obj.Data = new { header,line, livestock_new };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = "Data is not available.";
                    obj.Data = new { };
                }
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }

        [Route("api/get_dataentry_history")]
        [HttpGet]
        public IActionResult DataEntry_History(int Company_Id,int NOB_Id,int LOB_Id,int Batch_Id,string Month_Year)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (Company_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (NOB_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Please select Nature of business.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (LOB_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Please select Line of business.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Batch_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Please select Batch No.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Month_Year == null)
                {
                    obj.Status = "failure";
                    obj.Message = "Please select month/year.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var dt = _dataentryDB.Get_DataEntry_History(Company_Id,NOB_Id,LOB_Id,Batch_Id,Month_Year);
                if (dt.Rows.Count > 0)
                {
                    List<DataEntry_History> history = new List<DataEntry_History>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        DataEntry_History bd = new DataEntry_History();
                        bd.dataentry_id = Convert.ToInt32(dr["DATAENTRY_ID"].ToString());
                        bd.posting_date = dr["POSTING_DATE"].ToString();
                        history.Add(bd);
                    }
                    obj.Status = "success";
                    obj.Message = "Data entry history data.";
                    obj.Data = new { history };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = "Data is not available.";
                    obj.Data = new { };
                }
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }

        [Route("api/get_dataentry_history_details")]
        [HttpGet]
        public IActionResult DataEntry_History_Details(int Dataentry_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<DataEntry_Header> header = new List<DataEntry_Header>();
            List<DataEntry_Line> line = new List<DataEntry_Line>();
            try
            {
                if (Dataentry_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Data entry id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _dataentryDB.Get_DataEntry_History_Details(Dataentry_Id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DataEntry_Header dh = new DataEntry_Header();
                        dh.DATAENTRY_ID = Convert.ToInt32(dr["DATAENTRY_ID"].ToString());
                        dh.NOB_ID = Convert.ToInt32(dr["NOB_ID"].ToString());
                        dh.NATURE_OF_BUSINESS = dr["NATURE_OF_BUSINESS"].ToString();
                        dh.LOB_ID = Convert.ToInt32(dr["LOB_ID"].ToString());
                        dh.LINE_OF_BUSINESS = dr["LINE_OF_BUSINESS"].ToString();
                        dh.BATCH_ID = Convert.ToInt32(dr["BATCH_ID"].ToString());
                        dh.BATCH_NO = dr["BATCH_NO"].ToString();
                        dh.BREED_NAME = dr["BREED_NAME"].ToString();
                        dh.TEMPLATE_NAME = dr["TEMPLATE_NAME"].ToString();
                        dh.TEMPLATE_ID = Convert.ToInt32(dr["TEMPLATE_ID"].ToString());
                        dh.LOCATION_NAME = dr["LOCATION_NAME"].ToString();
                        dh.S_DATE = dr["START_DATE"].ToString();
                        dh.P_DATE = dr["POSTING_DATE"].ToString();
                        dh.END_DATE = dr["END_DATE"].ToString();
                        dh.OPENING_QTY = Convert.ToInt32(dr["OPENING_QUANTITY"].ToString());
                        dh.RUNNING_COST = Convert.ToDecimal(dr["RUNNING_COST"].ToString());
                        dh.AGE_DAYS = Convert.ToInt32(dr["AGE_DAYS"].ToString());
                        dh.AGE_WEEK = Convert.ToInt32(dr["AGE_WEEK"].ToString());
                        dh.STATUS = dr["STATUS"].ToString();
                        dh.REMAINING_QTY = Convert.ToInt32(dr["REMAINING_QTY"].ToString());
                        header.Add(dh);
                    }
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        DataEntry_Line dl = new DataEntry_Line();
                        dl.PARAMETER_ID = dr["PARAMETER_ID"].ToString();
                        dl.FORMULA_FLAG = dr["FORMULA_FLAG"].ToString();
                        dl.PARAMETER_TYPE = dr["PARAMETER_TYPE"].ToString();
                        dl.PARAMETER_NAME = dr["PARAMETER_NAME"].ToString();
                        dl.DATAENTRY_TYPE = dr["DATAENTRY_TYPE"].ToString();
                        dl.DATAENTRY_UOM = dr["UOM"].ToString();
                        dl.OCCURRENCE = dr["OCCURRENCE"].ToString();
                        dl.ITEM_NAME = dr["ITEM_NAME"].ToString();
                        dl.F_START_DATE = dr["FREQUENCY_START_DATE"].ToString();
                        dl.F_END_DATE = dr["FREQUENCY_END_DATE"].ToString();
                        dl.UNIT_COST = Convert.ToDecimal(dr["UNIT_COST"].ToString());
                        dl.ACTUAL_VALUE = Convert.ToDecimal(dr["ACTUAL_VALUE"].ToString());
                        dl.LINE_ID = Convert.ToInt32(dr["Line_id"].ToString());
                        dl.ITEM_ID = Convert.ToInt32(dr["ITEM_ID"].ToString());
                        dl.Livestock_flag = Convert.ToInt32(dr["Livestock_flag"].ToString());
                        List<DataEntry_Line_LiveStock> livestock = new List<DataEntry_Line_LiveStock>();
             
                            foreach (DataRow dr1 in ds.Tables[2].Rows)
                            {
                                DataEntry_Line_LiveStock d = new DataEntry_Line_LiveStock();
                                if (dr["PARAMETER_ID"].ToString() == dr1["L_PARAMETER_ID"].ToString() && dr["ITEM_ID"].ToString() == dr1["L_ITEM_ID"].ToString())
                                {
                                    d.Livestock_No = Convert.ToString(dr1["LiveStock_no"]);
                                    d.Total_Units = Convert.ToDecimal(dr1["Total_Units"]);
                                    d.Parameter_id = Convert.ToInt32(dr1["L_PARAMETER_ID"]);
                                    d.item_id = Convert.ToInt32(dr1["L_ITEM_ID"]);
                                    livestock.Add(d);
                                }
                            }
                            dl.livestock = livestock;
                            line.Add(dl);
                        }

                    obj.Status = "success";
                    obj.Message = "Data entry details  data.";
                    obj.Data = new { header, line };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = "Data is not available.";
                    obj.Data = new { };
                }
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }

        [Route("api/get_dataentry_adjustment_details")]
        [HttpGet]
        public IActionResult DataEntry_Adjustment_Details(int Dataentry_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<DataEntry_Header> header = new List<DataEntry_Header>();
            List<DataEntry_Line> line = new List<DataEntry_Line>();
            try
            {
                if (Dataentry_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Data entry id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                var batchId = 0;

                var ds = _dataentryDB.Get_DataEntry_Adjustment_Details(Dataentry_Id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DataEntry_Header dh = new DataEntry_Header();
                        dh.DATAENTRY_ID = Convert.ToInt32(dr["DATAENTRY_ID"].ToString());
                        dh.NOB_ID = Convert.ToInt32(dr["NOB_ID"].ToString());
                        dh.NATURE_OF_BUSINESS = dr["NATURE_OF_BUSINESS"].ToString();
                        dh.LOB_ID = Convert.ToInt32(dr["LOB_ID"].ToString());
                        dh.LINE_OF_BUSINESS = dr["LINE_OF_BUSINESS"].ToString();
                        dh.BATCH_ID = Convert.ToInt32(dr["BATCH_ID"].ToString());
                        dh.BATCH_NO = dr["BATCH_NO"].ToString();
                        dh.BREED_NAME = dr["BREED_NAME"].ToString();
                        dh.TEMPLATE_NAME = dr["TEMPLATE_NAME"].ToString();
                        dh.TEMPLATE_ID = Convert.ToInt32(dr["TEMPLATE_ID"].ToString());
                        dh.LOCATION_NAME = dr["LOCATION_NAME"].ToString();
                        dh.S_DATE = dr["START_DATE"].ToString();
                        dh.P_DATE = dr["POSTING_DATE"].ToString();
                        dh.END_DATE = dr["END_DATE"].ToString();
                        dh.OPENING_QTY = Convert.ToInt32(dr["OPENING_QUANTITY"].ToString());
                        dh.RUNNING_COST = Convert.ToDecimal(dr["RUNNING_COST"].ToString());
                        dh.AGE_DAYS = Convert.ToInt32(dr["AGE_DAYS"].ToString());
                        dh.AGE_WEEK = Convert.ToInt32(dr["AGE_WEEK"].ToString());
                        dh.STATUS = dr["STATUS"].ToString();
                        dh.REMAINING_QTY = Convert.ToInt32(dr["REMAINING_QTY"].ToString());
                        header.Add(dh);
                        batchId= Convert.ToInt32(dr["BATCH_ID"].ToString());
                    }
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        DataEntry_Line dl = new DataEntry_Line();
                        dl.PARAMETER_ID = dr["PARAMETER_ID"].ToString();
                        dl.FORMULA_FLAG = dr["FORMULA_FLAG"].ToString();
                        dl.PARAMETER_TYPE = dr["PARAMETER_TYPE"].ToString();
                        dl.PARAMETER_NAME = dr["PARAMETER_NAME"].ToString();
                        dl.DATAENTRY_TYPE = dr["DATAENTRY_TYPE"].ToString();
                        dl.DATAENTRY_UOM = dr["UOM"].ToString();
                        dl.OCCURRENCE = dr["OCCURRENCE"].ToString();
                        dl.ITEM_NAME = dr["ITEM_NAME"].ToString();
                        dl.F_START_DATE = dr["FREQUENCY_START_DATE"].ToString();
                        dl.F_END_DATE = dr["FREQUENCY_END_DATE"].ToString();
                        dl.UNIT_COST = Convert.ToDecimal(dr["UNIT_COST"].ToString());
                        dl.ACTUAL_VALUE = Convert.ToDecimal(dr["ACTUAL_VALUE"].ToString());
                        dl.LINE_ID = Convert.ToInt32(dr["Line_id"].ToString());
                        dl.ITEM_ID = Convert.ToInt32(dr["ITEM_ID"].ToString());
                        dl.Livestock_flag = Convert.ToInt32(dr["Livestock_flag"].ToString());
                        List<DataEntry_Line_LiveStock> livestock = new List<DataEntry_Line_LiveStock>();

                        foreach (DataRow dr1 in ds.Tables[2].Rows)
                        {
                            DataEntry_Line_LiveStock d = new DataEntry_Line_LiveStock();
                            if (dr["PARAMETER_ID"].ToString() == dr1["L_PARAMETER_ID"].ToString() && dr["ITEM_ID"].ToString() == dr1["L_ITEM_ID"].ToString())
                            {
                                d.Livestock_No = Convert.ToString(dr1["LiveStock_no"]);
                                d.Total_Units = Convert.ToDecimal(dr1["Total_Units"]);
                                d.Parameter_id = Convert.ToInt32(dr1["L_PARAMETER_ID"]);
                                d.item_id = Convert.ToInt32(dr1["L_ITEM_ID"]);
                                livestock.Add(d);
                            }
                        }
                        dl.livestock = livestock;
                        line.Add(dl);
                    }

                    try
                    {

                        //getLocationCode is empty means company id  not 56 in GetBatch_LocationCode
                        // call StockUpdate api in GetBatch_LocationCode
                        if (batchId>0)
                        {
                            var getLocationCode = _dataentryDB.GetBatch_LocationCode(batchId);
                        }
                        
                    }
                    catch (Exception ex){}

                    obj.Status = "success";
                    obj.Message = "Data entry details  data.";
                    obj.Data = new { header, line };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = "Data is not available.";
                    obj.Data = new { };
                }
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }

        [Route("api/get_dataentry_details_json")]
        [HttpGet]
        public IActionResult DataEntry_Details(int Batch_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<DataEntry_Header> header = new List<DataEntry_Header>();
            List<DataEntry_Line> line = new List<DataEntry_Line>();
            try
            {
                if (Batch_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Batch id can not blank.";
                    obj.Data = null;
                    return Ok(obj);
                }

                var ds = _dataentryDB.Get_DataEntry_Details_JSON(Batch_Id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(ds.Tables[0]);

                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        DataEntry_Header dh = new DataEntry_Header();
                        dh.NATURE_OF_BUSINESS = dr["NATURE_OF_BUSINESS"].ToString();
                        dh.LINE_OF_BUSINESS = dr["LINE_OF_BUSINESS"].ToString();
                        dh.BATCH_NO = dr["BATCH_NO"].ToString();
                        dh.BREED_NAME = dr["BREED_NAME"].ToString();
                        dh.TEMPLATE_NAME = dr["TEMPLATE_NAME"].ToString();
                        dh.LOCATION_NAME = dr["LOCATION_NAME"].ToString();
                        dh.S_DATE = dr["START_DATE"].ToString();
                        dh.P_DATE = dr["END_DATE"].ToString();
                        dh.OPENING_QTY = Convert.ToInt32(dr["OPENING_QUANTITY"].ToString());
                        dh.RUNNING_COST = Convert.ToDecimal(dr["RUNNING_COST"].ToString());
                        dh.AGE_DAYS = Convert.ToInt32(dr["AGE_DAYS"].ToString());
                        dh.STATUS = dr["STATUS"].ToString();
                        dh.REMAINING_QTY = Convert.ToInt32(dr["REMAINING_QTY"].ToString());
                        header.Add(dh);
                    }
                    obj.Status = "success";
                    obj.Message = "Data entry details data.";
                    obj.Data = new { result,header};
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = "Data is not available.";
                    obj.Data = new { };
                }
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }

        [Route("api/get_all_dataentry_details")]
        [HttpGet]
        public IActionResult DataEntry_Details_ALL(int Comp_id)
        {
            ResponseModel obj = new ResponseModel();
            List<DataEntry_Header> header = new List<DataEntry_Header>();
            List<DataEntry_Line> line = new List<DataEntry_Line>();
            List<DataEntry_Line_LiveStock> livestock_new = new List<DataEntry_Line_LiveStock>();

            try
            {
                if (Comp_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _dataentryDB.Get_DataEntry_Details_All(Comp_id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DataEntry_Header dh = new DataEntry_Header();
                        dh.DATAENTRY_ID = Convert.ToInt32(dr["DATAENTRY_ID"].ToString());
                        dh.NOB_ID = Convert.ToInt32(dr["NOB_ID"].ToString());
                        dh.NATURE_OF_BUSINESS = dr["NATURE_OF_BUSINESS"].ToString();
                        dh.LOB_ID = Convert.ToInt32(dr["LOB_ID"].ToString());
                        dh.LINE_OF_BUSINESS = dr["LINE_OF_BUSINESS"].ToString();
                        dh.BATCH_ID = Convert.ToInt32(dr["BATCH_ID"].ToString());
                        dh.BATCH_NO = dr["BATCH_NO"].ToString();
                        dh.BREED_NAME = dr["BREED_NAME"].ToString();
                        dh.TEMPLATE_NAME = dr["TEMPLATE_NAME"].ToString();
                        dh.TEMPLATE_ID = Convert.ToInt32(dr["TEMPLATE_ID"].ToString());
                        dh.LOCATION_NAME = dr["LOCATION_NAME"].ToString();
                        dh.S_DATE = dr["START_DATE"].ToString();
                        dh.P_DATE = dr["POSTING_DATE"].ToString();
                        dh.OPENING_QTY = Convert.ToInt32(dr["OPENING_QUANTITY"].ToString());
                        dh.RUNNING_COST = Convert.ToDecimal(dr["RUNNING_COST"].ToString());
                        dh.AGE_DAYS = Convert.ToInt32(dr["AGE_DAYS"].ToString());
                        dh.AGE_WEEK = Convert.ToInt32(dr["AGE_WEEK"].ToString());
                        dh.STATUS = dr["STATUS"].ToString();
                        header.Add(dh);
                    }
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        DataEntry_Line dl = new DataEntry_Line();
                        dl.PARAMETER_TYPE_ID = Convert.ToInt32(dr["PARAMETER_TYPE_ID"].ToString());
                        dl.PARAMETER_TYPE = dr["PARAMETER_TYPE"].ToString();
                        dl.PARAMETER_ID = dr["PARAMETER_ID"].ToString();
                        dl.PARAMETER_NAME = dr["PARAMETER_NAME"].ToString();
                        dl.FORMULA_FLAG = dr["FORMULA_FLAG"].ToString();
                        dl.DATAENTRY_TYPE_ID = Convert.ToInt32(dr["DATAENTRY_TYPE_ID"].ToString());
                        dl.DATAENTRY_TYPE = dr["DATAENTRY_TYPE"].ToString();
                        dl.DATAENTRY_UOM = dr["UOM"].ToString();
                        dl.OCCURRENCE = dr["OCCURRENCE"].ToString();
                        dl.ITEM_NAME = dr["ITEM_NAME"].ToString();
                        dl.ITEM_ID = Convert.ToInt32(dr["ITEM_ID"]);
                        dl.F_START_DATE = dr["FREQUENCY_START_DATE"].ToString();
                        dl.F_END_DATE = dr["FREQUENCY_END_DATE"].ToString();
                        dl.UNIT_COST = Convert.ToDecimal(dr["UNIT_COST"].ToString());
                        dl.ACTUAL_VALUE = Convert.ToDecimal(dr["ACTUAL_VALUE"].ToString());
                        dl.Batch_ID = Convert.ToInt32(dr["Batch_ID"].ToString());
                        dl.Livestock_flag = Convert.ToInt32(dr["Livestock_flag"].ToString());
                        line.Add(dl);
                    }
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow dr1 in ds.Tables[2].Rows)
                        {
                            DataEntry_Line_LiveStock d = new DataEntry_Line_LiveStock();
                            d.Livestock_No = Convert.ToString(dr1["LiveStock_no"]);
                            d.Total_Units = Convert.ToDecimal(dr1["Total_Units"]);
                            d.Parameter_id = Convert.ToInt32(dr1["L_Parameter_id"]);
                            d.item_id = Convert.ToInt32(dr1["L_ITEM_ID"]);
                            d.Batch_id = Convert.ToInt32(dr1["BATCH_ID"]);
                            livestock_new.Add(d);

                        }
                    }
                    obj.Status = "success";
                    obj.Message = "Data entry details  data.";
                    obj.Data = new { header, line , livestock_new };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = "Data is not available.";
                    obj.Data = new { };
                }
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }


        #region Dataentry Bulk Poultry
        [Route("api/get_dataentry_poultry_details_bulk")]
        [HttpGet]
        public IActionResult DataEntry_Poultry_Deatils(int company_id,int lob_id, int location_id ,string posting_date)
        {
            ResponseModel obj = new ResponseModel();
            List<DataEntry_Header> header = new List<DataEntry_Header>();
            List<DataEntry_Line> line = new List<DataEntry_Line>();

            List<DataEntry_Line_LiveStock> livestock_new = new List<DataEntry_Line_LiveStock>();


            try
            {
                if (company_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (lob_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "LOB id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (location_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Location id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _dataentryDB.Get_DataEntry_Poultry_Details_Bulk(company_id, lob_id, location_id, posting_date);
              

                if (ds.Tables[0].Rows.Count > 0)
                {
                    var PostingDate = ds.Tables[0].Rows[0]["POSTING_DATE"].ToString();

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            DataEntry_Line_Bulk dl = new DataEntry_Line_Bulk();
                            dl.PARAMETER_TYPE_ID = Convert.ToInt32(dr["PARAMETER_TYPE_ID"].ToString());
                            dl.PARAMETER_TYPE = dr["PARAMETER_TYPE"].ToString();
                            dl.PARAMETER_ID = dr["PARAMETER_ID"].ToString();
                            dl.PARAMETER_NAME = dr["PARAMETER_NAME"].ToString();
                            dl.FORMULA_FLAG = dr["FORMULA_FLAG"].ToString();
                            dl.DATAENTRY_TYPE_ID = Convert.ToInt32(dr["DATAENTRY_TYPE_ID"].ToString());
                            dl.DATAENTRY_TYPE = dr["DATAENTRY_TYPE"].ToString();
                            dl.DATAENTRY_UOM = dr["UOM"].ToString();
                            dl.OCCURRENCE = dr["OCCURRENCE"].ToString();
                            dl.ITEM_NAME = dr["ITEM_NAME"].ToString();
                            dl.F_START_DATE = dr["FREQUENCY_START_DATE"].ToString();
                            dl.F_END_DATE = dr["FREQUENCY_END_DATE"].ToString();
                            dl.UNIT_COST = Convert.ToDecimal(dr["UNIT_COST"].ToString());
                            dl.ACTUAL_VALUE = Convert.ToDecimal(dr["ACTUAL_VALUE"].ToString());
                            dl.ITEM_ID = Convert.ToInt32(dr["ITEM_ID"].ToString());
                            dl.Livestock_flag = Convert.ToInt32(dr["Livestock_flag"].ToString());
                            dl.Stock = Convert.ToDecimal(dr["Stock"].ToString());
                            dl.Company_id = Convert.ToInt32(dr["Company_id"].ToString());
                            dl.Parameter_input_type = dr["parameter_input_type"].ToString();
                            dl.Parameter_input_format = dr["parameter_input_format"].ToString();
                            dl.Parameter_input_value = dr["parameter_input_value"].ToString();
                            dl.Batch_ID = Convert.ToInt32(dr["BATCH_ID"].ToString());
                            dl.Batch_No = dr["BATCH_NO"].ToString();
                            line.Add(dl);


                        }
                        obj.Status = "success";
                        obj.Message = "Data entry poultry details data.";
                        obj.Data = new { POSTING_DATE = PostingDate, line };
                    }
                    else
                    {
                        obj.Status = "failure";
                        obj.Message = "Data entry poultry batch age is completed.";
                        obj.Data = new { POSTING_DATE = PostingDate, line };

                    }

                    
                    
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = "Data is not available.";
                    obj.Data = new { };
                }
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }



        [Route("api/insert_dataentry_bulk")]
        [HttpPost]
        public  IActionResult Data_Entry_bulk(DataEntryModel dataEntry_bulk)
        {


            ResponseModel obj = new ResponseModel();
            string action_Status = "";
            try
            {
                

                if (dataEntry_bulk.header.NOB_ID == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Nature of business can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (dataEntry_bulk.header.LOB_ID == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Line of business can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                
                if (dataEntry_bulk.header.CREATED_BY == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Emp Id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                
                if (dataEntry_bulk.header.STATUS == null)
                {
                    obj.Status = "failure";
                    obj.Message = "Status can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                
                if (dataEntry_bulk.lines == null)
                {
                    obj.Status = "failure";
                    obj.Message = "Dataentry line can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
               
                var distinctBatch_id = dataEntry_bulk.lines.Select(p => p.Batch_ID).Distinct();
                
                foreach (var item in distinctBatch_id)
                {
                    var ds = _dataentryDB.Get_DataEntry_Details(item);
                    DataEntryModel dm = new DataEntryModel();

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataEntry_Header header = new DataEntry_Header();
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            action_Status = dataEntry_bulk.header.STATUS;
                            header.DATAENTRY_ID = Convert.ToInt32(dr["DATAENTRY_ID"].ToString());
                            header.COMPANY_ID = dataEntry_bulk.header.COMPANY_ID;// Convert.ToInt32(dr["DATAENTRY_ID"].ToString());
                            header.NOB_ID = Convert.ToInt32(dr["NOB_ID"].ToString());
                            header.LOB_ID = Convert.ToInt32(dr["LOB_ID"].ToString());
                            header.BATCH_ID = Convert.ToInt32(dr["BATCH_ID"].ToString());
                            header.BREED_NAME = dr["BREED_NAME"].ToString();
                            header.TEMPLATE_NAME = dr["TEMPLATE_NAME"].ToString();
                            header.LOCATION_NAME = dr["LOCATION_NAME"].ToString();
                            header.POSTING_DATE = dataEntry_bulk.header.POSTING_DATE; //Convert.ToDateTime(dr["POSTING_DATE"].ToString());
                            header.AGE_DAYS = Convert.ToInt32(dr["AGE_DAYS"].ToString());
                            header.AGE_WEEK = Convert.ToInt32(dr["AGE_WEEK"].ToString());
                            header.OPENING_QTY = Convert.ToInt32(dr["OPENING_QUANTITY"].ToString());
                            header.START_DATE = Convert.ToDateTime(dr["START_DATE"].ToString());
                            header.RUNNING_COST = dataEntry_bulk.header.RUNNING_COST; //Convert.ToDecimal(dr["RUNNING_COST"].ToString());
                            header.CREATED_BY = dataEntry_bulk.header.CREATED_BY;
                            header.STATUS = dataEntry_bulk.header.STATUS;
                            header.CURRENT_LOCATION = dataEntry_bulk.header.CURRENT_LOCATION;
                            header.LOCATION = dataEntry_bulk.header.LOCATION;
                            header.ENTRY_FROM = dataEntry_bulk.header.ENTRY_FROM;
                            header.CHK_in_lat = dataEntry_bulk.header.CHK_in_lat;
                            header.CHK_in_long = dataEntry_bulk.header.CHK_in_long;
                            header.REMARK = dataEntry_bulk.header.REMARK; //dr["REMARK"].ToString();


                        }

                        List<DataEntry_Line> DTLine = new List<DataEntry_Line>();

                        string condition = "Batch_ID = '" + item + "'";
                        var FilterLine = dataEntry_bulk.lines.Where(p => p.Batch_ID == item)
                            .Select(q => new DataEntry_Line
                            {
                                LINE_ID = q.LINE_ID,
                                PARAMETER_TYPE_ID = q.PARAMETER_TYPE_ID,
                                PARAMETER_TYPE = q.PARAMETER_TYPE,
                                PARAMETER_ID = q.PARAMETER_ID,
                                PARAMETER_NAME = q.PARAMETER_NAME,
                                FORMULA_FLAG = q.FORMULA_FLAG,
                                ACTUAL_VALUE = q.ACTUAL_VALUE,
                                UNIT_COST = q.UNIT_COST,
                                DATAENTRY_TYPE_ID = q.DATAENTRY_TYPE_ID,
                                DATAENTRY_TYPE = q.DATAENTRY_TYPE,
                                DATAENTRY_UOM = q.DATAENTRY_UOM,
                                OCCURRENCE = q.OCCURRENCE,
                                FREQUENCY_START_DATE = q.FREQUENCY_START_DATE,
                                FREQUENCY_END_DATE = q.FREQUENCY_END_DATE,
                                F_END_DATE = q.F_END_DATE,
                                ITEM_NAME = q.ITEM_NAME,
                                LINE_AMOUNT = q.LINE_AMOUNT,
                                Batch_ID = q.Batch_ID,
                                F_START_DATE = q.F_START_DATE,
                                ITEM_ID = q.ITEM_ID,
                                Livestock_flag = q.Livestock_flag,
                                Status = q.Status,
                                Stock = q.Stock,
                                Parameter_input_type = q.Parameter_input_type,
                                Parameter_input_format = q.Parameter_input_format,
                                Parameter_input_value = q.Parameter_input_value,
                                Company_id = q.Company_id
                            }
                            ).ToList();
                        dm.header = header;
                        dm.lines = FilterLine;
                        IActionResult return_msg = Data_Entry(dm);

                        if (return_msg is ObjectResult objectResult)
                        {
                            // Extract status code and handle the result
                            var statusCode = objectResult.StatusCode;
                            var value = objectResult.Value;
                            var BatchResoponse = value as ResponseModel;
                           
                        }


                    }
                }
                if (action_Status== "posted")
                {
                    obj.Status = "success";
                    obj.Message = "Successfully Posted";
                    obj.Data = new { };

                }
                else
                {
                    obj.Status = "success";
                    obj.Message = "Successfully Save";
                    obj.Data = new { };
                }


               
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }
        #endregion



        [Route("api/insert_transfer")]
        [HttpPost]
        public IActionResult TRANSFER_Entry(TransferModel DE_Model)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                var header = DE_Model.header;
                var lines = DE_Model.lines;
                var livestock = DE_Model.livestock;

                if (header.LOCATION_ID == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "location can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (header.LOCATION_FROM == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Location from can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (header.LOCATION_TO == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Location To can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                if (header.TRANSFER_DATE == null)
                {
                    obj.Status = "failure";
                    obj.Message = "Transfer date can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                if (lines == null)
                {
                    obj.Status = "failure";
                    obj.Message = "Transfer line can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }


                DataTable headerTable = new DataTable("TBL_TRANSFER_HEADER");
                headerTable.Columns.Add("TRANSFER_ID", typeof(int));
                headerTable.Columns.Add("TRANSFER_TYPE", typeof(string));
                headerTable.Columns.Add("TRANSFER_DATE", typeof(DateTime));
                headerTable.Columns.Add("LOCATION_ID", typeof(int));
                headerTable.Columns.Add("LOCATION_FROM", typeof(int));
                headerTable.Columns.Add("LOCATION_TO", typeof(int));
                headerTable.Columns.Add("BATCH_FROM", typeof(int));
                headerTable.Columns.Add("BATCH_TO", typeof(int));
                headerTable.Columns.Add("REMARKS", typeof(string));
                headerTable.Columns.Add("COMPANY_ID", typeof(int));
                headerTable.Columns.Add("CREATED_BY", typeof(int));
                headerTable.Columns.Add("DATAENTRY_ID", typeof(int));
                headerTable.Columns.Add("IS_SYSTEM_GENERATED_SR_NO.", typeof(int));

                DataRow dr = null;
                dr = headerTable.NewRow();
                dr["TRANSFER_ID"] = header.TRANSFER_ID;
                dr["COMPANY_ID"] = header.COMPANY_ID;
                dr["LOCATION_ID"] = header.LOCATION_ID;
                dr["LOCATION_FROM"] = header.LOCATION_FROM;
                dr["LOCATION_TO"] = header.LOCATION_TO;
                dr["TRANSFER_TYPE"] = header.TRANSFER_TYPE;
                dr["BATCH_FROM"] = header.BATCH_FROM;
                dr["BATCH_TO"] = header.BATCH_TO;
                dr["TRANSFER_DATE"] = header.TRANSFER_DATE;
                dr["CREATED_BY"] = header.CREATED_BY;
                dr["REMARKS"] = header.REMARKS;
                dr["IS_SYSTEM_GENERATED_SR_NO"] = header.IS_SERIAL_NO_SYSTEM_GENERATED;
                headerTable.Rows.Add(dr);

                DataTable lineTable = new DataTable("TBL_TRANSFER_LINE");
                lineTable.Columns.Add("LINE_ID", typeof(int));
                lineTable.Columns.Add("TRANSFER_ID", typeof(int));
                lineTable.Columns.Add("ITEM_ID", typeof(string));
                lineTable.Columns.Add("UOM", typeof(string));
                lineTable.Columns.Add("QUANTITY", typeof(decimal));
                lineTable.Columns.Add("UNIT_COST", typeof(decimal));
                lineTable.Columns.Add("REMAINING_QTY", typeof(decimal));
                lineTable.Columns.Add("DATAENTRY_ID", typeof(int));
                lineTable.Columns.Add("DATAENTRY_LINE_ID", typeof(int));
                lineTable.Columns.Add("DEAD_ON_ARRIVAL", typeof(decimal));
                lineTable.Columns.Add("WEIGH_SCALE", typeof(decimal));
                lineTable.Columns.Add("ADJ_LINE_ID", typeof(int));
                lineTable.Columns.Add("BATCH_FROM", typeof(int));
                lineTable.Columns.Add("BATCH_TO", typeof(int));
                lineTable.Columns.Add("LOT_NO", typeof(string));
                DataRow drline = null;
                foreach (var pr in lines)
                {
                    drline = lineTable.NewRow();
                    drline["LINE_ID"] = pr.LINE_ID;
                    drline["TRANSFER_ID"] = pr.TRANSFER_ID;
                    drline["ITEM_ID"] = pr.ITEM_ID;
                    drline["UOM"] = pr.UOM;
                    drline["QUANTITY"] = pr.QUANTITY;
                    drline["UNIT_COST"] = pr.UNIT_COST;
                    drline["REMAINING_QTY"] = pr.REMAINING_QTY;
                    drline["DEAD_ON_ARRIVAL"] = pr.DEAD_ON_ARRIVAL;
                    drline["WEIGH_SCALE"] = pr.WEIGH_SCALE;
                    drline["BATCH_FROM"] = pr.BATCH_FROM;
                    drline["BATCH_TO"] = pr.BATCH_TO;
                    drline["LOT_NO"] = pr.LOT_NO;
                    lineTable.Rows.Add(drline);
                }

                DataTable livestockTable = new DataTable("TBL_TRANSFER_LINE_Livestock");
                livestockTable.Columns.Add("Livestock_No", typeof(string));
                livestockTable.Columns.Add("ITEM_ID", typeof(int));
                livestockTable.Columns.Add("Total_Units", typeof(decimal));
                if (livestock != null)
                {
                    DataRow drline1 = null;
                    foreach (var pr in livestock)
                    {
                        drline1 = livestockTable.NewRow();
                        drline1["Livestock_no"] = pr.Livestock_No;
                        drline1["ITEM_ID"] = pr.item_id;
                        drline1["Total_units"] = Convert.ToDecimal(pr.Total_Units);

                        livestockTable.Rows.Add(drline1);
                    }
                }

                string[] res = (_dataentryDB.Insert_Transfer(headerTable, lineTable, livestockTable)).Split(',');

                if (res[0] == "success")
                {
                    obj.Status = "success";
                    obj.Message = res[1];
                    obj.Data = new { };
                }
                else if (res[0] == "update")
                {
                    obj.Status = "success";
                    obj.Message = res[1];
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = res[1];
                    obj.Data = new { };
                }
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }


        [Route("api/get_transfer_summary")]
        [HttpGet]
        public IActionResult Transfer_Summary(int Company_Id, string Location_Id,string status)
        {
            ResponseModel obj = new ResponseModel();
            List<Transfer_Header_Summary> summarry = new List<Transfer_Header_Summary>();
            try
            {
                if (Company_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Location_Id == null || Location_Id == "null")
                {
                    obj.Status = "failure";
                    obj.Message = "Location details can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var dt = _dataentryDB.Get_Transfer_Summary(Company_Id, Location_Id, status);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Transfer_Header_Summary bs = new Transfer_Header_Summary();
                        bs.TRANSFER_ID = Convert.ToInt32(dr["TRANSFER_ID"]);
                        bs.TRANSFER_TYPE = dr["TRANSFER_TYPE"].ToString();
                        bs.TRANSFER_NUMBER = dr["TRANSFER_NUMBER"].ToString();
                        bs.TRANSFER_DATE = Convert.ToString(dr["TRANSFER_DATE"]);
                        bs.LOCATION = dr["LOCATION_NAME"].ToString();
                        bs.REMARKS = dr["REMARKS"].ToString();
                        bs.ENTRY_FROM = dr["ENTRY_FROM"].ToString();
                        bs.Status = dr["Status"].ToString();
                        summarry.Add(bs);
                    }
                    obj.Status = "success";
                    obj.Message = "Transfer summary data.";
                    obj.Data = new { summarry };
                }
                else
                {
                    obj.Status = "error";
                    obj.Message = "Transfer summary data.";
                    obj.Data = new { };
                }
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }

        [Route("api/get_transfer_details")]
        [HttpGet]
        public IActionResult Transfer_Details(int Transfer_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<Transfer_Header> header = new List<Transfer_Header>();
            List<Transfer_Line> line = new List<Transfer_Line>();
            try
            {
                if (Transfer_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Transfer id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _dataentryDB.Get_Transfer_Details(Transfer_Id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Transfer_Header dh = new Transfer_Header();
                        dh.TRANSFER_ID = Convert.ToInt32(dr["TRANSFER_ID"].ToString());
                        dh.TRANSFER_TYPE = Convert.ToString(dr["TRANSFER_TYPE"].ToString());
                        dh.TRANSFER_DATE = dr["TRANSFER_DATE"].ToString();
                        dh.LOCATION_ID = Convert.ToInt32(dr["LOCATION_ID"].ToString());
                        dh.LOCATION_FROM = Convert.ToInt32(dr["LOCATION_FROM"]);
                        dh.LOCATION_TO = Convert.ToInt32(dr["LOCATION_TO"]);
                        dh.BATCH_FROM = Convert.ToInt32(dr["BATCH_FROM"]);
                        dh.BATCH_TO = Convert.ToInt32(dr["BATCH_TO"]);
                        dh.REMARKS = dr["REMARKS"].ToString();
                        dh.TRANSFER_NUMBER = dr["TRANSFER_NUMBER"].ToString();
                        dh.ENTRY_FROM = dr["ENTRY_FROM"].ToString();
                        dh.IS_RECEIPT = dr["IS_RECEIPT"].ToString();
                        dh.IS_SHIP = dr["IS_SHIP"].ToString();
                        dh.IS_SERIAL_NO_SYSTEM_GENERATED = Convert.ToInt32(dr["IS_SYSTEM_GENERATED_SR_NO"].ToString());
                        header.Add(dh);
                    }
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        Transfer_Line dl = new Transfer_Line();
                        dl.LINE_ID = Convert.ToInt32(dr["LINE_ID"].ToString());
                        dl.TRANSFER_ID = Convert.ToInt32(dr["TRANSFER_ID"]);
                        dl.ITEM_ID = Convert.ToInt32(dr["ITEM_ID"]);
                        dl.UOM = dr["UOM"].ToString();
                        dl.ITEM_NAME = dr["ITEM_NAME"].ToString();
                        dl.QUANTITY = Convert.ToDecimal(dr["QUANTITY"]);
                        dl.UNIT_COST = Convert.ToDecimal(dr["UNIT_COST"]);
                        dl.REMAINING_QTY = Convert.ToDecimal(dr["REMAINING_QTY"]);
                        dl.INVENTORY_TYPE = dr["INVENTORY_TYPE"].ToString();
                        dl.LINE_NO= Convert.ToInt32(dr["LINE_NO"]);
                        dl.DEAD_ON_ARRIVAL = Convert.ToInt32(dr["DEAD_ON_ARRIVAL"]);
                        dl.WEIGH_SCALE = Convert.ToInt32(dr["WEIGH_SCALE"]);
                        dl.RECEIVED_QUANTITY = Convert.ToInt32(dr["RECEIVED_QUANTITY"]);
                        dl.IS_POSTED= Convert.ToInt32(dr["BPIL_IS_POSTED"]);
                        dl.ALTERNATE_QUANTITY = Convert.ToInt32(dr["ALTERNATE_QUANTITY"]);
                        dl.ALTERNATE_UOM = dr["ALTERNATE_UOM"].ToString();
                        dl.BATCH_FROM = Convert.ToInt32(dr["BATCH_FROM"].ToString());
                        dl.BATCH_TO = Convert.ToInt32(dr["BATCH_TO"].ToString());
                        dl.BATCH_FROM_NAME = (dr["BATCH_FROM_NAME"].ToString());
                        dl.BATCH_TO_NAME = (dr["BATCH_TO_NAME"].ToString());
                        dl.LOT_NO = (dr["LOT_NO"].ToString());
                        List<DataEntry_Line_LiveStock> livestock = new List<DataEntry_Line_LiveStock>();
                        foreach (DataRow dr1 in ds.Tables[2].Rows)
                        {
                            DataEntry_Line_LiveStock d = new DataEntry_Line_LiveStock();
                            if (dr["ITEM_ID"].ToString() == dr1["ITEM_ID"].ToString())
                            {
                                d.Livestock_No = Convert.ToString(dr1["Serial_Number"]);
                                d.Total_Units = Convert.ToDecimal(dr1["Qty"]);
                                d.item_id = Convert.ToInt32(dr1["ITEM_ID"]);
                                livestock.Add(d);
                            }
                        }
                        dl.livestock = livestock;

                        line.Add(dl);
                    }

                    obj.Status = "success";
                    obj.Message = "Transfer entry details data.";
                    obj.Data = new { header, line };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = "transfer is not available.";
                    obj.Data = new { };
                }
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }

        [Route("api/get_bpil_uom_list")]
        [HttpGet]
        public IActionResult getbpil_uom_list()
        {
            ResponseModel obj = new ResponseModel();
           
            try
            {
               
                DataTable dt = _dataentryDB.Get_BPIL_UOM_List();
                if (dt.Rows.Count > 0)
                {
                    var List = JsonConvert.SerializeObject(dt);


                    obj.Status = "success";
                    obj.Message = "BPIL UOM list data.";
                    obj.Data = new { List };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = "BPIL UOM  is not available.";
                    obj.Data = new { };
                }
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }

    }
}