using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmIT_Api.Models
{
    public class SubscriptionPlanModel
    {
        public int Plan_Id { set; get; }
        public string Plan_Name { set; get; }
        public string Plan_Type { set; get; }
        public string Plan_Details { set; get; }
        public string Button_Name { set; get; }
        //public List<Plan_Feature> Features { get; set; }
    }
    public class RazorPay_Plan_Model
    {
        //public string title { get; set; }
        //public string sub_title { get; set; }
        public string trial_period { get; set; }
        public string plan_start_date { get; set; }
        public string trial_end_date { get; set; }
        public object plans { get; set; }
    }
    public class RazorPay_Plan: RazorPay_Plan_Model
    {
        public string plan_id { get; set; }
        public string plan_description { get; set; }
        public string plan_duration { get; set; }
        public string plan_price { get; set; }
        public string current_timestamp { get; set; }
        public string base_price { get; set; }
        public string discount { get; set; }
        public string currency { get; set; }
        public string status { get; set; }
    }
    public class Plan_Feature
    {
        public int Feature_Id { set; get; }
        public string Feature_Name { set; get; }
    }
    public class Process_Configuration_Model
    {
        public Configuration_Model configuration_setup { get; set; }
        public IEnumerable<Process_Model> process_setup { get; set; }
        public IEnumerable<User_Model> user_setup { get; set; }
    }
    public class Configuration_Model
    {
        public int COMPANY_ID { get; set; }
        public int LICENSE { get; set; }
        public int SHARE_WITH_TEAM { get; set; }
        public string NAME { get; set; }
        public byte[] COMPANY_LOGO { get; set; }
        public string MOBILE_NO { get; set; }
        public string EMAIL { get; set; }
        public string ADDRESS { get; set; }
        public string TAX_NUM { get; set; }
        public string SOCIAL_ID { get; set; }
        public int CREATED_BY { get; set; }
        public int isDefault { get; set; }
    }
    public class Process_Model
    {
        public int NATURE_ID { set; get; }
        public int LINE_ID { set; get; }
        public string FARMER_TYPE { set; get; }
        public int isDefault { get; set; }
    }
    public class User_Model
    {
        public int USER_ID { get; set; }
        public string Name { set; get; }
        public string Mobile_No { set; get; }
        public string Email { set; get; }
        public string STATUS { set; get; }
        public int COMPANY_ID { get; set; }
    }
    public class Payment_Details
    {
        public int Emp_Id { get; set; }
        public decimal Payment_Amount { get; set; }
        public string Plan_Id { get; set; }
        public string Plan_Type { get; set; }
        public string Transaction_Id { get; set; }
        public DateTime Payment_Date { get; set; }
        public string Payment_Status { get; set; }
        public int licence { get; set; }
        public string frequency { get; set; }
        public string user_name { get; set; }
        public string email { get; set; }
        public string phone_no { get; set; }
        public int billing_info_id { get; set; }
        public string subscription_id { get; set; }
        public string razor_pay_invoice_no { get; set; }
        public string invoice_url { get; set; }
        //public long subscription_start_date { get; set; }
        //public long subscription_end_date { get; set; }
    }
    public class LOB_Model
    {
        public string text { get; set; }
        public string value { get; set; }
        public bool selected { get; set; }
        public string nature_id { get; set; }
    }
    public class Parameter_Model
    {
        public string text { get; set; }
        public string value { get; set; }
        public bool selected { get; set; }
        public string formula_flag { get; set; }
        public string parameter_id { get; set; }
        public string lob_id { get; set; }
        public string livestock_flag { get; set; }
    }
    public class Subscription_CancelModel
    {
        public int user_id { get; set; }
        public string subscription_id { get; set; }
        public string subscription_status { get; set; }
        public DateTime cancel_date { get; set; }
    }
    public class Subscription_UpdateModel
    {
        public int user_id { get; set; }
        public string plan_id { get; set; }
        public string subscription_id { get; set; }
        public int quantity { get; set; }
    }
    public class Subscription_UpgradeBody
    {
        public string plan_id { get; set; }
        public int quantity { get; set; }
        public int remaining_count { get; set; }
        public string schedule_change_at { get; set; }
        public int customer_notify { get; set; }
    }
    public class Subscription_Body
    {
        public int cancel_at_cycle_end { get; set; }
    }
    public class Invoice_Details_Model
    {
        public string entity { get; set; }
        public int count { get; set; }
        public Item_Details[] items { get; set; }
    }
    public class Item_Details
    {
        public string id { get; set; }
        public string entity { get; set; }
        public object receipt { get; set; }
        public object invoice_number { get; set; }
        public string customer_id { get; set; }
        public Customer_Details customer_details { get; set; }
        public string order_id { get; set; }
        public string subscription_id { get; set; }
        public Line_Items[] line_items { get; set; }
        public string payment_id { get; set; }
        public string status { get; set; }
        public int? expire_by { get; set; }
        public int issued_at { get; set; }
        public int paid_at { get; set; }
        public object cancelled_at { get; set; }
        public object expired_at { get; set; }
        public object sms_status { get; set; }
        public object email_status { get; set; }
        public int date { get; set; }
        public object terms { get; set; }
        public bool partial_payment { get; set; }
        public int gross_amount { get; set; }
        public int tax_amount { get; set; }
        public int taxable_amount { get; set; }
        public int amount { get; set; }
        public int amount_paid { get; set; }
        public int amount_due { get; set; }
        public string currency { get; set; }
        public string currency_symbol { get; set; }
        public object description { get; set; }
        public object notes { get; set; }
        public object comment { get; set; }
        public string short_url { get; set; }
        public bool view_less { get; set; }
        public int? billing_start { get; set; }
        public int? billing_end { get; set; }
        public string type { get; set; }
        public bool group_taxes_discounts { get; set; }
        public int created_at { get; set; }
        public object idempotency_key { get; set; }
    }
    public class Customer_Details
    {
        public string id { get; set; }
        public object name { get; set; }
        public string email { get; set; }
        public string contact { get; set; }
        public object gstin { get; set; }
        public object billing_address { get; set; }
        public object shipping_address { get; set; }
        public object customer_name { get; set; }
        public string customer_email { get; set; }
        public string customer_contact { get; set; }
    }
    public class Line_Items
    {
        public string id { get; set; }
        public object item_id { get; set; }
        public object ref_id { get; set; }
        public object ref_type { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int amount { get; set; }
        public int unit_amount { get; set; }
        public int gross_amount { get; set; }
        public int tax_amount { get; set; }
        public int taxable_amount { get; set; }
        public int net_amount { get; set; }
        public string currency { get; set; }
        public string type { get; set; }
        public bool tax_inclusive { get; set; }
        public object hsn_code { get; set; }
        public object sac_code { get; set; }
        public object tax_rate { get; set; }
        public object unit { get; set; }
        public int quantity { get; set; }
        public object[] taxes { get; set; }
    }
}
