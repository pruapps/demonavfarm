using System.Collections.Generic;

namespace FarmIT_Api.Models
{
    public class LogIn
    {
        public int USER_ID { get; set; }
        public int COMPANY_ID { get; set; }
        public string PLAN_TAKEN { get; set; }
        public string CONFIGURATION_SET { get; set; }
        public string NAME { get; set; }
        public string EMAIL { get; set; }
        public string MOBILE_NO { get; set; }
        public string PROFILE_PIC { get; set; }
        public string OTP { get; set; }
        public string TOKEN { get; set; }
        public int LICENCE { get; set; }
        public string BILLING_EMAIL { get; set; }
        public string BILLING_ADDRESS { get; set; }
    }
    public class SigninRequest
    {
        public string Mobile_No { get; set; }
        public string Password { get; set; }
        public string Social_Id { get; set; }
        public bool Signinwithotp { get; set; }
    }
    public class SignupRequest
    {
        public string User_Name { get; set; }
        public string Mobile_No { get; set; }
        public int IsSocial { get; set; }
        public int Is_FaceBook { get; set; }
        public int Is_Gmail { get; set; }
        public int Is_Apple_id { get; set; }

        public string Social_Id { get; set; }
        public string Social_Name { get; set; }
        public string Social_Email { get; set; }
        public string Social_Profile_Pic { get; set; }
    }
    public class SignOutRequest
    {
        public int User_Id { get; set; }
        public string Token { get; set; }
    }
    public class MenuModel
    {
        public string module_name { get; set; }
        public string module_url { get; set; }
        public string module_class { get; set; }
        public List<SubMenuModel> menu { get; set; }
    }
    public class SubMenuModel
    {
        public string submenu { get; set; }
        public string url { get; set; }
    }
    public class ProfileModel
    {
        public ProfileData profile_data { get; set; }
        public IEnumerable<ActiveDeactiveData> activedeactive_data { get; set; }
        public IEnumerable<User_Model> user_data { get; set; }
    }
    public class ProfileData
    {
        public int config_id { get; set; }
        public int user_id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public byte[] company_logo { get; set; }
    }
    public class ActiveDeactiveData
    {
        public int user_id { get; set; }
        public string status { get; set; }
    }
    public class ChnagePassword
    {
        public string old_password { get; set; }
        public string new_password { get; set; }
        public int user_id { get; set; }
    }
    public class Common_Details_Model
    {
        public string SUBSCRIPTION_ID { get; set; }
        public string BILLING_INFO_ID { get; set; }
        public string BILLING_USER_NAME { get; set; }
        public string BILLING_USER_EMAIL { get; set; }
        public string BILLING_USER_PHONE_NO { get; set; }
        public string FREQUENCY { get; set; }
        public int LICENCE { get; set; }
        public string PLAN_ID { get; set; }
        public string PLAN_TYPE { get; set; }
        public string NATURE_ID { get; set; }
        public string NATURE_NAME { get; set; }
        public string CREATED_AT { get; set; }
        public string CHARGE_AT { get; set; }
        public string PLAN_PRICE { get; set; }
        public string TOTAL_PRICE { get; set; } 
        public string SUBSCRIPTION_STATUS { get; set; }
        public string USER_STATUS { get; set; }
        public int ROLE { get; set; }
        public string LOCATION_ID { get; set; }
        public int LOCATION_PENDING { get; set; }
        public string FARMER_TYPE { get; set; }
        public int islocdefault { get; set; }
        public int SUB_LOCATION_ID { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public string WHATSAPP_NO { get; set; }

    }
}
