using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmIT_Api.Models
{
    public class DashboardModel
    {
        public int lob_id { get; set; }
        public string Line_Of_Business { get; set; }
        public List<DashboardData> Data { get; set; }
    }
    public class DashboardData
    {
        public object Labels { get; set; }
        public object  Values { get; set; }
        public object Icon_Name { get; set; }
    }
    public class Profile_Data
    {
        public int config_id { get; set; }
        public string company_name { get; set; }
        public string user_name { get; set; }
        public string email { get; set; }
        public string mobile_no { get; set; }
        public string address { get; set; }
        public string tax_num { get; set; }
        public string farmer_type { get; set; }
        public byte[] logo { get; set; }
        public int isDefault { get; set; }
    }
    public class User_Data
    {
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string email { get; set; }
        public string mobile_no { get; set; }
        public string status { get; set; }
    }
    public class User_List:User_Data
    {
        public string company_name { get; set; }
        public int location_id { get; set; }
        public string location_name { get; set; }
        public int role_id { get; set; }
        public string role_name { get; set; }
        public int sub_location_id { get; set; }
        public string sub_location_name { get; set; }
    }
    public class Payment_Data
    {
        public string package_description { get; set; }
        public string price { get; set; }
        public string estimated_tax { get; set; }
        public string order_total { get; set; }
        public string plan_type { get; set; }
        public string plan_start_date { get; set; }
        public string frequency { get; set; }
        public int licence { get; set; }
        public string razor_pay_invoice_no { get; set; }
        public string invoice_url { get; set; }
        public string payment_date { get; set; }
        public string subscription_id { get; set; }
    }
    public class Dashboard_Summary
    {
        public int lob_id { get; set; }
        public string Line_Of_Business { get; set; }
        public int batch_id { get; set; }
        public string batch_name { get; set; }
        public List<DashboardData> Data { get; set; }
    }
    public class Dashboard_Graph_Summary
    {
        public List<DashboardData> Data { get; set; }
    }
    public class Dashboard_Summary_Details
    {
        public string parameter_name { get; set; }
        public decimal actual_value { get; set; }
    }
    public class Dashboard_Summary_Details2
    {
        public string posting_date { get; set; }
        public decimal actual_value { get; set; }
    }
    public class Batch_Data
    {
        public string batch_no { get; set; }
        public string start_date { get; set; }
        public string quantity_kgs { get; set; }
    }
}
