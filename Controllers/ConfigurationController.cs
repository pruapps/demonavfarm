using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FarmIT_Api.database_accesslayer;
using FarmIT_Api.Models;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Newtonsoft.Json;

namespace FarmIT_Api.Controllers
{
    [ApiController]
    [Authorize]
    [RESTAuthorizeAttribute]
    public class ConfigurationController : ControllerBase
    {
        private IConfigDB _configdb;
        public ConfigurationController(IConfigDB configDB)
        {
            _configdb = configDB;
        }

        #region Plan
        [Route("api/subscription_plan")]
        [HttpGet]
        public IActionResult Subscription_Plan()
        {          
            ResponseModel obj = new ResponseModel();
            List<SubscriptionPlanModel> plan_list = new List<SubscriptionPlanModel>();
            try
            {
                var dt = _configdb.Get_Subscription_Plan();
                if(dt.Rows.Count>0)
                {
                    //int planid = 0;                    
                    //foreach (DataRow dr in dt.Rows)
                    //{                        
                    //    SubscriptionPlanModel sp = new SubscriptionPlanModel();
                                                
                    //    if(planid != Convert.ToInt32(dr["PLAN_ID"].ToString()))
                    //    {
                    //        planid = Convert.ToInt32(dr["PLAN_ID"].ToString());

                    //        sp.Plan_Id = Convert.ToInt32(dr["PLAN_ID"].ToString());
                    //        sp.Plan_Name = dr["PLAN_NAME"].ToString();
                    //        sp.Plan_Type = dr["PLAN_TYPE"].ToString();

                    //        List<Plan_Feature> featurelist = new List<Plan_Feature>();
                    //        foreach (DataRow dr1 in dt.Select("PLAN_ID = " + planid + ""))
                    //        {                                
                    //            Plan_Feature pf = new Plan_Feature();

                    //            pf.Feature_Id = Convert.ToInt32(dr1["FEATURE_ID"].ToString());
                    //            pf.Feature_Name = dr1["FEATURE_NAME"].ToString();
                    //            featurelist.Add(pf);
                    //        }
                    //        sp.Features = featurelist;

                    //        plan_list.Add(sp);
                    //    }                            
                    //}

                    foreach(DataRow dr in dt.Rows)
                    {
                        SubscriptionPlanModel sp = new SubscriptionPlanModel();
                        sp.Plan_Id = Convert.ToInt32(dr["PLAN_ID"].ToString());
                        sp.Plan_Name = dr["PLAN_NAME"].ToString();
                        sp.Plan_Type = dr["PLAN_TYPE"].ToString();
                        sp.Plan_Details = dr["PLAN_DETAILS"].ToString();
                        sp.Button_Name = dr["BUTTON_NAME"].ToString();
                        plan_list.Add(sp);
                    }
                    obj.Status = "success";
                    obj.Message = "plan data";
                    obj.Data = plan_list;
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = "No data available";
                    obj.Data = new { };
                }
            }
            catch(Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }

        [Route("api/razorpay_plan")]
        [HttpGet]
        public IActionResult RazorPay_Plan(int User_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<RazorPay_Plan_Model> plan_list = new List<RazorPay_Plan_Model>();
            List<RazorPay_Plan> plans = new List<RazorPay_Plan>();
            List<BillingInfoModel> billing_info = new List<BillingInfoModel>();
            try
            {
                var ds = _configdb.Get_RazorPay_Plan(User_Id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //int planid = 0;                    
                    //foreach (DataRow dr in dt.Rows)
                    //{                        
                    //    SubscriptionPlanModel sp = new SubscriptionPlanModel();

                    //    if(planid != Convert.ToInt32(dr["PLAN_ID"].ToString()))
                    //    {
                    //        planid = Convert.ToInt32(dr["PLAN_ID"].ToString());

                    //        sp.Plan_Id = Convert.ToInt32(dr["PLAN_ID"].ToString());
                    //        sp.Plan_Name = dr["PLAN_NAME"].ToString();
                    //        sp.Plan_Type = dr["PLAN_TYPE"].ToString();

                    //        List<Plan_Feature> featurelist = new List<Plan_Feature>();
                    //        foreach (DataRow dr1 in dt.Select("PLAN_ID = " + planid + ""))
                    //        {                                
                    //            Plan_Feature pf = new Plan_Feature();

                    //            pf.Feature_Id = Convert.ToInt32(dr1["FEATURE_ID"].ToString());
                    //            pf.Feature_Name = dr1["FEATURE_NAME"].ToString();
                    //            featurelist.Add(pf);
                    //        }
                    //        sp.Features = featurelist;

                    //        plan_list.Add(sp);
                    //    }                            
                    //}
                    DataTable dt = ds.Tables[0];
                    DataTable dt1 = ds.Tables[1];
                    int count = 0;
                    RazorPay_Plan_Model rpm = new RazorPay_Plan_Model();
                    foreach (DataRow dr in dt.Rows)
                    {
                        if(count==0)
                        {
                            //rpm.title = dr["TITLE"].ToString();
                            //rpm.sub_title = dr["SUB_TITLE"].ToString();
                            rpm.trial_period = dr["TRIAL_PERIOD"].ToString();
                            rpm.plan_start_date = dr["PLAN_START_DATE"].ToString();
                            rpm.trial_end_date = dr["TRIAL_END_DATE"].ToString();
                            count = count+ 1;
                        }
                        RazorPay_Plan rp = new RazorPay_Plan();
                        rp.plan_id = dr["PLAN_ID"].ToString();
                        rp.plan_description = dr["PLAN_DESCRIPTION"].ToString();
                        rp.plan_duration = dr["PLAN_DURATION"].ToString();
                        rp.plan_price = dr["PLAN_PRICE"].ToString();
                        rp.current_timestamp = dr["CURRENT_TIMESTAMP"].ToString();
                        rp.base_price = dr["BASE_AMOUNT"].ToString();
                        rp.discount = dr["DISCOUNT"].ToString();
                        rp.currency = dr["currency"].ToString();
                        plans.Add(rp);
                    }
                    rpm.plans = plans;
                    plan_list.Add(rpm);

                    foreach(DataRow dr in dt1.Rows)
                    {
                        BillingInfoModel bm = new BillingInfoModel();
                        bm.BILLING_INFO_ID =Convert.ToInt32(dr["BILLING_INFO_ID"].ToString());
                        bm.FULL_NAME = dr["FULL_NAME"].ToString();
                        bm.EMAIL = dr["EMAIL"].ToString();
                        bm.PHONE_NO = dr["PHONE_NO"].ToString();
                        bm.ADDRESS = dr["ADDRESS"].ToString();
                        bm.ZIP_CODE = Convert.ToInt32(dr["ZIP_CODE"].ToString());
                        bm.COUNTRY = dr["COUNTRY"].ToString();
                        bm.CURRENCY = dr["Currency"].ToString();
                        billing_info.Add(bm);
                    }

                    obj.Status = "success";
                    obj.Message = "plan data";
                    obj.Data =new { plan_list,billing_info };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = "No data available";
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
        [Route("api/razorpay_plan_all")]
        [HttpGet]
        public IActionResult RazorPay_Plan_All()
        {
            ResponseModel obj = new ResponseModel();
            List<RazorPay_Plan> plans = new List<RazorPay_Plan>();
            try
            {
                var ds = _configdb.Get_RazorPay_Plan();
                if (ds.Tables[0].Rows.Count > 0)
                {
                  
                    DataTable dt = ds.Tables[0];
                    
                    RazorPay_Plan_Model rpm = new RazorPay_Plan_Model();
                    foreach (DataRow dr in dt.Rows)
                    {                       
                        RazorPay_Plan rp = new RazorPay_Plan();
                        rp.plan_id = dr["PLAN_ID"].ToString();
                        rp.plan_description = dr["PLAN_DESCRIPTION"].ToString();
                        rp.plan_duration = dr["PLAN_DURATION"].ToString();
                        rp.plan_price = dr["PLAN_PRICE"].ToString();
                        rp.current_timestamp = dr["CURRENT_TIMESTAMP"].ToString();
                        rp.base_price = dr["BASE_AMOUNT"].ToString();
                        rp.discount = dr["DISCOUNT"].ToString();
                        rp.currency = dr["currency"].ToString();
                        rp.trial_period = dr["TRIAL_PERIOD"].ToString();
                        rp.plan_start_date = dr["PLAN_START_DATE"].ToString();
                        rp.trial_end_date = dr["TRIAL_END_DATE"].ToString();
                        rp.status = dr["Status"].ToString();
                        plans.Add(rp);
                    }

                    obj.Status = "success";
                    obj.Message = "plan data";
                    obj.Data = new { plans };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = "No data available";
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
        [Route("api/razorpay_plan_update")]
        [HttpPost]
        public IActionResult RazorPay_Plan_Update(string plan_id,string status)
        {
            ResponseModel obj = new ResponseModel();
             try
            {
                string res = _configdb.RazorPay_Plan_Update(plan_id, status);

                if (res == "success")
                {
                    obj.Status = "success";
                    obj.Message = "successfully updated plan status.";
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = res;
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

        #region Process Configuration Setup
        [Route("api/process_setup")]
        [HttpGet]
        public IActionResult Process_setup()
        {
            ResponseModel obj = new ResponseModel();
            List<SelectListItem> NATURE_OF_BUSNINESS = new List<SelectListItem>();
            List<LOB_Model> LINE_OF_BUSNINESS = new List<LOB_Model>();
            List<SelectListItem> FARMER_TYPE = new List<SelectListItem>();
            try
            {
                var ds = _configdb.Process_Setup();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            NATURE_OF_BUSNINESS.Add(new SelectListItem { Text = @dr["NATURE_OF_BUSNINESS"].ToString(), Value = @dr["NATURE_ID"].ToString() });
                        }
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            LINE_OF_BUSNINESS.Add(new LOB_Model { text = @dr["LINE_OF_BUSNINESS"].ToString(), value = @dr["LINE_ID"].ToString(),selected=false,nature_id = @dr["NATURE_ID"].ToString() });
                        }
                    }
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            FARMER_TYPE.Add(new SelectListItem { Text = @dr["FARMER_TYPE"].ToString(), Value = @dr["FARMER_TYPE_ID"].ToString() });
                        }
                    }

                    obj.Status = "success";
                    obj.Message = "plan data";
                    obj.Data = new { NATURE_OF_BUSNINESS, LINE_OF_BUSNINESS,FARMER_TYPE };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = "No data available";
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

        [Route("api/Get_Configuration_Setup")]
        [HttpGet]
        public IActionResult Get_Configuration_Setup(int company_id, string nature_id)
        {
            ResponseModel obj = new ResponseModel();
            List<SelectListItem> nob = new List<SelectListItem>();
            List<LOB_Model> lob = new List<LOB_Model>();
            List<Profile_Data> company = new List<Profile_Data>();
            List<User_Data> users = new List<User_Data>();
            string farmer_type = "";
            int count = 0;
            try
            {
                var ds = _configdb.Get_Configuration_Setup(company_id, nature_id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (count == 0)
                        {
                            farmer_type = @dr["FARMER_TYPE"].ToString();
                            count = count + 1;
                        }

                        nob.Add(new SelectListItem { Text = @dr["NATURE_OF_BUSINESS"].ToString(), Value = @dr["NATURE_ID"].ToString(), Selected = (dr["N_SELECTED"].ToString() == "0" ? false : true) });
                    }
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        lob.Add(new LOB_Model { text = @dr["LINE_OF_BUSINESS"].ToString(), value = @dr["LINE_ID"].ToString(), selected = (dr["L_SELECTED"].ToString() == "0" ? false : true), nature_id = @dr["NATURE_ID"].ToString() });
                    }
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        Profile_Data pd = new Profile_Data();
                        pd.config_id = Convert.ToInt32(dr["COMPANY_ID"].ToString());
                        pd.company_name = dr["NAME"].ToString();
                        pd.email = dr["EMAIL"].ToString();
                        pd.mobile_no = dr["MOBILE_NO"].ToString();
                        pd.address = dr["ADDRESS"].ToString();
                        pd.tax_num = dr["TAX_NUM"].ToString();
                        pd.logo = (byte[])dr["PIC"];
                        pd.isDefault = Convert.ToInt32(dr["isDefault"].ToString());
                        company.Add(pd);
                    }
                    foreach (DataRow dr1 in ds.Tables[3].Rows)
                    {
                        User_Data ud = new User_Data();
                        ud.user_id = Convert.ToInt32(dr1["USER_ID"].ToString());
                        ud.user_name = dr1["USER_NAME"].ToString();
                        ud.mobile_no = dr1["MOBILE_NO"].ToString();
                        ud.email = dr1["EMAIL"].ToString();
                        ud.status = dr1["STATUS"].ToString();
                        users.Add(ud);
                    }
                    obj.Status = "success";
                    obj.Message = "Selected configuration setup data.";
                    obj.Data = new { farmer_type, nob, lob, company, users };
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

        [Route("api/process_configuration_setup")]
        [HttpPost]
        public IActionResult Process_configuration_setup(Process_Configuration_Model PC_Model)
        {
            ResponseModel obj = new ResponseModel();
            string company_id = "";
            try
            {
                var config = PC_Model.configuration_setup;
                var process = PC_Model.process_setup;
                var user = PC_Model.user_setup;

                var farmertype = process.First().FARMER_TYPE;
                var sharewithteam = config.SHARE_WITH_TEAM;
                var created_by = config.CREATED_BY;
                var social_id = config.SOCIAL_ID;
                int license = (config.LICENSE - 1);
                int user_count = 0;
                if (user != null)
                {
                    user_count = user.Count();
                }

                if (config == null)
                {
                    obj.Status = "failure";
                    obj.Message = "Configuration setup data is not correct.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                if (process == null)
                {
                    obj.Status = "failure";
                    obj.Message = "Process setup data is not correct.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                if (farmertype.ToLower() == "company")
                {
                    if (sharewithteam != 0)
                    {
                        if (user == null)
                        {
                            obj.Status = "failure";
                            obj.Message = "User data is not correct.";
                            obj.Data = new { };
                            return Ok(obj);
                        }
                    }
                }

                if (social_id != null)
                {
                    if (config.MOBILE_NO == null)
                    {
                        obj.Status = "failure";
                        obj.Message = "Please enter mobile number.";
                        obj.Data = new { };
                        return Ok(obj);
                    }
                }

                if (license < user_count)
                {
                    obj.Status = "failure";
                    obj.Message = "No of users is not greater then the no of license.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                DataTable configTable = new DataTable("TBL_CONFIGURATION_SETUP");
                configTable.Columns.Add("COMPANY_ID", typeof(int));
                configTable.Columns.Add("SHARE_WITH_TEAM", typeof(int));
                configTable.Columns.Add("NAME", typeof(string));
                configTable.Columns.Add("COMPANY_LOGO", typeof(byte[]));
                configTable.Columns.Add("MOBILE", typeof(string));
                configTable.Columns.Add("EMAIL", typeof(string));
                configTable.Columns.Add("ADDRESS", typeof(string));
                configTable.Columns.Add("TAX_NUM", typeof(string));
                configTable.Columns.Add("SOCIAL_ID", typeof(string));
                configTable.Columns.Add("isDefault", typeof(int));

                DataRow dr = null;
                dr = configTable.NewRow();
                dr["COMPANY_ID"] = config.COMPANY_ID;
                dr["SHARE_WITH_TEAM"] = config.SHARE_WITH_TEAM;
                dr["NAME"] = config.NAME;
                dr["COMPANY_LOGO"] = config.COMPANY_LOGO;
                dr["MOBILE"] = config.MOBILE_NO;
                dr["EMAIL"] = config.EMAIL;
                dr["ADDRESS"] = config.ADDRESS;
                dr["TAX_NUM"] = config.TAX_NUM;
                dr["SOCIAL_ID"] = config.SOCIAL_ID;
                dr["isDefault"] = config.isDefault;
                configTable.Rows.Add(dr);

                DataTable processTable = new DataTable("TBL_PROCESS_SETUP");
                processTable.Columns.Add("NATURE_ID", typeof(int));
                processTable.Columns.Add("LINE_ID", typeof(int));
                processTable.Columns.Add("FARMER_TYPE", typeof(string));

                DataRow drProcess = null;
                foreach (var pr in process)
                {
                    drProcess = processTable.NewRow();
                    drProcess["NATURE_ID"] = Convert.ToInt32(pr.NATURE_ID);
                    drProcess["LINE_ID"] = Convert.ToInt32(pr.LINE_ID);
                    drProcess["FARMER_TYPE"] = pr.FARMER_TYPE;

                    processTable.Rows.Add(drProcess);
                }

                DataTable userTable = new DataTable("TBL_USER_SETUP");
                userTable.Columns.Add("USER_ID", typeof(int));
                userTable.Columns.Add("NAME", typeof(string));
                userTable.Columns.Add("MOBILE_NO", typeof(string));
                userTable.Columns.Add("EMAIL", typeof(string));
                userTable.Columns.Add("STATUS", typeof(string));

                DataRow drUser = null;
                if (user != null)
                {
                    foreach (var usr in user)
                    {
                        drUser = userTable.NewRow();
                        drUser["USER_ID"] = usr.USER_ID;
                        drUser["NAME"] = usr.Name;
                        drUser["MOBILE_NO"] = usr.Mobile_No;
                        drUser["EMAIL"] = usr.Email;
                        drUser["STATUS"] = usr.STATUS;

                        userTable.Rows.Add(drUser);
                    }
                }
                else
                {
                    userTable = null;
                }

                string[] res = (_configdb.Process_Configuration_Setup(configTable, processTable, userTable, created_by)).Split(',');

                if (res[0] == "success")
                {
                    company_id = res[1];
                    obj.Status = "success";
                    obj.Message = "successfully setup.";
                    obj.Data = new { company_id };
                }
                else if (res[0] == "update")
                {
                    company_id = res[1];
                    obj.Status = "success";
                    obj.Message = "Setup successfully updated.";
                    obj.Data = new { company_id };
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

        [Route("api/Get_Users_List")]
        [HttpGet]
        public IActionResult Users_List(int Company_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<User_List> users_list = new List<User_List>();
            try
            {
                var ds = _configdb.Get_Users_List(Company_Id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        User_List ul = new User_List();
                        ul.user_id = Convert.ToInt32(dr["USER_ID"].ToString());
                        ul.user_name = dr["USER_NAME"].ToString();
                        ul.company_name = dr["COMPANY_NAME"].ToString();
                        ul.email = dr["EMAIL"].ToString();
                        ul.mobile_no = dr["MOBILE_NO"].ToString();
                        ul.location_name = dr["LOCATION_NAME"].ToString();
                        ul.location_id = Convert.ToInt32(dr["LOCATION_ID"].ToString());
                        ul.role_name = dr["ROLE_NAME"].ToString();
                        ul.role_id = Convert.ToInt32(dr["ROLE_ID"].ToString());
                        ul.status = dr["STATUS"].ToString();
                        ul.sub_location_name = dr["SUB_LOCATION_NAME"].ToString();
                        ul.sub_location_id = Convert.ToInt32(dr["SUB_LOCATION_ID"].ToString());
                        users_list.Add(ul);
                    }

                    var locations = (from DataRow dr in ds.Tables[1].Rows
                                     select new SettingModel()
                                     {
                                         location_id = Convert.ToInt32(dr["LOCATION_ID"].ToString()),
                                         location_name = dr["LOCATION_NAME"].ToString()
                                     }).ToList();

                    obj.Status = "success";
                    obj.Message = "Users list data.";
                    obj.Data = new { users_list ,locations }  ;
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

        [Route("api/insert_user_setup")]
        [HttpPost]
        public IActionResult User_Setup(User_Model[] user_setup)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                var user = user_setup;
                DataTable userTable = new DataTable("TBL_USER_SETUP");
                userTable.Columns.Add("USER_ID", typeof(int));
                userTable.Columns.Add("NAME", typeof(string));
                userTable.Columns.Add("MOBILE_NO", typeof(string));
                userTable.Columns.Add("EMAIL", typeof(string));
                userTable.Columns.Add("STATUS", typeof(string));
                userTable.Columns.Add("COMPANY_ID", typeof(string));

                DataRow drUser = null;
                if (user != null)
                {
                    foreach (var usr in user)
                    {
                        drUser = userTable.NewRow();
                        drUser["USER_ID"] = usr.USER_ID;
                        drUser["NAME"] = usr.Name;
                        drUser["MOBILE_NO"] = usr.Mobile_No;
                        drUser["EMAIL"] = usr.Email;
                        drUser["STATUS"] = usr.STATUS;
                        drUser["COMPANY_ID"] = usr.COMPANY_ID;
                        userTable.Rows.Add(drUser);
                    }
                }
                else
                {
                    userTable = null;
                }

                string[] res = (_configdb.insert_user_Setup(userTable)).Split(',');

                if (res[0] == "success")
                {
                    obj.Status = "success";
                    obj.Message = "User successfully Created.";
                    obj.Data = new { };
                }
                else if (res[0] == "update")
                {
                    obj.Status = "success";
                    obj.Message = "User successfully updated.";
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

        #endregion

        #region Template Settings
        [Route("api/template_settings")]
        [HttpPost]
        public IActionResult Template_Settings(TemplateSettingsModel TS_Model)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                var header = TS_Model.header;
                var lines = TS_Model.lines;
                var KPIlines = TS_Model.KPIlines;

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
                if (header.LOB_ID != 16)
                {
                    if (header.BREED_ID == 0)
                    {
                        obj.Status = "failure";
                        obj.Message = "Breed name can not blank.";
                        obj.Data = new { };
                        return Ok(obj);
                    }
                }
                if (header.STATUS == null)
                {
                    obj.Status = "failure";
                    obj.Message = "Status can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (header.LOCATION == null)
                {
                    obj.Status = "failure";
                    obj.Message = "User location can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                if (lines == null)
                {
                    obj.Status = "failure";
                    obj.Message = "Template line can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                DataTable headerTable = new DataTable("TBL_TEMPLATE_HEADER");
                headerTable.Columns.Add("TEMPLATE_ID", typeof(int));
                headerTable.Columns.Add(new DataColumn { ColumnName = "COMPANY_ID", DataType = typeof(int), AllowDBNull = true });
                headerTable.Columns.Add("NOB_ID", typeof(int));
                headerTable.Columns.Add("LOB_ID", typeof(int));
                headerTable.Columns.Add("TEMPLATE_NAME", typeof(string));
                headerTable.Columns.Add("BREED_ID", typeof(int));
                headerTable.Columns.Add("STATUS", typeof(string));
                headerTable.Columns.Add("LOCATION", typeof(string));

                DataRow dr = null;
                dr = headerTable.NewRow();
                dr["TEMPLATE_ID"] = header.TEMPLATE_ID;
                dr["COMPANY_ID"] = header.COMPANY_ID;
                dr["NOB_ID"] = header.NOB_ID;
                dr["LOB_ID"] = header.LOB_ID;
                dr["TEMPLATE_NAME"] = header.TEMPLATE_NAME;
                dr["BREED_ID"] = header.BREED_ID;
                dr["STATUS"] = header.STATUS;
                dr["LOCATION"] = header.LOCATION;
                headerTable.Rows.Add(dr);

                DataTable lineTable = new DataTable("TBL_TEMPLATE_LINE");
                lineTable.Columns.Add("PARAMETER_TYPE_ID", typeof(int));
                lineTable.Columns.Add("PARAMETER_ID", typeof(int));
                lineTable.Columns.Add("DATAENTRY_TYPE_ID", typeof(int));
                lineTable.Columns.Add("OCCURRENCE", typeof(string));
                lineTable.Columns.Add("ITEM_ID", typeof(int));
                lineTable.Columns.Add("FREQUENCY_START_DATE", typeof(Int32));
                lineTable.Columns.Add("FREQUENCY_END_DATE", typeof(Int32));
                lineTable.Columns.Add("STATUS", typeof(string));
                lineTable.Columns.Add("UOM", typeof(string));

                DataRow drline = null;
                foreach (var pr in lines)
                {
                    drline = lineTable.NewRow();
                    drline["PARAMETER_TYPE_ID"] = pr.PARAMETER_TYPE_ID;
                    drline["PARAMETER_ID"] = pr.PARAMETER_ID;
                    drline["DATAENTRY_TYPE_ID"] = pr.DATAENTRY_TYPE_ID;
                    drline["OCCURRENCE"] = pr.OCCURRENCE;
                    drline["ITEM_ID"] = pr.ITEM_ID;
                    drline["FREQUENCY_START_DATE"] = pr.FREQUENCY_START_DATE;
                    drline["FREQUENCY_END_DATE"] = pr.FREQUENCY_END_DATE;
                    drline["STATUS"] = pr.STATUS;
                    drline["UOM"] = pr.UOM;
                    lineTable.Rows.Add(drline);
                }

                DataTable KPIlineTable = new DataTable("TBL_TEMPLATE_KPI_LINE");
                KPIlineTable.Columns.Add("PARAMETER_TYPE_ID", typeof(int));
                KPIlineTable.Columns.Add("PARAMETER_ID", typeof(int));
                KPIlineTable.Columns.Add("DATAENTRY_TYPE_ID", typeof(int));
                KPIlineTable.Columns.Add("OCCURRENCE", typeof(string));
                KPIlineTable.Columns.Add("ITEM_ID", typeof(int));
                KPIlineTable.Columns.Add("FREQUENCY_START_DATE", typeof(Int32));
                KPIlineTable.Columns.Add("FREQUENCY_END_DATE", typeof(Int32));
                KPIlineTable.Columns.Add("STATUS", typeof(string));
                KPIlineTable.Columns.Add("UOM", typeof(string));
                KPIlineTable.Columns.Add("FREQUENCY_KPI_START_DATE", typeof(Int32));
                KPIlineTable.Columns.Add("FREQUENCY_KPI_END_DATE", typeof(Int32));
                KPIlineTable.Columns.Add("KPI_TYPE", typeof(string));
                KPIlineTable.Columns.Add("KPI_VALUE", typeof(Double));

                DataRow KPIdrline = null;
                foreach (var kpi in KPIlines)
                {
                    KPIdrline = KPIlineTable.NewRow();
                    KPIdrline["PARAMETER_TYPE_ID"] = kpi.PARAMETER_TYPE_ID;
                    KPIdrline["PARAMETER_ID"] = kpi.PARAMETER_ID;
                    KPIdrline["DATAENTRY_TYPE_ID"] = kpi.DATAENTRY_TYPE_ID;
                    KPIdrline["OCCURRENCE"] = kpi.OCCURRENCE;
                    KPIdrline["ITEM_ID"] = kpi.ITEM_ID;
                    KPIdrline["FREQUENCY_START_DATE"] = kpi.FREQUENCY_START_DATE;
                    KPIdrline["FREQUENCY_END_DATE"] = kpi.FREQUENCY_END_DATE;
                    KPIdrline["STATUS"] = kpi.STATUS;
                    KPIdrline["UOM"] = kpi.UOM;
                    KPIdrline["FREQUENCY_KPI_START_DATE"] = kpi.FREQUENCY_KPI_START_DATE;
                    KPIdrline["FREQUENCY_KPI_END_DATE"] = kpi.FREQUENCY_KPI_END_DATE;
                    KPIdrline["KPI_TYPE"] = kpi.KPI_TYPE;
                    KPIdrline["KPI_VALUE"] = kpi.KPI_VALUE;
                    KPIlineTable.Rows.Add(KPIdrline);
                }

                string[] res = (_configdb.Insert_Template_Settings(headerTable, lineTable, KPIlineTable, TS_Model.Created_by, TS_Model.header.BATCH_START_FROM)).Split(',');

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

        [Route("api/get_template_details")]
        [HttpGet]
        public IActionResult Template_Details(int Template_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<Template_Header> header = new List<Template_Header>();
            List<Template_Line> lines = new List<Template_Line>();
            try
            {
                var ds = _configdb.Get_Template_Details(Template_Id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Template_Header th = new Template_Header();
                        th.TEMPLATE_ID = Convert.ToInt32(dr["TEMPLATE_ID"].ToString());
                        th.TEMPLATE_NAME = dr["TEMPLATE_NAME"].ToString();
                        th.NOB_ID = Convert.ToInt32(dr["NOB_ID"].ToString());
                        th.NATURE_OF_BUSINESS = dr["NATURE_OF_BUSINESS"].ToString();
                        th.LOB_ID = Convert.ToInt32(dr["LOB_ID"].ToString());
                        th.LINE_OF_BUSINESS = dr["LINE_OF_BUSINESS"].ToString();
                        th.BREED_ID = Convert.ToInt32(dr["BREED_ID"].ToString());
                        th.BREED_NAME = dr["BREED_NAME"].ToString();
                        th.STATUS = dr["STATUS"].ToString();
                        th.BATCH_START_FROM = dr["BATCH_START_FROM"].ToString();
                        header.Add(th);
                    }
                    var table2 = ds.Tables[2];
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        Template_Line tl = new Template_Line();
                        tl.LINE_ID = Convert.ToInt32(dr["LINE_ID"].ToString());
                        tl.PARAMETER_TYPE_ID = Convert.ToInt32(dr["PARAMETER_TYPE_ID"].ToString());
                        tl.PARAMETER_TYPE = dr["PARAMETER_TYPE"].ToString();
                        tl.PARAMETER_ID = Convert.ToInt32(dr["PARAMETER_ID"].ToString());
                        tl.PARAMETER_NAME = dr["PARAMETER_NAME"].ToString();
                        tl.DATAENTRY_TYPE_ID = Convert.ToInt32(dr["DATAENTRY_TYPE_ID"].ToString());
                        tl.DATAENTRY_TYPE = dr["DATAENTRY_TYPE"].ToString();
                        tl.UOM = dr["UOM"].ToString();
                        tl.ALTERNATE_UOM = dr["ALTERNATE_UOM"].ToString();
                        tl.OCCURRENCE = dr["OCCURRENCE"].ToString();
                        tl.ITEM_ID = Convert.ToInt32(dr["ITEM_ID"].ToString());
                        tl.ITEM_NAME = dr["ITEM_NAME"].ToString();
                        tl.F_START_DATE = dr["FREQUENCY_START_DATE"].ToString();
                        tl.F_END_DATE = dr["FREQUENCY_END_DATE"].ToString();

                        List<TemplateKpi_Line> KPIlines = new List<TemplateKpi_Line>();

                        foreach (DataRow kpidr in table2.Rows)
                        {
                            if (Convert.ToInt32(kpidr["LINE_ID"].ToString()) == Convert.ToInt32(dr["LINE_ID"].ToString()))
                            {
                                TemplateKpi_Line kpi = new TemplateKpi_Line();
                                kpi.LINE_ID = Convert.ToInt32(kpidr["LINE_ID"].ToString());
                                kpi.PARAMETER_TYPE_ID = Convert.ToInt32(kpidr["PARAMETER_TYPE_ID"].ToString());
                                kpi.PARAMETER_TYPE = kpidr["PARAMETER_TYPE"].ToString();
                                kpi.PARAMETER_ID = Convert.ToInt32(kpidr["PARAMETER_ID"].ToString());
                                kpi.PARAMETER_NAME = kpidr["PARAMETER_NAME"].ToString();
                                kpi.DATAENTRY_TYPE_ID = Convert.ToInt32(kpidr["DATAENTRY_TYPE_ID"].ToString());
                                kpi.DATAENTRY_TYPE = kpidr["DATAENTRY_TYPE"].ToString();
                                kpi.UOM = kpidr["UOM"].ToString();
                                kpi.ALTERNATE_UOM = kpidr["ALTERNATE_UOM"].ToString();
                                kpi.OCCURRENCE = kpidr["OCCURRENCE"].ToString();
                                kpi.ITEM_ID = Convert.ToInt32(kpidr["ITEM_ID"].ToString());
                                kpi.ITEM_NAME = kpidr["ITEM_NAME"].ToString();
                                kpi.F_START_DATE = kpidr["FREQUENCY_START_DATE"].ToString();
                                kpi.F_END_DATE = kpidr["FREQUENCY_END_DATE"].ToString();
                                kpi.FREQUENCY_KPI_START_DATE = Convert.ToInt32(kpidr["Std_Start"].ToString());
                                kpi.FREQUENCY_KPI_END_DATE = Convert.ToInt32(kpidr["Std_End"].ToString());
                                kpi.KPI_TYPE = kpidr["KPI_TYPE"].ToString();
                                kpi.KPI_VALUE = Convert.ToDouble(kpidr["KPI_Value"].ToString());
                                KPIlines.Add(kpi);
                            }
                        }

                        tl.KpiLine = KPIlines;
                        lines.Add(tl);
                    }

                    obj.Status = "success";
                    obj.Message = "Template details data.";
                    obj.Data = new { header, lines };
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

        [Route("api/get_template_detailss")]
        [HttpGet]
        public IActionResult Template_Details(int Nature_Id, int Lob_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<Template_Header> header = new List<Template_Header>();
            List<Template_Line> lines = new List<Template_Line>();
            try
            {
                var ds = _configdb.Get_Template_Details(Nature_Id, Lob_Id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Template_Header th = new Template_Header();
                        th.TEMPLATE_ID = Convert.ToInt32(dr["TEMPLATE_ID"].ToString());
                        th.TEMPLATE_NAME = dr["TEMPLATE_NAME"].ToString();
                        th.NOB_ID = Convert.ToInt32(dr["NOB_ID"].ToString());
                        th.NATURE_OF_BUSINESS = dr["NATURE_OF_BUSINESS"].ToString();
                        th.LOB_ID = Convert.ToInt32(dr["LOB_ID"].ToString());
                        th.LINE_OF_BUSINESS = dr["LINE_OF_BUSINESS"].ToString();
                        th.BREED_ID = Convert.ToInt32(dr["BREED_ID"].ToString());
                        th.BREED_NAME = dr["BREED_NAME"].ToString();
                        th.STATUS = dr["STATUS"].ToString();
                        header.Add(th);
                    }
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        Template_Line tl = new Template_Line();
                        tl.LINE_ID = Convert.ToInt32(dr["LINE_ID"].ToString());
                        tl.PARAMETER_TYPE_ID = Convert.ToInt32(dr["PARAMETER_TYPE_ID"].ToString());
                        tl.PARAMETER_TYPE = dr["PARAMETER_TYPE"].ToString();
                        tl.PARAMETER_ID = Convert.ToInt32(dr["PARAMETER_ID"].ToString());
                        tl.PARAMETER_NAME = dr["PARAMETER_NAME"].ToString();
                        tl.DATAENTRY_TYPE_ID = Convert.ToInt32(dr["DATAENTRY_TYPE_ID"].ToString());
                        tl.DATAENTRY_TYPE = dr["DATAENTRY_TYPE"].ToString();
                        tl.UOM = dr["UOM"].ToString();
                        tl.ALTERNATE_UOM = dr["ALTERNATE_UOM"].ToString();
                        tl.OCCURRENCE = dr["OCCURRENCE"].ToString();
                        tl.ITEM_ID = Convert.ToInt32(dr["ITEM_ID"].ToString());
                        tl.ITEM_NAME = dr["ITEM_NAME"].ToString();
                        tl.F_START_DATE = dr["FREQUENCY_START_DATE"].ToString();
                        tl.F_END_DATE = dr["FREQUENCY_END_DATE"].ToString();
                        lines.Add(tl);
                    }

                    obj.Status = "success";
                    obj.Message = "Template details data.";
                    obj.Data = new { header, lines };
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

        [Route("api/get_template_summary")]
        [HttpGet]
        public IActionResult Template_Summary(string Nature_Id, int Company_Id,string Location_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<Template_Summary> summarry = new List<Template_Summary>();
            try
            {
                if (Nature_Id == null || Nature_Id == "null")
                {
                    obj.Status = "failure";
                    obj.Message = "Nature of business can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Company_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company details can not blank.";
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

                var dt = _configdb.Get_Template_Summary(Nature_Id, Company_Id,Location_Id);
                if (dt.Rows.Count > 0)
                {
                    string line_of_business = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        Template_Summary ts = new Template_Summary();
                        if (line_of_business != dr["LINE_OF_BUSNINESS"].ToString())
                        {
                            line_of_business = dr["LINE_OF_BUSNINESS"].ToString();

                            ts.nob_id = dr["NOB_ID"].ToString();
                            ts.nature_of_business = dr["NATURE_OF_BUSNINESS"].ToString();
                            ts.line_of_business = dr["LINE_OF_BUSNINESS"].ToString();
                            ts.lob_id = dr["LOB_ID"].ToString();

                            List<Template_Details> details = new List<Template_Details>();
                            foreach (DataRow dr1 in dt.Select("LINE_OF_BUSNINESS = '" + line_of_business + "'"))
                            {
                                Template_Details td = new Template_Details();

                                td.template_id = Convert.ToInt32(dr1["TEMPLATE_ID"].ToString());
                                td.template_name = dr1["TEMPLATE_NAME"].ToString();
                                td.breed_name = dr1["BREED_NAME"].ToString();
                                td.template_type = dr1["TEMPLATE_TYPE"].ToString();
                                td.status = dr1["STATUS"].ToString();

                                details.Add(td);
                            }
                            ts.templates = details;

                            summarry.Add(ts);
                        }
                    }

                    obj.Status = "success";
                    obj.Message = "Template summary data.";
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

        [Route("api/get_default_template_summary")]
        [HttpGet]
        public IActionResult Default_Template_Summary()
        {
            ResponseModel obj = new ResponseModel();
            List<Template_Summary> summarry = new List<Template_Summary>();
            try
            {
                var dt = _configdb.Default_template();
                if (dt.Rows.Count > 0)
                {
                    string line_of_business = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        Template_Summary ts = new Template_Summary();
                        if (line_of_business != dr["LINE_OF_BUSNINESS"].ToString())
                        {
                            line_of_business = dr["LINE_OF_BUSNINESS"].ToString();

                            ts.nob_id = dr["NOB_ID"].ToString();
                            ts.nature_of_business = dr["NATURE_OF_BUSNINESS"].ToString();
                            ts.line_of_business = dr["LINE_OF_BUSNINESS"].ToString();
                            ts.lob_id = dr["LOB_ID"].ToString();

                            List<Template_Details> details = new List<Template_Details>();
                            foreach (DataRow dr1 in dt.Select("LINE_OF_BUSNINESS = '" + line_of_business + "'"))
                            {
                                Template_Details td = new Template_Details();

                                td.template_id = Convert.ToInt32(dr1["TEMPLATE_ID"].ToString());
                                td.template_name = dr1["TEMPLATE_NAME"].ToString();
                                td.breed_name = dr1["BREED_NAME"].ToString();
                                td.template_type = dr1["TEMPLATE_TYPE"].ToString();
                                td.status = dr1["STATUS"].ToString();

                                details.Add(td);
                            }
                            ts.templates = details;

                            summarry.Add(ts);
                        }
                    }

                    obj.Status = "success";
                    obj.Message = "Template summary data.";
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

        [Route("api/get_location_template")]
        [HttpGet]
        public IActionResult Location_Template(int Lob_Id, int Company_Id,int Nob_id,int edit_batch_id)
        {
            ResponseModel obj = new ResponseModel();
            //List<SelectListItem> location = new List<SelectListItem>();
            List<ListItem_Class> location = new List<ListItem_Class>();
            List<Template_Details> template = new List<Template_Details>();
            try
            {
                var ds = _configdb.Get_Location_Template(Lob_Id, Company_Id,Nob_id, edit_batch_id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        location.Add(new ListItem_Class { area = @dr["AREA"].ToString(), Text = @dr["LOCATION_NAME"].ToString(), Value = @dr["LOCATION_ID"].ToString() });
                    }

                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        Template_Details td = new Template_Details();
                        td.template_id = Convert.ToInt32(dr["TEMPLATE_ID"].ToString());
                        td.template_name = dr["TEMPLATE_NAME"].ToString();
                        td.breed_id = Convert.ToInt32(dr["BREED_ID"].ToString());
                        td.breed_name = dr["BREED_NAME"].ToString();
                        td.status = dr["STATUS"].ToString();
                        template.Add(td);
                    }

                    obj.Status = "success";
                    obj.Message = "Location data.";
                    obj.Data = new { location, template };
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

        #region Batch
        [Route("api/insert_batch")]
        [HttpPost]
        public IActionResult Batch_Creation(BatchModel batchModel)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                var header = batchModel.batchHeader;
                var line = batchModel.batchlivestock;
                var batch_line = batchModel.batchitems;
                var batch_machine = batchModel.batchmachine;
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
                if (header.TEMPLATE_ID == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Template name can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (header.BREED_ID == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Breed name can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (header.BATCH_QUANTITY == 0 && header.LOB_ID !=16)
                {
                    obj.Status = "failure";
                    obj.Message = "Batch Quantity can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (header.LOCATION_ID == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Location Name can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (header.START_DATE == null)
                {
                    obj.Status = "failure";
                    obj.Message = "Start Date can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (header.HATCHING_DATE == null)
                {
                    obj.Status = "failure";
                    obj.Message = "Hatching Date can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (header.STATUS == null)
                {
                    obj.Status = "failure";
                    obj.Message = "Status can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (header.LOCATION == null)
                {
                    obj.Status = "failure";
                    obj.Message = "User location can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                DataTable batchTable = new DataTable("TBL_BATCH");
                batchTable.Columns.Add("BATCH_ID", typeof(int));
                batchTable.Columns.Add("COMPANY_ID", typeof(int));
                batchTable.Columns.Add("NOB_ID", typeof(int));
                batchTable.Columns.Add("LOB_ID", typeof(int));
                batchTable.Columns.Add("TEMPLATE_ID", typeof(int));
                batchTable.Columns.Add("BREED_ID", typeof(int));
                batchTable.Columns.Add("MALE_BIRD", typeof(int));
                batchTable.Columns.Add("FEMALE_BIRD", typeof(int));
                batchTable.Columns.Add("BATCH_QUANTITY", typeof(int));
                batchTable.Columns.Add("LOCATION_ID", typeof(int));
                batchTable.Columns.Add("START_DATE", typeof(DateTime));
                batchTable.Columns.Add("END_DATE", typeof(DateTime));
                batchTable.Columns.Add("HATCHING_DATE", typeof(DateTime));
                batchTable.Columns.Add("BATCH_NO", typeof(string));
                batchTable.Columns.Add("STATUS", typeof(string));
                batchTable.Columns.Add("REMARKS", typeof(string));
                batchTable.Columns.Add("CREATED_BY", typeof(int));
                batchTable.Columns.Add("LOCATION", typeof(string));
                batchTable.Columns.Add("CURRENT_LOCATION", typeof(string));
                batchTable.Columns.Add("ITEM_ID_M", typeof(int));
                batchTable.Columns.Add("ITEM_ID_F", typeof(int));

                DataRow dr = null;
                dr = batchTable.NewRow();
                dr["BATCH_ID"] = header.BATCH_ID;
                dr["COMPANY_ID"] = header.COMPANY_ID;
                dr["NOB_ID"] = header.NOB_ID;
                dr["LOB_ID"] = header.LOB_ID;
                dr["TEMPLATE_ID"] = header.TEMPLATE_ID;
                dr["BREED_ID"] = header.BREED_ID;
                dr["MALE_BIRD"] = header.MALE_BIRD;
                dr["FEMALE_BIRD"] = header.FEMALE_BIRD;
                dr["BATCH_QUANTITY"] = header.BATCH_QUANTITY;
                dr["LOCATION_ID"] = header.LOCATION_ID;
                dr["START_DATE"] = header.START_DATE;
                dr["END_DATE"] = (object)header.END_DATE ?? DBNull.Value;
                dr["HATCHING_DATE"] = header.HATCHING_DATE;
                dr["BATCH_NO"] = header.BATCH_NO;
                dr["STATUS"] = header.STATUS;
                dr["REMARKS"] = header.REMARKS;
                dr["CREATED_BY"] = header.CREATED_BY;
                dr["LOCATION"] = header.LOCATION;
                dr["CURRENT_LOCATION"] = header.CURRENT_LOCATION;
                dr["ITEM_ID_M"] = header.ITEM_ID_M;
                dr["ITEM_ID_F"] = header.ITEM_ID_F;
                batchTable.Rows.Add(dr);

                DataTable livestockTable = new DataTable("MST_BATCH_LIVESTOCK");
                livestockTable.Columns.Add("BATCH_LS_ID", typeof(int));
                livestockTable.Columns.Add("BATCH_ID", typeof(int));
                livestockTable.Columns.Add("ANIMAL_REG_ID", typeof(int));
                livestockTable.Columns.Add("ITEM_ID", typeof(int));
                DataRow drProcess = null;
                if (line != null)
                {
                    foreach (var pr in line)
                    {
                        drProcess = livestockTable.NewRow();
                        drProcess["BATCH_LS_ID"] = Convert.ToInt32(pr.batch_ls_id);
                        drProcess["BATCH_ID"] = Convert.ToInt32(pr.batch_id);
                        drProcess["ANIMAL_REG_ID"] = Convert.ToInt32(pr.animal_reg_id);
                        drProcess["ITEM_ID"] = Convert.ToInt32(pr.item_id);
                        livestockTable.Rows.Add(drProcess);
                    }
                }

                DataTable BatchitemTable = new DataTable("MST_BATCH_ITEM");
                BatchitemTable.Columns.Add("BATCH_ITEM_ID", typeof(int));
                BatchitemTable.Columns.Add("BATCH_ID", typeof(int));
                BatchitemTable.Columns.Add("ITEM_ID", typeof(int));
                BatchitemTable.Columns.Add("Quantity", typeof(decimal));
                BatchitemTable.Columns.Add("Rem_Qty", typeof(decimal));
                BatchitemTable.Columns.Add("Original_batch_No", typeof(string));
                BatchitemTable.Columns.Add("Remarks", typeof(string));
                BatchitemTable.Columns.Add("UOM", typeof(string));
                BatchitemTable.Columns.Add("AW_GMS", typeof(decimal));
                BatchitemTable.Columns.Add("BIOMASS", typeof(decimal));
                BatchitemTable.Columns.Add("DATE", typeof(string));
                BatchitemTable.Columns.Add("WEIGHT_SLAUGHTER", typeof(decimal));
                BatchitemTable.Columns.Add("Original_type", typeof(string));

                DataRow drProcess1 = null;
                if (batch_line != null)
                {
                    foreach (var pr in batch_line)
                    {
                        drProcess1 = BatchitemTable.NewRow();
                        drProcess1["BATCH_ITEM_ID"] = Convert.ToInt32(pr.Batch_item_id);
                        drProcess1["BATCH_ID"] = Convert.ToInt32(pr.Batch_id);
                        drProcess1["ITEM_ID"] = Convert.ToInt32(pr.Item_id);
                        drProcess1["Quantity"] = Convert.ToDecimal(pr.Quantity);
                        drProcess1["Rem_Qty"] = Convert.ToDecimal(pr.Rem_Qty);
                        drProcess1["Original_batch_no"] = Convert.ToString(pr.Original_batch_no);
                        drProcess1["Remarks"] = Convert.ToString(pr.Remarks);
                        drProcess1["UOM"] = Convert.ToString(pr.UOM);

                        drProcess1["AW_GMS"] = Convert.ToString(pr.Aw_gms);
                        drProcess1["BIOMASS"] = Convert.ToString(pr.Biomass);
                        drProcess1["DATE"] = Convert.ToString(pr.date);
                        drProcess1["WEIGHT_SLAUGHTER"] = Convert.ToString(pr.Slaughter_Weight);
                        drProcess1["Original_type"] = Convert.ToString(pr.Original_type);
                        BatchitemTable.Rows.Add(drProcess1);
                    }
                }

                DataTable MachineTable = new DataTable("MST_BATCH_MACHINE");
                MachineTable.Columns.Add("BATCH_MACHINE_ID", typeof(int));
                MachineTable.Columns.Add("Batch_id", typeof(int));
                MachineTable.Columns.Add("RESOURCE_CARD_ID", typeof(int));
                MachineTable.Columns.Add("CAPACITY", typeof(decimal));
                MachineTable.Columns.Add("QUANTITY", typeof(decimal));
                MachineTable.Columns.Add("ALLOCATED", typeof(decimal));
                MachineTable.Columns.Add("REMAINING", typeof(decimal));
                DataRow drProcessMachine = null;
                if (batch_machine != null)
                {
                    foreach (var pr in batch_machine)
                    {
                        drProcessMachine = MachineTable.NewRow();
                        drProcessMachine["BATCH_MACHINE_ID"] = Convert.ToInt32(pr.batch_machine_id);
                        drProcessMachine["Batch_id"] = Convert.ToInt32(pr.batch_id);
                        drProcessMachine["RESOURCE_CARD_ID"] = Convert.ToInt32(pr.resource_card_id);
                        drProcessMachine["CAPACITY"] = Convert.ToInt32(pr.capacity);
                        drProcessMachine["QUANTITY"] = Convert.ToInt32(pr.quantity);
                        drProcessMachine["ALLOCATED"] = Convert.ToInt32(pr.allocated);
                        drProcessMachine["REMAINING"] = Convert.ToInt32(pr.remaining);
                        MachineTable.Rows.Add(drProcessMachine);
                    }
                }


                string[] res = (_configdb.Insert_Batch(batchTable, livestockTable,BatchitemTable, header.BATCH_CAPACITY, MachineTable)).Split(',');

                if (res[0] == "success")
                {
                    obj.Status = "success";
                    obj.Message = "successfully created with batch No. "+res[2].ToString();
                    obj.Data = new { batch_id = res[1] };
                }

          
                else if (res[0] == "update")
                {
                    obj.Status = "success";
                    obj.Message = "successfully updated batch.";
                    obj.Data = new { batch_id = res[1] };
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

        [Route("api/get_Batch_summary")]
        [HttpGet]
        public IActionResult Batch_Summary(string Nature_Id, int Company_Id, string Location_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<Batch_Summary> summarry = new List<Batch_Summary>();
            try
            {
                if (Nature_Id == null || Nature_Id == "null")
                {
                    obj.Status = "failure";
                    obj.Message = "Nature of business can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Company_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Company details can not blank.";
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

                var dt = _configdb.Get_Batch_Summary(Nature_Id, Company_Id,Location_Id);
                if (dt.Rows.Count > 0)
                {
                    string line_of_business = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        Batch_Summary bs = new Batch_Summary();
                        if (line_of_business != dr["LINE_OF_BUSNINESS"].ToString())
                        {
                            line_of_business = dr["LINE_OF_BUSNINESS"].ToString();

                            bs.nob_id = dr["NOB_ID"].ToString();
                            bs.nature_of_business = dr["NATURE_OF_BUSNINESS"].ToString();
                            bs.line_of_business = dr["LINE_OF_BUSNINESS"].ToString();
                            bs.lob_id = dr["LOB_ID"].ToString();
                            bs.location_name = dr["LOCATION_NAME"].ToString();
                            
                            List<Batch_Details> details = new List<Batch_Details>();
                            foreach (DataRow dr1 in dt.Select("LINE_OF_BUSNINESS = '" + line_of_business + "'"))
                            {
                                Batch_Details bd = new Batch_Details();

                                bd.batch_id = Convert.ToInt32(dr1["BATCH_ID"].ToString());
                                bd.batch_no = dr1["BATCH_NO"].ToString();
                                bd.start_date = dr1["START_DATE"].ToString();
                                bd.opening_stocks = Convert.ToInt32(dr1["OPENING_QTY"].ToString());
                                bd.ITEM_ID_M = Convert.ToInt32(dr1["ITEM_ID_M"].ToString());
                                bd.ITEM_ID_F = Convert.ToInt32(dr1["ITEM_ID_F"].ToString());
                                bd.status = dr1["STATUS"].ToString();
                                bd.remark = dr1["REMARK"].ToString();
                                bd.ITEM_NAME_M = dr1["ITEM_NAME_M"].ToString();
                                bd.ITEM_NAME_F = dr1["ITEM_NAME_F"].ToString();
                                bd.Location_name = dr1["LOCATION_NAME"].ToString();
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

        //[Route("api/get_batch_summary_web")]
        //[HttpGet]
        //public IActionResult Batch_Summary_Web(int Company_Id)
        //{
        //    ResponseModel obj = new ResponseModel();
        //    List<Batch_Details_Web> summarry = new List<Batch_Details_Web>();
        //    try
        //    {
        //        var dt = _configdb.Get_Batch_Summary_Web(Company_Id);
        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                Batch_Details_Web bd = new Batch_Details_Web();

        //                bd.batch_id = Convert.ToInt32(dr["BATCH_ID"].ToString());
        //                bd.batch_no = dr["BATCH_NO"].ToString();
        //                bd.start_date = dr["START_DATE"].ToString();
        //                bd.opening_stocks = Convert.ToInt32(dr["OPENING_QTY"].ToString());
        //                bd.nature_of_business = dr["NATURE_OF_BUSINESS"].ToString();
        //                bd.line_of_business = dr["LINE_OF_BUSINESS"].ToString();
        //                bd.status = dr["STATUS"].ToString();
        //                summarry.Add(bd);
        //            }

        //            obj.Status = "success";
        //            obj.Message = "Batch summary data.";
        //            obj.Data = new { summarry };
        //        }
        //        else
        //        {
        //            obj.Status = "failure";
        //            obj.Message = "Data is not available.";
        //            obj.Data = new { };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        obj.Status = "error";
        //        obj.Message = ex.Message;
        //        obj.Data = new { };
        //    }
        //    return Ok(obj);
        //}

        [Route("api/get_batch_details")]
        [HttpGet]
        public IActionResult Batch_Details(int batch_id)
        {
            ResponseModel obj = new ResponseModel();
            List<BatchHeader> details = new List<BatchHeader>();
            List<File_Details> uploaded_files = new List<File_Details>();
            try
            {
                var ds = _configdb.Get_Batch_Details(batch_id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        BatchHeader bm = new BatchHeader();
                        bm.BATCH_ID = Convert.ToInt32(dr["BATCH_ID"].ToString());
                        bm.BATCH_NO = dr["BATCH_NO"].ToString();
                        bm.NOB_ID = Convert.ToInt32(dr["NOB_ID"].ToString());
                        bm.NATURE_OF_BUSINESS = dr["NATURE_OF_BUSINESS"].ToString();
                        bm.LOB_ID = Convert.ToInt32(dr["LOB_ID"].ToString());
                        bm.LINE_OF_BUSINESS = dr["LINE_OF_BUSINESS"].ToString();
                        bm.TEMPLATE_ID = Convert.ToInt32(dr["TEMPLATE_ID"].ToString());
                        bm.TEMPLATE_NAME = dr["TEMPLATE_NAME"].ToString();
                        bm.BREED_ID = Convert.ToInt32(dr["BREED_ID"].ToString());
                        bm.BREED_NAME = dr["BREED_NAME"].ToString();
                        bm.MALE_BIRD = Convert.ToInt32(dr["MALE_BIRD"].ToString());
                        bm.FEMALE_BIRD = Convert.ToInt32(dr["FEMALE_BIRD"].ToString());
                        bm.BATCH_QUANTITY = Convert.ToInt32(dr["OPENING_QTY"].ToString());
                        bm.S_DATE = dr["START_DATE"].ToString();
                        bm.E_DATE = dr["END_DATE"].ToString();
                        bm.H_DATE = dr["HATCHING_DATE"].ToString();
                        bm.LOCATION_ID = Convert.ToInt32(dr["LOCATION_ID"].ToString());
                        bm.LOCATION_NAME = dr["LOCATION_NAME"].ToString();
                        bm.STATUS = dr["STATUS"].ToString();
                        bm.REMARKS = dr["REMARK"].ToString();
                        bm.ITEM_ID_M = Convert.ToInt32(dr["ITEM_ID_M"].ToString());
                        bm.ITEM_ID_F = Convert.ToInt32(dr["ITEM_ID_F"].ToString());
                        bm.ITEM_NAME_M = dr["ITEM_NAME_M"].ToString();
                        bm.ITEM_NAME_F = dr["ITEM_NAME_F"].ToString();
                        bm.Remaining_Stock_F = Convert.ToDecimal(dr["RemainingStock_F"].ToString());
                        bm.Remaining_Stock_M = Convert.ToDecimal(dr["RemainingStock_M"].ToString());
                        bm.Flag_F = Convert.ToInt32(dr["flag_F"].ToString());
                        bm.Flag_M = Convert.ToInt32(dr["flag_M"].ToString());
                        bm.BATCH_CAPACITY = Convert.ToDecimal(dr["BATCH_CAPACITY"].ToString());
                        details.Add(bm);
                    }
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        File_Details fd = new File_Details();
                        fd.file_id = Convert.ToInt32(dr["FILE_ID"].ToString());
                        fd.file_name = dr["FILE_NAME"].ToString();
                        uploaded_files.Add(fd);
                    }
                    List<Animal_Master> livestock = new List<Animal_Master>();
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        Animal_Master bd = new Animal_Master();
                        bd.batch_id = Convert.ToInt32(dr["Batch_ID"].ToString());
                        bd.batch_ls_id = Convert.ToInt32(dr["Batch_ls_id"].ToString());
                        bd.ANIMAL_REG_ID = Convert.ToInt32(dr["ANIMAL_REG_ID"].ToString());
                        bd.Birth_Date = dr["Birth_Date"].ToString();
                        bd.Dad_Number = dr["Dad_Number"].ToString();
                        bd.Mom_Number = dr["Mom_Number"].ToString();
                        bd.Livestock_Number = Convert.ToString(dr["Livestock_Number"].ToString());
                        bd.Death_Date = dr["Death_Date"].ToString();
                        bd.Breed_Name = dr["Breed_Name"].ToString();
                        bd.Mom_Breed_Name = dr["Mom_Breed_Name"].ToString();
                        bd.Sub_Breed_Name = dr["Sub_Breed_Name"].ToString();
                        bd.Location_Name = dr["Location_Name"].ToString();
                        bd.Sub_Location_Name = dr["Sub_Location_Name"].ToString();
                        bd.Gender = dr["Gender"].ToString();
                        bd.Serial_No = dr["Serial_No"].ToString();
                        bd.Item_id = Convert.ToInt32(dr["Item_id"].ToString());
                        bd.Item_Name = Convert.ToString(dr["Item_Name"].ToString());
                        livestock.Add(bd);
                    }

                    List<BatchItems> batchitems = new List<BatchItems>();
                    foreach (DataRow dr in ds.Tables[3].Rows)
                    {
                        BatchItems bd = new BatchItems();
                        bd.Batch_item_id = Convert.ToInt32(dr["Batch_item_ID"].ToString());
                        bd.Batch_id = Convert.ToInt32(dr["Batch_id"].ToString());
                        bd.Item_id = Convert.ToInt32(dr["Item_id"].ToString());
                        bd.Item_name = Convert.ToString(dr["Item_Name"].ToString());
                        bd.Original_batch_no = dr["Original_batch_no"].ToString();
                        bd.Remarks = dr["Remarks"].ToString();
                        bd.Quantity = Convert.ToDecimal(dr["Quantity"].ToString());
                        bd.Rem_Qty = Convert.ToDecimal(dr["Rem_Qty"].ToString());
                        bd.UOM = Convert.ToString(dr["UOM"].ToString());
                        bd.INVENTORY_TYPE = Convert.ToString(dr["INVENTORY_TYPE"].ToString());
                        bd.Flag = Convert.ToString(dr["isAllow"].ToString());
                        bd.Aw_gms = Convert.ToDecimal(dr["AW_GMS"].ToString());
                        bd.Biomass = Convert.ToDecimal(dr["BIOMASS"].ToString());
                        bd.date = dr["DATE"].ToString();
                        bd.Slaughter_Weight = Convert.ToDecimal(dr["WEIGHT_SLAUGHTER"].ToString());
                        bd.Original_type = (dr["Original_type"].ToString());
                        batchitems.Add(bd);
                    }

                    List<BatchMachine> batchmachine = new List<BatchMachine>();
                    foreach (DataRow dr in ds.Tables[4].Rows)
                    {
                        BatchMachine bd = new BatchMachine();
                        bd.batch_machine_id = Convert.ToInt32(dr["BATCH_MACHINE_ID"].ToString());
                        bd.batch_id = Convert.ToInt32(dr["Batch_id"].ToString());
                        bd.resource_card_id = Convert.ToInt32(dr["RESOURCE_CARD_ID"].ToString());
                        bd.capacity = Convert.ToDecimal(dr["CAPACITY"].ToString());
                        bd.quantity = Convert.ToDecimal(dr["QUANTITY"].ToString());
                        bd.allocated = Convert.ToDecimal(dr["ALLOCATED"].ToString());
                        bd.remaining = Convert.ToDecimal(dr["REMAINING"].ToString());
                        bd.resource_card_name =dr["RESOURCE_NAME"].ToString();

                        
                        batchmachine.Add(bd);
                    }

                    obj.Status = "success";
                    obj.Message = "Batch details data.";
                    obj.Data = new { details, uploaded_files, livestock, batchitems, batchmachine };
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

        [Route("api/delete_batch_file")]
        [HttpPost]
        public IActionResult Batch_File_Delete(int File_Id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (File_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "File id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                string res = _configdb.Delete_Batch_File(File_Id, "batch");

                if (res == "success")
                {
                    obj.Status = "success";
                    obj.Message = "successfully deleted file.";
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = res;
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
        [Route("api/get_UOM_BY_TYPE")]
        [HttpGet]
        public IActionResult Get_UOM_LIST(int Item_id)
        {
            ResponseModel obj = new ResponseModel();
            List<SelectListItem> BATCHES = new List<SelectListItem>();
            try
            {
                var dt = _configdb.Get_UOM_LIST(Item_id );
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        BATCHES.Add(new SelectListItem { Text = @dr["UOM"].ToString(), Value = @dr["UOM_ID"].ToString() });
                    }

                    obj.Status = "success";
                    obj.Message = "UOM data.";
                    obj.Data = BATCHES;
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

        [Route("api/get_batches")]
        [HttpGet]
        public IActionResult Get_Batches(int Lob_Id, int Company_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<SelectListItem> BATCHES = new List<SelectListItem>();
            try
            {
                var dt = _configdb.Get_Batches(Lob_Id, Company_Id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        BATCHES.Add(new SelectListItem { Text = @dr["BATCH_NO"].ToString(), Value = @dr["BATCH_ID"].ToString() });
                    }

                    obj.Status = "success";
                    obj.Message = "Batch data.";
                    obj.Data = BATCHES;
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
        [Route("api/get_batchno")]
        [HttpGet]
        public IActionResult Get_Batchno(int Company_Id,string Batchno)
        {
            ResponseModel obj = new ResponseModel();
            List<SelectListItem> BATCHES = new List<SelectListItem>();
            try
            {
                var dt = _configdb.Get_Batcheno(Company_Id,Batchno);
                if (dt.Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(dt);
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    BATCHES.Add(new SelectListItem { Text = @dr["BATCH_NO"].ToString(), Value = @dr["BATCH_NO"].ToString() });
                    //}

                    obj.Status = "success";
                    obj.Message = "Batch data.";
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

        [Route("api/get_original_batchno")]
        [HttpGet]
        public IActionResult Get_Original_Batchno(int Company_Id, int batch_id, int item_id ,int location_id, int lob_id,string isbatch_tranfer)
        {
            ResponseModel obj = new ResponseModel();
            List<SelectListItem> BATCHES = new List<SelectListItem>();
            try
            {
                var dt = _configdb.Get_OriginalBatcheno(Company_Id, batch_id, item_id ,location_id, lob_id, isbatch_tranfer);
                if (dt.Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(dt);
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    BATCHES.Add(new SelectListItem { Text = @dr["BATCH_NO"].ToString(), Value = @dr["BATCH_NO"].ToString() });
                    //}

                    obj.Status = "success";
                    obj.Message = "Batch data.";
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
        [Route("api/get_batches_by_location")]
        [HttpGet]
        public IActionResult Get_Batches_Location(int Lob_Id, int Company_Id,int loc_id)
        {
            ResponseModel obj = new ResponseModel();
            List<SelectListItem> BATCHES = new List<SelectListItem>();
            try
            {
                var dt = _configdb.Get_Batches_location(Lob_Id, Company_Id,loc_id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        BATCHES.Add(new SelectListItem { Text = @dr["BATCH_NO"].ToString(), Value = @dr["BATCH_ID"].ToString() });
                    }

                    obj.Status = "success";
                    obj.Message = "Batch data.";
                    obj.Data = BATCHES;
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

        [Route("api/get_all_batches")]
        [HttpGet]
        public IActionResult Get_All_Batches(int Company_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<SelectListItem> BATCHES = new List<SelectListItem>();
            try
            {
                var dt = _configdb.Get_All_Batches(Company_Id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        BATCHES.Add(new SelectListItem { Text = @dr["BATCH_NO"].ToString(), Value = @dr["BATCH_ID"].ToString() });
                    }

                    obj.Status = "success";
                    obj.Message = "Batch data.";
                    obj.Data = BATCHES;
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

        [Route("api/get_location_batches")]
        [HttpGet]
        public IActionResult Get_Location_Batches(int Loc_Id, int Company_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<SelectListItem> BATCHES = new List<SelectListItem>();
            try
            {
                var dt = _configdb.Get_Loc_Batches(Loc_Id, Company_Id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        BATCHES.Add(new SelectListItem { Text = @dr["BATCH_NO"].ToString(), Value = @dr["BATCH_ID"].ToString() });
                    }

                    obj.Status = "success";
                    obj.Message = "Batch data.";
                    obj.Data = BATCHES;
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

        [Route("api/get_all_breeds")]
        [HttpGet]
        public IActionResult Get_All_breedss(int Nob_id,int Company_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<SelectListItem> BATCHES = new List<SelectListItem>();
            try
            {
                var dt = _configdb.Get_All_Breeds(Nob_id,Company_Id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        BATCHES.Add(new SelectListItem { Text = @dr["BREED_NAME"].ToString(), Value = @dr["BREED_ID"].ToString() });
                    }

                    obj.Status = "success";
                    obj.Message = "Breed data.";
                    obj.Data = BATCHES;
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

        #region Common Api's
        [Route("api/line_of_business")]
        [HttpGet]
        public IActionResult Line_of_business(string Id,string Company_Id)
        {
            ResponseModel obj = new ResponseModel();          
            List<LOB_Model> LINE_OF_BUSINESS = new List<LOB_Model>();            
            try
            {
                if (Id == null || Id == "null")
                {
                    obj.Status = "failure";
                    obj.Message = "Nature Id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Company_Id == null || Company_Id == "null")
                {
                    Company_Id = null;
                }

                var dt = _configdb.Line_of_business(Id,Company_Id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        LINE_OF_BUSINESS.Add(new LOB_Model { text = @dr["LINE_OF_BUSINESS"].ToString(), value = @dr["LINE_ID"].ToString(), selected = (dr["L_SELECTED"].ToString() == "0" ? false : true),nature_id = @dr["NATURE_ID"].ToString() });
                    }

                    obj.Status = "success";
                    obj.Message = "Line of business data";
                    obj.Data =  LINE_OF_BUSINESS;
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = "No data available";
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
        
        [Route("api/get_nob")]
        [HttpGet]
        public IActionResult Nature_of_business(int Company_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<SelectListItem> NATURE_OF_BUSNINESS = new List<SelectListItem>();
            try
            {
                var dt = _configdb.Get_Nature_of_business(Company_Id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        NATURE_OF_BUSNINESS.Add(new SelectListItem { Text = @dr["NATURE_OF_BUSNINESS"].ToString(), Value = @dr["NATURE_ID"].ToString() });
                    }

                    obj.Status = "success";
                    obj.Message = "Nature of business data.";
                    obj.Data = new { NATURE_OF_BUSNINESS };
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

        [Route("api/get_lob")]
        [HttpGet]
        public IActionResult Line_of_business(int Nature_Id,int Company_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<SelectListItem> LINE_OF_BUSNINESS = new List<SelectListItem>();
            try
            {
                var dt = _configdb.Line_of_business(Nature_Id, Company_Id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        LINE_OF_BUSNINESS.Add(new SelectListItem { Text = @dr["LINE_OF_BUSNINESS"].ToString(), Value = @dr["LINE_ID"].ToString() });
                    }

                    obj.Status = "success";
                    obj.Message = "Line of business data.";
                    obj.Data = new { LINE_OF_BUSNINESS };
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

        [Route("api/get_breed")]
        [HttpGet]
        public IActionResult Breed(int Nob_Id, int Lob_Id, int Company_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<Breed> Breed = new List<Breed>();
            List<DataEntryType> DataEntry_Type = new List<DataEntryType>();
            List<Item> Items = new List<Item>();
            try
            {
                var ds = _configdb.Breed_Dataentry_Item(Nob_Id, Lob_Id, Company_Id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Breed bd = new Breed();
                        bd.breed_id = Convert.ToInt32(dr["BREED_ID"].ToString());
                        bd.breed_no = dr["BREED_NO"].ToString();
                        bd.breed_name = dr["BREED_NAME"].ToString();
                        Breed.Add(bd);
                    }
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        DataEntryType de = new DataEntryType();
                        de.dataentry_id = Convert.ToInt32(dr["DATAENTRY_TYPE_ID"].ToString());
                        de.dataentry_type = dr["DATAENTRY_TYPE"].ToString();
                        de.alternate_uom = dr["ALTERNATE_UOM"].ToString();
                        de.uom = dr["UOM"].ToString();
                        DataEntry_Type.Add(de);
                    }
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        Item it = new Item();
                        it.item_id = Convert.ToInt32(dr["ITEM_ID"].ToString());
                        it.item_no = dr["ITEM_NO"].ToString();
                        it.item_name = dr["ITEM_NAME"].ToString();
                        it.uom = dr["UOM"].ToString();
                        it.unit_cost = Convert.ToDecimal(dr["UNIT_COST"].ToString());
                        it.class_name = dr["class_name"].ToString();
                        Items.Add(it);
                    }
                    obj.Status = "success";
                    obj.Message = "Breed,Data Entry Type,Item data.";
                    obj.Data = new { Breed, DataEntry_Type, Items };
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
        [Route("api/get_item_list")]
        [HttpGet]
        public IActionResult Get_Item_List(int Nob_Id, int isdefault, int Company_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<Breed> Breed = new List<Breed>();
            List<DataEntryType> DataEntry_Type = new List<DataEntryType>();
            List<Item> Items = new List<Item>();
            try
            {
                var ds = _configdb.Get_Item_List(Nob_Id, isdefault, Company_Id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Item it = new Item();
                        it.item_id = Convert.ToInt32(dr["ITEM_ID"].ToString());
                        it.item_no = dr["ITEM_NO"].ToString();
                        it.item_name = dr["ITEM_NAME"].ToString();
                        it.uom = dr["UOM"].ToString();
                        it.unit_cost = Convert.ToDecimal(dr["UNIT_COST"].ToString());
                        Items.Add(it);
                    }
                    obj.Status = "success";
                    obj.Message = "item data.";
                    obj.Data = new { Breed, DataEntry_Type, Items };
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
        [Route("api/get_item_details")]
        [HttpGet]
        public IActionResult Get_Item_Details(int Company_Id,int item_id,int loc_id)
        {
            ResponseModel obj = new ResponseModel();
            List<Breed> Breed = new List<Breed>();
            List<DataEntryType> DataEntry_Type = new List<DataEntryType>();
            List<Item> Items = new List<Item>();
            try
            {
                var ds = _configdb.Get_Item_Details(Company_Id,item_id,loc_id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Item it = new Item();
                        it.item_id = Convert.ToInt32(dr["ITEM_ID"].ToString());
                        it.item_no = dr["ITEM_NO"].ToString();
                        it.item_name = dr["ITEM_NAME"].ToString();
                        it.uom = dr["UOM"].ToString();
                        it.unit_cost = Convert.ToDecimal(dr["UNIT_COST"].ToString());
                        it.remainingstock = Convert.ToDecimal(dr["RemainingStock"].ToString());
                        it.flag = Convert.ToInt32(dr["flag"].ToString());
                        Items.Add(it);
                    }
                    obj.Status = "success";
                    obj.Message = "Item,Data Entry Type,Item data.";
                    obj.Data = new { Breed, DataEntry_Type, Items };
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

        [Route("api/get_Item_all")]
        [HttpGet]
        public IActionResult Get_datatype_item_all(int Company_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<DataEntryType> DataEntry_Type = new List<DataEntryType>();
            List<Item> Items = new List<Item>();
            try
            {
                var ds = _configdb.Dataentry_Item(Company_Id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DataEntryType de = new DataEntryType();
                        de.lob_id = Convert.ToInt32(dr["lob_id"].ToString());
                        de.nob_id = Convert.ToInt32(dr["nature_id"].ToString());
                        de.dataentry_id = Convert.ToInt32(dr["DATAENTRY_TYPE_ID"].ToString());
                        de.dataentry_type = dr["DATAENTRY_TYPE"].ToString();
                        de.alternate_uom = dr["ALTERNATE_UOM"].ToString();
                        de.uom = dr["UOM"].ToString();
                        DataEntry_Type.Add(de);
                    }
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        Item it = new Item();
                        it.nature_id = Convert.ToInt32(dr["NATURE_ID"].ToString());
                        it.item_id = Convert.ToInt32(dr["ITEM_ID"].ToString());
                        it.item_no = dr["ITEM_NO"].ToString();
                        it.item_name = dr["ITEM_NAME"].ToString();
                        it.uom = dr["UOM"].ToString();
                        it.unit_cost = Convert.ToDecimal(dr["UNIT_COST"].ToString());
                        Items.Add(it);
                    }
                    obj.Status = "success";
                    obj.Message = "Data Entry Type,Item data.";
                    obj.Data = new { DataEntry_Type, Items };
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
        [Route("api/get_LOB_item_details")]
        [HttpGet]
        public IActionResult Get_LOB_Item_Details(int Company_Id, int loc_id,int nob_id)
        {
            ResponseModel obj = new ResponseModel();
            List<Breed> Breed = new List<Breed>();
            List<DataEntryType> DataEntry_Type = new List<DataEntryType>();
            List<Item> Items = new List<Item>();
            try
            {
                var ds = _configdb.Get_LOB_Item_Details(Company_Id, loc_id,nob_id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Item it = new Item();
                        it.item_id = Convert.ToInt32(dr["ITEM_ID"].ToString());
                        it.item_no = dr["ITEM_NO"].ToString();
                        it.item_name = dr["ITEM_NAME"].ToString();
                        it.uom = dr["UOM"].ToString();
                        it.unit_cost = Convert.ToDecimal(dr["UNIT_COST"].ToString());
                        it.remainingstock = Convert.ToDecimal(dr["RemainingStock"].ToString());
                        it.flag = Convert.ToInt32(dr["flag"].ToString());
                        it.INVENTORY_TYPE = dr["INVENTORY_TYPE"].ToString();
                        Items.Add(it);
                    }
                    obj.Status = "success";
                    obj.Message = "Item data.";
                    obj.Data = new {Items };
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
        [Route("api/parameter_type")]
        [HttpGet]
        public IActionResult Parameter_Type()
        {
            ResponseModel obj = new ResponseModel();
            List<SelectListItem> Parameter_Type = new List<SelectListItem>();
            try
            {
                var dt = _configdb.Parameter_Type();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Parameter_Type.Add(new SelectListItem { Text = @dr["PARAMETER_TYPE"].ToString(), Value = @dr["PARAMETER_TYPE_ID"].ToString() });
                    }

                    obj.Status = "success";
                    obj.Message = "Parameter Type data.";
                    obj.Data = new { Parameter_Type };
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

        [Route("api/parameter")]
        [HttpGet]
        public IActionResult Parameter(int Parameter_type_id,int Company_Id,int Lob_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<Parameter_Model> Parameter = new List<Parameter_Model>();
            List<DataEntryType> DataEntry_Type = new List<DataEntryType>();
            List<Item> Items = new List<Item>();
            try
            {
                var dt = _configdb.Parameter(Parameter_type_id, Company_Id,Lob_Id);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        Parameter.Add(new Parameter_Model { text = @dr["PARAMETER_NAME"].ToString(), value = @dr["PARAMETER_ID"].ToString(), selected = false,formula_flag = @dr["FORMULA_FLAG"].ToString(), livestock_flag = @dr["Livestock_flag"].ToString() });
                    }
                    foreach (DataRow dr in dt.Tables[1].Rows)
                    {
                        DataEntryType de = new DataEntryType();
                        de.dataentry_id = Convert.ToInt32(dr["DATAENTRY_TYPE_ID"].ToString());
                        de.dataentry_type = dr["DATAENTRY_TYPE"].ToString();
                        de.alternate_uom = dr["ALTERNATE_UOM"].ToString();
                        de.uom = dr["UOM"].ToString();
                        DataEntry_Type.Add(de);
                    }
                    foreach (DataRow dr in dt.Tables[2].Rows)
                    {
                        Item it = new Item();
                        it.item_id = Convert.ToInt32(dr["ITEM_ID"].ToString());
                        it.item_no = dr["ITEM_NO"].ToString();
                        it.item_name = dr["ITEM_NAME"].ToString();
                        it.uom = dr["UOM"].ToString();
                        it.unit_cost = Convert.ToDecimal(dr["UNIT_COST"].ToString());
                        it.class_name = dr["class_name"].ToString();
                        Items.Add(it);
                    }

                    obj.Status = "success";
                    obj.Message = "Parameter data.";
                    obj.Data = new { Parameter, DataEntry_Type, Items };
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

        [Route("api/parameter_all")]
        [HttpGet]
        public IActionResult Parameter_ALL(int Company_id)
        {
            ResponseModel obj = new ResponseModel();
            List<Parameter_Model> Parameter = new List<Parameter_Model>();
            try
            {
                var dt = _configdb.Parameter_ALL(Company_id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Parameter.Add(new Parameter_Model { text = @dr["PARAMETER_NAME"].ToString(), value = @dr["PARAMETER_ID"].ToString(), selected = Convert.ToBoolean(@dr["flag"]), formula_flag = @dr["FORMULA_FLAG"].ToString(),parameter_id=dr["PARAMETER_TYPE_ID"].ToString(),lob_id=dr["lob_id"].ToString() });
                    }

                    obj.Status = "success";
                    obj.Message = "Parameter data.";
                    obj.Data = new { Parameter };
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

        #region Notification
        [Route("api/Insert_device_token")]
        [HttpPost]
        public IActionResult Insert_Device_token(DeviceTokenModel dtm)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (dtm.USER_ID == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "User details can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (dtm.DEVICE_TOKEN == null || dtm.DEVICE_TOKEN == "null")
                {
                    obj.Status = "failure";
                    obj.Message = "Device details can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (dtm.DEVICE_TYPE == null || dtm.DEVICE_TYPE == "null")
                {
                    obj.Status = "failure";
                    obj.Message = "Device details can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                string res = _configdb.Insert_Device_Token(dtm.DEVICE_TOKEN,dtm.DEVICE_TYPE,dtm.USER_ID);
                if(res == "success")
                {
                    obj.Status = "success";
                    obj.Message = "Device token successfully received.";
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = res;
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

        [Route("api/Get_Notifications")]
        [HttpGet]
        public IActionResult Get_Notifications(int User_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<Notifications> list = new List<Notifications>();
            if (User_Id == 0)
            {
                obj.Status = "failure";
                obj.Message = "User details can't blank.";
                obj.Data = new { };
                return Ok(obj);
            }
            try
            {
               var dt = _configdb.Get_Notifications(User_Id);
                if (dt.Rows.Count > 0)
                {
                    foreach(DataRow dr in dt.Rows)
                    {
                        Notifications ns = new Notifications();
                        ns.notification_id = Convert.ToInt32(dr["NOTIFICATION_ID"].ToString());
                        ns.title = dr["TITLE"].ToString();
                        ns.body = dr["BODY"].ToString();
                        ns.screenName = dr["SCREEN_NAME"].ToString();
                        ns.web_screenName = dr["WEB_SCREEN_NAME"].ToString();
                        ns.batch_id = Convert.ToInt32(dr["BATCH_ID"].ToString());
                        ns.opening_qyt = Convert.ToInt32(dr["OPENING_QTY"].ToString());
                        ns.remaing_qyt = Convert.ToInt32(dr["REMAINING_QTY"].ToString());
                        ns.batch_stauts = dr["BATCH_STATUS"].ToString();
                        ns.created_date = dr["CREATED_DATE"].ToString();
                        ns.is_read = Convert.ToInt32(dr["IS_READ"].ToString());
                        ns.current_date_status = dr["CURRENT_DATE_STATUS"].ToString();
                        list.Add(ns);
                    }
                    obj.Status = "success";
                    obj.Message = "Notification Data";
                    obj.Data = list;
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

        [Route("api/Notification_Status")]
        [HttpPost]
        public IActionResult Notification_Status(Notification_Status ns)
        {
            ResponseModel obj = new ResponseModel();
            if (ns.USER_ID == 0)
            {
                obj.Status = "failure";
                obj.Message = "User details can't blank.";
                obj.Data = new { };
                return Ok(obj);
            }
            try
            {
                var count = _configdb.Update_Notification_Status(ns.ID, ns.IS_READ, ns.CLEAR, ns.USER_ID);

                obj.Status = "success";
                obj.Message = "Successfully get record .";
                obj.Data = count;
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }

        [Route("api/turn_On_Off_notifications")]
        [HttpPost]
        public IActionResult Turn_On_Off_Notifications(Notification_Status ns)
        {
            ResponseModel obj = new ResponseModel();
            if (ns.USER_ID == 0)
            {
                obj.Status = "failure";
                obj.Message = "User details can't blank.";
                obj.Data = new { };
                return Ok(obj);
            }
            try
            {
                var res = _configdb.Turn_On_Off_Notifications(ns.USER_ID,ns.STATUS);
                
                obj.Status = "success";
                obj.Message = res;
                obj.Data = new { };
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }

        [Route("api/Get_CurrentNotifications")]
        [HttpGet]
        public IActionResult Get_Current_Notifications(int User_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<Notifications> list = new List<Notifications>();
            if (User_Id == 0)
            {
                obj.Status = "failure";
                obj.Message = "User details can't blank.";
                obj.Data = new { };
                return Ok(obj);
            }
            try
            {
                var dt = _configdb.Get_CurrentNotifications(User_Id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Notifications ns = new Notifications();
                        ns.notification_id = Convert.ToInt32(dr["NOTIFICATION_ID"].ToString());
                        ns.title = dr["TITLE"].ToString();
                        ns.body = dr["BODY"].ToString();
                        ns.screenName = dr["SCREEN_NAME"].ToString();
                        ns.web_screenName = dr["WEB_SCREEN_NAME"].ToString();
                        ns.batch_id = Convert.ToInt32(dr["BATCH_ID"].ToString());
                        ns.opening_qyt = Convert.ToInt32(dr["OPENING_QTY"].ToString());
                        ns.remaing_qyt = Convert.ToInt32(dr["REMAINING_QTY"].ToString());
                        ns.batch_stauts = dr["BATCH_STATUS"].ToString();
                        ns.created_date = dr["CREATED_DATE"].ToString();
                        ns.is_read = Convert.ToInt32(dr["IS_READ"].ToString());
                        ns.current_date_status = dr["CURRENT_DATE_STATUS"].ToString();
                        list.Add(ns);
                    }
                    obj.Status = "success";
                    obj.Message = "Notification Data";
                    obj.Data = list;
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

        #region Batch Machine Allocation

        [Route("api/machine_list")]
        [HttpGet]
        public IActionResult machine_list(int Company_id)
        {
            ResponseModel obj = new ResponseModel();
            List<resource_card_list> list = new List<resource_card_list>();
            try
            {
                var dt = _configdb.Machine_List(Company_id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        list.Add(new resource_card_list
                        {
                            resource_id = Convert.ToInt32(@dr["RESOURCE_ID"].ToString()),
                            resource_name = @dr["RESOURCE_NAME"].ToString(),
                            capacity = Convert.ToDecimal(@dr["CAPACITY"].ToString()),
                            allocated = Convert.ToDecimal(@dr["ALLOCATED"].ToString()),
                            type = @dr["TYPE"].ToString(),
                            uom = @dr["UOM"].ToString()
                        });
                    }

                    obj.Status = "success";
                    obj.Message = "Machine List.";
                    obj.Data = new { list };
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

        [Route("api/get_all_locations")]
        [HttpGet]
        public IActionResult Get_All_Locatioins(int Company_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<SelectListItem> BATCHES = new List<SelectListItem>();
            try
            {
                List<ListItem_Class> location = new List<ListItem_Class>();
                var dt = _configdb.Get_All_Locations(Company_Id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        location.Add(new ListItem_Class { area = @dr["AREA"].ToString(), Text = @dr["LOCATION_NAME"].ToString(), Value = @dr["LOCATION_ID"].ToString() });
                    }
                    obj.Status = "success";
                    obj.Message = "Location data.";
                    obj.Data = new { location };
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

        [Route("api/get_user_dashboard_nob")]
        [HttpGet]
        public IActionResult Get_User_dashboard_nob(int user_id)
        {
            ResponseModel obj = new ResponseModel();
            var result = JsonConvert.SerializeObject(new DataTable());
            try
            {
              
                var dt = _configdb.Get_User_Dashboard_Nob(user_id);
                if (dt.Rows.Count > 0)
                {
                    result = JsonConvert.SerializeObject(dt);
                    obj.Status = "success";
                    obj.Message = "User NOB right list.";
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