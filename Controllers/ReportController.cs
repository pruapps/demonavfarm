using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FarmIT_Api.database_accesslayer;
using Microsoft.AspNetCore.Authorization;
using FarmIT_Api.Models;
using System.Data;
using Newtonsoft.Json;
namespace FarmIT_Api.Controllers
{
    /// <summary>
    /// CR-NTS (point-7) 18 Apr 2023(I)Transfer stock reportII)Ledger stock report done
    /// </summary>
    [ApiController]
    [Authorize]
    [RESTAuthorizeAttribute]
    public class ReportController : ControllerBase
    {
        private readonly IReportDB _reportDB;
       
        public ReportController(IReportDB reportDB)
        {
            _reportDB = reportDB;
        }

        [Route("api/get_weekly_report")]
        [HttpGet]
        public IActionResult WEEKLY_REPORT(int NOB_ID,int LOB_ID,int COMPANY_ID,string FROM,string TO,int BATCH_ID)
        {
            ResponseModel obj = new ResponseModel();          
            try
            {
                if (NOB_ID == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Nature of business can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (LOB_ID == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Line of business can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (COMPANY_ID == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company id can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }                
                if (FROM == null || FROM == "null")
                {
                    obj.Status = "failure";
                    obj.Message = "From can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (TO == null || TO == "null")
                {
                    obj.Status = "failure";
                    obj.Message = "To can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (BATCH_ID == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Batch can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _reportDB.Get_Weekly_Report(NOB_ID,LOB_ID,COMPANY_ID,FROM,TO,BATCH_ID);
              
                if (ds.Tables[1].Rows.Count > 0)
                {
                    
                    List<string> label_list = new List<string>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        label_list.Add(dr["LABELS"].ToString());                        
                    }
                    string[] Labels = label_list.ToArray();
                    
                    List<ReportModel> Batches = new List<ReportModel>();
                    string batch_name = "";
                    string week_no = "";
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        ReportModel rm = new ReportModel();
                        if (week_no != dr["WEEK_NO"].ToString())
                        {
                            batch_name = dr["BATCH_NAME"].ToString();
                            week_no = dr["WEEK_NO"].ToString();

                            List<ReportValueData> data_list = new List<ReportValueData>();
                            List<string> Value_list = new List<string>();
                            DataTable dt = ds.Tables[1];
                            ReportValueData dd = new ReportValueData();

                            foreach (DataRow dr1 in dt.Select("BATCH_NAME = '" + batch_name + "' AND WEEK_NO = '"+week_no+"'"))
                            {
                                Value_list.Add(dr1["LABEL_VALUES"].ToString());
                            }

                            
                            string[] ar_values = Value_list.ToArray();
                          
                            dd.Values = ar_values;
                            data_list.Add(dd);
                            
                            rm.batch_no = batch_name;
                            rm.week_no = week_no;
                            rm.details = data_list;
                            Batches.Add(rm);
                        }                       
                    }
                    
                    obj.Status = "success";
                    obj.Message = "Weekly Report data.";
                    obj.Data =new { Labels, Batches };
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

        [Route("api/get_laying_report")]
        [HttpGet]
        public IActionResult LAYING_REPORT(int Batch_Id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (Batch_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Batch can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _reportDB.Get_Laying_Report(Batch_Id);

                List<ReportValueData> values = new List<ReportValueData>();
                List<ReportValueData> values2 = new List<ReportValueData>();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(ds.Tables[0]);
                    var result2 = JsonConvert.SerializeObject(ds.Tables[1]);
                    obj.Status = "success";
                    obj.Message = "Laying Report data.";
                    obj.Data = new { result, result2 };

                    //ReportValueData dd = new ReportValueData();
                    //List<string> label_list = new List<string>();
                    //ReportValueData dd2 = new ReportValueData();
                    //List<string> label_list2 = new List<string>();

                    //foreach (DataRow dr in ds.Tables[0].Rows)
                    //{
                    //    label_list.Add(dr["LABELS"].ToString());
                    //}
                    //foreach (DataRow dr in ds.Tables[2].Rows)
                    //{
                    //    label_list2.Add(dr["LABELS"].ToString());
                    //}

                    //string[] Labels = label_list.ToArray();
                    //string[] Labels2 = label_list2.ToArray();

                    //List<Laying_Report> Value_list = new List<Laying_Report>();
                    //List<Laying_Report_SUMM> Value_list2 = new List<Laying_Report_SUMM>();
                    //foreach (DataRow dr in ds.Tables[1].Rows)
                    //{
                    //    Laying_Report bl = new Laying_Report();
                    //    bl.Entry_Date = dr["Entry_Date"].ToString();
                    //    bl.Male = Convert.ToInt32(dr["Male"].ToString());
                    //    bl.Female = Convert.ToInt32(dr["Female"].ToString());
                    //    bl.Culls_M = Convert.ToInt32(dr["Culls_M"].ToString());
                    //    bl.Culls_F = Convert.ToInt32(dr["Culls_F"].ToString());
                    //    bl.Sex_error = Convert.ToInt32(dr["Sex_error"].ToString());
                    //    bl.Comu_Mortality = Convert.ToInt32(dr["Comu_Mortality"].ToString());
                    //    bl.Comu_Mortality_Per = Convert.ToDecimal(dr["Comu_Mortality_Per"].ToString());
                    //    bl.Rem_Birds = Convert.ToInt32(dr["Rem_Birds"].ToString());
                    //    bl.Weight_Male = Convert.ToDecimal(dr["Weight_Male"].ToString());
                    //    bl.Weight_Female = Convert.ToDecimal(dr["Weight_Female"].ToString());
                    //    bl.Growth_Per_M = Convert.ToDecimal(dr["Growth_Per_M"].ToString());
                    //    bl.Growth_Per_F = Convert.ToDecimal(dr["Growth_Per_F"].ToString());
                    //    bl.Feed_Consum_M = Convert.ToDecimal(dr["Feed_Consum_M"].ToString());
                    //    bl.Feed_Consum_F = Convert.ToDecimal(dr["Feed_Consum_F"].ToString());
                    //    bl.Comu_Feed_Consum = Convert.ToDecimal(dr["Comu_Feed_Consum"].ToString());
                    //    bl.FCR = Convert.ToDecimal(dr["FCR"].ToString());
                    //    bl.Age_Day = Convert.ToInt32(dr["Age_Day"].ToString());
                    //    bl.Age_Week = Convert.ToInt32(dr["Age_Week"].ToString());
                    //    bl.Eggs_Collection = Convert.ToDecimal(dr["Eggs_Collection"].ToString());
                    //    bl.Table_Collection = Convert.ToDecimal(dr["Table_Eggs"].ToString());
                    //    bl.Creck_Eggs = Convert.ToDecimal(dr["Creck_Eggs"].ToString());
                    //    bl.Jumbo_Eggs = Convert.ToDecimal(dr["Jumbo_Eggs"].ToString());
                    //    //bl.Pullet_Eggs = Convert.ToDecimal(dr["Pullet_Eggs"].ToString());
                    //    bl.Current_Feed_Kgs = Convert.ToDecimal(dr["Current_Feed_Kgs"].ToString());
                    //    bl.Current_Feed_Bird_grams = Convert.ToDecimal(dr["Current_Feed_Bird_grams"].ToString());
                    //    bl.Current_Feed_Egg_grams = Convert.ToDecimal(dr["Current_Feed_Egg_grams"].ToString());
                    //    bl.Cumulative_Feed_Egg_grams = Convert.ToDecimal(dr["Cumulative_Feed_Egg_grams"].ToString());
                    //    bl.Total_Cost = Convert.ToDecimal(dr["Total_Cost"].ToString());
                    //    Value_list.Add(bl);
                    //}

                    //foreach (DataRow dr in ds.Tables[3].Rows)
                    //{
                    //    Laying_Report_SUMM bl = new Laying_Report_SUMM();
                    //    bl.Male = Convert.ToInt32(dr["Male"].ToString());
                    //    bl.Female = Convert.ToInt32(dr["Female"].ToString());
                    //    bl.Culls_M = Convert.ToInt32(dr["Culls_M"].ToString());
                    //    bl.Culls_F = Convert.ToInt32(dr["Culls_F"].ToString());
                    //    bl.Sex_error = Convert.ToInt32(dr["Sex_error"].ToString());
                    //    bl.Rem_Birds = Convert.ToInt32(dr["Rem_Birds"].ToString());
                    //    bl.Weight_Male = Convert.ToDecimal(dr["Weight_Male"].ToString());
                    //    bl.Weight_Female = Convert.ToDecimal(dr["Weight_Female"].ToString());
                    //    bl.Feed_Consum_M = Convert.ToDecimal(dr["Feed_Consum_M"].ToString());
                    //    bl.Feed_Consum_F = Convert.ToDecimal(dr["Feed_Consum_F"].ToString());
                    //    bl.Eggs_Collection = Convert.ToDecimal(dr["Eggs_Collection"].ToString());
                    //    //bl.Table_Collection = Convert.ToDecimal(dr["Table_Eggs"].ToString());
                    //    //bl.Creck_Eggs = Convert.ToDecimal(dr["Creck_Eggs"].ToString());
                    //    bl.Total_Cost = Convert.ToDecimal(dr["Total_Cost"].ToString());
                    //    Value_list2.Add(bl);
                    //}

                    //dd.Values = Value_list;
                    //values.Add(dd);

                    //dd2.Values = Value_list2;
                    //values2.Add(dd2);

                    //obj.Status = "success";
                    //obj.Message = "Laying report data.";
                    //obj.Data = new { Labels, Values = values , Labels2, Values2 = values2 };
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

        [Route("api/get_rearing_report")]
        [HttpGet]
        public IActionResult REARING_REPORT(int Batch_Id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (Batch_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Batch can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _reportDB.Get_Rearing_Report(Batch_Id);

                List<ReportValueData> values = new List<ReportValueData>();
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ReportValueData dd = new ReportValueData();
                    List<string> label_list = new List<string>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        label_list.Add(dr["LABELS"].ToString());
                    }

                    string[] Labels = label_list.ToArray();

                    List<Rearing_Report> Value_list = new List<Rearing_Report>();
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        Rearing_Report bl = new Rearing_Report();
                        bl.Entry_Date = dr["Entry_Date"].ToString();
                        bl.Male = Convert.ToInt32(dr["Male"].ToString());
                        bl.Female = Convert.ToInt32(dr["Female"].ToString());
                        bl.Culls_M = Convert.ToInt32(dr["Culls_M"].ToString());
                        bl.Culls_F = Convert.ToInt32(dr["Culls_F"].ToString());
                        bl.Comu_Mortality = Convert.ToInt32(dr["Comu_Mortality"].ToString());
                        bl.Comu_Mortality_Per = Convert.ToDecimal(dr["Comu_Mortality_Per"].ToString());
                        bl.Rem_Birds = Convert.ToInt32(dr["Rem_Birds"].ToString());
                        bl.Weight_Male = Convert.ToInt32(dr["Weight_Male"].ToString());
                        bl.Weight_Female = Convert.ToInt32(dr["Weight_Female"].ToString());
                        bl.Growth_Per = Convert.ToDecimal(dr["Growth_Per"].ToString());
                        bl.Feed_Consum_M = Convert.ToInt32(dr["Feed_Consum_M"].ToString());
                        bl.Feed_Consum_F = Convert.ToInt32(dr["Feed_Consum_F"].ToString());
                        bl.Comu_Feed_Consum = Convert.ToInt32(dr["Comu_Feed_Consum"].ToString());
                        bl.FCR = Convert.ToDecimal(dr["FCR"].ToString());
                        bl.Age_Day = Convert.ToInt32(dr["Age_Day"].ToString());
                        bl.Age_Week = Convert.ToInt32(dr["Age_Week"].ToString());
                        Value_list.Add(bl);
                    }

                    dd.Values = Value_list;
                    values.Add(dd);

                    obj.Status = "success";
                    obj.Message = "Rearing report data.";
                    obj.Data = new { Labels, Values = values };
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

        [Route("api/get_breeding_laying_report_part1")]
        [HttpGet]
        public IActionResult BREEDING_LAYING_REPORT(int Batch_Id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (Batch_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Batch can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _reportDB.Get_BreedingLaying_Report(Batch_Id);

                List<ReportValueData> values = new List<ReportValueData>();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(ds.Tables[0]);
                    var result2 = JsonConvert.SerializeObject(ds.Tables[1]);
                    obj.Status = "success";
                    obj.Message = "Laying Report data.";
                    obj.Data = new { result, result2 };
                    //ReportValueData dd = new ReportValueData();
                    //List<string> label_list = new List<string>();

                    //foreach (DataRow dr in ds.Tables[0].Rows)
                    //{
                    //    label_list.Add(dr["LABELS"].ToString());
                    //}

                    //string[] Labels = label_list.ToArray();
                    //List<Breeding_Laying_Report_Part1> Value_list = new List<Breeding_Laying_Report_Part1>();
                    //foreach (DataRow dr in ds.Tables[1].Rows)
                    //{
                    //    var result = JsonConvert.SerializeObject(ds.Tables[0]);
                    //    var result2 = JsonConvert.SerializeObject(ds.Tables[1]);
                    //    obj.Status = "success";
                    //    obj.Message = "Laying Report data2.";
                    //    obj.Data = new { result, result2 };
                    //Breeding_Laying_Report_Part1 bl = new Breeding_Laying_Report_Part1();
                    //bl.Batch_No = dr["BATCH_NO"].ToString();
                    //bl.User_Name = dr["USER_NAME"].ToString();
                    //bl.Start_Date = dr["START_DATE"].ToString();
                    //bl.Entry_Date = dr["ENTRY_DATE"].ToString();
                    //bl.Week = Convert.ToInt32(dr["WEEK"].ToString());
                    //bl.Breed = dr["Breed"].ToString();
                    //bl.Location = dr["LOCATION"].ToString();
                    //bl.age_day = Convert.ToInt32(dr["AGE_DAY"].ToString());
                    //bl.age_week = Convert.ToInt32(dr["AGE_WEEK"].ToString());
                    //bl.opening_balance = Convert.ToInt32(dr["OPENING_BALANCE"].ToString());
                    //bl.remaining_birds = Convert.ToInt32(dr["REMAINING_BIRDS"].ToString());
                    //bl.hatching_date = dr["HATCH_DATE"].ToString();
                    //bl.mortality = Convert.ToInt32(dr["MORTALITY"].ToString());
                    //bl.culls = Convert.ToInt32(dr["CULLS"].ToString());
                    //bl.sex_error = Convert.ToInt32(dr["Sex_Error"].ToString());
                    //bl.mortality_per = Convert.ToDecimal(dr["MORTALITY_PER"].ToString());
                    //bl.feed_per_day = Convert.ToInt32(dr["FEED_PER_DAY"].ToString());
                    //bl.fcr = Convert.ToDecimal(dr["FCR"].ToString());
                    ////bl.body_weight_kg = Convert.ToDecimal(dr["BODY_WEIGHT_KG"].ToString());
                    ////bl.growth_per = Convert.ToDecimal(dr["GROWTH_PER"].ToString());
                    //Value_list.Add(bl);
                    //}

                    //dd.Values = Value_list;
                    //values.Add(dd);

                    //obj.Status = "success";
                    //obj.Message = "Breeding laying report data part 1";
                    //obj.Data = new { Labels, Values = values };
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

        [Route("api/get_breeding_laying_report_part2")]
        [HttpGet]
        public IActionResult BREEDING_LAYING_REPORT_PART2(int Batch_Id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (Batch_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Batch can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _reportDB.Get_BreedingLaying_Report_Part2(Batch_Id);

                List<ReportValueData> values = new List<ReportValueData>();
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ReportValueData dd = new ReportValueData();
                    List<string> label_list = new List<string>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        label_list.Add(dr["LABELS"].ToString());
                    }

                    string[] Labels = label_list.ToArray();
                    List<Breeding_Laying_Report_Part2> Value_list = new List<Breeding_Laying_Report_Part2>();
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        Breeding_Laying_Report_Part2 bl = new Breeding_Laying_Report_Part2();
                        bl.Batch_No = dr["BATCH_NO"].ToString();
                        bl.User_Name = dr["USER_NAME"].ToString();
                        bl.Start_Date = dr["START_DATE"].ToString();
                        bl.Entry_Date = dr["ENTRY_DATE"].ToString();
                        bl.Week = Convert.ToInt32(dr["WEEK"].ToString());
                        bl.Breed = dr["Breed"].ToString();
                        bl.Location = dr["LOCATION"].ToString();
                        bl.total_feed_cost = Convert.ToDecimal(dr["TOTAL_FEED_COST"].ToString());
                        bl.total_feed_consumed = Convert.ToDecimal(dr["TOTAL_FEED_CONSUMED"].ToString());
                        bl.commercial_eggs_qty = Convert.ToInt32(dr["TOTAL_EGGS"].ToString());
                        bl.hatchable_eggs_qty = Convert.ToInt32(dr["HATCHABLE_EGG"].ToString());
                        bl.crack_egg = Convert.ToInt32(dr["CRACK_EGGS"].ToString());
                        bl.water_cost = Convert.ToDecimal(dr["WATER_COST"].ToString());
                        bl.electricity_cost = Convert.ToDecimal(dr["ELECTRICITY_COST"].ToString());
                        bl.labour_cost = Convert.ToDecimal(dr["LABOUR_COST"].ToString());
                        bl.electricity_units = Convert.ToDecimal(dr["ELECTRICITY_UNITS"].ToString());
                        bl.water_units = Convert.ToDecimal(dr["WATER_UNITS"].ToString());
                        bl.vaccine_cost = Convert.ToDecimal(dr["VACCINE_COST"].ToString());
                        bl.vaccine_units = Convert.ToDecimal(dr["VACCINE_UNITS"].ToString());
                        bl.batch_performance = Convert.ToDecimal(dr["BATCH_PERFORMANCE"].ToString());
                        Value_list.Add(bl);
                    }

                    dd.Values = Value_list;
                    values.Add(dd);

                    obj.Status = "success";
                    obj.Message = "Breeding laying report data part 2.";
                    obj.Data = new { Labels, Values = values };
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

        [Route("api/get_hatchery_report_part1")]
        [HttpGet]
        public IActionResult HATCHERY_REPORT_PART1(int Batch_Id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (Batch_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Batch details can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _reportDB.Get_Hatchery_Report(Batch_Id);

                var ds1 = _reportDB.Get_Dashboard_Hatch_Output_Graph(Batch_Id);

                List<ReportValueData> values = new List<ReportValueData>();
                List<ReportValueData> values2 = new List<ReportValueData>();
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    ReportValueData dd = new ReportValueData();
                //    ReportValueData dd2 = new ReportValueData();
                //    List<string> label_list = new List<string>();
                //    List<string> label_list2 = new List<string>();

                    //    foreach (DataRow dr in ds.Tables[1].Rows)
                    //    {
                    //        label_list.Add(dr["LABELS"].ToString());
                    //    }
                    //    foreach (DataRow dr in ds.Tables[2].Rows)
                    //    {
                    //        label_list2.Add(dr["LABELS"].ToString());
                    //    }
                    //    string[] Labels = label_list.ToArray();
                    //    string[] Labels2 = label_list2.ToArray();
                    //    List<HATCHERY_REPORT_PART1> Value_list = new List<HATCHERY_REPORT_PART1>();
                    //    List<HATCHERY_REPORT_SUMM> Value_list2 = new List<HATCHERY_REPORT_SUMM>();

                    //    foreach (DataRow dr in ds.Tables[0].Rows)
                    //    {
                    //        HATCHERY_REPORT_PART1 hr = new HATCHERY_REPORT_PART1();
                    //        hr.Batch_No = dr["BATCH_NO"].ToString();
                    //        hr.Supervisor_Name = dr["Supervisor_Name"].ToString();
                    //        hr.Egg_Setting_Date = dr["Egg_Setting_Date"].ToString();
                    //        hr.Entry_Date = dr["ENTRY_DATE"].ToString();
                    //        hr.Week = Convert.ToInt32(dr["WEEK"].ToString());
                    //        hr.Breed = dr["BREED_NAME"].ToString();
                    //        hr.Location = dr["LOCATION_NAME"].ToString();
                    //        hr.Egg_Laying_Date = dr["Egg_Laying_Date"].ToString();
                    //        hr.Opening_Egg = Convert.ToInt32(dr["Opening_EGG"].ToString());
                    //        hr.Crack_Eggs = Convert.ToInt32(dr["Crack_Eggs"].ToString());
                    //        hr.Setable_hatching_Egg = Convert.ToInt32(dr["Setable_hatching_Egg"].ToString());
                    //        hr.DOC_Produced_M = Convert.ToInt32(dr["DOC_Produced_M"].ToString());
                    //        hr.DOC_Produced_F = Convert.ToInt32(dr["DOC_Produced_F"].ToString());
                    //        hr.Culls_Chicks = Convert.ToInt32(dr["CULLS_CHICKS"].ToString());
                    //        hr.DOC_COST = Convert.ToDecimal(dr["DOC_COST"].ToString());

                    //        hr.SaleQty = Convert.ToDecimal(dr["SaleQty"].ToString());
                    //        hr.SaleAmount = Convert.ToDecimal(dr["SaleAmount"].ToString());

                    //        Value_list.Add(hr);
                    //    }
                    //    foreach (DataRow dr in ds.Tables[3].Rows)
                    //    {
                    //        HATCHERY_REPORT_SUMM hr = new HATCHERY_REPORT_SUMM();

                    //        hr.Opening_Egg = Convert.ToInt32(dr["Opening_EGG"].ToString());
                    //        hr.Opening_Amount= Convert.ToDecimal(dr["OP_Cost"].ToString());
                    //        hr.Crack_Eggs = Convert.ToInt32(dr["SETABLE_HATCHING_EGG"].ToString());
                    //        hr.Remaining = Convert.ToInt32(dr["Remaining"].ToString());
                    //        hr.DOC_Produced_M = Convert.ToInt32(dr["DOC_Produced_M"].ToString());
                    //        hr.DOC_Produced_F = Convert.ToInt32(dr["DOC_Produced_F"].ToString());
                    //        hr.Culls_Chicks = Convert.ToInt32(dr["CULLS_CHICKS"].ToString());


                    //        hr.SaleQty = Convert.ToDecimal(dr["SaleQty"].ToString());
                    //        hr.SaleAmount = Convert.ToDecimal(dr["SaleAmount"].ToString());
                    //        hr.DOC_COST = Convert.ToDecimal(dr["Doc_Cost"].ToString());
                    //        Value_list2.Add(hr);
                    //    }

                    //    dd.Values = Value_list;
                    //    values.Add(dd);

                    //    dd2.Values = Value_list2;
                    //    values2.Add(dd2);

                    //    obj.Status = "success";
                    //    obj.Message = "Hatchery report data part 1.";
                    //    obj.Data = new { Labels, Values = values,Labels2,Values2=values2 };
                    //}
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];

                    DataTable dt1 = new DataTable();
                    dt1 = ds.Tables[3];

                    DataTable dt3 = new DataTable();
                    dt3 = ds1.Tables[0];


                    obj.Status = "success";
                    obj.Message = "Hatchery report data part 1.";
                    obj.Data = new { Labels=new { }, Values = dt, Labels2= new { }, Values2 = dt1 , Values3 = dt3 };
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

        [Route("api/get_hatchery_report_part2")]
        [HttpGet]
        public IActionResult HATCHERY_REPORT_PART2(int Batch_Id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (Batch_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Batch details can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _reportDB.Get_Hatchery_Report(Batch_Id);

                List<ReportValueData> values = new List<ReportValueData>();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ReportValueData dd = new ReportValueData();
                    List<string> label_list = new List<string>();

                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        label_list.Add(dr["LABELS"].ToString());
                    }

                    string[] Labels = label_list.ToArray();
                    List<HATCHERY_REPORT_PART2> Value_list = new List<HATCHERY_REPORT_PART2>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        HATCHERY_REPORT_PART2 hr = new HATCHERY_REPORT_PART2();
                        hr.Batch_No = dr["BATCH_NO"].ToString();
                        hr.Supervisor_Name = dr["Supervisor_Name"].ToString();
                        hr.Egg_Setting_Date = dr["Egg_Setting_Date"].ToString();
                        hr.Entry_Date = dr["ENTRY_DATE"].ToString();
                        hr.Week = Convert.ToInt32(dr["WEEK"].ToString());
                        hr.Breed = dr["BREED_NAME"].ToString();
                        hr.Location = dr["LOCATION_NAME"].ToString();
                        hr.Total_Water_Cost = Convert.ToDecimal(dr["Water_Expense"].ToString());
                        hr.Total_Electricity_Cost = Convert.ToDecimal(dr["Electricity_Expense"].ToString());
                        hr.Total_Labour_Cost = Convert.ToDecimal(dr["labour_Expense"].ToString());
                        hr.Total_Electricity_Units = Convert.ToDecimal(dr["Electricity_Units"].ToString());
                        hr.Total_Water_Consumed_Units = Convert.ToDecimal(dr["Water_Units"].ToString());
                        hr.Total_Vaccine_Cost = Convert.ToDecimal(dr["VACCINE_COST"].ToString());
                        hr.Total_Vaccine_Units = Convert.ToDecimal(dr["VACCINE_UNITS"].ToString());
                        hr.Batch_Performance_Amount = Convert.ToDecimal(dr["batch_amount"].ToString());
                        Value_list.Add(hr);
                    }

                    dd.Values = Value_list;
                    values.Add(dd);

                    obj.Status = "success";
                    obj.Message = "Hatchery report data part 2.";
                    obj.Data = new { Labels, Values = values };
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

        [Route("api/get_flock_wise_fcr")]
        [HttpGet]
        public IActionResult Flock_Wise_Fcr_Report(int Company_id, int Lob_Id, string Batch_Id, int? Location_Id, string From_Date,string To_Date)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (Company_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company details can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Lob_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Line of business can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _reportDB.Get_FlockWiseFcr_Report(Company_id, Lob_Id, Batch_Id, Location_Id, From_Date, To_Date);

                List<ReportValueData> values = new List<ReportValueData>();
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    ReportValueData dd = new ReportValueData();
                //    List<string> label_list = new List<string>();

                //    foreach (DataRow dr in ds.Tables[1].Rows)
                //    {
                //        label_list.Add(dr["LABELS"].ToString());
                //    }

                //    string[] Labels = label_list.ToArray();
                //    List<Flock_Wise_Fcr_Report> Value_list = new List<Flock_Wise_Fcr_Report>();
                //    List<Flock_Wise_Fcr_Report2> Value_list2 = new List<Flock_Wise_Fcr_Report2>();
                //    List<Flock_Wise_Fcr_Report3> Value_list3 = new List<Flock_Wise_Fcr_Report3>();
                //    foreach (DataRow dr in ds.Tables[0].Rows)
                //    {

                //        if (Lob_Id != 2 && Lob_Id != 1)
                //        {
                //            Flock_Wise_Fcr_Report hr = new Flock_Wise_Fcr_Report();
                //            hr.Batch_No = dr["BATCH_NO"].ToString();
                //            hr.Breed = dr["Breed"].ToString();
                //            hr.Location_name = dr["LOCATION_NAME"].ToString();
                //            hr.Placement_Date = dr["Placement_Date"].ToString();
                //            hr.Depletion_Date = dr["Depletion_Date"].ToString();
                //            hr.Age = Convert.ToInt32(dr["Age_in_WK"].ToString());
                //            hr.Age_in_days = Convert.ToInt32(dr["Age_in_Days"].ToString());
                //            hr.Chicks_Placed = Convert.ToInt32(dr["Chicks_Placed"].ToString());
                //            hr.Mortality = Convert.ToDecimal(dr["Mortality"].ToString());
                //            hr.Previous_day_Mortality = Convert.ToDecimal(dr["Previous_day_Mortality"].ToString());
                //            hr.DBY_Mortaltiy = Convert.ToDecimal(dr["DBY_Mortaltiy"].ToString());
                //            hr.Total_Culls = Convert.ToDecimal(dr["TOTAL_CULLS"].ToString());
                //            hr.Mortality_Per = Convert.ToDecimal(dr["Mortality_Per"].ToString());
                //            hr.Remaining_Chicks = Convert.ToDecimal(dr["Remaining_Chicks"].ToString());
                //            hr.Output = Convert.ToDecimal(dr["total_Output"].ToString());
                //            hr.Feed_consumption = Convert.ToDecimal(dr["Feed_Consumption"].ToString());
                //            hr.Per_Bird_Feed_Consume = Convert.ToDecimal(dr["Per_Bird_Feed_Consume"].ToString());
                //            hr.Average_Weight = Convert.ToDecimal(dr["Average_Weight"].ToString());
                //            hr.Total_Body_Weight = Convert.ToDecimal(dr["Total_Body_Weight"].ToString());
                //            hr.Final_Avg_body_weight = Convert.ToDecimal(dr["Final_Avgerage_weight"].ToString());
                //            hr.Fcr = Convert.ToDecimal(dr["FCR"].ToString());
                //            hr.Livability_Per = Convert.ToDecimal(dr["Livability_Per"].ToString());
                //            hr.PEF = Convert.ToDecimal(dr["PEF"].ToString());
                //            Value_list.Add(hr);
                //        }
                //        else if (Lob_Id == 1)
                //        {
                //            Flock_Wise_Fcr_Report3 hr3 = new Flock_Wise_Fcr_Report3();
                //            hr3.Batch_No = dr["BATCH_NO"].ToString();
                //            hr3.Age = Convert.ToInt32(dr["Age_in_WK"].ToString());
                //            hr3.Age_in_days = Convert.ToInt32(dr["Age_in_Days"].ToString());
                //            hr3.Opening_M = Convert.ToInt32(dr["Opening_M"].ToString());
                //            hr3.Opening_F = Convert.ToInt32(dr["Opening_F"].ToString());
                //            hr3.Closing_M = Convert.ToInt32(dr["Closing_M"].ToString());
                //            hr3.Closing_F = Convert.ToInt32(dr["Closing_F"].ToString());
                //            hr3.Hatch_Egg = Convert.ToInt32(dr["Hatch_egg"].ToString());
                //            hr3.Table_Egg = Convert.ToInt32(dr["Table_Egg"].ToString());
                //            hr3.FCR = Convert.ToDecimal(dr["FCR"].ToString());
                //            hr3.Feed_consumption = Convert.ToDecimal(dr["Feed_Consumption"].ToString());
                //            hr3.Per_Bird_Feed_Consume = Convert.ToDecimal(dr["Per_Bird_Feed_Consume"].ToString());
                //            hr3.Per_Egg_Feed = Convert.ToDecimal(dr["Avg_Feed_per_Egg"].ToString());
                //            hr3.Avg_Egg_Weight = Convert.ToDecimal(dr["Avg_Egg_wt"].ToString());
                //            Value_list3.Add(hr3);
                //        }
                //        else if(Lob_Id == 2)
                //        {
                //            Flock_Wise_Fcr_Report2 hr2 = new Flock_Wise_Fcr_Report2();
                //            hr2.Batch_No = dr["BATCH_NO"].ToString();
                //            hr2.Chicks_Placed = Convert.ToInt32(dr["Chicks_Placed"].ToString());
                //            hr2.Cracked_EGGS = Convert.ToInt32(dr["Cracked_EGGS"].ToString());
                //            hr2.Cracked_per = Convert.ToDecimal(dr["Cracked_per"].ToString());
                //            hr2.Setable_EGGS = Convert.ToDecimal(dr["Setable_EGGS"].ToString());
                //            hr2.Egg_Setting_Date = dr["Egg_Setting_Date"].ToString();
                //            hr2.Hatch_transfer_date = dr["Egg_Transfer_Date"].ToString();
                //            hr2.Infertile_EGGS = Convert.ToDecimal(dr["Infertile_EGGS"].ToString());
                //            hr2.Infertile_EGGS_per = Convert.ToDecimal(dr["Infertile_EGGS_per"].ToString());
                //            hr2.FDOC = Convert.ToDecimal(dr["FDOC"].ToString());
                //            hr2.MDOC = Convert.ToDecimal(dr["MDOC"].ToString());
                //            hr2.CULL = Convert.ToDecimal(dr["CULL"].ToString());
                //            Value_list2.Add(hr2);
                //        }
                //    }

                //    if (Lob_Id == 1)
                //    {
                //        dd.Values = Value_list3;
                //    }
                //    else if (Lob_Id == 2)
                //    {
                //        dd.Values = Value_list2;
                //    }
                //    else
                //    { dd.Values = Value_list; }


                //    values.Add(dd);

                //    obj.Status = "success";
                //    obj.Message = "Flock Wise Fcr Report.";
                //    obj.Data = new { Labels, Values = values };
                //}
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var label = JsonConvert.SerializeObject(ds.Tables[0]);
                    var value = JsonConvert.SerializeObject(ds.Tables[0]);
                    obj.Status = "success";
                    obj.Message = "Flock Wise Fcr Report.";
                    obj.Data = new { Labels=label, Values = value };
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

        [Route("api/get_cbf_report_part1")]
        [HttpGet]
        public IActionResult CBF_REPORT_PART1(int Batch_Id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (Batch_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Batch details can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _reportDB.Get_Cbf_Report(Batch_Id);

                List<ReportValueData> values = new List<ReportValueData>();
                List<ReportValueData> values2 = new List<ReportValueData>();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //ReportValueData dd = new ReportValueData();
                    //ReportValueData dd2 = new ReportValueData();
                    //List<string> label_list = new List<string>();
                    //List<string> label_list2 = new List<string>();
                    // foreach (DataRow dr in ds.Tables[0].Rows)
                    //{
                    //    label_list.Add(dr["LABELS"].ToString());
                    //}
                    //foreach (DataRow dr in ds.Tables[2].Rows)
                    //{
                    //    label_list2.Add(dr["LABELS2"].ToString());
                    //}
                    //string[] Labels = label_list.ToArray();
                    //string[] Labels2 = label_list2.ToArray();

                    //List<CBF_Report_Part1> Value_list = new List<CBF_Report_Part1>();
                    //List<CBF_Report_SUMM> Value_list2 = new List<CBF_Report_SUMM>();
                    //foreach (DataRow dr in ds.Tables[1].Rows)
                    //{
                    //    CBF_Report_Part1 bl = new CBF_Report_Part1();
                    //    bl.Batch_No = dr["BATCH_NO"].ToString();
                    //    bl.User_Name = dr["USER_NAME"].ToString();
                    //    bl.Start_Date = dr["START_DATE"].ToString();
                    //    bl.Entry_Date = dr["ENTRY_DATE"].ToString();
                    //    bl.Week = Convert.ToInt32(dr["WEEK"].ToString());
                    //    bl.Breed = dr["Breed"].ToString();
                    //    bl.Location = dr["LOCATION"].ToString();
                    //    bl.age_day = Convert.ToInt32(dr["AGE_DAY"].ToString());
                    //    bl.age_week = Convert.ToInt32(dr["AGE_WEEK"].ToString());
                    //    bl.opening_balance = Convert.ToInt32(dr["OPENING_BALANCE"].ToString());
                    //    bl.remaining_birds = Convert.ToInt32(dr["REMAINING_BIRDS"].ToString());
                    //    bl.hatching_date = dr["HATCH_DATE"].ToString();
                    //    bl.mortality = Convert.ToInt32(dr["MORTALITY"].ToString());
                    //    bl.culls = Convert.ToInt32(dr["CULLS"].ToString());
                    //    bl.mortality_per = Convert.ToDecimal(dr["MORTALITY_PER"].ToString());
                    //   // bl.body_weight_gm = Convert.ToDecimal(dr["BODY_WEIGHT_G"].ToString());
                    //     bl.body_weight_kg = Convert.ToDecimal(dr["BODY_WEIGHT_KG"].ToString());
                    //    bl.growth_per = Convert.ToDecimal(dr["GROWTH_PER"].ToString());
                    //    bl.Output = Convert.ToDecimal(dr["Coutput"].ToString());
                    //  //  bl.doc_output = Convert.ToDecimal(dr["doc_output"].ToString());
                    //    bl.total_feed = Convert.ToDecimal(dr["total_feed"].ToString());
                    //    bl.comm_feed = Convert.ToDecimal(dr["CUMM_feed"].ToString());
                    //    bl.FCR = Convert.ToDecimal(dr["fcr"].ToString());
                    //    bl.KPI = Convert.ToDecimal(dr["KPI"].ToString());
                    //    bl.Sale_qty = Convert.ToDecimal(dr["SaleQty"].ToString());
                    //    bl.Sale_amount = Convert.ToDecimal(dr["SaleAmount"].ToString());

                    //    Value_list.Add(bl);
                    //}

                    //foreach (DataRow dr in ds.Tables[3].Rows)
                    //{
                    //    CBF_Report_SUMM b2 = new CBF_Report_SUMM();
                    //     b2.opening_balance = Convert.ToInt32(dr["OPENING_BALANCE"].ToString());
                    //    b2.remaining_birds = Convert.ToInt32(dr["REMAINING_BIRDS"].ToString());
                    //    b2.mortality = Convert.ToInt32(dr["MORTALITY"].ToString());
                    //    b2.culls = Convert.ToInt32(dr["CULLS"].ToString());
                    //    b2.body_weight_kg = Convert.ToDecimal(dr["BODY_WEIGHT_KG"].ToString());
                    //    b2.Output = Convert.ToDecimal(dr["Coutput"].ToString());
                    //    b2.total_feed = Convert.ToDecimal(dr["total_feed"].ToString());
                    //    b2.FCR = Convert.ToDecimal(dr["fcr"].ToString());
                    //    b2.Age = Convert.ToDecimal(dr["AGE_DAY"].ToString());

                    //    b2.Cost_Bird = Convert.ToDecimal(dr["Cost_Per_Bird"].ToString());
                    //    b2.Sale_Qty = Convert.ToDecimal(dr["Sale_Qty"].ToString());
                    //    b2.Sale_Amount = Convert.ToDecimal(dr["Sale_Amount"].ToString());


                    //    Value_list2.Add(b2);
                    //}

                    //dd.Values = Value_list;
                    //values.Add(dd);
                    //dd2.Values = Value_list2;
                    //values2.Add(dd2);

                    DataTable dt = new DataTable();
                    DataTable dt1 = new DataTable();
                    dt = ds.Tables[1];
                    dt1 = ds.Tables[3];


                    obj.Status = "success";
                    obj.Message = "CBF report data part1.";
                    obj.Data = new { Labels=new { }, Values = dt,Labels2=new {},Values2= dt1 };
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

        [Route("api/get_cbf_report_part2")]
        [HttpGet]
        public IActionResult CBF_REPORT_PART2(int Batch_Id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (Batch_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Batch details can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _reportDB.Get_Cbf_Report_Part2(Batch_Id);

                List<ReportValueData> values = new List<ReportValueData>();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ReportValueData dd = new ReportValueData();
                    List<string> label_list = new List<string>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        label_list.Add(dr["LABELS"].ToString());
                    }

                    string[] Labels = label_list.ToArray();
                    List<CBF_Report_Part2> Value_list = new List<CBF_Report_Part2>();
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        CBF_Report_Part2 bl = new CBF_Report_Part2();
                        bl.Batch_No = dr["BATCH_NO"].ToString();
                        bl.User_Name = dr["USER_NAME"].ToString();
                        bl.Start_Date = dr["START_DATE"].ToString();
                        bl.Entry_Date = dr["ENTRY_DATE"].ToString();
                        bl.Week = Convert.ToInt32(dr["WEEK"].ToString());
                        bl.Breed = dr["Breed"].ToString();
                        bl.Location = dr["LOCATION"].ToString();
                        bl.total_feed_cost = Convert.ToDecimal(dr["TOTAL_FEED_COST"].ToString());
                        bl.total_feed_consumed = Convert.ToDecimal(dr["TOTAL_FEED"].ToString());
                        bl.fcr = Convert.ToDecimal(dr["FCR"].ToString());
                        bl.pef = Convert.ToDecimal(dr["PEF"].ToString());
                        bl.water_cost = Convert.ToDecimal(dr["WATER_COST"].ToString());
                        bl.electricity_cost = Convert.ToDecimal(dr["ELECTRICITY_COST"].ToString());
                        bl.labour_cost = Convert.ToDecimal(dr["LABOUR_COST"].ToString());
                        bl.electricity_units = Convert.ToDecimal(dr["ELECTRICITY_UNITS"].ToString());
                        bl.water_units = Convert.ToDecimal(dr["WATER_UNITS"].ToString());
                        bl.vaccine_cost = Convert.ToDecimal(dr["VACCINE_COST"].ToString());
                        bl.vaccine_units = Convert.ToDecimal(dr["VACCINE_UNITS"].ToString());
                        bl.batch_performance = Convert.ToDecimal(dr["BATCH_PERFORMANCE"].ToString());
                        Value_list.Add(bl);
                    }

                    dd.Values = Value_list;
                    values.Add(dd);

                    obj.Status = "success";
                    obj.Message = "CBF report data part2.";
                    obj.Data = new { Labels, Values = values };
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

        [Route("api/get_feed_report")]
        [HttpGet]
        public IActionResult FEED_REPORT(int Company_Id,int Lob_Id,int Batch_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (Company_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company details can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Lob_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Lob details can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _reportDB.Get_feed_Report(Company_Id,Lob_Id,Batch_id);

                List<ReportValueData> values = new List<ReportValueData>();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //ReportValueData dd = new ReportValueData();
                    //List<string> label_list = new List<string>();

                    //foreach (DataRow dr in ds.Tables[1].Rows)
                    //{
                    //    label_list.Add(dr["LABELS"].ToString());
                    //}

                    //string[] Labels = label_list.ToArray();
                    //List<Feed_Report> Value_list = new List<Feed_Report>();
                    //foreach (DataRow dr in ds.Tables[0].Rows)
                    //{
                    //    Feed_Report bl = new Feed_Report();
                    //    bl.Batch_Date = dr["BATCH_DATE"].ToString();
                    //    bl.Batch_No = dr["BATCH_NO"].ToString();
                    //    bl.Formulation_Name = dr["FORMULATION_NAME"].ToString();
                    //    bl.Plant_Incharge = dr["PLANT_INCHARGE"].ToString();
                    //    bl.Breed = dr["BREED_NAME"].ToString();
                    //    bl.Location_name = dr["LOCATION_NAME"].ToString();
                    //    //bl.FG_Item_Name = dr["FG_ITEM_NAME"].ToString();
                    //    //bl.FG_Item = dr["FG_ITEM"].ToString();
                    //    bl.FG_Item_Qty = Convert.ToDecimal(dr["FG_ITEM_QTY"].ToString());
                    //    bl.OP_Qty = Convert.ToDecimal(dr["Opening_Qty"].ToString());
                    //    bl.RM = dr["RM"].ToString();
                    //    bl.RM_Qty = Convert.ToDecimal(dr["RM_QTY"].ToString());
                    //    bl.RM_Standard_Yield = Convert.ToInt32(dr["RM_STANDARD_YIELD"].ToString());
                    //    bl.RM_Cost = Convert.ToDecimal(dr["RM_COST"].ToString());

                    //    bl.Batch_Performance_Amount = Convert.ToDecimal(dr["Batch_Performance"].ToString());
                    //    Value_list.Add(bl);
                    //}

                    //dd.Values = Value_list;
                    //values.Add(dd);

                    //obj.Status = "success";
                    //obj.Message = "Feed report data.";
                    //obj.Data = new { Labels, Values = values };

                    var result = JsonConvert.SerializeObject(ds.Tables[0]);
                    var result1 = JsonConvert.SerializeObject(ds.Tables[1]);
                    obj.Status = "success";
                    obj.Message = "Feed report data.";
                    obj.Data = new { result, result1 };
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

        [Route("api/get_slaughter_report")]
        [HttpGet]
        public IActionResult SLAUGHTER_REPORT(int Company_Id, int Lob_Id,int Batch_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (Company_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company details can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Lob_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Lob details can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _reportDB.Get_Slaughter_Report(Company_Id, Lob_Id,Batch_id);

                List<ReportValueData> values = new List<ReportValueData>();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //ReportValueData dd = new ReportValueData();
                    //List<string> label_list = new List<string>();

                    //foreach (DataRow dr in ds.Tables[1].Rows)
                    //{
                    //    label_list.Add(dr["LABELS"].ToString());
                    //}

                    //string[] Labels = label_list.ToArray();
                    //List<Slaughter_Report> Value_list = new List<Slaughter_Report>();
                    //foreach (DataRow dr in ds.Tables[0].Rows)
                    //{
                    //    Slaughter_Report bl = new Slaughter_Report();
                    //    bl.Batch_Date = dr["BATCH_DATE"].ToString();
                    //    bl.Batch_No = dr["BATCH_NO"].ToString();                        
                    //    bl.Plant_Incharge = dr["PLANT_INCHARGE"].ToString();
                    //    bl.Breed = dr["BREED_NAME"].ToString();
                    //    bl.Location_name = dr["LOCATION_NAME"].ToString();
                    //    bl.FG_Item_Name = dr["FG_ITEM_NAME"].ToString();
                    //    //bl.FG_Item = dr["FG_ITEM"].ToString();
                    //    bl.OP_Qty = Convert.ToDecimal(dr["Opening_Qty"].ToString());
                    //    bl.FG_Item_Qty = Convert.ToDecimal(dr["FG_ITEM_QTY"].ToString());
                    //    bl.RM = dr["RM"].ToString();
                    //    bl.RM_Qty = Convert.ToDecimal(dr["RM_QTY"].ToString());
                    //    bl.RM_Standard_Yield = Convert.ToInt32(dr["RM_STANDARD_YIELD"].ToString());
                    //    bl.RM_Cost = Convert.ToDecimal(dr["RM_COST"].ToString());
                    //    bl.Batch_Performance_Amount = Convert.ToDecimal(dr["Batch_Performance"].ToString());
                    //    Value_list.Add(bl);
                    //}

                    //dd.Values = Value_list;
                    //values.Add(dd);
                    var result = JsonConvert.SerializeObject(ds.Tables[0]);
                    var result1 = JsonConvert.SerializeObject(ds.Tables[1]);
                     
                    obj.Status = "success";
                    obj.Message = "Slaughter report data.";
                    obj.Data = new { result, result1 };
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

        [Route("api/get_farm_performance_location_report")]
        [HttpGet]
        public IActionResult FARM_PREFORMANCE_LOCATION_REPORT(int Company_id,string batch_id )
        {
            ResponseModel obj = new ResponseModel();
            try
            {            
                var ds = _reportDB.Get_Farm_Performance_Location_Report(Company_id, batch_id);

                List<ReportValueData> values = new List<ReportValueData>();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ReportValueData dd = new ReportValueData();
                    List<string> label_list = new List<string>();

                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        label_list.Add(dr["LABELS"].ToString());
                    }

                    string[] Labels = label_list.ToArray();
                    List<Farm_Performance_Location_report> Value_list = new List<Farm_Performance_Location_report>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Farm_Performance_Location_report bl = new Farm_Performance_Location_report();
                        bl.NOB = dr["NATURE_OF_BUSNINESS"].ToString();
                        bl.Location_Name = dr["LOCATION_NAME"].ToString();
                        bl.Batch_No = dr["BATCH_NO"].ToString();
                        bl.Opening_Qty = Convert.ToInt32(dr["OPENING_QTY"].ToString());
                        bl.Output_Qty = Convert.ToDecimal(dr["OUTPUT_QTY"].ToString());
                        //bl.Mortality_Per = Convert.ToDecimal(dr["MORTALITY_PER"].ToString());
                        //bl.FCR = Convert.ToDecimal(dr["FCR"].ToString());
                        //bl.Growth_Per = Convert.ToDecimal(dr["GROWTH_PER"].ToString());
                        bl.Electricity_Cost = Convert.ToDecimal(dr["ELECTRICITY_Cost"].ToString());
                        bl.Electricity_Units = Convert.ToDecimal(dr["ELECTRICITY_UNITS"].ToString());
                        bl.Water_Cost = Convert.ToDecimal(dr["WATER_Cost"].ToString());
                        bl.Water_Units = Convert.ToDecimal(dr["WATER_UNITS"].ToString());
                        bl.Labour_Cost = Convert.ToDecimal(dr["Labour_COST"].ToString());
                        bl.Vaccine_Cost = Convert.ToDecimal(dr["VACCINE_COST"].ToString());
                        bl.Vaccine_Units = Convert.ToDecimal(dr["VACCINE_UNITS"].ToString());
                        bl.Expense_Amount = Convert.ToDecimal(dr["EXPENSE_AMOUNT"].ToString());
                        bl.Feed_Cost = Convert.ToDecimal(dr["FEED_COST"].ToString());
                        bl.Feed_Consumed = Convert.ToDecimal(dr["FEED_CONSUMED"].ToString());
                        bl.Running_Cost = Convert.ToDecimal(dr["RUNNING_COST"].ToString());
                        Value_list.Add(bl);
                    }

                    dd.Values = Value_list;
                    values.Add(dd);

                    obj.Status = "success";
                    obj.Message = "Farm Performance location report data.";
                    obj.Data = new { Labels, Values = values };
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

        [Route("api/get_farm_performance_weekly_report")]
        [HttpGet]
        public IActionResult FARM_PREFORMANCE_WK_REPORT(int Company_Id,string Start_Date,string End_Date)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (Company_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company details can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                var ds = _reportDB.Get_Farm_Performance_Wk_Report(Company_Id,Start_Date,End_Date);

                List<ReportValueData> values = new List<ReportValueData>();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ReportValueData dd = new ReportValueData();
                    List<string> label_list = new List<string>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        label_list.Add(dr["LABELS"].ToString());
                    }

                    string[] Labels = label_list.ToArray();
                    List<Farm_Performance_Wk_Report> Value_list = new List<Farm_Performance_Wk_Report>();
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        Farm_Performance_Wk_Report bl = new Farm_Performance_Wk_Report();
                        bl.Week_No = Convert.ToInt32(dr["Week_No"].ToString());
                        bl.Total_Running_Batch = Convert.ToInt32(dr["Total_Running_Batch"].ToString());
                        bl.USER_NAME = dr["USER_NAME"].ToString();
                        //bl.WATER_COST = Convert.ToDecimal(dr["WATER_COST"].ToString());
                        //bl.ELECTRICITY_COST = Convert.ToDecimal(dr["ELECTRICITY_COST"].ToString());
                        //bl.LABOUR_COST = Convert.ToDecimal(dr["LABOUR_COST"].ToString());
                        //bl.ELECTRICITY_UNITS = Convert.ToDecimal(dr["ELECTRICITY_UNITS"].ToString());
                        //bl.WATER_UNITS = Convert.ToDecimal(dr["WATER_UNITS"].ToString());
                        //bl.VACCINE_COST = Convert.ToDecimal(dr["VACCINE_COST"].ToString());
                        //bl.VACCINE_UNITS = Convert.ToDecimal(dr["VACCINE_UNITS"].ToString());
                        //bl.FEED_COST = Convert.ToDecimal(dr["FEED_COST"].ToString());
                        //bl.FEED_UNITS = Convert.ToDecimal(dr["FEED_UNITS"].ToString());
                        bl.Total_Running_Cost = Convert.ToDecimal(dr["Total_Running_Cost"].ToString());
                        bl.Misc_Expenses = Convert.ToDecimal(dr["Misc_Expenses"].ToString());
                        bl.Payments = Convert.ToDecimal(dr["Payments"].ToString());
                        bl.Recipets = Convert.ToDecimal(dr["Recipets"].ToString());
                        bl.FarmPL = Convert.ToDecimal(dr["FarmPL"].ToString());
                        Value_list.Add(bl);
                    }

                    dd.Values = Value_list;
                    values.Add(dd);

                    obj.Status = "success";
                    obj.Message = "Farm Performance Weekly report data.";
                    obj.Data = new { Labels, Values = values };
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

        [Route("api/get_batch_output_performance_report")]
        [HttpGet]
        public IActionResult BATCH_OUTPUT_PREFORMANCE_REPORT(int Company_Id, string Start_Date, string End_Date)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (Company_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company details can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                var ds = _reportDB.Get_Batch_Output_Performance_Report(Company_Id,Start_Date,End_Date);

                List<ReportValueData> values = new List<ReportValueData>();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ReportValueData dd = new ReportValueData();
                    List<string> label_list = new List<string>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        label_list.Add(dr["LABELS"].ToString());
                    }

                    string[] Labels = label_list.ToArray();
                    List<Batch_Output_Performance_Report> Value_list = new List<Batch_Output_Performance_Report>();
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        Batch_Output_Performance_Report bl = new Batch_Output_Performance_Report();
                        bl.LOB = dr["LINE_OF_BUSNINESS"].ToString();
                      
                        bl.Batch_No = dr["Batch_No"].ToString();
                        bl.Week_No = Convert.ToInt32(dr["Week_No"].ToString());
                        bl.Unit_Cost = Convert.ToDecimal(dr["Unit_Cost"].ToString());
                        bl.Output_Item_Qty = Convert.ToDecimal(dr["Output_Item_Qty"].ToString());
                        bl.Total_Running_Cost = Convert.ToDecimal(dr["Total_Running_Cost"].ToString());
                        Value_list.Add(bl);
                    }

                    dd.Values = Value_list;
                    values.Add(dd);

                    obj.Status = "success";
                    obj.Message = "Batch Output Performance report data.";
                    obj.Data = new { Labels, Values = values };
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

        [Route("api/get_cattle_report_part1")]
        [HttpGet]
        public IActionResult Cattle_Report_Part1(int Batch_Id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (Batch_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Batch details can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                var ds = _reportDB.Get_Cattle_Report(Batch_Id);

                List<ReportValueData> values = new List<ReportValueData>();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ReportValueData dd = new ReportValueData();
                    List<string> label_list = new List<string>();

                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        label_list.Add(dr["LABELS"].ToString());
                    }

                    string[] Labels = label_list.ToArray();
                    List<Cattle_Report_Part1> Value_list = new List<Cattle_Report_Part1>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Cattle_Report_Part1 bl = new Cattle_Report_Part1();
                        bl.Batch_No = dr["BATCH_NO"].ToString();
                        //bl.Supervisor_Name = dr["Supervisor_Name"].ToString();
                        bl.Start_Date = dr["Start_Date"].ToString();
                        bl.Entry_Date = dr["ENTRY_DATE"].ToString();                        
                        bl.Breed = dr["BREED_NAME"].ToString();
                        //bl.Location = dr["LOCATION_NAME"].ToString();
                        bl.Age_in_Days = Convert.ToInt32(dr["Age_in_Days"].ToString());
                        bl.Opening_Balance = Convert.ToInt32(dr["Opening_Balance"].ToString());
                        bl.Mortality_M = Convert.ToDecimal(dr["Mortality_M"].ToString());
                        bl.Mortality_F = Convert.ToDecimal(dr["Mortality_F"].ToString());
                        bl.Body_Weight_M = Convert.ToDecimal(dr["Body_Weight_M"].ToString());
                        bl.Body_Weight_F = Convert.ToDecimal(dr["Body_Weight_F"].ToString());
                        bl.FEED = Convert.ToDecimal(dr["Feed_consumption"].ToString());
                        bl.Growth = Convert.ToDecimal(dr["Growth"].ToString());
                        bl.Vaccine = Convert.ToDecimal(dr["VACCINE_UNITS"].ToString());
                        bl.Medicine = Convert.ToDecimal(dr["Medicine"].ToString());
                        bl.FCR = Convert.ToDecimal(dr["FCR"].ToString());
                        bl.Livability_Per = Convert.ToDecimal(dr["Livability_Per"].ToString());
                        // bl.Milk_Extracted = Convert.ToDecimal(dr["Milk"].ToString());
                        Value_list.Add(bl);
                    }

                    dd.Values = Value_list;
                    values.Add(dd);

                    obj.Status = "success";
                    obj.Message = "Cattle report part1 data.";
                    obj.Data = new { Labels, Values = values };
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

        [Route("api/get_cattle_report_part2")]
        [HttpGet]
        public IActionResult Cattle_Report_Part2(int Batch_Id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (Batch_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Batch details can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                var ds = _reportDB.Get_Cattle_Report(Batch_Id);

                List<ReportValueData> values = new List<ReportValueData>();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ReportValueData dd = new ReportValueData();
                    List<string> label_list = new List<string>();

                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        label_list.Add(dr["LABELS"].ToString());
                    }

                    string[] Labels = label_list.ToArray();
                    List<Cattle_Report_Part2> Value_list = new List<Cattle_Report_Part2>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Cattle_Report_Part2 bl = new Cattle_Report_Part2();
                        bl.Batch_No = dr["BATCH_NO"].ToString();
                        bl.Supervisor_Name = dr["Supervisor_Name"].ToString();
                        bl.Start_Date = dr["Start_Date"].ToString();
                        bl.Entry_Date = dr["ENTRY_DATE"].ToString();                        
                        bl.Breed = dr["BREED_NAME"].ToString();
                        bl.Location = dr["LOCATION_NAME"].ToString();
                        bl.Opening_Balance = Convert.ToInt32(dr["Opening_Balance"].ToString());
                        bl.Total_Water_Cost = Convert.ToDecimal(dr["Water_Expense"].ToString());
                        bl.Total_Feed_Cost = Convert.ToDecimal(dr["feed_cost"].ToString());
                        bl.Total_Feed_Consumed = Convert.ToDecimal(dr["Feed_consumption"].ToString());
                        bl.Total_Electricity_Cost = Convert.ToDecimal(dr["Electricity_Expense"].ToString());
                        bl.Total_Labour_Cost = Convert.ToDecimal(dr["labour_Expense"].ToString());
                        bl.Total_Electricity_Units = Convert.ToDecimal(dr["Electricity_Units"].ToString());
                        bl.Total_Water_Consumed = Convert.ToDecimal(dr["Water_Units"].ToString());
                        bl.Total_Vaccine_Cost = Convert.ToDecimal(dr["VACCINE_COST"].ToString());
                        bl.Total_Vaccine_Units = Convert.ToDecimal(dr["VACCINE_UNITS"].ToString());
                        bl.Batch_Performance = Convert.ToDecimal(dr["batch_amount"].ToString());
                        bl.Body_Weight = Convert.ToDecimal(dr["Body_Weight"].ToString());
                        bl.Growth = Convert.ToDecimal(dr["Growth"].ToString());
                        Value_list.Add(bl);
                    }

                    dd.Values = Value_list;
                    values.Add(dd);

                    obj.Status = "success";
                    obj.Message = "Cattle report part2 data.";
                    obj.Data = new { Labels, Values = values };
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

        [Route("api/get_cattle_Dairy_report")]
        [HttpGet]
        public IActionResult Cattle_Dairy_Report(string fromdate,string todate,string Batch_Id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (Batch_Id == "0")
                {
                    obj.Status = "failure";
                    obj.Message = "Batch details can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                var ds = _reportDB.Get_Cattle_Dairy_Report(fromdate,todate,Batch_Id);

                List<ReportValueData> values = new List<ReportValueData>();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ReportValueData dd = new ReportValueData();
                    List<string> label_list = new List<string>();

                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        label_list.Add(dr["LABELS"].ToString());
                    }

                    string[] Labels = label_list.ToArray();
                    List<Cattle_Report_Dairy> Value_list = new List<Cattle_Report_Dairy>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Cattle_Report_Dairy bl = new Cattle_Report_Dairy();
                        bl.Batch_No = dr["BATCH_NO"].ToString();
                        bl.Enrty_date = dr["ENTRY_DATE"].ToString();

                        bl.Batch_Quantity = Convert.ToInt32(dr["BATCH_QUANTITY"].ToString());
                        bl.YM = Convert.ToDecimal(dr["YM"].ToString());
                        bl.YE = Convert.ToDecimal(dr["YE"].ToString());
                        bl.Total_Yield = Convert.ToDecimal(dr["Total_Yield"].ToString());
                        bl.CM = Convert.ToDecimal(dr["CM"].ToString());
                        bl.CE = Convert.ToDecimal(dr["CE"].ToString());
                        bl.Total_Calves = Convert.ToDecimal(dr["Total_Calves"].ToString());
                        bl.Avg_Per_day = Convert.ToDecimal(dr["Avg_Per_Day"].ToString());
                        bl.Total_Processing = Convert.ToDecimal(dr["Total_Milk"].ToString());
                        
                        Value_list.Add(bl);
                    }

                    dd.Values = Value_list;
                    values.Add(dd);

                    obj.Status = "success";
                    obj.Message = "Cattle report Dairy data.";
                    obj.Data = new { Labels, Values = values };
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

        [Route("api/get_animal_drug_report")]
        [HttpGet]
        public IActionResult Animal_Drug_report(int company_id,int lob_id,int Year, int Month, string Batch_Id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (company_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company id can not blank.";
                    obj.Data = null;
                    return Ok(obj);
                }

                var ds = _reportDB.Get_Animal_Drug_Report(company_id,lob_id,Month, Year, Batch_Id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(ds.Tables[0]);
                    obj.Status = "success";
                    obj.Message = "Report data.";
                    obj.Data = new { result };
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

        [Route("api/get_feed_usage_report")]
        [HttpGet]
        public IActionResult Feed_Usage_Report(int company_id, int lob_id, string fromdate, string todate, string Batch_Id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (company_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company id can not blank.";
                    obj.Data = null;
                    return Ok(obj);
                }

                var ds = _reportDB.Get_Feed_Usage_Report(company_id, lob_id, fromdate, todate, Batch_Id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(ds.Tables[0]);
                    var result2 = JsonConvert.SerializeObject(ds.Tables[1]);
                    obj.Status = "success";
                    obj.Message = "Report data.";
                    obj.Data = new { result,result2 };
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

        //item stock report
        [Route("api/get_item_stock_report")]
        [HttpGet]
        public IActionResult Item_Stock_Report(int company_id, string fromdate, string todate, int item_id, int location_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (company_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company id can not blank.";
                    obj.Data = null;
                    return Ok(obj);
                }

                var ds = _reportDB.Get_Item_Stock_Report(company_id, fromdate, todate, item_id, location_id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(ds.Tables[0]);
                    obj.Status = "success";
                    obj.Message = "Report data.";
                    obj.Data = new { result };
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

        //Location Wise Item stock report
        [Route("api/get_item_stock_report_location_wise")]
        [HttpGet]
        public IActionResult Item_Stock_Report_location_wise(int company_id, string fromdate, string todate, int item_id, int location_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (company_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company id can not blank.";
                    obj.Data = null;
                    return Ok(obj);
                }

                var ds = _reportDB.Get_Item_Stock_Report_location_wise(company_id, fromdate, todate, item_id, location_id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(ds.Tables[0]);
                    obj.Status = "success";
                    obj.Message = "Report data.";
                    obj.Data = new { result };
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

        #region Aqua report Api


        [Route("api/get_aqua_rearing_report")]
        [HttpGet]
        public IActionResult Aqua_Rearing_Report(int company_id, int lob_id, int batch_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (company_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company id can not blank.";
                    obj.Data = null;
                    return Ok(obj);
                }
                if (lob_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Lob id can not blank.";
                    obj.Data = null;
                    return Ok(obj);
                }
                if (batch_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "batch can not blank.";
                    obj.Data = null;
                    return Ok(obj);
                }

                var ds = _reportDB.Get_Aqua_rearing_report(company_id,lob_id, batch_id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(ds.Tables[0]);
                    var result1 = JsonConvert.SerializeObject(ds.Tables[1]);
                    obj.Status = "success";
                    obj.Message = "Aqua Report data.";
                    obj.Data = new { result, result1 };
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


        [Route("api/get_aqua_rearing_growthpercentagfcr_report")]
        [HttpGet]
        public IActionResult Aqua_RearingGrowthPercentagFcr_Report(int company_id, int lob_id, int batch_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (company_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company id can not blank.";
                    obj.Data = null;
                    return Ok(obj);
                }
                if (lob_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Lob id can not blank.";
                    obj.Data = null;
                    return Ok(obj);
                }
                if (batch_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "batch can not blank.";
                    obj.Data = null;
                    return Ok(obj);
                }

                var ds = _reportDB.Get_Aqua_rearing_growth_percentage_fcr_report(company_id, lob_id, batch_id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(ds.Tables[0]);
                    //var result1 = JsonConvert.SerializeObject(ds.Tables[1]);
                    obj.Status = "success";
                    obj.Message = "Aqua Report data.";
                    obj.Data = new { result };
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


        [Route("api/get_aqua_feed_planning_report")]
        [HttpGet]
        public IActionResult Aqua_feed_planning_Report(int company_id, int batch_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (company_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company id can not blank.";
                    obj.Data = null;
                    return Ok(obj);
                }
                if (batch_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "batch can not blank.";
                    obj.Data = null;
                    return Ok(obj);
                }

                var ds = _reportDB.Get_Aqua_FeedPlanning_report(company_id, batch_id);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    
                    obj.Status = "success";
                    obj.Message = "Aqua Feed Planning data.";
                    obj.Data = new { result= dt };
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

        #endregion
        [Route("api/get_piggery_report")]
        [HttpGet]
        public IActionResult Piggery_Report(int batch_id)
        {
            ResponseModel obj = new ResponseModel();
            string result= JsonConvert.SerializeObject(new DataTable());
            string result1= JsonConvert.SerializeObject(new DataTable());

            try
            {
           
                if (batch_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "batch can not blank.";
                    obj.Data = null;
                    return Ok(obj);
                }

                var ds = _reportDB.Get_Piggery_Report(batch_id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                     result = JsonConvert.SerializeObject(ds.Tables[0]);
                     result1 = JsonConvert.SerializeObject(ds.Tables[1]);
                    obj.Status = "success";
                    obj.Message = "Piggery Report data.";
                    obj.Data = new { result, result1 };
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


        [Route("api/get_piggery_report_graph")]
        [HttpGet]
        public IActionResult Piggery_Report_Chart(int batch_id, int type)
        {
            ResponseModel obj = new ResponseModel();
            string result = JsonConvert.SerializeObject(new DataTable());
            try
            {

                if (batch_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "batch can not blank.";
                    obj.Data = null;
                    return Ok(obj);
                }

                var ds = _reportDB.Get_Piggery_Report_Graph(batch_id, type);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    result = JsonConvert.SerializeObject(ds.Tables[0]);
                   
                    obj.Status = "success";
                    obj.Message = "Piggery Report Graph data.";
                    obj.Data = new { result };
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
        
        [Route("api/get_agri_report")]
        [HttpGet]
        public IActionResult Agri_Report(int batch_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {

                if (batch_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "batch can not blank.";
                    obj.Data = null;
                    return Ok(obj);
                }

                var ds = _reportDB.Get_Agri_Report(batch_id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(ds.Tables[0]);
                    var result2 = JsonConvert.SerializeObject(ds.Tables[1]);
                    obj.Status = "success";
                    obj.Message = "Agriculture Report data.";
                    obj.Data = new { result, result2 };
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
        [Route("api/get_agri_summ_report")]
        [HttpGet]
        public IActionResult Agri_Summary_Report(int company_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {

                if (company_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Comapany can not blank.";
                    obj.Data = null;
                    return Ok(obj);
                }

                var ds = _reportDB.Get_Agri_Summ_Report(company_id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(ds.Tables[0]);
                    obj.Status = "success";
                    obj.Message = "Agriculture Report data.";
                    obj.Data = new { result };
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


        //Commercial Broiler KPI chart
        [Route("api/get_crb_kpi_report")]
        [HttpGet]
        public IActionResult get_crb_kpi_report(int company_id, int batch_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {

                if (company_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Comapany can not blank.";
                    obj.Data = null;
                    return Ok(obj);
                }

                var ds = _reportDB.Get_CBR_KPI_Report(company_id,batch_id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                  //  var result = JsonConvert.SerializeObject(ds.Tables[0]);
                    obj.Status = "success";
                    obj.Message = "Commercial Broiler KPI Report data.";
                    obj.Data = ds.Tables[0];
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

        //Commercial Broiler Monitring chart
        [Route("api/get_crb_monitring_report")]
        [HttpGet]
        public IActionResult get_crb_monitring_report(int company_id, int batch_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {

                if (company_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Comapany can not blank.";
                    obj.Data = null;
                    return Ok(obj);
                }

                var ds = _reportDB.Get_CBR_Monitring_Report(company_id, batch_id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                     var result = JsonConvert.SerializeObject(ds.Tables[0]);
                    obj.Status = "success";
                    obj.Message = "Commercial Broiler monitring Report data.";
                    obj.Data = result;
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


        //Laying Feed KPI Report
        [Route("api/get_laying_feed_kpi_report")]
        [HttpGet]
        public IActionResult get_laying_feed_kpi_report(int company_id, int batch_id ,int type)
        {
            ResponseModel obj = new ResponseModel();
            try
            {

                if (company_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Comapany can not blank.";
                    obj.Data = null;
                    return Ok(obj);
                }

                var ds = _reportDB.get_laying_feed_kpi_report(company_id, batch_id,type);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //  var result = JsonConvert.SerializeObject(ds.Tables[0]);
                    obj.Status = "success";
                    obj.Message = "Laying "+ type.ToString()+ " KPI Report data.";
                    obj.Data = ds.Tables[0];
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
        //Laying Feed KPI Report Standard
        [Route("api/get_laying_feed_kpi_report_standard")]
        [HttpGet]
        public IActionResult get_laying_feed_kpi_report_standard(int company_id, int batch_id, int type,int user_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {

                if (company_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Comapany can not blank.";
                    obj.Data = null;
                    return Ok(obj);
                }

                var ds = _reportDB.get_laying_feed_kpi_report_standard(company_id, batch_id, type, user_id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //  var result = JsonConvert.SerializeObject(ds.Tables[0]);
                    obj.Status = "success";
                    obj.Message = "Laying " + type.ToString() + " KPI Report data.";
                    obj.Data = ds.Tables[0];
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

        #region Aqua Dashboard Api

        //Laying Feed KPI Report
        [Route("api/get_aqua_dashboard_feed")]
        [HttpGet]
        public IActionResult get_aqua_dashboard_feed(int company_id,int lob_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {

                if (company_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Comapany can not blank.";
                    obj.Data = null;
                    return Ok(obj);
                }

                var dt = _reportDB.get_aqua_dashbard_feed(company_id,lob_id);
                if (dt.Rows.Count > 0)
                {
                    
                    obj.Status = "success";
                    obj.Message = "Get Aqua feed dashboard data.";
                    obj.Data = dt;
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

        // Aqua Dashboard Growth Percentage FCR
        [Route("api/get_aqua_growth_percentage_fcr_dashboard")]
        [HttpGet]
        public IActionResult Aqua_growth_percentage_fcr_dashboard(int company_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (company_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company id can not blank.";
                    obj.Data = null;
                    return Ok(obj);
                }
                //if (lob == 0)
                //{
                //    obj.Status = "failure";
                //    obj.Message = "Line of Bussiness can not blank.";
                //    obj.Data = null;
                //    return Ok(obj);
                //}

                var ds = _reportDB.GET_Aqua_growth_percentage_fcr_dashboard(company_id);
                DataTable dt = new DataTable();
             //   dt = ds.Tables[0];
                if (ds.Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(ds);

                    obj.Status = "success";
                    obj.Message = "Aqua Growth % FCR Report data.";
                    obj.Data = new { result = result };
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
        //using this api on dashboard
        // Aqua Dashboard Growth Percentage FCR
        [Route("api/get_dashboard_running_cost")]
        [HttpGet]
        public IActionResult Aqua_dashboard_running_cost(int company_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (company_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company id can not blank.";
                    obj.Data = null;
                    return Ok(obj);
                }
                //if (lob == 0)
                //{
                //    obj.Status = "failure";
                //    obj.Message = "Line of Bussiness can not blank.";
                //    obj.Data = null;
                //    return Ok(obj);
                //}

                var ds = _reportDB.GET_Aqua_dashboard_running_cost(company_id);
                DataTable dt = new DataTable();
                //   dt = ds.Tables[0];
                if (ds.Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(ds);

                    obj.Status = "success";
                    obj.Message = "Aqua Dashoard Running Cost";
                    obj.Data = new { result = result };
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



        #endregion


        [Route("api/get_descriptive_report")]
        [HttpGet]
        public IActionResult GETDescriptive_report(int batch_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {

                if (batch_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "batch_id can not blank.";
                    obj.Data = null;
                    return Ok(obj);
                }

                var Table = _reportDB.GET_Descriptive_report(batch_id);
                if (Table.Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(Table);
                    obj.Status = "success";
                    obj.Message = "Descriptive Report data.";
                    obj.Data = new { result };
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

        [Route("api/get_inventory_summary_report")]
        [HttpGet]
        public IActionResult GETInventory_summary_report(int company_id, int location_id, int item_id, string transfer_type,string from_date,string to_date)
        {
            ResponseModel obj = new ResponseModel();
            try
            {

                if (company_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "company_id can not blank.";
                    obj.Data = null;
                    return Ok(obj);
                }

                var Table = _reportDB.GET_Inventory_summary_report(company_id, location_id, item_id, transfer_type, from_date, to_date);
                if (Table.Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(Table);
                    obj.Status = "success";
                    obj.Message = "Inventory Summary Report data.";
                    obj.Data = new { result };
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
        [Route("api/get_stock_valuation_report")]
        [HttpGet]
        public IActionResult stock_valuation_report(int company_id, int Batch_Id ,string from_date,string to_date)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (company_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _reportDB.Get_Stock_Valuation_report(company_id, Batch_Id,from_date,to_date);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(ds.Tables[0]);
                  
                    obj.Status = "success";
                    obj.Message = "Stock Valuation Report data.";
                    obj.Data = new { result };

                     
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
        //Poultry Slaughtering Report (chart api)
        [Route("api/get_slaughtering_chart")]
        [HttpGet]
        public IActionResult slaughtering_chart(int company_id, int Batch_Id,int Type)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (company_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Batch_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Batch can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _reportDB.Get_slaughtering_chart(company_id, Batch_Id, Type);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(ds.Tables[0]);

                    obj.Status = "success";
                    obj.Message = "Slaughtering Chart data.";
                    obj.Data = new { result };


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

        //Item byes Location Stock Report
        [Route("api/get_item_location_stock_report")]
        [HttpGet]
        public IActionResult item_location_stock_report(int company_id, string item_id, string location_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (company_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _reportDB.Get_item_location_stock_report(company_id, item_id, location_id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(ds.Tables[0]);

                    obj.Status = "success";
                    obj.Message = "Stock Valuation Report data.";
                    obj.Data = new { result };


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

        //Transfer summary Report
        [Route("api/get_transfer_summary_report")]
        [HttpGet]
        public IActionResult transfer_summary_report(int company_id, string from_date, string to_date)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (company_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _reportDB.Get_Transfer_Summary_report(company_id, from_date, to_date);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(ds.Tables[0]);

                    obj.Status = "success";
                    obj.Message = "Stock Valuation Report data.";
                    obj.Data = new { result };


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

        //Transfer stock Report
        [Route("api/get_inventory_stock_report")]
        [HttpGet]
        public IActionResult inventory_stock_report(int item_id,string location_id, string fromdate, string todate, int company_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (company_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _reportDB.Get_Inventory_Stock_Report(company_id, fromdate, todate, item_id, location_id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(ds.Tables[0]);

                    obj.Status = "success";
                    obj.Message = "Stock Valuation Report data.";
                    obj.Data = new { result };


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


        //Ledger stock report
        [Route("api/get_ledger_stock_report")]
        [HttpGet]
        public IActionResult ledger_Stock_Report(int company_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (company_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company id can not blank.";
                    obj.Data = null;
                    return Ok(obj);
                }

                var ds = _reportDB.Get_Ledger_Stock_Report(company_id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(ds.Tables[0]);
                    obj.Status = "success";
                    obj.Message = "Report data.";
                    obj.Data = new { result };
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
        //Transacation Ledger report
        [Route("api/get_transacation_ledger_report")]
        [HttpGet]
        public IActionResult Transacation_Ledger_Report(int company_id,int user_id,int nob_id, string location, string batch, string item, string item_category, string transfer)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (company_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company id can not blank.";
                    obj.Data = null;
                    return Ok(obj);
                }

                var ds = _reportDB.Get_Transacation_Ledger_Report(company_id, user_id, nob_id, location, batch, item, item_category, transfer);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(ds.Tables[0]);
                    obj.Status = "success";
                    obj.Message = "Report data.";
                    obj.Data = new { result };
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
        //Feed ForeCast report
        [Route("api/get_feed_forecast_report")]
        [HttpGet]
        public IActionResult feed_forecast_report(int batch_id, int item_id, string from_date, string to_date)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                var ds = _reportDB.Get_feed_forecast_report(batch_id, item_id, from_date, to_date);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    obj.Status = "success";
                    obj.Message = "feed forcast report data.";
                    obj.Data = ds.Tables[0];
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

        [Route("api/get_Transactions_apply_entry_report")]
        [HttpGet]
        public IActionResult Transactions_apply_entry_report(int company_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                var ds = _reportDB.Get_Transactions_Apply_Entry_Report(company_id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(ds.Tables[0]);
                    obj.Status = "success";
                    obj.Message = "Get data.";
                    obj.Data = new { result };
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

        [Route("api/get_trace_report")]
        [HttpGet]
        public IActionResult Trace_Report(int company_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                var ds = _reportDB.Get_Trace_Report(company_id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(ds.Tables[0]);
                    obj.Status = "success";
                    obj.Message = "Trace Report data.";
                    obj.Data = new { result };
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
        [Route("api/get_item_stock_summary")]
        [HttpGet]
        public IActionResult item_stock_summary(int company_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                var ds = _reportDB.Get_Item_Stock_Summary_Report(company_id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(ds.Tables[0]);
                    obj.Status = "success";
                    obj.Message = "Report data.";
                    obj.Data = new { result };
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

        [Route("api/get_breed_attribute_report")]
        [HttpGet]
        public IActionResult breed_attribute_report(int company_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                var ds = _reportDB.Get_BreedAttribute_Report(company_id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(ds.Tables[0]);
                    obj.Status = "success";
                    obj.Message = "Report data.";
                    obj.Data = new { result };
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

    }
}