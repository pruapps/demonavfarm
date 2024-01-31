using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FarmIT_Api.database_accesslayer;
using FarmIT_Api.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net;

namespace FarmIT_Api.Controllers
{
    [ApiController]
    [Authorize]
    [RESTAuthorizeAttribute]
    public class MasterController : ControllerBase
    {
        private IMasterDB _masterDB;

        public MasterController(IMasterDB masterDB)
        {
            _masterDB = masterDB;
        }

        [Route("api/get_item_summary")]
        [HttpGet]
        public IActionResult Item_Master(int Company_Id,string Nature_Id,int batch_id)
        {
            ResponseModel obj = new ResponseModel();
            List<ItemHeader> summarry = new List<ItemHeader>();
            try
            {
                if (Company_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Nature_Id == null || Nature_Id == "null")
                {
                    obj.Status = "failure";
                    obj.Message = "Nature id id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                var dt = _masterDB.Get_Item_Master(Company_Id, Nature_Id,batch_id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ItemHeader bd = new ItemHeader();

                        bd.item_id = Convert.ToInt32(dr["ITEM_ID"].ToString());
                        bd.item_name = dr["ITEM_NAME"].ToString();
                        bd.item_category = dr["ITEM_CATEGORY"].ToString();
                        bd.industry_type = dr["INDUSTRY_TYPE"].ToString(); 
                        bd.uom = dr["UOM"].ToString();
                        bd.unit_cost = Convert.ToDecimal(dr["UNIT_COST"].ToString());
                        bd.status = dr["STATUS"].ToString();
                        bd.company_id = Convert.ToInt32(dr["COMPANY_ID"].ToString());
                        bd.isbatchapp = Convert.ToString(dr["ISBATCHAPP"]);
                        bd.quantity = Convert.ToDecimal(dr["QUANTITY"].ToString());
                        bd.item_type = Convert.ToString(dr["item_type"].ToString());
                        bd.isNegativeallow = Convert.ToInt32(dr["isNegativeAllow"]);
                        bd.isLobItem = Convert.ToInt32(dr["isLOB_ITEM"].ToString());
                        bd.industry_id = Convert.ToInt32(dr["NATURE_ID"].ToString());
                        bd.isERP = Convert.ToInt32(dr["isERP"].ToString());

                        bd.INVENTORY_TYPE = dr["INVENTORY_TYPE"].ToString();
                        summarry.Add(bd);
                    }

                    obj.Status = "success";
                    obj.Message = "Item master data.";
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

        [Route("api/get_overheadcost_summary")]
        [HttpGet]
        public IActionResult Overheadcost_Master(int Company_Id, string Nature_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<OverHeadCost_Master> summarry = new List<OverHeadCost_Master>();
            try
            {
                if (Company_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Nature_Id == null || Nature_Id == "null")
                {
                    obj.Status = "failure";
                    obj.Message = "Nature id id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                var dt = _masterDB.Get_OverHeadCost_Master(Company_Id,Nature_Id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        OverHeadCost_Master bd = new OverHeadCost_Master();

                        bd.expense_id = Convert.ToInt32(dr["EXPENSE_ID"].ToString());
                        bd.expense_name = dr["EXPENSE_NAME"].ToString();
                        bd.expense_type = dr["EXPENSE_TYPE"].ToString();
                        bd.industry_type = dr["INDUSTRY_TYPE"].ToString();
                        bd.line_of_business = dr["LINE_OF_BUSINESS"].ToString();
                        bd.amount = Convert.ToDecimal(dr["AMOUNT"].ToString());
                        bd.status = dr["STATUS"].ToString();
                        bd.company_id = Convert.ToInt32(dr["COMPANY_ID"].ToString());

                        summarry.Add(bd);
                    }

                    obj.Status = "success";
                    obj.Message = "Over head cost master data.";
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

        [Route("api/get_location_summary")]
        [HttpGet]
        public IActionResult location_Master(int Company_Id,string Nature_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<Location_Master> summarry = new List<Location_Master>();
            try
            {
                if (Company_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Nature_Id == null || Nature_Id == "null")
                {
                    obj.Status = "failure";
                    obj.Message = "Nature id id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                var dt = _masterDB.Get_Location_Master(Company_Id, Nature_Id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Location_Master bd = new Location_Master();

                        bd.location_id = Convert.ToInt32(dr["LOCATION_ID"].ToString());
                        bd.location_name = dr["LOCATION_NAME"].ToString();
                        bd.farm_type = dr["FARM_TYPE"].ToString();
                        bd.farm_type_id = Convert.ToInt32(dr["FARM_TYPE_ID"].ToString());
                        bd.industry_type = dr["INDUSTRY_TYPE"].ToString();
                        bd.industry_id = Convert.ToInt32(dr["INDUSTRY_ID"].ToString());
                        bd.address = dr["ADDRESS"].ToString();
                        bd.modification_date = dr["LAST_UPDATE_DATE"].ToString();
                        bd.status = dr["STATUS"].ToString();
                        bd.company_id = Convert.ToInt32(dr["COMPANY_ID"].ToString());
                      //  bd.sub_location_id = Convert.ToInt32(dr["sub_location_id"].ToString());
                        bd.islocdefault = Convert.ToInt32(dr["islocdefault"].ToString());
                        bd.Primary_location_id = Convert.ToInt32(dr["Primary_location_id"]);
                        bd.locationtype = dr["Locationtype"].ToString();
                        bd.primary_location_name = dr["P_LOCATION_NAME"].ToString();
                        bd.area  = dr["AREA"].ToString();
                        bd.farmer_name = dr["Farmer_Name"].ToString();
                        bd.description = dr["DESCRIPTION"].ToString();
                        bd.industry_id_list = dr["INDUSTRY_TYPE_LIST"].ToString();
                        bd.location_code = dr["LOCATION_CODE"].ToString();
                        summarry.Add(bd);
                    }

                    obj.Status = "success";
                    obj.Message = "Location master data.";
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

        [Route("api/get_breed_summary")]
        [HttpGet]
        public IActionResult breed_Master(int Company_Id,string Nature_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<Breed_Master> summarry = new List<Breed_Master>();
            try
            {
                if (Company_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Nature_Id == null || Nature_Id == "null")
                {
                    obj.Status = "failure";
                    obj.Message = "Nature id id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                var dt = _masterDB.Get_Breed_Master(Company_Id, Nature_Id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Breed_Master bd = new Breed_Master();

                        bd.breed_id = Convert.ToInt32(dr["BREED_ID"].ToString());
                        bd.breed_name = dr["BREED_NAME"].ToString();
                        bd.industry_type = dr["INDUSTRY_TYPE"].ToString();
                        bd.modification_date = dr["LAST_UPDATE_DATE"].ToString();
                        bd.status = dr["STATUS"].ToString();
                        bd.company_id = Convert.ToInt32(dr["COMPANY_ID"].ToString());
                        bd.breed_no = dr["BREED_NO"].ToString();

                        summarry.Add(bd);
                    }

                    obj.Status = "success";
                    obj.Message = "Breed master data.";
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

        [Route("api/get_parameter_summary")]
        [HttpGet]
        public IActionResult parameter_Master(int Company_Id,string Nature_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<Parameter_Master> summarry = new List<Parameter_Master>();
            try
            {
                if (Company_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Nature_Id == null || Nature_Id == "null")
                {
                    obj.Status = "failure";
                    obj.Message = "Nature id id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                var dt = _masterDB.Get_Parameter_Master(Company_Id, Nature_Id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Parameter_Master bd = new Parameter_Master();

                        bd.parameter_id = Convert.ToInt32(dr["PARAMETER_ID"].ToString());
                        bd.parameter_name = dr["PARAMETER_NAME"].ToString();
                        bd.parameter_type = dr["PARAMETER_TYPE"].ToString();
                        bd.parameter_type_id = Convert.ToInt32(dr["PARAMETER_TYPE_ID"].ToString());
                        bd.lob_id = Convert.ToInt32(dr["LOB_ID"].ToString());
                        bd.line_of_business = dr["LINE_OF_BUSINESS"].ToString();
                        bd.industry_type = dr["INDUSTRY_TYPE"].ToString();
                        bd.industry_id = Convert.ToInt32(dr["INDUSTRY_ID"].ToString());
                        bd.status = dr["STATUS"].ToString();
                        bd.company_id = Convert.ToInt32(dr["COMPANY_ID"].ToString());
                        bd.livestock_flag = Convert.ToInt32(dr["livestock_flag"].ToString());
                        bd.parameter_input_type = dr["PARAMETER_INPUT_TYPE"].ToString();
                        bd.parameter_input_format = dr["PARAMETER_INPUT_FORMAT"].ToString();
                        bd.parameter_no = dr["PARAMETER_NO"].ToString();
                        bd.livestock_type = dr["LIVESTOCK_TYPE"].ToString();
                        summarry.Add(bd);
                    }

                    obj.Status = "success";
                    obj.Message = "Parameter master data.";
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

        [Route("api/get_dataentry_type_summary")]
        [HttpGet]
        public IActionResult dataentry_type_Master(int Company_Id,string Nature_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<DataEntryType_Master> summarry = new List<DataEntryType_Master>();
            try
            {
                if (Company_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Nature_Id == null || Nature_Id == "null")
                {
                    obj.Status = "failure";
                    obj.Message = "Nature id id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                var dt = _masterDB.Get_DataEntryType_Master(Company_Id, Nature_Id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DataEntryType_Master bd = new DataEntryType_Master();

                        bd.dataentry_type_id = Convert.ToInt32(dr["DATAENTRY_TYPE_ID"].ToString());
                        bd.dataentry_type = dr["DATAENTRY_TYPE"].ToString();
                        bd.nature_of_business = dr["NATURE_OF_BUSINESS"].ToString();
                        bd.line_of_business = dr["LINE_OF_BUSINESS"].ToString();
                        bd.lob_id = Convert.ToInt32(dr["LOB_ID"].ToString());
                        bd.uom = dr["UOM"].ToString();
                        bd.alternate_uom = dr["ALTERNATE_UOM"].ToString();
                        bd.status = dr["STATUS"].ToString();
                        bd.company_id = Convert.ToInt32(dr["COMPANY_ID"].ToString());

                        summarry.Add(bd);
                    }

                    obj.Status = "success";
                    obj.Message = "Data entry type master data.";
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

        [Route("api/get_lob_summary")]
        [HttpGet]
        public IActionResult LOB_Master()
        {
            ResponseModel obj = new ResponseModel();
            List<LOB_Master> summarry = new List<LOB_Master>();
            try
            {
                var dt = _masterDB.Get_LineOfBusiness();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        LOB_Master bd = new LOB_Master();

                        bd.lob_id = Convert.ToInt32(dr["LINE_ID"].ToString());
                        bd.line_of_business = dr["LINE_OF_BUSINESS"].ToString();
                        bd.nature_of_business = dr["NATURE_OF_BUSINESS"].ToString();
                        bd.status = dr["STATUS"].ToString();

                        summarry.Add(bd);
                    }

                    obj.Status = "success";
                    obj.Message = "lob master data.";
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

        [Route("api/get_farm_type")]
        [HttpGet]
        public IActionResult Farm_Type()
        {
            ResponseModel obj = new ResponseModel();
            List<SelectListItem> FARM_TYPE = new List<SelectListItem>();
            try
            {
                var dt = _masterDB.Get_FarmType();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        FARM_TYPE.Add(new SelectListItem { Text = @dr["FARM_TYPE"].ToString(), Value = @dr["FARM_TYPE_ID"].ToString() });
                    }

                    obj.Status = "success";
                    obj.Message = "Fram Type data.";
                    obj.Data = FARM_TYPE ;
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

        [Route("api/get_item_category")]
        [HttpGet]
        public IActionResult Item_Category(int nature_id)
        {
            ResponseModel obj = new ResponseModel();
            List<SelectListItem> item_category = new List<SelectListItem>();
            try
            {
                var dt = _masterDB.Get_ItemCategory(nature_id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        item_category.Add(new SelectListItem { Text = @dr["CATEGORY_NAME"].ToString(), Value = @dr["CATEGORY_ID"].ToString() });
                    }

                    obj.Status = "success";
                    obj.Message = "ITEM CATEGORY data.";
                    obj.Data = item_category;
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

        [Route("api/get_livestock_type")]
        [HttpGet]
        public IActionResult Livestock_Type(int company_id)
        {
            ResponseModel obj = new ResponseModel();
            List<SelectListItem> animal_Livestock_type = new List<SelectListItem>();
            List<SelectListItem> parameter_Livestock_type = new List<SelectListItem>();
            try
            {
                var dt = _masterDB.Get_Livestock_Type(company_id);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        animal_Livestock_type.Add(new SelectListItem { Text = @dr["LIVESTOCK_TYPE"].ToString(), Value = @dr["LIVESTOCK_TYPE_ID"].ToString() });
                    }

                    if (dt.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Tables[1].Rows)
                        {
                            parameter_Livestock_type.Add(new SelectListItem { Text = @dr["LIVESTOCK_TYPE"].ToString(), Value = @dr["LIVESTOCK_TYPE_ID"].ToString() });
                        }

                    }

                    obj.Status = "success";
                    obj.Message = "Livestock Type data.";
                    obj.Data = new {animal_livestock_type= animal_Livestock_type, parameter_livestock_type= parameter_Livestock_type } ;
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

        [Route("api/get_uom")]
        [HttpGet]
        public IActionResult uom()
        {
            ResponseModel obj = new ResponseModel();
            List<SelectListItem> uom = new List<SelectListItem>();
            try
            {
                var dt = _masterDB.Get_Uom();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        uom.Add(new SelectListItem { Text = @dr["UOM"].ToString(), Value = @dr["UOM_ID"].ToString() });
                    }

                    obj.Status = "success";
                    obj.Message = "UOM data.";
                    obj.Data = uom;
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

        [Route("api/get_uom_type_wise")]
        [HttpGet]
        public IActionResult uom_typeWise(string Type)
        {
            ResponseModel obj = new ResponseModel();
            List<SelectListItem> uom = new List<SelectListItem>();
            try
            {
                var dt = _masterDB.Get_Uom(Type);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        uom.Add(new SelectListItem { Text = @dr["UOM"].ToString(), Value = @dr["UOM_ID"].ToString() });
                    }

                    obj.Status = "success";
                    obj.Message = "UOM data.";
                    obj.Data = uom;
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

        [Route("api/get_company")]
        [HttpGet]
        public IActionResult Company()
        {
            ResponseModel obj = new ResponseModel();
            List<SelectListItem> uom = new List<SelectListItem>();
            try
            {
                var dt = _masterDB.Get_Company();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        uom.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["CONFIG_ID"].ToString() });
                    }

                    obj.Status = "success";
                    obj.Message = "Company data.";
                    obj.Data = uom;
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

        [Route("api/insert_item")]
        [HttpPost]
        public IActionResult INSERT_ITEM_Master(Item_Master IM)
        {
            ResponseModel obj = new ResponseModel();
             try
            {
                DataTable ItemTable = new DataTable("TBL_ITEM_SERIAL_DETAIL");
                ItemTable.Columns.Add("ITEM_SL_ID", typeof(int));
                ItemTable.Columns.Add("ITEM_ID", typeof(int));
                ItemTable.Columns.Add("SERIAL_NO", typeof(string));
                ItemTable.Columns.Add("QUANTITY", typeof(int));

                DataRow drline = null;
                foreach (var pr in IM.itemLine)
                {
                    drline = ItemTable.NewRow();
                    drline["ITEM_SL_ID"] = pr.Item_sl_id;
                    drline["ITEM_ID"] = pr.Item_id;
                    drline["SERIAL_NO"] = pr.Serial_No;
                    drline["QUANTITY"] = pr.Quantity;

                    ItemTable.Rows.Add(drline);
                }
                string[] str = (_masterDB.Insert_Item_Master(IM.itemHeader, ItemTable)).Split(',');
                if (str[0] == "success")
                {                   
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { };
                }
                else if (str[0] == "update")
                {
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = str[1];
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
        [Route("api/insert_item_bulk")]
        [HttpPost]
        public IActionResult INSERT_ITEM_Master_Bulk(DataTable dt)
        {
            ResponseModel obj = new ResponseModel();
            int Totalcount = 0, successcount = 0, failurecount = 0;
            List<ItemHeader> ilist = new List<ItemHeader>();
            ItemHeader IM = new ItemHeader();

            try
            {
                Totalcount = dt.Rows.Count;
                foreach (DataRow dr in dt.Rows)
                { 
                    IM.industry_id = Convert.ToInt32(dr["industry_id"]);
                    IM.industry_type = dr["industry_type"].ToString();
                    IM.item_category = dr["item_category"].ToString();
                    IM.item_id = 0;
                    IM.item_name = dr["item_name"].ToString();
                    IM.status = dr["status"].ToString();
                    IM.unit_cost = Convert.ToDecimal(dr["unit_cost"].ToString());
                    IM.uom = dr["uom"].ToString();
                    IM.company_id = Convert.ToInt32(dr["company_id"].ToString());
                    IM.created_by = Convert.ToInt32(dr["created_by"].ToString());
                    IM.quantity= Convert.ToInt32(dr["quantity"].ToString());
                    IM.isbatchapp= Convert.ToString(dr["isbatchapp"].ToString());
                    IM.item_type = Convert.ToString(dr["item_type"].ToString());
                    IM.isNegativeallow = Convert.ToInt32(dr["isNegativeallow"].ToString());
                    IM.isLobItem = Convert.ToInt32(dr["isLobItem"].ToString());
                    IM.Inventory_type = Convert.ToString(dr["Inventory_type"].ToString());

                    string[] str = (_masterDB.Insert_Item_Master(IM,null)).Split(',');
                    if (str[0] == "success")
                    {
                        successcount = successcount + 1;
                        obj.Status = "success,"+Totalcount+","+successcount+","+failurecount;
                        obj.Message = str[1];
                        obj.Data = new { };
                    }
                    else if (str[0] == "update")
                    {
                        successcount = successcount + 1;
                        obj.Status = "success," + Totalcount + "," + successcount + "," + failurecount; ;
                        obj.Message = str[1];
                        obj.Data = new { };
                    }
                    else
                    {
                        ilist.Add(IM);
                        failurecount = failurecount + 1;
                        obj.Status = "failure," + Totalcount + "," + successcount + "," + failurecount; ;
                        obj.Message = str[1];
                        obj.Data = new { ilist };
                    }
                }
            }
            catch (Exception ex)
            {
                ilist.Add(IM);
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { ilist };
            }
        
            return Ok(obj);
        }

        [Route("api/insert_uom_conversion")]
        [HttpPost]
        public IActionResult INSERT_UOM_CONVERSION(UOM_Master IM)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                string[] str = (_masterDB.Insert_UOM_Master(IM.uom_id,IM.uom_type, IM.uom, IM.use_base_uom, IM.base_uom, IM.uom_conversion, IM.company_id, IM.created_by)).Split(',');
                if (str[0] == "success")
                {
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { };
                }
                else if (str[0] == "update")
                {
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = str[1];
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

        [Route("api/insert_breed")]
        [HttpPost]
        public IActionResult INSERT_BREED_Master(Breed_Master BM)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                string[] str = (_masterDB.Insert_Breed_Master(BM)).Split(',');
                if (str[0] == "success")
                {
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { };
                }
                else if (str[0] == "update")
                {
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = str[1];
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

        [Route("api/insert_location")]
        [HttpPost]
        public IActionResult INSERT_LOCATION_Master(Location_Master LM)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                string[] str = (_masterDB.Insert_Location_Master(LM)).Split(',');
                if (str[0] == "success")
                {
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { };
                }
                else if (str[0] == "update")
                {
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = str[1];
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



        [Route("api/insert_parameter")]
        [HttpPost]
        public IActionResult INSERT_PARAMETER_Master(Parameter_Master PM)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                string[] str = (_masterDB.Insert_Parameter_Master(PM)).Split(',');
                if (str[0] == "success")
                {
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { };
                }
                else if (str[0] == "update")
                {
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = str[1];
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
        [Route("api/insert_parameter_kpi")]
        [HttpPost]
        public IActionResult INSERT_PARAMETER_Master_kpi(Parameter_Master_Kpi PM)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                string[] str = (_masterDB.Insert_Parameter_Master_KPI(PM)).Split(',');
                if (str[0] == "success")
                {
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { };
                }
                else if (str[0] == "update")
                {
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = str[1];
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
        [Route("api/get_parameter_kpi_summary")]
        [HttpGet]
        public IActionResult parameter_Master_Kpi_Summary(int Company_Id, string Nature_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<Parameter_Master> summarry = new List<Parameter_Master>();
            try
            {
                if (Company_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Nature_Id == null || Nature_Id == "null")
                {
                    obj.Status = "failure";
                    obj.Message = "Nature id id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                var dt = _masterDB.Get_Parameter_Master_kpi(Company_Id, Nature_Id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Parameter_Master_Kpi bd = new Parameter_Master_Kpi();
                        bd.parameter_kpi_id = Convert.ToInt32(dr["parameter_kpi_id"].ToString());
                        bd.dataentry_type_id = Convert.ToInt32(dr["dataentry_type_id"].ToString());
                        bd.parameter_id = Convert.ToInt32(dr["PARAMETER_ID"].ToString());
                        bd.parameter_name = dr["PARAMETER_NAME"].ToString();
                        bd.parameter_type = dr["PARAMETER_TYPE"].ToString();
                        bd.parameter_type_id = Convert.ToInt32(dr["PARAMETER_TYPE_ID"].ToString());
                        bd.lob_id = Convert.ToInt32(dr["LOB_ID"].ToString());
                        bd.line_of_business = dr["LINE_OF_BUSINESS"].ToString();
                        bd.industry_type = dr["INDUSTRY_TYPE"].ToString();
                        bd.industry_id = Convert.ToInt32(dr["INDUSTRY_ID"].ToString());
                        bd.kpi_type = dr["kpi_type"].ToString();
                        bd.kpi_value = Convert.ToDecimal(dr["kpi_value"]);
                        bd.company_id = Convert.ToInt32(dr["COMPANY_ID"].ToString());

                        summarry.Add(bd);
                    }

                    obj.Status = "success";
                    obj.Message = "Parameter master data.";
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
        [Route("api/insert_category")]
        [HttpPost]
        public IActionResult INSERT_CATEGORY_Master(DataEntryType_Master DM)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                string[] str = (_masterDB.Insert_Category_Master(DM)).Split(',');
                if (str[0] == "success")
                {
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { };
                }
                else if (str[0] == "update")
                {
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = str[1];
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

        [Route("api/get_uom_conversion_summary")]
        [HttpGet]
        public IActionResult UOM_CONVERSION_Summary(int Company_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<UOM_Master> summarry = new List<UOM_Master>();
            try
            {
                if (Company_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
               
                var dt = _masterDB.Get_UOM_Conversion_Master(Company_Id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        UOM_Master bs = new UOM_Master();
                        bs.uom_id = Convert.ToInt32(dr["CUOM_ID"]);
                        bs.uom_type = dr["UOM_TYPE"].ToString();
                        bs.base_uom = dr["BASE_UOM"].ToString();
                        bs.use_base_uom = Convert.ToInt32(dr["USE_BASE_UOM"]);
                        bs.uom_conversion = Convert.ToDecimal(dr["UOM_CONVERSION"]);
                        bs.uom = dr["UOM"].ToString();
                        summarry.Add(bs);
                    }
                    obj.Status = "success";
                    obj.Message = "uom summary data.";
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

        [Route("api/get_uom_details")]
        [HttpGet]
        public IActionResult UOM_Conversion_Details(int uom_id)
        {
            ResponseModel obj = new ResponseModel();
            List<UOM_Master> summarry = new List<UOM_Master>();
            try
            {
                if (uom_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "UOM id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var dt = _masterDB.Get_UOM_Conversion_Details(uom_id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        UOM_Master bs = new UOM_Master();
                        bs.uom_id = Convert.ToInt32(dr["CUOM_ID"]);
                        bs.uom_type = dr["UOM_TYPE"].ToString();
                        bs.base_uom = dr["BASE_UOM"].ToString();
                        bs.use_base_uom = Convert.ToInt32(dr["USE_BASE_UOM"]);
                        bs.uom_conversion = Convert.ToDecimal(dr["UOM_CONVERSION"]);
                        bs.uom = dr["UOM"].ToString();
                        summarry.Add(bs);
                    }
                    obj.Status = "success";
                    obj.Message = "uom detail data.";
                    obj.Data = new { summarry };
                }
                else
                {
                    obj.Status = "error";
                    obj.Message = "uom detail data.";
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
        [Route("api/insert_animal_register")]
        [HttpPost]
        public IActionResult INSERT_Animal_register_Master(Animal_Master IM)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                string[] str = (_masterDB.Insert_ANIMAL_REGISTER_Master(IM)).Split(',');
                if (str[0] == "success")
                {
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { };
                }
                else if (str[0] == "update")
                {
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = str[1];
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
        [Route("api/insert_animal_register_bulk")]
        [HttpPost]
        public IActionResult INSERT_Animal_register_Master_bulk(List<Animal_Master> AM)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                DataTable dt = new DataTable("TBL_ANIMAL_REGISTER");
                dt.Columns.Add("ANIMAL_REG_ID", typeof(int));
                dt.Columns.Add("SERIAL_NO", typeof(string));
                dt.Columns.Add("BIRTH_DATE", typeof(string));
                dt.Columns.Add("DAD_NUMBER", typeof(string));
                dt.Columns.Add("MOM_NUMBER", typeof(string));
                dt.Columns.Add("LIVESTOCK_NUMBER", typeof(string));
                dt.Columns.Add("GENDER", typeof(string));
                DataColumn dd = dt.Columns.Add("DEATH_DATE", typeof(string));
                dd.AllowDBNull = true;
                dt.Columns.Add("IS_PREGNENT", typeof(int));
                dt.Columns.Add("PREGNENT_DATE", typeof(string));
                dt.Columns.Add("REMARKS", typeof(string));
                dt.Columns.Add("LOCATION_ID", typeof(int));
                dt.Columns.Add("ITEM_ID", typeof(int));
                dt.Columns.Add("DAD_BREED", typeof(int));
                dt.Columns.Add("MOM_BREED", typeof(int));
                dt.Columns.Add("BREED", typeof(int));
                dt.Columns.Add("SUB_BREED", typeof(int));
                DataRow drProcess = null;
                if (AM != null)
                {
                    foreach (var rw in AM)
                    {
                        drProcess = dt.NewRow();
                        drProcess["ANIMAL_REG_ID"] = 0;
                        drProcess["SERIAL_NO"] = rw.Serial_No;
                        drProcess["BIRTH_DATE"] = rw.Birth_Date;// == "" ? null: rw.Birth_Date;
                        drProcess["DAD_NUMBER"] = rw.Dad_Number;
                        drProcess["MOM_NUMBER"] = rw.Mom_Number;
                        drProcess["LIVESTOCK_NUMBER"] = rw.Livestock_Number;
                        drProcess["GENDER"] = rw.Gender;
                        drProcess["DEATH_DATE"] = rw.Death_Date;// =="" ? DBNull.Value : rw.Death_Date;
                        drProcess["IS_PREGNENT"] = rw.Pregnancy;
                        drProcess["PREGNENT_DATE"] = rw.Pregnancy_date;//== "" ? null : rw.Pregnancy_date;
                        drProcess["REMARKS"] = rw.Remarks;
                        drProcess["LOCATION_ID"] = rw.LOCATION_ID;
                        drProcess["ITEM_ID"] = rw.Item_id;
                        drProcess["DAD_BREED"] = rw.Dad_Breed;
                        drProcess["MOM_BREED"] = rw.Mom_Breed;
                        drProcess["BREED"] = rw.Breed;
                        drProcess["SUB_BREED"] = rw.Sub_Breed;

                        dt.Rows.Add(drProcess);
                    }

                }
                string[] str = (_masterDB.Insert_ANIMAL_REGISTER_Master_bulk(dt, AM[0].COMPANY_ID, AM[0].CREATED_BY)).Split(',');
                if (str[0] == "success")
                {
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { };
                }
                else if (str[0] == "update")
                {
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = str[1];
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


        [Route("api/get_animal_summary")]
        [HttpGet]
        public IActionResult Animal_Master(int Company_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<Animal_Master> summarry = new List<Animal_Master>();
            try
            {
                if (Company_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var dt = _masterDB.Get_Animal_Master(Company_Id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Animal_Master bd = new Animal_Master();

                        bd.ANIMAL_REG_ID = Convert.ToInt32(dr["ANIMAL_REG_ID"].ToString());
                        bd.Birth_Date = dr["Birth_Date"].ToString();
                        bd.Dad_Number = dr["Dad_Number"].ToString();
                        bd.Mom_Breed = dr["Mom_Breed"].ToString();
                        bd.Dad_Breed = dr["Dad_Breed"].ToString();
                        bd.Mom_Number = dr["Mom_Number"].ToString();
                        bd.Livestock_Number = Convert.ToString(dr["Livestock_Number"].ToString());
                        bd.Death_Date = dr["Death_Date"].ToString();
                        bd.Age_Day = Convert.ToInt32(dr["Age_Day"].ToString());
                        bd.Age_Month = Convert.ToInt32(dr["Age_Month"]);
                        bd.Age_Year = Convert.ToInt32(dr["Age_Year"].ToString());
                        bd.COMPANY_ID = Convert.ToInt32(dr["COMPANY_ID"].ToString());
                        bd.CREATED_BY = Convert.ToInt32(dr["CREATED_BY"]);
                        bd.Breed = Convert.ToInt32(dr["Breed"].ToString());
                        bd.Remarks = Convert.ToString(dr["Remarks"].ToString());
                        bd.Shed_Number = dr["Shed_Number"].ToString();
                        bd.Sub_Breed = Convert.ToInt32(dr["Sub_Breed"].ToString());
                        bd.Breed_Name = dr["Breed_Name"].ToString();
                        bd.Mom_Breed_Name = dr["Mom_Breed_Name"].ToString();
                        bd.Sub_Breed_Name = dr["Sub_Breed_Name"].ToString();
                        bd.Location_Name = dr["Location_Name"].ToString();
                        bd.Sub_Location_Name = dr["Sub_Location_Name"].ToString();
                        bd.Gender = dr["Gender"].ToString();
                        bd.Item_id = Convert.ToInt32(dr["Item_id"]);
                        bd.Item_Name = dr["Item_Name"].ToString();
                        bd.Serial_No = dr["Serial_No"].ToString();
                        bd.Dad_Breed_Name = dr["Dad_Breed_Name"].ToString();
                        bd.LOCATION_ID = Convert.ToInt32(dr["Location_id"].ToString());
                        bd.Pregnancy = Convert.ToInt32(dr["isPregnent"]);
                        bd.Pregnancy_date = Convert.ToString(dr["Pregnent_Date"]);
                        bd.livestock_type = Convert.ToString(dr["LIVESTOCK_TYPE"]);
                        summarry.Add(bd);
                    }

                    obj.Status = "success";
                    obj.Message = "Item master data.";
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
        [Route("api/get_animal_detail")]
        [HttpGet]
        public IActionResult Animal_Master_Detail(int animal_id)
        {
            ResponseModel obj = new ResponseModel();
            List<Animal_Master> summarry = new List<Animal_Master>();
            try
            {
                if (animal_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Animal id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var dt = _masterDB.Get_Animal_Detail(animal_id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Animal_Master bd = new Animal_Master();

                        bd.ANIMAL_REG_ID = Convert.ToInt32(dr["ANIMAL_REG_ID"].ToString());
                        bd.Birth_Date = dr["Birth_Date"].ToString();
                        bd.Dad_Number = dr["Dad_Number"].ToString();
                        bd.Mom_Breed = dr["Mom_Breed"].ToString();
                        bd.Mom_Number = dr["Mom_Number"].ToString();
                        bd.Livestock_Number = Convert.ToString(dr["Livestock_Number"].ToString());
                        bd.Death_Date = dr["Death_Date"].ToString();
                        bd.LOCATION_ID = Convert.ToInt32(dr["LOCATION_ID"].ToString());
                        bd.SUB_LOCATION_ID = Convert.ToInt32(dr["SUB_LOCATION_ID"]);
                        //  bd.Age_Year = Convert.ToInt32(dr["Age_Year"].ToString());
                        bd.COMPANY_ID = Convert.ToInt32(dr["COMPANY_ID"].ToString());
                        bd.CREATED_BY = Convert.ToInt32(dr["CREATED_BY"]);
                        bd.Breed = Convert.ToInt32(dr["Breed"].ToString());
                        bd.Remarks = Convert.ToString(dr["Remarks"].ToString());
                        bd.Shed_Number = dr["Shed_Number"].ToString();
                        bd.Gender = dr["Gender"].ToString();
                        bd.Sub_Breed = Convert.ToInt32(dr["Sub_Breed"].ToString());
                        bd.Location_Name = dr["Location_Name"].ToString();
                        bd.Sub_Location_Name = dr["Sub_Location_Name"].ToString();
                        bd.Item_id = Convert.ToInt32(dr["Item_id"]);
                        bd.Item_Name = dr["Item_Name"].ToString();
                        bd.Serial_No = dr["Serial_No"].ToString();
                        bd.Pregnancy = Convert.ToInt32(dr["isPregnent"]);
                        bd.Pregnancy_date = Convert.ToString(dr["Pregnent_Date"]);
                        summarry.Add(bd);
                    }

                    obj.Status = "success";
                    obj.Message = "Item master data.";
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

        [Route("api/get_location")]
        [HttpGet]
        public IActionResult Location_List(int company_id,int Nob_id)
        {
            
               ResponseModel obj = new ResponseModel();
           // List<SelectListItem> item_category = new List<SelectListItem>();
            List<ListItem_Class> item_category = new List<ListItem_Class>();
            try
            {
                var dt = _masterDB.Get_Location_List(company_id,Nob_id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        item_category.Add(new ListItem_Class { area = @dr["AREA"].ToString(), attr_1 = @dr["FARM_TYPE_ID"].ToString(),  Text = @dr["LOCATION_NAME"].ToString(), Value = @dr["LOCATION_ID"].ToString() });
                    }

                    obj.Status = "success";
                    obj.Message = "Location data.";
                    obj.Data = item_category;
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = "Data is not available.";
                    obj.Data = item_category;
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
        [Route("api/get_sub_location")]
        [HttpGet]
        public IActionResult Sub_Location_List(int location_id,int company_id)
        {
            ResponseModel obj = new ResponseModel();
            List<SelectListItem> item_category = new List<SelectListItem>();
            try
            {
                var dt = _masterDB.Get_Sub_Location_List(location_id,company_id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        item_category.Add(new SelectListItem { Text = @dr["SUB_LOCATION_NAME"].ToString(), Value = @dr["SUB_LOCATION_ID"].ToString() });
                    }

                    obj.Status = "success";
                    obj.Message = "Location data.";
                    obj.Data = item_category;
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

        [Route("api/get_animal_livestock")]
        [HttpGet]
        public IActionResult GET_Animal_Livestock(int Company_Id, int item_id, int batch_id)
        {
            ResponseModel obj = new ResponseModel();
            List<Animal_Master> summarry = new List<Animal_Master>();
            try
            {
                if (Company_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var dt = _masterDB.Get_Animal_Livestock(Company_Id, item_id, batch_id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Animal_Master bd = new Animal_Master();

                        bd.ANIMAL_REG_ID = Convert.ToInt32(dr["ANIMAL_REG_ID"].ToString());
                        bd.Birth_Date = dr["Birth_Date"].ToString();
                        bd.Dad_Number = dr["Dad_Number"].ToString();
                        bd.Mom_Breed = dr["Mom_Breed"].ToString();
                        bd.Mom_Number = dr["Mom_Number"].ToString();
                        bd.Livestock_Number = Convert.ToString(dr["Livestock_Number"].ToString());
                        bd.Death_Date = dr["Death_Date"].ToString();
                        bd.Breed_Name = dr["Breed_Name"].ToString();
                        bd.Mom_Breed_Name = dr["Mom_Breed_Name"].ToString();
                        bd.Sub_Breed_Name = dr["Sub_Breed_Name"].ToString();
                        bd.Location_Name = dr["Location_Name"].ToString();
                        bd.Sub_Location_Name = dr["Sub_Location_Name"].ToString();
                        bd.Gender = dr["Gender"].ToString();
                        bd.Serial_No = dr["Serial_no"].ToString();
                        bd.Item_id = Convert.ToInt32(dr["Item_id"]);
                        bd.Item_Name = dr["Item_Name"].ToString();
                        bd.batch_id = Convert.ToInt32(dr["Batch_id"]);
                        bd.Pregnancy = Convert.ToInt32(dr["isPregnent"]);
                        bd.Pregnancy_date = Convert.ToString(dr["Pregnent_Date"]);
                        summarry.Add(bd);
                    }

                    obj.Status = "success";
                    obj.Message = "Livestock data.";
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

        //created by hare krishna 18-jan-2022
        [Route("api/get_animal_livestock_for_inventory")]
        [HttpGet]
        public IActionResult GET_Animal_Livestock_For_Inventory(int Company_Id, int item_id, int batch_id, int inventory)
        {
            ResponseModel obj = new ResponseModel();
            List<Animal_Master> summarry = new List<Animal_Master>();
            try
            {
                if (Company_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                var dt = _masterDB.Get_Animal_Livestock_For_Inventory(Company_Id, item_id, batch_id, inventory);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Animal_Master bd = new Animal_Master();
                        bd.Transfer_id = Convert.ToInt32(dr["Transfer_id"].ToString());
                        bd.ANIMAL_REG_ID = Convert.ToInt32(dr["ANIMAL_REG_ID"].ToString());
                        bd.Birth_Date = dr["Birth_Date"].ToString();
                        bd.Dad_Number = dr["Dad_Number"].ToString();
                        bd.Mom_Breed = dr["Mom_Breed"].ToString();
                        bd.Mom_Number = dr["Mom_Number"].ToString();
                        bd.Livestock_Number = Convert.ToString(dr["Livestock_Number"].ToString());
                        bd.Death_Date = dr["Death_Date"].ToString();
                        bd.Breed_Name = dr["Breed_Name"].ToString();
                        bd.Mom_Breed_Name = dr["Mom_Breed_Name"].ToString();
                        bd.Sub_Breed_Name = dr["Sub_Breed_Name"].ToString();
                        bd.Location_Name = dr["Location_Name"].ToString();
                        bd.Sub_Location_Name = dr["Sub_Location_Name"].ToString();
                        bd.Gender = dr["Gender"].ToString();
                        bd.Serial_No = dr["Serial_no"].ToString();
                        bd.Item_id = Convert.ToInt32(dr["Item_id"]);
                        bd.Item_Name = dr["Item_Name"].ToString();
                        bd.batch_id = Convert.ToInt32(dr["Batch_id"]);
                        bd.Pregnancy = Convert.ToInt32(dr["isPregnent"]);
                        bd.Pregnancy_date = Convert.ToString(dr["Pregnent_Date"]);
                        summarry.Add(bd);
                    }

                    obj.Status = "success";
                    obj.Message = "Livestock data.";
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

        [Route("api/get_Item_master_detail")]
        [HttpGet]
        public IActionResult Item_Master_SERIAL_NO(int Item_id, string inventory_type)
        {
            ResponseModel obj = new ResponseModel();
            List<ItemHeader> summarry = new List<ItemHeader>();
            List<ItemLine> line = new List<ItemLine>();
            try
            {
                if (Item_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Item id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
              
                var dt = _masterDB.Get_Item_Master_Serialno(Item_id, inventory_type);

                if (dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        ItemHeader bd = new ItemHeader();

                        bd.item_id = Convert.ToInt32(dr["ITEM_ID"].ToString());
                        bd.item_name = dr["ITEM_NAME"].ToString();
                        bd.item_category = dr["ITEM_CATEGORY"].ToString();
                        bd.industry_type = dr["INDUSTRY_TYPE"].ToString();
                        bd.uom = dr["UOM"].ToString();
                        bd.unit_cost = Convert.ToDecimal(dr["UNIT_COST"].ToString());
                        bd.status = dr["STATUS"].ToString();
                        bd.company_id = Convert.ToInt32(dr["COMPANY_ID"].ToString());
                        bd.isbatchapp = Convert.ToString(dr["ISBATCHAPP"]);
                        bd.quantity = Convert.ToDecimal(dr["QUANTITY"].ToString());
                        bd.item_type = Convert.ToString(dr["item_type"].ToString());
                        bd.isNegativeallow = Convert.ToInt32(dr["isNegativeAllow"]);
                        bd.isLobItem = Convert.ToInt32(dr["isLOB_ITEM"].ToString());
                        bd.industry_id = Convert.ToInt32(dr["NATURE_ID"].ToString());
                        bd.Inventory_type = Convert.ToString(dr["INVENTORY_TYPE"].ToString());
                        summarry.Add(bd);
                    }
                }
                if (dt.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Tables[1].Rows)
                    {
                        ItemLine il = new ItemLine();

                        il.Item_sl_id = Convert.ToInt32(dr["ITEM_LS_ID"].ToString());
                        il.Item_id = Convert.ToInt32(dr["ITEM_ID"]);
                        il.Serial_No = dr["Serial_No"].ToString();
                        il.Quantity = Convert.ToInt32(dr["Quantity"]);

                        line.Add(il);
                    }
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = "Data is not available.";
                    obj.Data = new { };
                }

                obj.Status = "success";
                obj.Message = "Item master data.";
                obj.Data = new { summarry, line };
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }
        [Route("api/get_SerialNo")]
        [HttpGet]
        public IActionResult Get_SerialNo(int company_id,int item_id, string serial_no)
        {
            ResponseModel obj = new ResponseModel();
            List<SelectListItem> item_category = new List<SelectListItem>();
            try
            {
                var dt = _masterDB.Get_SerialNo(company_id, item_id, serial_no);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        item_category.Add(new SelectListItem { Text = @dr["SERIAL_NO"].ToString(), Value = @dr["SERIAL_NO"].ToString() });
                    }

                    obj.Status = "success";
                    obj.Message = "SerialNo data.";
                    obj.Data = item_category;
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


        //DataEntry get Parameter_input_type on Parameter_Name Selection
        [Route("api/Get_Parameter_Input_Detail")]
        public IActionResult Get_Parameter_Input_Detail(int parameter_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                var dt = _masterDB.Get_ParameterInput_Detail(parameter_id);

                obj.Status = "success";
                obj.Message = "Successfully get record.";
                obj.Data = dt;
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }


        //Delete Location Master
        [Route("api/delete_location_master")]
        public IActionResult deletelocationmaster(int company_id, int location_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                var dt = _masterDB.Delete_location_master(company_id,location_id);

                obj.Status = "success";
                obj.Message = "Successfully get record.";
                obj.Data = dt;
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }

        #region GL Template
        [Route("api/insert_GL_Template")]
        [HttpPost]
        public IActionResult insert_GL_Template(GL_Template GL)
        {
            ResponseModel obj = new ResponseModel();
            try
            {



                string[] str = (_masterDB.Insert_GL_Template_Master(GL)).Split(',');
                if (str[0] == "success")
                {
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { };
                }
                else if (str[0] == "update")
                {
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = str[1];
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

        [Route("api/Get_GL_Template_Summary")]
        public IActionResult Get_GL_Template_Summary(int company_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                var dt = _masterDB.Get_GL_Template_Summary(company_id);

                obj.Status = "success";
                obj.Message = "Successfully get record.";
                obj.Data = dt;
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }

        [Route("api/Get_GL_Code")]
        public IActionResult Get_GL_CODE(int company_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                DataSet ds = new DataSet();
                ds = _masterDB.Get_GL_CODE(company_id);
                var result = ds.Tables[0];
                var result1 = ds.Tables[1];

                obj.Status = "success";
                obj.Message = "Successfully get record.";
                obj.Data = new { result = result, result1 = result1 };
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }

        //use in gl_templete bulk update
        [Route("api/Get_dataentry_type_detail_byname")]
        public IActionResult Get_dataentry_type_detail_byname(int company_id, int lob_id, string item_category_name)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                DataSet ds = new DataSet();
                ds = _masterDB.Get_dataentry_type_detail_byname(company_id, lob_id, item_category_name);
                var result = JsonConvert.SerializeObject(ds.Tables[0]);


                obj.Status = "success";
                obj.Message = "Successfully get record.";
                obj.Data = result;
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

        #region Resourse Card Master
        //Insert resource card 
        [Route("api/insert_resource_card")]
        [HttpPost]
        public IActionResult INSERT_RESOURCE_CARD(resource_cardModel rs)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                string[] str = (_masterDB.Insert_resourcecard(rs)).Split(',');
                if (str[0] == "success")
                {
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { };
                }
                else if (str[0] == "update")
                {
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = str[1];
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

        //Item resource card  Summary
        [Route("api/Get_resource_card_summary")]
        public IActionResult Get_resource_card_summary(int company_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                DataSet ds = new DataSet();
                ds = _masterDB.Get_Resource_Card_summary(company_id);

                var result = JsonConvert.SerializeObject(ds.Tables[0]);
                obj.Status = "success";
                obj.Message = "Successfully get record.";
                obj.Data = new { result };
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }

        //Item resource card  Load
        [Route("api/Get_resource_card_load")]
        public IActionResult Get_resource_card_load(int resourcecard_id ,string view_by)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                DataSet ds = new DataSet();
                ds = _masterDB.Get_Resource_Card_Load(resourcecard_id, view_by);
                var result = JsonConvert.SerializeObject(ds.Tables[0]);
                obj.Status = "success";
                obj.Message = "Successfully get record.";
                obj.Data = new { result };
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

        #region Add Breed Attribure
        //Insert Attribute/Item Attribute 
        //attribute_form(attribute/attribute_item)
        [Route("api/insert_breed_attribute")]
        [HttpPost]
        public IActionResult INSERT_BREED_ATTRIBUTE(Breed_Attribute ba)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                string[] str = (_masterDB.Insert_breed_Attribute(ba)).Split(',');
                if (str[0] == "success")
                {
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { };
                }
                else if (str[0] == "update")
                {
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = str[1];
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

        //Breed Attribure Summart
        [Route("api/Get_breed_attribute_summary")]
        public IActionResult Get_breed_attribute_summary(int company_id, int breed_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                DataSet ds = new DataSet();
                ds = _masterDB.Get_Breed_attribute_summary(company_id, breed_id);

                var result = JsonConvert.SerializeObject(ds.Tables[0]);
                var result1 = JsonConvert.SerializeObject(ds.Tables[1]);

                obj.Status = "success";
                obj.Message = "Successfully get record.";
                obj.Data = new { result, result1 };
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

        #region Add Item Attribure
        //Insert Attribute/Item Attribute 
        //attribute_form(attribute/attribute_item)
        [Route("api/insert_item_attribute")]
        [HttpPost]
        public IActionResult INSERT_ITEM_ATTRIBUTE(item_Attribute ba)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                string[] str = (_masterDB.Insert_item_Attribute(ba)).Split(',');
                if (str[0] == "success")
                {
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { };
                }
                else if (str[0] == "update")
                {
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = str[1];
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

        //Item Attribure Summart
        [Route("api/Get_item_attribute_summary")]
        public IActionResult Get_item_attribute_summary(int company_id, int item_id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                DataSet ds = new DataSet();
                ds = _masterDB.Get_Item_attribute_summary(company_id, item_id);

                var result = JsonConvert.SerializeObject(ds.Tables[0]);
                var result1 = JsonConvert.SerializeObject(ds.Tables[1]);

                obj.Status = "success";
                obj.Message = "Successfully get record.";
                obj.Data = new { result, result1 };
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

        [Route("api/get_all_nob_location")]
        [HttpGet]
        public IActionResult All_nob_location(int nob_id, int company_id)
        {
            ResponseModel obj = new ResponseModel();
            List<SelectListItem> item_category = new List<SelectListItem>();
            try
            {
                var dt = _masterDB.Get_All_NOB_Location_List(nob_id, company_id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        item_category.Add(new SelectListItem { Text = @dr["SUB_LOCATION_NAME"].ToString(), Value = @dr["SUB_LOCATION_ID"].ToString() });
                    }

                    obj.Status = "success";
                    obj.Message = "Location data.";
                    obj.Data = item_category;
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

        [Route("api/get_all_batch_bylocation_list")]
        [HttpGet]
        public IActionResult all_batch_bylocation_list(int nob_id, int company_id,string location)
        {
            ResponseModel obj = new ResponseModel();
            List<SelectListItem> item_category = new List<SelectListItem>();
            try
            {
                var dt = _masterDB.Get_All_Batch_ByLocation_List(nob_id, company_id, location);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        item_category.Add(new SelectListItem { Text = @dr["BATCH_NO"].ToString(), Value = @dr["BATCH_ID"].ToString() });
                    }

                    obj.Status = "success";
                    obj.Message = "Location data.";
                    obj.Data = item_category;
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

        [Route("api/get_all_item_bylocation_and_batch")]
        [HttpGet]
        public IActionResult all_item_bylocation_and_batch_list(int nob_id, int company_id, string location,string batch, string category, string transfer)
        {
            ResponseModel obj = new ResponseModel();
            List<SelectListItem> item = new List<SelectListItem>();
            List<SelectListItem> item_category = new List<SelectListItem>();
            List<SelectListItem> transfer_type = new List<SelectListItem>();
            try
            {
                DataSet ds = new DataSet();
                 ds = _masterDB.Get_All_Item_ByLocation_And_Batch_List(nob_id, company_id, location, batch, category, transfer);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        item.Add(new SelectListItem { Text = @dr["ITEM_NAME"].ToString(), Value = @dr["ITEM_ID"].ToString() });
                    }
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        item_category.Add(new SelectListItem { Text = @dr["ITEM_CATEGORY"].ToString(), Value = @dr["ITEM_CATEGORY"].ToString() });
                    }
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        transfer_type.Add(new SelectListItem { Text = @dr["TRANSFER_TYPE"].ToString(), Value = @dr["TRANSFER_TYPE"].ToString() });
                    }
                }
                obj.Status = "success";
                    obj.Message = "Location data.";
                    obj.Data = new { item, item_category , transfer_type };
                
                 
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