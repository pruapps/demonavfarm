using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FarmIT_Api.database_accesslayer;
using Microsoft.AspNetCore.Authorization;
using FarmIT_Api.Models;
using System.Data;

namespace FarmIT_Api.Controllers
{
    [ApiController]
    [Authorize]
    [RESTAuthorizeAttribute]
    public class AdminDashBoardController : ControllerBase
    {
        /// <summary>
        /// this controller contains the information regard user activity , active users, paid and unpaid users details
        /// </summary>
        private readonly IDashBoardDB _dashboardDB;
        public AdminDashBoardController(IDashBoardDB dashboardDB)
        {
            _dashboardDB = dashboardDB;
        }
        #region [User Activity Details]
       
        //This method is used for getting month wise user registration details
        [Route("api/get_month_wise_registration")]
        [HttpGet]
        public IActionResult MONTH_WISE_REGISTRATION()
        {
            ResponseModel obj = new ResponseModel();
            List<DashBoardChart> summarry = new List<DashBoardChart>();
            try
            {
                 DataTable dt = _dashboardDB.MONTH_WISE_REGISTRATION().Tables[0];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DashBoardChart bd = new DashBoardChart();
                        bd.mon = Convert.ToInt32(dr["mon"].ToString());
                        bd.Month_name = dr["Month_name"].ToString();
                        bd.Total_Registration = Convert.ToInt32(dr["Total_Registration"]);
                        summarry.Add(bd);
                    }

                    obj.Status = "success";
                    obj.Message = "Month Wise Registration Data.";
                    obj.Data = summarry;
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
        //This method is used for getting month wise user registration cancellation details
        [Route("api/get_month_wise_cancellation")]
        [HttpGet]
        public IActionResult MONTH_WISE_CANCELLATION()
        {
            ResponseModel obj = new ResponseModel();
            List<DashBoard_CANCELLATION_REFUND> summarry = new List<DashBoard_CANCELLATION_REFUND>();
            try
            {
                DataTable dt = _dashboardDB.MONTH_WISE_CANCELLATION_REFUND().Tables[0];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DashBoard_CANCELLATION_REFUND bd = new DashBoard_CANCELLATION_REFUND();

                        bd.mon = Convert.ToInt32(dr["mon"].ToString());
                        bd.Month_name = dr["Month_name"].ToString();
                        bd.Total_Cancellations = Convert.ToInt32(dr["Total_Cancellations"]);
                        bd.Total_Refunds = Convert.ToInt32(dr["Total_Refunds"]);
                        summarry.Add(bd);
                    }

                    obj.Status = "success";
                    obj.Message = "Month Wise Cancellation Data.";
                    obj.Data = summarry;
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
        //This method is used for getting active and inactive users list
        [Route("api/get_active_inactive_users")]
        [HttpGet]
        public IActionResult Active_Inactive_users()
        {
            ResponseModel obj = new ResponseModel();
            List<DashBoard_ACTIVE_INACTIVE> summarry = new List<DashBoard_ACTIVE_INACTIVE>();
            try
            {
                DataTable dt = _dashboardDB.ACTIVE_INACTIVE_USERS().Tables[0];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DashBoard_ACTIVE_INACTIVE bd = new DashBoard_ACTIVE_INACTIVE();

                        bd.Total_Active_Users = Convert.ToInt32(dr["Total_Active_Users"]);
                        bd.Total_InActive_Users = Convert.ToInt32(dr["Toatl_InActive_Users"]);
                        summarry.Add(bd);
                    }

                    obj.Status = "success";
                    obj.Message = "Active and Inactive Data.";
                    obj.Data = summarry;
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
        //This method is used for getting trail and paid details
        [Route("api/get_trai_paid_users")]
        [HttpGet]
        public IActionResult Trail_Paid_users()
        {
            ResponseModel obj = new ResponseModel();
            List<DashBoard__TRAIL_PAID> summarry = new List<DashBoard__TRAIL_PAID>();
            try
            {
                DataTable dt = _dashboardDB.TRAIL_PAID_USERS().Tables[0];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DashBoard__TRAIL_PAID bd = new DashBoard__TRAIL_PAID();

                        bd.Total_Trail_Users = Convert.ToInt32(dr["Total_Trail_Users"]);
                        bd.Total_Paid_Users = Convert.ToInt32(dr["Total_Paid_Users"]);
                        summarry.Add(bd);
                    }

                    obj.Status = "success";
                    obj.Message = "Trail and Paid Data.";
                    obj.Data = summarry;
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
        //This method is used for getting active and inactive details
        [Route("api/get_active_inactive_users_details")]
        [HttpGet]
        public IActionResult Active_Inactive_users_details()
        {
            ResponseModel obj = new ResponseModel();
            List<DashBoard_ACTIVE_INACTIVE_DETAILS> summarry = new List<DashBoard_ACTIVE_INACTIVE_DETAILS>();
            try
            {
                DataTable dt = _dashboardDB.ACTIVE_INACTIVE_USERS_DETAILS().Tables[0];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DashBoard_ACTIVE_INACTIVE_DETAILS bd = new DashBoard_ACTIVE_INACTIVE_DETAILS();

                        bd.User_ID = Convert.ToInt32(dr["User_ID"]);
                        bd.USER_NAME = Convert.ToString(dr["USER_NAME"]);
                        bd.MOBILE_NO = Convert.ToString(dr["MOBILE_NO"]);
                        bd.EMAIL = Convert.ToString(dr["EMAIL"]);
                        bd.Company_Name = Convert.ToString(dr["Company_Name"]);
                        bd.STATUS = Convert.ToString(dr["STATUS"]);
                        bd.LAST_LOGIN_DATE = Convert.ToString(dr["LAST_LOGIN_DATE"]);
                        summarry.Add(bd);
                    }

                    obj.Status = "success";
                    obj.Message = "Active and Inactive Data.";
                    obj.Data = summarry;
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
        //This method is used for getting trail and paid users details
        [Route("api/get_trai_paid_users_details")]
        [HttpGet]
        public IActionResult Trail_Paid_users_details()
        {
            ResponseModel obj = new ResponseModel();
            List<DashBoard_TRAIL_PAID_DEtails> summarry = new List<DashBoard_TRAIL_PAID_DEtails>();
            try
            {
                DataTable dt = _dashboardDB.TRAIL_PAID_USERS_DETAILS().Tables[0];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DashBoard_TRAIL_PAID_DEtails bd = new DashBoard_TRAIL_PAID_DEtails();

                        bd.USER_NAME = Convert.ToString(dr["USER_NAME"]);
                        bd.PAYMENT_AMOUNT = Convert.ToDecimal(dr["PAYMENT_AMOUNT"]);
                        bd.SUBSCRIPTION_START_DATE = Convert.ToString(dr["SUBSCRIPTION_START_DATE"]);
                        bd.SUBSCRIPTION_END_DATE = Convert.ToString(dr["SUBSCRIPTION_END_DATE"]);
                        bd.Next_Payment_Due_Date = Convert.ToString(dr["Next_Payment_Due_Date"]);
                        bd.REMARK = Convert.ToString(dr["REMARK"]);
                        summarry.Add(bd);
                    }

                    obj.Status = "success";
                    obj.Message = "Trail and Paid Data.";
                    obj.Data = summarry;
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
        //This method is used for getting users details
        [Route("api/get_users_details")]
        [HttpGet]
        public IActionResult Users_Details(int user_id)
        {
            ResponseModel obj = new ResponseModel();
            List<DashBoard_USERS_DETAILS> summarry = new List<DashBoard_USERS_DETAILS>();
            List<User_FeedBack> remarks = new List<User_FeedBack>();
            try
            {
                DataSet ds = _dashboardDB.USERS_DETAILS(user_id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DashBoard_USERS_DETAILS bd = new DashBoard_USERS_DETAILS();

                        bd.User_ID = Convert.ToInt32(dr["User_ID"]);
                        bd.USER_NAME = Convert.ToString(dr["USER_NAME"]);
                        bd.MOBILE_NO = Convert.ToString(dr["MOBILE_NO"]);
                        bd.EMAIL = Convert.ToString(dr["EMAIL"]);
                        bd.Company_Name = Convert.ToString(dr["Company_Name"]);
                        bd.STATUS = Convert.ToString(dr["STATUS"]);
                        bd.PAYMENT_AMOUNT = Convert.ToDecimal(dr["PAYMENT_AMOUNT"]);
                        bd.SUBSCRIPTION_START_DATE = Convert.ToString(dr["SUBSCRIPTION_START_DATE"]);
                        bd.SUBSCRIPTION_END_DATE = Convert.ToString(dr["SUBSCRIPTION_END_DATE"]);
                        bd.Next_Payment_Due_Date = Convert.ToString(dr["Next_Payment_Due_Date"]);
                        bd.REMARK = Convert.ToString(dr["REMARK"]);
                        bd.Comp_Mobile = Convert.ToString(dr["Comp_Mobile"]);
                        bd.Comp_Email = Convert.ToString(dr["Comp_Email"]);
                        bd.PlanName = Convert.ToString(dr["PlanName"]);
                        bd.Plan_description = Convert.ToString(dr["Plan_description"]);
                        bd.tax_num = Convert.ToString(dr["TAX_NUM"]);
                        bd.farmer_type = Convert.ToString(dr["FARMER_TYPE"]);
                        summarry.Add(bd);
                    }
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        User_FeedBack uf = new User_FeedBack();
                        uf.feedback_id = Convert.ToInt32(dr["FEEDBACK_ID"].ToString());
                        uf.remark = dr["REMARK"].ToString();
                        uf.remark_date = dr["REMARK_DATE"].ToString();
                        remarks.Add(uf);
                    }
                    obj.Status = "success";
                    obj.Message = "User details with remarks data.";
                    obj.Data = new { summarry, remarks };
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

        //This method is used for saving user feedback
        [Route("api/insert_feedback")]
        [HttpPost]
        public IActionResult Insert_Feedback(FeedBack fb)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (fb.user_id == 0)
                {
                    obj.Status = "failed";
                    obj.Message = "User details can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                if (fb.remarks == "null" || fb.remarks == null)
                {
                    obj.Status = "failed";
                    obj.Message = "Remarks can't be blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }
                string[] res = _dashboardDB.Insert_FeedBack(fb.user_id,fb.remarks).Split(',');
                if (res[0] == "success")
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
        #endregion

    }
}
