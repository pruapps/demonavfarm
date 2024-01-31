using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FarmIT_Api.Models;
using FarmIT_Api.database_accesslayer;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Data;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FarmIT_Api.Controllers
{    
    [ApiController]
    [Authorize]
    public class LoginController : ControllerBase
    {
        /// <summary>
        /// Created by- Ankit Rajput Updated by Avinash Bhatia
        /// Dated:- 01-04-2020
        /// Method Details
        /// -----------------------------------------------------------------------------------------
        /// This controller contains methods for Signin/Out/Out All, Change Password, Dashboard, Profile/Menu, 
        /// User Plan and billing information, Subscription details,
        /// comman Api's, common function for SMS and Razorpay Subcription function
        /// </summary>
        private IloginDB _loginDB;
        private IConfigDB _configDB;
        public IConfiguration configuration;
        public LoginController(IloginDB iloginDB, IConfiguration iconfig,IConfigDB configDB)
        {
            _loginDB = iloginDB;
            configuration = iconfig;
            _configDB = configDB;
        }

        #region Sign In/Out/Out All
        //This method is used for login user by username and password or with OTP
        [Route("api/Signin")]
        [HttpPost]
        public IActionResult SignIn(SigninRequest sr)
            {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (sr.Social_Id == "null" || sr.Social_Id == null)
                {
                    sr.Social_Id = null;

                    if (sr.Mobile_No == "null" || sr.Mobile_No == null)
                    {
                        obj.Status = "failure";
                        obj.Message = "Mobile No. can't be blank.";
                        obj.Data = new { };
                        return Ok(obj);
                    }
                    if (sr.Signinwithotp == false)
                    {
                        if (sr.Password == "null" || sr.Password == null)
                        {
                            obj.Status = "failure";
                            obj.Message = "Password can't be blank.";
                            obj.Data = new { };
                            return Ok(obj);
                        }
                    }
                    List<LogIn> login = new List<LogIn>();
                    var vall = (_loginDB.User_Login(sr.Mobile_No, sr.Password, sr.Social_Id)).Split(',');
                    if (vall[0] == "success")
                    {
                        var user_agent = Request.Headers["User-Agent"].ToString();
                        var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                        Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                        var access_token = Utility.GenerateToken(vall[4], vall[8], ip, user_agent, unixTimestamp);                       

                        if (sr.Signinwithotp == true)
                        {
                            Random generator = new Random();
                            string otp = generator.Next(0, 999999).ToString("D6");
                            string res = OtpForLogin(sr.Mobile_No, otp);
                            if (res == "SUBMIT_SUCCESS ")
                            {
                                _loginDB.Insert_User_Token(access_token, Convert.ToInt32(vall[1]));

                                LogIn lg = new LogIn();
                                lg.USER_ID = Convert.ToInt32(vall[1]);
                                lg.PLAN_TAKEN = vall[2] == "0" ? "No" : "Yes";
                                lg.CONFIGURATION_SET = vall[3] == "0" ? "No" : "Yes";
                                lg.COMPANY_ID = Convert.ToInt32(vall[3]);
                                lg.NAME = vall[4];
                                lg.MOBILE_NO = vall[5];
                                lg.EMAIL = vall[6];
                                lg.PROFILE_PIC = vall[7];
                                lg.OTP = otp;
                                lg.TOKEN = access_token;
                                lg.LICENCE = Convert.ToInt32(vall[9]);

                                lg.BILLING_EMAIL = vall[10];
                                lg.BILLING_ADDRESS = vall[11];
                                login.Add(lg);

                                obj.Status = "success";
                                obj.Message = "Successfully login";
                                obj.Data = new { login };
                            }
                            else
                            {
                                obj.Status = "failure";
                                obj.Message = res;
                                obj.Data = new { };
                            }
                        }
                        else
                        {
                            _loginDB.Insert_User_Token(access_token, Convert.ToInt32(vall[1]));

                            LogIn lg = new LogIn();
                            lg.USER_ID = Convert.ToInt32(vall[1]);
                            lg.PLAN_TAKEN = vall[2] == "0" ? "No" : "Yes";
                            lg.CONFIGURATION_SET = vall[3] == "0" ? "No" : "Yes";
                            lg.COMPANY_ID = Convert.ToInt32(vall[3]);
                            lg.NAME = vall[4];
                            lg.MOBILE_NO = vall[5];
                            lg.EMAIL = vall[6];
                            lg.PROFILE_PIC = vall[7];
                            lg.TOKEN = access_token;
                            lg.LICENCE = Convert.ToInt32(vall[9]);

                            lg.BILLING_EMAIL = vall[10];
                            lg.BILLING_ADDRESS = vall[11];

                            login.Add(lg);

                            obj.Status = "success";
                            obj.Message = "Successfully login";
                            obj.Data = new { login };
                        }
                    }
                    else
                    {
                        obj.Status = "failure";
                        obj.Message = vall[1];
                        obj.Data = new { };
                    }
                }
                else
                {
                    List<LogIn> login = new List<LogIn>();
                    var vall = (_loginDB.User_Login(sr.Mobile_No, sr.Password, sr.Social_Id)).Split(',');
                    if (vall[0] == "success")
                    {
                        var user_agent = Request.Headers["User-Agent"].ToString();
                        var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                        Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                        var access_token = Utility.GenerateToken(vall[4], vall[8], ip, user_agent, unixTimestamp);

                        _loginDB.Insert_User_Token(access_token, Convert.ToInt32(vall[1]));

                        //var dt = _loginDB.Get_Billing_Info(Convert.ToInt32(vall[1]));

                        LogIn lg = new LogIn();
                        lg.USER_ID = Convert.ToInt32(vall[1]);
                        lg.PLAN_TAKEN = vall[2] == "0" ? "No" : "Yes";
                        lg.CONFIGURATION_SET = vall[3] == "0" ? "No" : "Yes";
                        lg.COMPANY_ID = Convert.ToInt32(vall[3]);
                        lg.NAME = vall[4];
                        lg.MOBILE_NO = vall[5];
                        lg.EMAIL = vall[6];
                        lg.PROFILE_PIC = vall[7];
                        lg.TOKEN = access_token;
                        lg.LICENCE = Convert.ToInt32(vall[9]);

                        lg.BILLING_EMAIL = vall[10];
                        lg.BILLING_ADDRESS = vall[11];
                        login.Add(lg);

                        obj.Status = "success";
                        obj.Message = "Successfully login";
                        obj.Data = new { login };
                    }
                    else
                    {
                        obj.Status = "failure";
                        obj.Message = vall[1];
                        obj.Data = new { };
                    }
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
        //This method is used for user registeration
        [Route("api/Signup")]
        [HttpPost]
        public IActionResult SignUP(SignupRequest sr)
        {
            ResponseModel obj = new ResponseModel();
            string User_Id = "";
            try
            {
                if (sr.IsSocial == 0)
                {
                    if (sr.User_Name == "null" || sr.User_Name == null)
                    {
                        obj.Status = "failure";
                        obj.Message = "User Name can't be blank.";
                        obj.Data = new { };
                        return Ok(obj);
                    }
                    if (sr.Mobile_No == "null" || sr.Mobile_No == null)
                    {
                        obj.Status = "failure";
                        obj.Message = "Mobile No. can't be blank.";
                        obj.Data = new { };
                        return Ok(obj);
                    }

                    sr.Social_Name = sr.User_Name;
                    sr.Social_Id = null;
                    sr.Social_Email = null;
                    sr.Social_Profile_Pic = null;
                }
                else
                {
                    if (sr.Social_Id == null)
                    {
                        obj.Status = "failure";
                        obj.Message = "Social id can't be blank.";
                        obj.Data = new { };
                        return Ok(obj);
                    }
                }

                var vall = (_loginDB.Registration(sr.Social_Name, sr.Mobile_No, sr.Social_Id, sr.Social_Email, sr.Social_Profile_Pic, sr.Is_FaceBook, sr.Is_Gmail,sr.Is_Apple_id)).Split(',');
                if (vall[0] == "success")
                {
                    if (sr.IsSocial == 0)
                    {
                        string msg = "Dear " + sr.User_Name + ", Thank you for successfully registering with us. Your default password for accessing the NAVFARM Web and Mobile application is navfarm@1234       .Navfarm";

                        SendSms(sr.Mobile_No, msg);
                    }
                    var user_agent = Request.Headers["User-Agent"].ToString();
                    var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                    Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                    var token = Utility.GenerateToken(sr.User_Name, "navfarm@1234", ip, user_agent, unixTimestamp);

                    _loginDB.Insert_User_Token(token, Convert.ToInt32(vall[1]));

                    User_Id = vall[1];
                    obj.Status = "success";
                    obj.Message = "Successfully signup.";
                    obj.Data = new { User_Id, token };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = vall[1];
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
        //This method is used for user logout
        [Route("api/Signout")]
        [RESTAuthorizeAttribute]
        [HttpPost]
        public IActionResult SignOut(SignOutRequest sr)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (sr.User_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "User Id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (sr.Token == null || sr.Token == "null")
                {
                    obj.Status = "failure";
                    obj.Message = "Token can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var vall = _loginDB.User_Logout(sr.User_Id, sr.Token);
                if (vall == "success")
                {
                    obj.Status = "success";
                    obj.Message = "Successfully signout.";
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = "invalid credentials.";
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
        //This method is used for all user logout
        [Route("api/Signout_all")]
        [RESTAuthorizeAttribute]
        [HttpPost]
        public IActionResult SignOut_All(SignOutRequest sr)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (sr.User_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "User Id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var vall = _loginDB.User_Logout(sr.User_Id);
                if (vall == "success")
                {
                    obj.Status = "success";
                    obj.Message = "Successfully signout.";
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = "invalid credentials.";
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

        #region Change/Forgot Password
        //This method is used for password change by user
        [Route("api/change_password")]
        [RESTAuthorizeAttribute]
        [HttpPost]
        public IActionResult Change_Password(ChnagePassword cp)
        {
            string msg = "";
            ResponseModel obj = new ResponseModel();
            try
            {
                if (cp.old_password == "null" || cp.old_password == null)
                {
                    obj.Status = "failure";
                    obj.Message = "Old password can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (cp.new_password == "null" || cp.new_password == null)
                {
                    obj.Status = "failure";
                    obj.Message = "New password can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (cp.user_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "User id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                msg = _loginDB.ChangePassword(cp.old_password, cp.new_password, cp.user_id);
                if (msg == "success")
                {
                    obj.Status = "success";
                    obj.Message = "Password successfully changed.";
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = msg;
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

        //This method is used for reset the password if user forgot the same.
        [Route("api/forgotpassword")]
        [HttpPost]
        public IActionResult Forgot_Password(SignupRequest SR)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (SR.Mobile_No == "null" || SR.Mobile_No == null)
                {
                    obj.Status = "failed";
                    obj.Message = "Phone No. can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                string[] response = (_loginDB.ForgotPassword(SR.Mobile_No)).Split(',');
                if (response[0] == "success")
                {
                    string msg = "Your new password for accessing the NAVFARM Web and Mobile application is "+ response[1] + "        .Navfarm";

                    SendSms("+" + SR.Mobile_No, msg);

                    obj.Status = "success";
                    obj.Message = "Password successfully reset.";
                    obj.Data = "";
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = response[1];
                    obj.Data = "";
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

        #region Dashboard
        //This is common dashboard  display for all nature of business 
        [Route("api/get_dashboard_all_nob")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult getdashboard_all_nob(int Company_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<DashboardModel> ds_list = new List<DashboardModel>();
            try
            { 
                if (Company_Id == 0)
                {
                    obj.Status = "failed";
                    obj.Message = "Company id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                var dt = _loginDB.Get_Dashboard_All_Nob(Company_Id);
                if (dt.Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(dt);
                    obj.Status = "success";
                    obj.Message = "Dashboard data.";
                    obj.Data = new { result };
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
        //This is used for dashboard display nature of business wise
        [Route("api/get_dashboard")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult Dashboard(int Nature_Id,int Company_Id,int user_id)
        {
            ResponseModel obj = new ResponseModel();
            List<DashboardModel> ds_list = new List<DashboardModel>();
            try
            {
                if (Nature_Id == 0)
                {
                    obj.Status = "failed";
                    obj.Message = "Nature of business id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Company_Id == 0)
                {
                    obj.Status = "failed";
                    obj.Message = "Company id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                var dt = _loginDB.Get_Dashboard(Nature_Id,Company_Id, user_id);

                if (dt.Rows.Count > 0)
                {
                    string line_of_business = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        DashboardModel dm = new DashboardModel();
                        List<DashboardData> data_list = new List<DashboardData>();
                        if (line_of_business != dr["LINE_OF_BUSINESS"].ToString())
                        {
                            line_of_business = dr["LINE_OF_BUSINESS"].ToString();
                            DashboardData dd = new DashboardData();
                            
                            List<string> label_list = new List<string>();
                            List<string> Value_list = new List<string>();
                            List<string> icon_list = new List<string>();

                            foreach (DataRow dr1 in dt.Select("LINE_OF_BUSINESS = '" + line_of_business + "'"))
                            {
                                label_list.Add(dr1["LABELS"].ToString());
                                
                                Value_list.Add(dr1["LABEL_VALUES"].ToString());
                                icon_list.Add(dr1["ICON_NAME"].ToString());
                            }
                            string[] ar_labels = label_list.ToArray();
                            string[] ar_values = Value_list.ToArray();
                            string[] ar_icon = icon_list.ToArray();
                            dd.Labels = ar_labels;
                            dd.Values = ar_values;
                            dd.Icon_Name = ar_icon;
                            data_list.Add(dd);

                            dm.lob_id = Convert.ToInt32(dr["LINE_ID"].ToString());
                            dm.Line_Of_Business = dr["LINE_OF_BUSINESS"].ToString();
                            dm.Data = data_list;
                            ds_list.Add(dm);
                        }                        
                    }
                    obj.Status = "success";
                    obj.Message = "Dashboard data";
                    obj.Data = ds_list;
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
        //This is used for dashboard summary nature of business wise
        [Route("api/get_dashboard_summary")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult Dashboard_Summary(int LOB_Id, int Company_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<Dashboard_Summary> ds_list = new List<Dashboard_Summary>();
            try
            {
                if (LOB_Id == 0)
                {
                    obj.Status = "failed";
                    obj.Message = "Line of business id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Company_Id == 0)
                {
                    obj.Status = "failed";
                    obj.Message = "Company id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                var dt = _loginDB.Get_Dashboard_Summary(LOB_Id, Company_Id);

                if (dt.Rows.Count > 0)
                {
                    string batch_name = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        Dashboard_Summary dm = new Dashboard_Summary();
                        List<DashboardData> data_list = new List<DashboardData>();
                        if (batch_name != dr["BATCH_NAME"].ToString())
                        {
                            batch_name = dr["BATCH_NAME"].ToString();
                            DashboardData dd = new DashboardData();

                            List<string> label_list = new List<string>();
                            List<string> Value_list = new List<string>();

                            foreach (DataRow dr1 in dt.Select("BATCH_NAME = '" + batch_name + "'"))
                            {
                                label_list.Add(dr1["LABELS"].ToString());

                                Value_list.Add(dr1["LABEL_VALUES"].ToString());
                            }
                            string[] ar_labels = label_list.ToArray();
                            string[] ar_values = Value_list.ToArray();
                            dd.Labels = ar_labels;
                            dd.Values = ar_values;
                            data_list.Add(dd);

                            dm.lob_id = Convert.ToInt32(dr["LINE_ID"].ToString());
                            dm.Line_Of_Business = dr["LINE_OF_BUSINESS"].ToString();
                            dm.batch_id = Convert.ToInt32(dr["BATCH_ID"].ToString());
                            dm.batch_name = dr["BATCH_NAME"].ToString();
                            dm.Data = data_list;
                            ds_list.Add(dm);
                        }                       
                    }
                    obj.Status = "success";
                    obj.Message = "Dashboard summary step1 data";
                    obj.Data = ds_list;
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
        //This is used for dashboard summary details nature of business wise
        [Route("api/get_dashboard_summary_Details")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult Dashboard_Summary_Details(int Batch_Id,int Lob_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<Dashboard_Summary_Details> ds_list = new List<Dashboard_Summary_Details>();
            try
            {
                if (Batch_Id == 0)
                {
                    obj.Status = "failed";
                    obj.Message = "Batch id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var dt = _loginDB.Get_Dashboard_Summary_Details(Batch_Id,Lob_Id);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Dashboard_Summary_Details dm = new Dashboard_Summary_Details();
                        dm.parameter_name = dr["PARAMETER_NAME"].ToString();
                        dm.actual_value = Convert.ToDecimal(dr["ACTUAL_VALUE"].ToString());
                        ds_list.Add(dm);                        
                    }
                    obj.Status = "success";
                    obj.Message = "Dashboard summary step 2 data";
                    obj.Data = ds_list;
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
        //This is used for dashboard summary details nature of business wise
        [Route("api/get_dashboard_summary_Details2")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult Dashboard_Summary_Details2(int Batch_Id,string Parameter_Name,string Month_Year)
        {
            ResponseModel obj = new ResponseModel();
            List<Dashboard_Summary_Details2> ds_list = new List<Dashboard_Summary_Details2>();
            try
            {
                if (Batch_Id == 0)
                {
                    obj.Status = "failed";
                    obj.Message = "Batch id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Parameter_Name == null || Parameter_Name == "null")
                {
                    obj.Status = "failed";
                    obj.Message = "Parameter name can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var dt = _loginDB.Get_Dashboard_Summary_Details2(Batch_Id,Parameter_Name,Month_Year);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Dashboard_Summary_Details2 dm = new Dashboard_Summary_Details2();
                        dm.posting_date = dr["POSTING_DATE"].ToString();
                        dm.actual_value = Convert.ToDecimal(dr["ACTUAL_VALUE"].ToString());
                        ds_list.Add(dm);
                    }
                    obj.Status = "success";
                    obj.Message = "Dashboard summary step 3 data";
                    obj.Data = ds_list;
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
        //This method is used for dispalying month wise ,Week Wise and today graph on dashborad lob for laying
        [Route("api/get_dashboard_laying_output_month_Graph")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult Laying_Monthly_Output_Graph( int Company_Id, int LOB_Id,int Type)
        {
            ResponseModel obj = new ResponseModel();
            List<Dashboard_Graph_Summary> ds_list = new List<Dashboard_Graph_Summary>();
            try
            {
                if (LOB_Id == 0)
                {
                    obj.Status = "failed";
                    obj.Message = "Line of business id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Company_Id == 0)
                {
                    obj.Status = "failed";
                    obj.Message = "Company id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                var dt = _loginDB.Get_Dashboard_Laying_Monthly_Output(Company_Id,LOB_Id,Type );

                if (dt.Rows.Count > 0)
                {

                            Dashboard_Graph_Summary dm = new Dashboard_Graph_Summary();
                            List<DashboardData> data_list = new List<DashboardData>();
                       
                            DashboardData dd = new DashboardData();

                            List<string> label_list = new List<string>();
                            List<string> Value_list = new List<string>();
                            List<string> Value_list2 = new List<string>();

                           foreach (DataRow dr1 in dt.Rows)
                            {
                                label_list.Add(dr1["Months"].ToString());
                                Value_list.Add(dr1["TOTAL_EGGS"].ToString());
                                Value_list2.Add(dr1["AVG_WEIGHT"].ToString());
                    }
                            string[] ar_labels = label_list.ToArray();
                            string[] ar_values = Value_list.ToArray();
                            string[] ar_values2 = Value_list2.ToArray();
                            dd.Labels = ar_labels;
                            dd.Values = ar_values;
                            dd.Icon_Name = ar_values2;
                             data_list.Add(dd);

                            dm.Data = data_list;
                            ds_list.Add(dm);
                     
                    
                    obj.Status = "success";
                    obj.Message = "Dashboard Laying Monthly Output";
                    obj.Data = ds_list;
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

        //NEW This method is used for dispalying month wise ,Week Wise and today graph on dashborad lob for laying
        [Route("api/get_dashboard_laying_output_month_Graph1")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult Laying_Monthly_Output_Graph1(int Company_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<Dashboard_Graph_Summary> ds_list = new List<Dashboard_Graph_Summary>();
            try
            {
                if (Company_Id == 0)
                {
                    obj.Status = "failed";
                    obj.Message = "Company id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                var dt = _loginDB.Get_Dashboard_Laying_Monthly_Output1(Company_Id);

                if (dt.Rows.Count > 0)
                {
                    obj.Status = "success";
                    obj.Message = "New Dashboard Laying Monthly Output ";
                    obj.Data = dt;
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
        //This method is used for dispalying month wise ,Week Wise and today graph on dashborad lob for commercial broiler
        [Route("api/get_dashboard_comm_output_month_Graph")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult COMM_Monthly_Output_Graph(int Company_Id, int LOB_Id, int Type)
        {
            ResponseModel obj = new ResponseModel();
            List<Dashboard_Graph_Summary> ds_list = new List<Dashboard_Graph_Summary>();
            try
            {
                if (LOB_Id == 0)
                {
                    obj.Status = "failed";
                    obj.Message = "Line of business id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Company_Id == 0)
                {
                    obj.Status = "failed";
                    obj.Message = "Company id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                var dt = _loginDB.Get_Dashboard_COMM_Monthly_Output(Company_Id, LOB_Id, Type);

                if (dt.Rows.Count > 0)
                {

                    Dashboard_Graph_Summary dm = new Dashboard_Graph_Summary();
                    List<DashboardData> data_list = new List<DashboardData>();

                    DashboardData dd = new DashboardData();

                    List<string> label_list = new List<string>();
                    List<string> Value_list = new List<string>();
                    List<string> Value_list2 = new List<string>();

                    foreach (DataRow dr1 in dt.Rows)
                    {
                        label_list.Add(dr1["Months"].ToString());
                        Value_list.Add(dr1["TOTAL_EGGS"].ToString());
                        Value_list2.Add(dr1["AVG_WEIGHT"].ToString());
                    }
                    string[] ar_labels = label_list.ToArray();
                    string[] ar_values = Value_list.ToArray();
                    string[] ar_values2 = Value_list2.ToArray();
                    dd.Labels = ar_labels;
                    dd.Values = ar_values;
                    dd.Icon_Name = ar_values2;
                    data_list.Add(dd);

                    dm.Data = data_list;
                    ds_list.Add(dm);


                    obj.Status = "success";
                    obj.Message = "Dashboard Comm Monthly Output";
                    obj.Data = ds_list;
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
        //This method is used for dispalying month wise ,Week Wise and today graph on dashborad lob for hatching
        [Route("api/get_dashboard_laying_output_month_Graph2")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult Laying_Monthly_Output_Graph2(int Company_Id, int LOB_Id, int Type)
        {
            ResponseModel obj = new ResponseModel();
            List<Dashboard_Graph_Summary> ds_list = new List<Dashboard_Graph_Summary>();
            try
            {
                if (LOB_Id == 0)
                {
                    obj.Status = "failed";
                    obj.Message = "Line of business id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Company_Id == 0)
                {
                    obj.Status = "failed";
                    obj.Message = "Company id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                var dt = _loginDB.Get_Dashboard_Laying_Monthly_Output(Company_Id, LOB_Id, Type);

                if (dt.Rows.Count > 0)
                {

                    Dashboard_Graph_Summary dm = new Dashboard_Graph_Summary();
                    List<DashboardData> data_list = new List<DashboardData>();

                    DashboardData dd = new DashboardData();

                    List<string> label_list = new List<string>();
                    List<string> Value_list = new List<string>();

                    foreach (DataRow dr1 in dt.Rows)
                    {
                        label_list.Add(dr1["Months"].ToString());

                        Value_list.Add(dr1["TOTAL_EGGS"].ToString());
                    }
                    string[] ar_labels = label_list.ToArray();
                    string[] ar_values = Value_list.ToArray();
                    dd.Labels = ar_labels;
                    dd.Values = ar_values;
                    data_list.Add(dd);

                    dm.Data = data_list;
                    ds_list.Add(dm);


                    obj.Status = "success";
                    obj.Message = "Dashboard Laying Monthly Output2";
                    obj.Data = ds_list;
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
        //This method is used for dispalying month ,last week and today mortality graph on dashborad lob wise
        [Route("api/get_dashboard_laying_mortality_Graph")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult Laying_Mortality_Graph(int Company_Id, int LOB_Id)
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
                if (LOB_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Lob details can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _loginDB.Get_Dashboard_Laying_Mortality(Company_Id, LOB_Id);

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
                    List<Laying_Mortality> Value_list = new List<Laying_Mortality>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Laying_Mortality bl = new Laying_Mortality();
                        bl.Mortality_Per = Convert.ToDecimal(dr["Mortality_Per"].ToString());
                        bl.Previous_WK_Mortality = Convert.ToDecimal(dr["Previous_WK_Mortality"].ToString());
                        bl.Prevoius_Mon_Mortaltiy = Convert.ToDecimal(dr["Prevoius_Mon_Mortaltiy"].ToString());
                        bl.Today_Mortaltiy = Convert.ToInt32(dr["Today_mortality"].ToString());
                        Value_list.Add(bl);
                    }

                    dd.Values = Value_list;
                    values.Add(dd);

                    obj.Status = "success";
                    obj.Message = "Feed report data.";
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


        //This method is used for dispalying Batch Wise month ,last week and today mortality graph on dashborad lob wise
        [Route("api/get_dashboard_female_mortality_graph")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult FemaleLaying_Mortality_Graph(int Company_Id, int LOB_Id)
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
                if (LOB_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Lob details can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _loginDB.Get_Dashboard_Female_Laying_Mortality(Company_Id, LOB_Id);

                List<ReportValueData> values = new List<ReportValueData>();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    obj.Status = "success";
                    obj.Message = "Feed report data.";
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
        //This method is used for dispalying month wise ,Week Wise and today milk graph on dashborad lob for dairy
        [Route("api/get_dashboard_dairy_milk_Graph")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult Dairy_Milk_Graph(int Company_Id, int LOB_Id)
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
                if (LOB_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Lob details can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _loginDB.Get_Dashboard_Dairy_Milk(Company_Id, LOB_Id);

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
                    List<Laying_Mortality> Value_list = new List<Laying_Mortality>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Laying_Mortality bl = new Laying_Mortality();
                        bl.Mortality_Per = Convert.ToDecimal(dr["Mortality_Per"].ToString());
                        bl.Previous_WK_Mortality = Convert.ToDecimal(dr["Previous_WK_Mortality"].ToString());
                        bl.Prevoius_Mon_Mortaltiy = Convert.ToDecimal(dr["Prevoius_Mon_Mortaltiy"].ToString());
                        bl.Today_Mortaltiy = Convert.ToInt32(dr["Today_mortality"].ToString());
                        Value_list.Add(bl);
                    }

                    dd.Values = Value_list;
                    values.Add(dd);

                    obj.Status = "success";
                    obj.Message = "Milk report data.";
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
        //This method is used for dispalying month wise ,Week Wise and today reject egg graph on dashborad lob for hatching
        [Route("api/get_dashboard_hatch_reject_Graph")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult Hatch_Reject_Graph(int Company_Id, int LOB_Id)
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
                if (LOB_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Lob details can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _loginDB.Get_Dashboard_Hatch_Reject(Company_Id, LOB_Id);

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
                    List<Laying_Mortality> Value_list = new List<Laying_Mortality>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Laying_Mortality bl = new Laying_Mortality();
                        bl.Mortality_Per = Convert.ToDecimal(dr["Reject_Per"].ToString());
                        bl.Previous_WK_Mortality = Convert.ToDecimal(dr["Previous_WK_Mortality"].ToString());
                        bl.Prevoius_Mon_Mortaltiy = Convert.ToDecimal(dr["Prevoius_Mon_Mortaltiy"].ToString());
                        bl.Today_Mortaltiy = Convert.ToInt32(dr["Today_mt"].ToString());
                        Value_list.Add(bl);
                    }

                    dd.Values = Value_list;
                    values.Add(dd);

                    obj.Status = "success";
                    obj.Message = "Hatch report data.";
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

        //This method is used for dispalying rejected ,iter egg
        [Route("api/get_dashboard_hatch_egg_Graph")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult Hatch_egg_Graph(int Company_Id, int LOB_Id)
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
                if (LOB_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Lob details can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _loginDB.Get_Dashboard_Hatch_Egg_Graph(Company_Id, LOB_Id);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    obj.Status = "success";
                    obj.Message = "Hatch report data.";
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

        //This method is used for dispalying rejected ,iter egg
        [Route("api/get_dashboard_hatch_mortality_graph")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult Hatch_Mortality_Graph(int Company_Id, int LOB_Id)
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
                if (LOB_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Lob details can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _loginDB.Get_Dashboard_Hatch_Mortality_Graph(Company_Id, LOB_Id);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    obj.Status = "success";
                    obj.Message = "Hatch report data.";
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

        //This method is used for dispalying Monthly Output report
        [Route("api/get_dashboard_hatch_output_graph")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult Hatch_Output_Graph(int Company_Id, int LOB_Id)
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
                if (LOB_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Lob details can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _loginDB.Get_Dashboard_Hatch_Output_Graph(Company_Id, LOB_Id);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    obj.Status = "success";
                    obj.Message = "Hatch output report data.";
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
        //This method is used for dispalying Monthly Output report
        [Route("api/get_dashboard_commercialbroiler_output_graph")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult CommercialBroiler_Output_Graph(int Company_Id, int LOB_Id)
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
                if (LOB_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Lob details can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _loginDB.Get_Dashboard_CommercialBroiler_Output_Graph(Company_Id, LOB_Id);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    obj.Status = "success";
                    obj.Message = "Hatch output report data.";
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
        //This is used for dispalying least performance batches on dashboard
        [Route("api/get_batch_least_performance_report")]
        [HttpGet]
        public IActionResult BATCH_LEAST_PREFORMANCE_REPORT(int Company_Id, int Lob_id)
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
                if (Lob_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Lob details can't blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                var ds = _loginDB.Get_Dashboard_Least_Performance(Company_Id, Lob_id);

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
                    List<Batch_Least_Performance_Report> Value_list = new List<Batch_Least_Performance_Report>();
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        Batch_Least_Performance_Report bl = new Batch_Least_Performance_Report();
                       
                        bl.Batch_No = dr["Batch_No"].ToString();
                        bl.Start_Date = Convert.ToString(dr["Start_Date"].ToString());
                        bl.Last_entry_Date = Convert.ToString(dr["Last_Entry_Date"].ToString());
                        bl.Opening_Stock = Convert.ToDecimal(dr["Opening_Stock"].ToString());
                        bl.Remaning_Stock = Convert.ToDecimal(dr["Remaining_Stock"].ToString());
                        bl.total_Output_Per = Convert.ToDecimal(dr["total_Output_Per"].ToString());
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
        //This is used for dispalying location graph on dashboard
        [Route("api/get_dashboard_Location_Graph")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult Location_Output_Graph(int Company_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<Dashboard_Graph_Summary> ds_list = new List<Dashboard_Graph_Summary>();
            try
            {

                if (Company_Id == 0)
                {
                    obj.Status = "failed";
                    obj.Message = "Company id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                var dt = _loginDB.Get_Dashboard_Location_Output(Company_Id);

                if (dt.Rows.Count > 0)
                {

                    Dashboard_Graph_Summary dm = new Dashboard_Graph_Summary();
                    List<DashboardData> data_list = new List<DashboardData>();

                    DashboardData dd = new DashboardData();
                    List<string> Value_list = new List<string>();

                    foreach (DataRow dr1 in dt.Rows)
                    {
                        Value_list.Add(dr1["LOCATION_NAME"].ToString());
                    }
                    string[] ar_values = Value_list.ToArray();
                    dd.Values = ar_values;
                    data_list.Add(dd);

                    dm.Data = data_list;
                    ds_list.Add(dm);


                    obj.Status = "success";
                    obj.Message = "Dashboard Laying Monthly Output";
                    obj.Data = ds_list;
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
        //This is used for dispalying breeding month wise graph on dashboard
        [Route("api/get_dashboard_lbreeding_output_month_Graph")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult LBREEDING_Monthly_Output_Graph(int Company_Id, int LOB_Id, int Type)
        {
            ResponseModel obj = new ResponseModel();
            List<Dashboard_Graph_Summary> ds_list = new List<Dashboard_Graph_Summary>();
            try
            {
                if (LOB_Id == 0)
                {
                    obj.Status = "failed";
                    obj.Message = "Line of business id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Company_Id == 0)
                {
                    obj.Status = "failed";
                    obj.Message = "Company id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                var dt = _loginDB.Get_Dashboard_LBREEDING_Monthly_Output(Company_Id, LOB_Id, Type);

                if (dt.Rows.Count > 0)
                {

                    Dashboard_Graph_Summary dm = new Dashboard_Graph_Summary();
                    List<DashboardData> data_list = new List<DashboardData>();

                    DashboardData dd = new DashboardData();

                    List<string> label_list = new List<string>();
                    List<string> Value_list = new List<string>();
                    List<string> Value_list2 = new List<string>();

                    foreach (DataRow dr1 in dt.Rows)
                    {
                        label_list.Add(dr1["Months"].ToString());
                        Value_list.Add(dr1["TOTAL_EGGS"].ToString());
                        Value_list2.Add(dr1["AVG_WEIGHT"].ToString());
                    }
                    string[] ar_labels = label_list.ToArray();
                    string[] ar_values = Value_list.ToArray();
                    string[] ar_values2 = Value_list2.ToArray();
                    dd.Labels = ar_labels;
                    dd.Values = ar_values;
                    dd.Icon_Name = ar_values2;
                    data_list.Add(dd);

                    dm.Data = data_list;
                    ds_list.Add(dm);


                    obj.Status = "success";
                    obj.Message = "Dashboard Comm Monthly Output";
                    obj.Data = ds_list;
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
        //This is used for dispalying dairy month wise graph on dashboard
        [Route("api/get_dashboard_dairy_output_month_Graph")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult DAIRY_Monthly_Output_Graph(int Company_Id, int LOB_Id, int Type)
        {
            ResponseModel obj = new ResponseModel();
            List<Dashboard_Graph_Summary> ds_list = new List<Dashboard_Graph_Summary>();
            try
            {
                if (LOB_Id == 0)
                {
                    obj.Status = "failed";
                    obj.Message = "Line of business id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Company_Id == 0)
                {
                    obj.Status = "failed";
                    obj.Message = "Company id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                var dt = _loginDB.Get_Dashboard_LDairy_Monthly_Output(Company_Id, LOB_Id, Type);

                if (dt.Rows.Count > 0)
                {

                    Dashboard_Graph_Summary dm = new Dashboard_Graph_Summary();
                    List<DashboardData> data_list = new List<DashboardData>();

                    DashboardData dd = new DashboardData();

                    List<string> label_list = new List<string>();
                    List<string> Value_list = new List<string>();
                    List<string> Value_list2 = new List<string>();

                    foreach (DataRow dr1 in dt.Rows)
                    {
                        label_list.Add(dr1["Months"].ToString());
                        Value_list.Add(dr1["TOTAL_Milk"].ToString());
                        Value_list2.Add(dr1["liveStock"].ToString());
                    }
                    string[] ar_labels = label_list.ToArray();
                    string[] ar_values = Value_list.ToArray();
                    string[] ar_values2 = Value_list2.ToArray();
                    dd.Labels = ar_labels;
                    dd.Values = ar_values;
                    dd.Icon_Name = ar_values2;
                    data_list.Add(dd);

                    dm.Data = data_list;
                    ds_list.Add(dm);


                    obj.Status = "success";
                    obj.Message = "Dashboard Comm Monthly Output";
                    obj.Data = ds_list;
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

        [Route("api/get_dashboard_laying_egg_graph")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult Laying_EGGS_Graph(int Company_Id, int LOB_Id, int Type)
        {
            ResponseModel obj = new ResponseModel();
            List<Dashboard_Graph_Summary> ds_list = new List<Dashboard_Graph_Summary>();
            try
            {
                if (LOB_Id == 0)
                {
                    obj.Status = "failed";
                    obj.Message = "Line of business id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Company_Id == 0)
                {
                    obj.Status = "failed";
                    obj.Message = "Company id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                var dt = _loginDB.Get_Dashboard_Laying_Egg_Output(Company_Id, LOB_Id, Type);

                if (dt.Rows.Count > 0)
                {

                    Dashboard_Graph_Summary dm = new Dashboard_Graph_Summary();
                    List<DashboardData> data_list = new List<DashboardData>();

                    DashboardData dd = new DashboardData();

                    List<string> label_list = new List<string>();
                    List<string> Value_list = new List<string>();
                    List<string> Value_list2 = new List<string>();

                    foreach (DataRow dr1 in dt.Rows)
                    {
                        label_list.Add(dr1["BATCH_NO"].ToString());
                        Value_list.Add(dr1["TOTAL_EGGS"].ToString());
                        Value_list2.Add(dr1["Hatch_Egg"].ToString());
                    }
                    string[] ar_labels = label_list.ToArray();
                    string[] ar_values = Value_list.ToArray();
                    string[] ar_values2 = Value_list2.ToArray();
                    dd.Labels = ar_labels;
                    dd.Values = ar_values;
                    dd.Icon_Name = ar_values2;
                    data_list.Add(dd);

                    dm.Data = data_list;
                    ds_list.Add(dm);


                    obj.Status = "success";
                    obj.Message = "Dashboard Comm Monthly Output";
                    obj.Data = ds_list;
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
        [Route("api/get_dashboard_laying_egg_wt_graph")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult Laying_EGGS_WT_Graph(int Company_Id, int LOB_Id, int Type)
        {
            ResponseModel obj = new ResponseModel();
            List<Dashboard_Graph_Summary> ds_list = new List<Dashboard_Graph_Summary>();
            try
            {
                if (LOB_Id == 0)
                {
                    obj.Status = "failed";
                    obj.Message = "Line of business id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Company_Id == 0)
                {
                    obj.Status = "failed";
                    obj.Message = "Company id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                var dt = _loginDB.Get_Dashboard_Laying_Egg_WT_Output(Company_Id, LOB_Id, Type);

                if (dt.Rows.Count > 0)
                {

                    Dashboard_Graph_Summary dm = new Dashboard_Graph_Summary();
                    List<DashboardData> data_list = new List<DashboardData>();

                    DashboardData dd = new DashboardData();

                    List<string> label_list = new List<string>();
                    List<string> Value_list = new List<string>();
                    List<string> Value_list2 = new List<string>();

                    foreach (DataRow dr1 in dt.Rows)
                    {
                        label_list.Add(dr1["BATCH_NO"].ToString());
                        Value_list.Add(dr1["TOTAL_EGGS"].ToString());
                        Value_list2.Add(dr1["Hatch_Egg"].ToString());
                    }
                    string[] ar_labels = label_list.ToArray();
                    string[] ar_values = Value_list.ToArray();
                    string[] ar_values2 = Value_list2.ToArray();
                    dd.Labels = ar_labels;
                    dd.Values = ar_values;
                    dd.Icon_Name = ar_values2;
                    data_list.Add(dd);

                    dm.Data = data_list;
                    ds_list.Add(dm);


                    obj.Status = "success";
                    obj.Message = "Dashboard Comm Monthly Output";
                    obj.Data = ds_list;
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
       

        [Route("api/get_dashboard_laying_batch_graph")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult Laying_EGGS_BATCH_Graph(int Batch_id)
        {
            ResponseModel obj = new ResponseModel();
            List<Dashboard_Graph_Summary> ds_list = new List<Dashboard_Graph_Summary>();
            try
            {

                var dt = _loginDB.Get_Dashboard_Laying_Egg_BATCH_Output(Batch_id);

                if (dt.Rows.Count > 0)
                {

                    Dashboard_Graph_Summary dm = new Dashboard_Graph_Summary();
                    List<DashboardData> data_list = new List<DashboardData>();

                    DashboardData dd = new DashboardData();

                    List<string> label_list = new List<string>();
                    List<string> Value_list = new List<string>();
                    foreach (DataRow dr1 in dt.Rows)
                    {
                        label_list.Add(dr1["BATCH_NO"].ToString());
                        Value_list.Add(dr1["TOTAL_EGGS"].ToString());
                    }
                    string[] ar_labels = label_list.ToArray();
                    string[] ar_values = Value_list.ToArray();
                    dd.Labels = ar_labels;
                    dd.Values = ar_values;
                    data_list.Add(dd);

                    dm.Data = data_list;
                    ds_list.Add(dm);


                    obj.Status = "success";
                    obj.Message = "Dashboard Comm Monthly Output";
                    obj.Data = ds_list;
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
        #endregion

        #region Profile/Menu
        //This method is used for dipalying user profile information
        [Route("api/get_profile")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult Profile(int Company_Id, int User_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<Profile_Data> profile = new List<Profile_Data>();
            List<User_Data> users = new List<User_Data>();
            List<Payment_Data> payment = new List<Payment_Data>();
            try
            {
                if (User_Id == 0)
                {
                    obj.Status = "failed";
                    obj.Message = "User id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (Company_Id == 0)
                {
                    obj.Status = "failed";
                    obj.Message = "Company id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                var ds = _loginDB.Get_Profile(Company_Id,User_Id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Profile_Data pd = new Profile_Data();
                        pd.config_id = Convert.ToInt32(dr["CONFIG_ID"].ToString());
                        pd.company_name= dr["COMPANY_NAME"].ToString();
                        pd.user_name = dr["USER_NAME"].ToString();
                        pd.email = dr["EMAIL"].ToString();
                        pd.mobile_no = dr["MOBILE_NO"].ToString();
                        pd.address = dr["ADDRESS"].ToString();
                        pd.tax_num = dr["TAX_NUM"].ToString();
                        pd.farmer_type = dr["FARMER_TYPE"].ToString();
                        pd.logo = (byte[])dr["PIC"];
                        profile.Add(pd);
                    }
                    foreach (DataRow dr1 in ds.Tables[1].Rows)
                    {
                        Payment_Data pd = new Payment_Data();
                        pd.package_description = dr1["PACKAGE_DESCRIPTION"].ToString();
                        pd.price = dr1["PRICE"].ToString();
                        pd.estimated_tax = dr1["ESTIMATED_TAX"].ToString();
                        pd.order_total = dr1["ORDER_TOTAL"].ToString();
                        pd.plan_start_date = dr1["PLAN_START_DATE"].ToString();
                        pd.frequency = dr1["FREQUENCY"].ToString();
                        pd.licence = Convert.ToInt32(dr1["LICENCE"].ToString());
                        pd.plan_type = dr1["PLAN_TYPE"].ToString();
                        pd.razor_pay_invoice_no = dr1["RAZOR_PAY_INVOICE_NO"].ToString();
                        pd.invoice_url = dr1["INVOICE_URL"].ToString();
                        pd.payment_date = dr1["PAYMENT_DATE"].ToString();
                        pd.subscription_id = dr1["SUBSCRIPTION_ID"].ToString();
                        payment.Add(pd);
                    }
                    foreach (DataRow dr1 in ds.Tables[2].Rows)
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
                    obj.Message = "Profile/Payment data";
                    obj.Data = new {profile,payment,users };
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
        //This method is used for updating user profile information
        [Route("api/update_profile")]
        [RESTAuthorizeAttribute]
        [HttpPost]
        public IActionResult Update_Profile(ProfileModel pm)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                var Config = pm.profile_data;
                var ActiveInactive = pm.activedeactive_data;
                var User = pm.user_data;

                DataTable configTable = new DataTable("TBL_UCONFIGURATION_SETUP");
                configTable.Columns.Add("CONFIG_ID", typeof(int));
                configTable.Columns.Add("USER_ID", typeof(int));
                configTable.Columns.Add("NAME", typeof(string));
                configTable.Columns.Add("COMPANY_LOGO", typeof(byte[]));
                configTable.Columns.Add("EMAIL", typeof(string));
                configTable.Columns.Add("ADDRESS", typeof(string));

                DataRow dr = null;
                dr = configTable.NewRow();
                dr["CONFIG_ID"] = Config.config_id;
                dr["USER_ID"] = Config.user_id;
                dr["NAME"] = Config.name;
                dr["COMPANY_LOGO"] = Config.company_logo;
                dr["EMAIL"] = Config.email;
                dr["ADDRESS"] = Config.address;
                configTable.Rows.Add(dr);

                DataTable processTable = new DataTable("TBL_ACTIVE_DEACTIVE");
                processTable.Columns.Add("USER_ID", typeof(int));
                processTable.Columns.Add("STATUS", typeof(string));

                DataRow drProcess = null;
                if(ActiveInactive!=null)
                {
                    foreach (var pr in ActiveInactive)
                    {
                        drProcess = processTable.NewRow();
                        drProcess["USER_ID"] = pr.user_id;
                        drProcess["STATUS"] = pr.status;

                        processTable.Rows.Add(drProcess);
                    }
                }
                else
                {
                    processTable = null;
                }

                DataTable userTable = new DataTable("TBL_USER_SETUP");
                userTable.Columns.Add("NAME", typeof(string));
                userTable.Columns.Add("MOBILE_NO", typeof(string));
                userTable.Columns.Add("EMAIL", typeof(string));

                DataRow drUser = null;
                if (User != null)
                {
                    foreach (var usr in User)
                    {
                        drUser = userTable.NewRow();
                        drUser["NAME"] = usr.Name;
                        drUser["MOBILE_NO"] = usr.Mobile_No;
                        drUser["EMAIL"] = usr.Email;

                        userTable.Rows.Add(drUser);
                    }
                }
                else
                {
                    userTable = null;
                }
                var str = (_loginDB.Update_Profile(configTable,processTable,userTable)).Split(',');
                if (str[0]=="success")
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
        //This method is used for dipalying user menu
        [Route("api/get_menu")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult Menu(int User_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<MenuModel> Mobile = new List<MenuModel>();
            List<MenuModel> Web = new List<MenuModel>();
            try
            {
                var dt = _loginDB.GetMenu(User_Id);
                if (dt.Rows.Count > 0)
                {
                    int moduleid = 0,moduleid1 = 0 ;

                    foreach (DataRow dr in dt.Select("M_ANDROID_ENTRY = 1"))
                    {
                        MenuModel mm = new MenuModel();

                        if (moduleid != Convert.ToInt32(dr["MODULE_ID"].ToString()))
                        {
                            moduleid = Convert.ToInt32(dr["MODULE_ID"].ToString());

                            mm.module_name = dr["MODULE_NAME"].ToString();
                            mm.module_url = dr["MODULE_URL"].ToString();
                            mm.module_class = dr["MODULE_CLASS"].ToString();

                            List<SubMenuModel> submenulist = new List<SubMenuModel>();
                            string sortOrder = "ORDER_NO ASC";
                            foreach (DataRow dr1 in dt.Select("MODULE_ID = " + moduleid + " AND ANDROID_ENTRY=1 AND SUB_MENU = 1", sortOrder))
                            {
                                SubMenuModel sm = new SubMenuModel();

                                sm.submenu = dr1["PAGE_NAME"].ToString();
                                sm.url = dr1["PAGE_URL"].ToString();
                                if (dr1["PAGE_NAME"].ToString() != "")
                                {
                                    submenulist.Add(sm);
                                }
                            }
                            mm.menu = submenulist;

                            Mobile.Add(mm);
                        }
                    }

                    foreach (DataRow dr in dt.Select("M_WEB_ENTRY = 1"))
                    {
                        MenuModel mm = new MenuModel();

                        if (moduleid1 != Convert.ToInt32(dr["MODULE_ID"].ToString()))
                        {
                            moduleid1 = Convert.ToInt32(dr["MODULE_ID"].ToString());

                            mm.module_name = dr["MODULE_NAME"].ToString();
                            mm.module_url = dr["MODULE_URL"].ToString();
                            mm.module_class = dr["MODULE_CLASS"].ToString();

                            List<SubMenuModel> submenulist = new List<SubMenuModel>();
                            string sortOrder = "ORDER_NO ASC";
                            foreach (DataRow dr1 in dt.Select("MODULE_ID = " + moduleid1 + " AND SUB_MENU = 1", sortOrder))
                            {
                                SubMenuModel sm = new SubMenuModel();

                                sm.submenu = dr1["PAGE_NAME"].ToString();
                                sm.url = dr1["PAGE_URL"].ToString();
                                if (dr1["PAGE_NAME"].ToString() != "")
                                {
                                    submenulist.Add(sm);
                                }
                            }
                            mm.menu = submenulist;

                            Web.Add(mm);
                        }
                    }

                    obj.Status = "success";
                    obj.Message = "menu data";
                    obj.Data = new { Mobile, Web};
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
        #endregion

        #region User Plan/Billing Infomation Insert
        //this method is used for saved the plan information
        [Route("api/insert_plan")]
        [RESTAuthorizeAttribute]
        [HttpPost]
        public IActionResult Insert_Plan(PLAN_MODEL _plan)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                var plans = _plan.plan;
                var description = _plan.plan_descriptions;

                DataTable plan = new DataTable("TBL_PLAN");
                plan.Columns.Add("PLAN_ID", typeof(string));
                plan.Columns.Add("PLAN_ENTITY", typeof(string));
                plan.Columns.Add("PLAN_INTERVAL", typeof(Int32));
                plan.Columns.Add("PLAN_PERIOD", typeof(string));
                plan.Columns.Add("STATUS", typeof(string));

                DataRow dr = null;
                foreach (var pr in plans)
                {
                    dr = plan.NewRow();
                    dr["PLAN_ID"] = pr.plan_id;
                    dr["PLAN_ENTITY"] = pr.entity;
                    dr["PLAN_INTERVAL"] = pr.interval;
                    dr["PLAN_PERIOD"] = pr.period;
                    dr["STATUS"] = pr.status;

                    plan.Rows.Add(dr);
                }

                DataTable plan_desc = new DataTable("TBL_PLAN_DESCRIPTION");
                plan_desc.Columns.Add("ID", typeof(string));
                plan_desc.Columns.Add("PLAN_ID", typeof(string));
                plan_desc.Columns.Add("NAME", typeof(string));
                plan_desc.Columns.Add("DESCRIPTION", typeof(string));
                plan_desc.Columns.Add("AMOUNT", typeof(string));
                plan_desc.Columns.Add("UNIT_AMOUNT", typeof(string));
                plan_desc.Columns.Add("CURRENCY", typeof(string));
                plan_desc.Columns.Add("TYPE", typeof(string));
                plan_desc.Columns.Add("UNIT", typeof(string));
                plan_desc.Columns.Add("TAX_INCLUSIVE", typeof(Int32));
                plan_desc.Columns.Add("HSN_CODE", typeof(string));
                plan_desc.Columns.Add("SAC_CODE", typeof(string));
                plan_desc.Columns.Add("TAX_RATE", typeof(string));
                plan_desc.Columns.Add("TAX_ID", typeof(string));
                plan_desc.Columns.Add("TAX_GROUP_ID", typeof(string));
                plan_desc.Columns.Add("CREATED_AT", typeof(string));
                plan_desc.Columns.Add("UPDATED_AT", typeof(string));
                plan_desc.Columns.Add("STATUS", typeof(string));

                DataRow dr1 = null;
                foreach (var pr1 in description)
                {
                    dr1 = plan_desc.NewRow();
                    dr1["ID"] = pr1.id;
                    dr1["PLAN_ID"] = pr1.plan_id;
                    dr1["NAME"] = pr1.name;
                    dr1["DESCRIPTION"] = pr1.description;
                    dr1["AMOUNT"] = pr1.amount;
                    dr1["UNIT_AMOUNT"] = pr1.unit_amount;
                    dr1["CURRENCY"] = pr1.currency;
                    dr1["TYPE"] = pr1.type;
                    dr1["UNIT"] = pr1.unit;
                    dr1["TAX_INCLUSIVE"] = pr1.tax_inclusive;
                    dr1["HSN_CODE"] = pr1.hsn_code;
                    dr1["SAC_CODE"] = pr1.sac_code;
                    dr1["TAX_RATE"] = pr1.tax_rate;
                    dr1["TAX_ID"] = pr1.tax_id;
                    dr1["TAX_GROUP_ID"] = pr1.tax_group_id;
                    dr1["CREATED_AT"] = pr1.created_at;
                    dr1["UPDATED_AT"] = pr1.updated_at;
                    dr1["STATUS"] = pr1.status;

                    plan_desc.Rows.Add(dr1);
                }

                string[] response = (_loginDB.insert_plan(plan, plan_desc)).Split(',');
                if (response[0] == "success")
                {
                    obj.Status = "success";
                    obj.Message = response[1];
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = response[1];
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
        //this method is used for saved the billing information
        [Route("api/insert_billing_info")]
        [RESTAuthorizeAttribute]
        [HttpPost]
        public IActionResult Billing_Info(BillingInfoModel bm, string plan_type)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                DataTable billing_info = new DataTable("TBL_BILLING_INFO");
                billing_info.Columns.Add("USER_ID", typeof(int));
                billing_info.Columns.Add("FULL_NAME", typeof(string));
                billing_info.Columns.Add("EMAIL", typeof(string));
                billing_info.Columns.Add("PHONE_NO", typeof(string));
                billing_info.Columns.Add("ADDRESS", typeof(string));
                billing_info.Columns.Add("ZIP_CODE", typeof(int));
                billing_info.Columns.Add("COUNTRY", typeof(string));

                DataRow dr = null;
                dr = billing_info.NewRow();
                dr["USER_ID"] = bm.USER_ID;
                dr["FULL_NAME"] = bm.FULL_NAME;
                dr["EMAIL"] = bm.EMAIL;
                dr["PHONE_NO"] = bm.PHONE_NO;
                dr["ADDRESS"] = bm.ADDRESS;
                dr["ZIP_CODE"] = bm.ZIP_CODE;
                dr["COUNTRY"] = bm.COUNTRY;
                billing_info.Rows.Add(dr);

                string[] str = (_loginDB.Insert_Billing_Info(billing_info, plan_type)).Split(',');
                //if (str[0] == "success")
                //{
                //    string billing_info_id = str[1];
                //    obj.Status = "success";
                //    obj.Message = "successfully adding billing information.";
                //    obj.Data = new { billing_info_id };
                //}
                //else
                //{
                //    obj.Status = "failure";
                //    obj.Message = str[1];
                //    obj.Data = new { };
                //}
                // new code
                var resStr = str[0].Split('~');
                if (resStr[0] == "success")
                {
                    string billing_info_id = str[1];
                    obj.Status = "success";
                    obj.Message = resStr[1];
                    obj.Data = new { billing_info_id };
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
        //this method is used for saved the payment information
        [Route("api/insert_payment_details")]
        [RESTAuthorizeAttribute]
        [HttpPost]
        public IActionResult Insert_payment_details(Payment_Details pd)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (pd.Emp_Id == 0)
                {
                    if (pd.user_name == null || pd.user_name == "null")
                    {
                        obj.Status = "failure";
                        obj.Message = "First & last name can't be blank.";
                        obj.Data = new { };
                        return Ok(obj);
                    }

                    if (pd.phone_no == null || pd.phone_no == "null")
                    {
                        obj.Status = "failure";
                        obj.Message = "Phone no can't be blank.";
                        obj.Data = new { };
                        return Ok(obj);
                    }
                }

                if (pd.billing_info_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Billing info id can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                if (pd.Plan_Id == "null" || pd.Plan_Id == null)
                {
                    obj.Status = "failure";
                    obj.Message = "Plan id can not be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                if ((pd.Plan_Type).ToLower() == "paid")
                {
                    if (pd.Payment_Amount == 0)
                    {
                        obj.Status = "failure";
                        obj.Message = "Payment amount can not be zero.";
                        obj.Data = new { };
                        return Ok(obj);
                    }
                    //if (pd.Transaction_Id == null)
                    //{
                    //    obj.Status = "failure";
                    //    obj.Message = "Payment transaction id can not be blank.";
                    //    obj.Data = new { };
                    //    return Ok(obj);
                    //}
                    if (pd.Payment_Status == null)
                    {
                        obj.Status = "failure";
                        obj.Message = "Payment status can not be blank.";
                        obj.Data = new { };
                        return Ok(obj);
                    }
                    if (pd.razor_pay_invoice_no == null || pd.razor_pay_invoice_no == "null")
                    {
                        obj.Status = "failure";
                        obj.Message = "Invoice no can not be blank.";
                        obj.Data = new { };
                        return Ok(obj);
                    }
                    if (pd.invoice_url == null || pd.invoice_url == "null")
                    {
                        obj.Status = "failure";
                        obj.Message = "Invoice url can not be blank.";
                        obj.Data = new { };
                        return Ok(obj);
                    }
                }

                if (pd.licence == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Licence can not be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                if (pd.frequency == null || pd.frequency == "null")
                {
                    obj.Status = "failure";
                    obj.Message = "Frequency can not be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                string res = _loginDB.Insert_Payment_details(pd);

                if (res == "success")
                {
                    if (pd.Emp_Id == 0)
                    {
                        string msg = "Dear " + pd.user_name + ", Thank you for successfully registering with us.Your default passwod for accessing the NAVFARM Web and Mobile application is navfarm@1234      .Navfarm";

                        SendSms(pd.phone_no, msg);
                    }

                    obj.Status = "success";
                    obj.Message = "successfully insert payment details.";
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

        #region Subscription
        //this method is used for cancel user subscription
        [Route("api/subscription_cancel")]
        [RESTAuthorizeAttribute]
        [HttpPost]
        public IActionResult Subscription_Cancellation(Subscription_CancelModel sc)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (sc.user_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "User details can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                if (sc.subscription_id == null || sc.subscription_id == "null")
                {
                    obj.Status = "failure";
                    obj.Message = "Subscription details can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (sc.subscription_status == null || sc.subscription_status == "null")
                {
                    obj.Status = "failure";
                    obj.Message = "Cancellation details can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (sc.cancel_date == null)
                {
                    obj.Status = "failure";
                    obj.Message = "Cancellation details can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                string[] res = (_loginDB.Subscription_cancelation(sc)).Split('?');

                if (res[0] == "success")
                {
                    if(res[3]!="")
                    {
                        string SENDER_MAIL_TEXT = configuration.GetSection("Email_Config").GetSection("SENDER_MAIL_TEXT").Value;
                        string SENDER_EMAIL = configuration.GetSection("Email_Config").GetSection("SENDER_EMAIL").Value;
                        string IsDefaultCredentials = configuration.GetSection("Email_Config").GetSection("IsDefaultCredentials").Value.ToString();
                        string EnableSSL_Credentials = configuration.GetSection("Email_Config").GetSection("EnableSSL_Credentials").Value.ToString();
                        string MAIL_PASSWORD = configuration.GetSection("Email_Config").GetSection("MAIL_PASSWORD").Value;
                        int SMTPPort = Convert.ToInt32(configuration.GetSection("Email_Config").GetSection("SMTPPort").Value);
                        string SMTPServer = configuration.GetSection("Email_Config").GetSection("SMTPServer").Value;
                        string TEMPLATE_PATH = configuration.GetSection("Email_Config").GetSection("TEMPLATE_PATH").Value;

                        string mail_response = EmailService.Send_Email_Cancellation(res[2], res[3], res[4], TEMPLATE_PATH, SENDER_EMAIL, MAIL_PASSWORD, SENDER_MAIL_TEXT, SMTPServer, SMTPPort, IsDefaultCredentials, EnableSSL_Credentials);
                        if(mail_response == "Sent")
                        {
                            _loginDB.Insert_Email_Logs(sc.user_id, res[3], "Subscription Cancelation", sc.subscription_id,mail_response,"Success");
                        }
                        else
                        {
                            _loginDB.Insert_Email_Logs(sc.user_id, res[3], "Subscription Cancelation", sc.subscription_id,mail_response, "Failed");
                        }
                    }

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
        //this method is used for update user subscription
        [Route("api/update_subscription")]
        [RESTAuthorizeAttribute]
        [HttpPost]
        public IActionResult Update_Subscription(Subscription_UpdateModel sm)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (sm.user_id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "User details can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (sm.plan_id == null|| sm.plan_id == "null")
                {
                    obj.Status = "failure";
                    obj.Message = "Plan details can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                if (sm.subscription_id == null || sm.subscription_id == "null")
                {
                    obj.Status = "failure";
                    obj.Message = "Subscription details can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (sm.quantity == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "Subscription updation details can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                
                string[] res = (_loginDB.Subscription_Upgrade(sm)).Split(',');

                if (res[0] == "success")
                {
                    string upgrade_response = RazorPay_UpgradeSubscription(sm.subscription_id, sm.plan_id, sm.quantity);
                    if (upgrade_response == "success")
                    {
                        string update_payment_response = Update_Payment_Details(sm.subscription_id);
                        if (res[3] != "")
                        {
                            string SENDER_MAIL_TEXT = configuration.GetSection("Email_Config").GetSection("SENDER_MAIL_TEXT").Value;
                            string SENDER_EMAIL = configuration.GetSection("Email_Config").GetSection("SENDER_EMAIL").Value;
                            string IsDefaultCredentials = configuration.GetSection("Email_Config").GetSection("IsDefaultCredentials").Value.ToString();
                            string EnableSSL_Credentials = configuration.GetSection("Email_Config").GetSection("EnableSSL_Credentials").Value.ToString();
                            string MAIL_PASSWORD = configuration.GetSection("Email_Config").GetSection("MAIL_PASSWORD").Value;
                            int SMTPPort = Convert.ToInt32(configuration.GetSection("Email_Config").GetSection("SMTPPort").Value);
                            string SMTPServer = configuration.GetSection("Email_Config").GetSection("SMTPServer").Value;
                            string TEMPLATE_PATH = configuration.GetSection("Email_Config").GetSection("TEMPLATE_PATH").Value;

                            string mail_response = EmailService.Send_Email_Upgradation(res[2], res[3], res[4], TEMPLATE_PATH, SENDER_EMAIL, MAIL_PASSWORD, SENDER_MAIL_TEXT, SMTPServer, SMTPPort, IsDefaultCredentials, EnableSSL_Credentials);
                            if (mail_response == "Sent")
                            {
                                _loginDB.Insert_Email_Logs(sm.user_id, res[3], "Subscription Upgradation", sm.subscription_id, mail_response, "Success");
                            }
                            else
                            {
                                _loginDB.Insert_Email_Logs(sm.user_id, res[3], "Subscription Upgradation", sm.subscription_id, mail_response, "Failed");
                            }
                        }
                        obj.Status = "success";
                        obj.Message = res[1];
                        obj.Data = new { };
                    }
                    else
                    {
                        obj.Status = "success";
                        obj.Message = res[1];
                        obj.Data = new { };
                    }                    
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

        #region Common Api's
        //this method is used for validate mobile no.
        [Route("api/validate_mobile")]
        [HttpGet]
        public IActionResult Validate_MobileNo(string Mobile_No)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                var res = _loginDB.Validate_Mobile_No(Mobile_No);
                if (res == "success")
                {
                    obj.Status = "success";
                    obj.Message = "";
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
        //this method is used for displaying support reason
        [Route("api/support_reason")]
        [HttpGet]
        public IActionResult Support_Reason()
        {
            ResponseModel obj = new ResponseModel();
            List<SelectListItem> Support_Reasons = new List<SelectListItem>();
            try
            {
                var dt = _loginDB.Get_Support_Reason();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Support_Reasons.Add(new SelectListItem { Text = @dr["REASON"].ToString(), Value = @dr["REASON_ID"].ToString() });
                    }

                    obj.Status = "success";
                    obj.Message = "Support reason data.";
                    obj.Data = new { Support_Reasons };
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
        //this method is used for fetching details
        [Route("api/get_common_details")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult Common_details(int Company_Id,int User_Id)
        {
            ResponseModel obj = new ResponseModel();
            List<Common_Details_Model> common_details = new List<Common_Details_Model>();
            try
            {
                var dt = _loginDB.Get_Billing_Info(Company_Id,User_Id);
                if (dt.Rows.Count > 0)
                {
                    Common_Details_Model cd = new Common_Details_Model();
                    cd.SUBSCRIPTION_ID = dt.Rows[0][0].ToString();
                    cd.BILLING_INFO_ID = dt.Rows[0][1].ToString();
                    cd.BILLING_USER_NAME = dt.Rows[0][2].ToString();
                    cd.BILLING_USER_EMAIL = dt.Rows[0][3].ToString();
                    cd.BILLING_USER_PHONE_NO = dt.Rows[0][4].ToString();
                    cd.FREQUENCY = dt.Rows[0][5].ToString();
                    cd.LICENCE = Convert.ToInt32(dt.Rows[0][6].ToString());
                    cd.PLAN_ID = dt.Rows[0][7].ToString();
                    cd.PLAN_TYPE = dt.Rows[0][8].ToString();
                    cd.NATURE_ID = dt.Rows[0][9].ToString();
                    cd.NATURE_NAME = dt.Rows[0][10].ToString();
                    cd.CREATED_AT = dt.Rows[0][11].ToString();
                    cd.CHARGE_AT = dt.Rows[0][12].ToString();
                    cd.PLAN_PRICE = dt.Rows[0][13].ToString();
                    cd.TOTAL_PRICE = dt.Rows[0][14].ToString();
                    cd.SUBSCRIPTION_STATUS = dt.Rows[0][15].ToString();
                    cd.USER_STATUS = dt.Rows[0][16].ToString();
                    cd.ROLE = Convert.ToInt32(dt.Rows[0][17].ToString());
                    cd.LOCATION_ID = dt.Rows[0][18].ToString();
                    cd.LOCATION_PENDING = Convert.ToInt32(dt.Rows[0][19].ToString());
                    cd.FARMER_TYPE = dt.Rows[0][20].ToString();
                    cd.islocdefault = Convert.ToInt32(dt.Rows[0]["isLocDefault"]);
                    cd.SUB_LOCATION_ID = Convert.ToInt32(dt.Rows[0]["SUB_LOCATION_ID"]);
                    cd.Currency = dt.Rows[0]["Currency"].ToString();
                    cd.Status = dt.Rows[0]["Status"].ToString();
                    cd.WHATSAPP_NO = dt.Rows[0]["WHATSAPP_NO"].ToString();
                    common_details.Add(cd);

                    if(cd.ROLE == 1 && cd.LOCATION_ID =="1")
                    {
                        obj.Status = "success";
                        obj.Message = "Common details data.";
                        obj.Data = new { common_details };
                    }
                    else if (cd.ROLE == 2 && cd.LOCATION_ID != "1")
                    {
                        obj.Status = "success";
                        obj.Message = "Common details data.";
                        obj.Data = new { common_details };
                    }
                    else
                    {
                        obj.Status = "success";
                        obj.Message = "Contact Administrator to set up the location.";
                        obj.Data = new { common_details };
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
        //this method is used for fetching user details
        [Route("api/get_user_details_bytoken")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult UserDetails_ByToken(string Token)
        {
            ResponseModel obj = new ResponseModel();
            List<LogIn> login = new List<LogIn>();
            try
            {
                var dt = _loginDB.UserDetails_ByToken(Token);
                if (dt.Rows.Count > 0)
                {
                    LogIn lg = new LogIn();
                    lg.USER_ID = Convert.ToInt32(dt.Rows[0][0].ToString());
                    lg.NAME = dt.Rows[0][1].ToString();
                    lg.EMAIL = dt.Rows[0][2].ToString();
                    lg.MOBILE_NO = dt.Rows[0][3].ToString();
                    lg.COMPANY_ID = Convert.ToInt32(dt.Rows[0][4].ToString());
                    lg.PROFILE_PIC = dt.Rows[0][5].ToString();
                    lg.CONFIGURATION_SET = dt.Rows[0][6].ToString();
                    lg.PLAN_TAKEN = dt.Rows[0][7].ToString();
                    lg.LICENCE = Convert.ToInt32(dt.Rows[0][8].ToString());
                    lg.OTP = "";
                    lg.TOKEN = Token;
                    
                    login.Add(lg);

                    obj.Status = "success";
                    obj.Message = "User details data.";
                    obj.Data = new { login };
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
        [Route("api/payment_enabled_disabled")]
        [HttpGet]
        public IActionResult Payment_Enabled_Disabled()
        {
            ResponseModel obj = new ResponseModel();
            try
            {

                int flag = 1;
                var vall = _loginDB.Enable_Disable();
                if (vall.Tables[0].Rows.Count > 0)
                {
                    flag = Convert.ToInt32(vall.Tables[0].Rows[0][0]);
                    obj.Status = "success";
                    obj.Message = "Flag Enable Disable.";
                    obj.Data = new { flag };
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

        ////New Dashboard locatin wise running cost pie chart
        [Route("api/get_dashboard_location_running_cost_graph")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult dashboard_location_running_cost_graph(int Company_Id ,int User_ID,int nature_id)
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
                var ds = _loginDB.Get_Dashboard_locationWiseRunningCostGraph(Company_Id, User_ID, nature_id);

                List<ReportValueData> values = new List<ReportValueData>();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(ds.Tables[0]);
                    obj.Status = "success";
                    obj.Message = "get data.";
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

        [Route("api/get_dashboard_location_output_graph")]
        [RESTAuthorizeAttribute]
        [HttpGet]
        public IActionResult dashboard_location_output_graph(int Company_Id ,int User_ID,int nature_id)
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
                var ds = _loginDB.Get_Dashboard_locationWiseOutputGraph(Company_Id, User_ID, nature_id);

                List<ReportValueData> values = new List<ReportValueData>();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var result = JsonConvert.SerializeObject(ds.Tables[0]);
                    obj.Status = "success";
                    obj.Message = "get data.";
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
        #endregion

        #region Common Function For SMS
        //This method is used for sending sms on mobile no
        private string SendSms(string mobile_no,string message)
        {
            string resp = "";
            try
            {
                string url = AesOperation.DecryptString(configuration.GetSection("SmsApi").GetSection("Url").Value) + mobile_no + "&route=INT&message=" + message + "";

                //HTTP connection
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
                //Get response from Ozeki NG SMS Gateway Server and read the answer
                HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
                System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
                var res = (respStreamReader.ReadToEnd()).Split('|');
                respStreamReader.Close();
                myResp.Close();
                resp = res[0];
            }
            catch (Exception ex)
            {
                resp = ex.Message;
            }           
            return resp;
        }
        //this method is used sending otp for login
        private string OtpForLogin(string mobile_no, string otp)
        {
            string resp = "";
            try
            {
                string msg = "Hi "+mobile_no+", Your OTP for NAVFARM application and portal is "+otp+". Please enter the same to complete your sign in process. Navfarm";
                string url = AesOperation.DecryptString(configuration.GetSection("SmsApi").GetSection("Url").Value) + mobile_no + "&route=INT&message=" + msg + "";

                //HTTP connection
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
                //Get response from Ozeki NG SMS Gateway Server and read the answer
                HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
                System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
                var res = (respStreamReader.ReadToEnd()).Split('|');
                respStreamReader.Close();
                myResp.Close();
                resp = res[0];
            }
            catch (Exception ex)
            {
                resp = ex.Message;
            }
            return resp;
        }
        #endregion

        #region RazorPay Subscription Function
        //this method is used for upgrade subscription
        private string RazorPay_UpgradeSubscription(string Subscription_Id,string Plan_Id,int Qty)
        {
            string response = "";
            try
            {
                string base_url = configuration.GetSection("RazorPay").GetSection("Base_Url").Value;
                string serviceUserName = configuration.GetSection("RazorPay").GetSection("User_Name").Value;
                string servicePassword = configuration.GetSection("RazorPay").GetSection("Password").Value;

                Subscription_UpgradeBody sb = new Subscription_UpgradeBody();
                sb.plan_id = Plan_Id;
                sb.quantity = Qty;
                sb.remaining_count = 40;
                sb.schedule_change_at = "now";
                sb.customer_notify = 1;

                var json = JsonConvert.SerializeObject(sb);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var authToken = Encoding.ASCII.GetBytes($"{serviceUserName}:{servicePassword}");
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(base_url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(authToken));
                    //Get Method  
                    var responseTask = client.PatchAsync("subscriptions/" + Subscription_Id,data);
                    responseTask.Wait();
                    if (responseTask.Result.IsSuccessStatusCode)
                    {
                        var nobresult = responseTask.Result.Content.ReadAsStringAsync().Result;
                        dynamic getRess = JsonConvert.DeserializeObject<dynamic>(nobresult);

                        response = "success";
                    }
                }
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return response;
        }
        //this method is used for update payment details
        private string Update_Payment_Details(string Subscription_Id)
        {
            string response = "";
            try
            {
                int totalCount = 0, successCount = 0, failureCount = 0;
                string plan_id = "", status = "";
                long start_date = 0, end_date = 0;
                DataTable dt = _loginDB.GetRazorPay_Invoices(Subscription_Id);

                if (dt.Rows.Count > 0)
                {
                    string subscription_invoices = configuration.GetSection("RazorPay").GetSection("Base_Url").Value;
                    string serviceUserName = configuration.GetSection("RazorPay").GetSection("User_Name").Value;
                    string servicePassword = configuration.GetSection("RazorPay").GetSection("Password").Value;

                    foreach (DataRow row in dt.Rows)
                    {
                        var authToken = Encoding.ASCII.GetBytes($"{serviceUserName}:{servicePassword}");
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(subscription_invoices);
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                            Convert.ToBase64String(authToken));
                            //Get Method  
                            var responseTask = client.GetAsync("subscriptions/" + row["SUBSCRIPTION_ID"].ToString());
                            responseTask.Wait();
                            if (responseTask.Result.IsSuccessStatusCode)
                            {
                                var nobresult = responseTask.Result.Content.ReadAsStringAsync().Result;
                                dynamic getRess = JsonConvert.DeserializeObject<dynamic>(nobresult);
                                if (getRess.status == "active")
                                {
                                    plan_id = getRess.plan_id;
                                    start_date = getRess.start_at;
                                    end_date = getRess.charge_at;
                                    status = getRess.status;
                                }
                            }
                        }
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(subscription_invoices);
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                            Convert.ToBase64String(authToken));
                            //Get Method  
                            var responseTask = client.GetAsync("invoices?subscription_id=" + row["SUBSCRIPTION_ID"].ToString());
                            responseTask.Wait();
                            if (responseTask.Result.IsSuccessStatusCode)
                            {
                                totalCount += 1;
                                var nobresult = responseTask.Result.Content.ReadAsStringAsync().Result;
                                dynamic getRess = JsonConvert.DeserializeObject<Invoice_Details_Model>(nobresult);

                                int user_id = Convert.ToInt32(row["USER_ID"].ToString());
                                string subscription_id = row["SUBSCRIPTION_ID"].ToString();
                                string frequency = row["FREQUENCY"].ToString();
                                int company_id = Convert.ToInt32(row["COMPANY_ID"].ToString());
                                int billing_id = Convert.ToInt32(row["BILLING_INFO_ID"].ToString());
                                int licence = Convert.ToInt32(getRess.items[0].line_items[0].quantity);
                                decimal payment_amount = ((decimal)(getRess.items[0].gross_amount) / (decimal)100);
                                string transaction_id = getRess.items[0].payment_id;
                                long paid_at = getRess.items[0].paid_at;
                                string email = getRess.items[0].customer_details.customer_email;
                                string phone_no = getRess.items[0].customer_details.customer_contact;
                                string invoice_no = getRess.items[0].id;
                                string invoice_url = getRess.items[0].short_url;
                                string currency = getRess.items[0].currency;

                                string res = _loginDB.UpdateSubscriptionByInvoiceDetails(user_id, payment_amount, plan_id, transaction_id, paid_at, company_id, licence, frequency, email, phone_no,
                                      billing_id, subscription_id, status, start_date, end_date, invoice_no, invoice_url,currency   );

                                if (res == "suucess")
                                {
                                    successCount += 1;
                                }
                            }
                            else
                            {
                                failureCount += 1;
                            }
                        }
                    }
                    response = "success";
                }
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return response;
        }
        #endregion
    }
}
