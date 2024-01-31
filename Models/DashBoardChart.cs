using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmIT_Api.Models
{
    public class DashBoardChart
    {
        public int mon { get; set; }
        public string Month_name { get; set; }
        public int Total_Registration { get; set; }
    }

    public class DashBoard_CANCELLATION_REFUND
    {
        public int mon { get; set; }
        public string Month_name { get; set; }
        public int Total_Cancellations { get; set; }
        public int Total_Refunds { get; set; }
    }

    public class DashBoard_ACTIVE_INACTIVE
    {
        public int Total_Active_Users { get; set; }
        public int Total_InActive_Users { get; set; }
    }
    public class DashBoard__TRAIL_PAID
    {
        public int Total_Paid_Users { get; set; }
        public int Total_Trail_Users { get; set; }
    }


    public class DashBoard_ACTIVE_INACTIVE_DETAILS
    {
        public int User_ID { get; set; }
        public string USER_NAME { get; set; }
        public string MOBILE_NO { get; set; }
        public string EMAIL { get; set; }
        public string Company_Name { get; set; }
        public string STATUS { get; set; }
        public string LAST_LOGIN_DATE { get; set; }
    }

    public class DashBoard_TRAIL_PAID_DEtails : DashBoard_ACTIVE_INACTIVE_DETAILS
    {
        //public string USER_NAME { get; set; }
        public decimal PAYMENT_AMOUNT { get; set; }
        public string SUBSCRIPTION_START_DATE { get; set; }
        public string SUBSCRIPTION_END_DATE { get; set; }
        public string REMARK { get; set; }
        public string Next_Payment_Due_Date { get; set; }
    }
    public class DashBoard_USERS_DETAILS : DashBoard_TRAIL_PAID_DEtails
    {
        public string Comp_Mobile { get; set; }
        public string Comp_Email { get; set; }
        public string Plan_description { get; set; }
        public string PlanName { get; set; }
        public string tax_num { get; set; }
        public string farmer_type { get; set; }
    }
    public class FeedBack
    {
        public int user_id { get; set; }
        public string remarks { get; set; }
    }

    public class User_FeedBack
    {
        public int feedback_id { get; set; }
        public string remark { get; set; }
        public string remark_date { get; set; }
    }
}

