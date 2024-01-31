using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmIT_Api.Models
{
    public class Item_Master
    {
        public ItemHeader itemHeader { get; set; }
        public IEnumerable<ItemLine> itemLine { get; set; }
    }
    public class ItemHeader
    {
        public int item_id { get; set; }
        public string item_name { get; set; }
        public string item_category { get; set; }
        public string industry_type { get; set; }
        public int industry_id { get; set; }
        public string uom { get; set; }
        public decimal unit_cost { get; set; }
        public string status { get; set; }
        public int company_id { get; set; }
        public int created_by { get; set; }
        public decimal quantity { get; set; }
        public string isbatchapp { get; set; }
        public string item_type { get; set; }
        public int isNegativeallow { get; set; }
        public int isLobItem { get; set; }
        public string Inventory_type { get; set; }
        public int isERP { get; set; }

        public string INVENTORY_TYPE { get; set; }
    }
    public class ItemLine
    {
        public int Item_sl_id { get; set; }
        public int Item_id { get; set; }
        public string Serial_No { get; set; }
        public int Quantity { get; set; }
    }
    public class UOM_Master
    {
        public int uom_id { get; set; }
        public string uom { get; set; }
        public string uom_type { get; set; }
        public int use_base_uom { get; set; }
        public string base_uom { get; set; }
        public decimal uom_conversion { get; set; }
        public int company_id { get; set; }
        public int created_by { get; set; }
    }
    public class OverHeadCost_Master
    {
        public int expense_id { get; set; }
        public string expense_type { get; set; }
        public string expense_name { get; set; }
        public string industry_type { get; set; }
        public string line_of_business { get; set; }
        public decimal amount { get; set; }
        public string status { get; set; }
        public int company_id { get; set; }
    }
    public class Location_Master
    {
        public int location_id { get; set; }
        public string location_name { get; set; }
        public string primary_location_name { get; set; }
        public string farm_type { get; set; }
        public int farm_type_id { get; set; }
        public string industry_type { get; set; }
        public int industry_id { get; set; }
        public string address { get; set; }
        public int islocdefault { get; set; }
        public int sub_location_id { get; set; }
        public int Primary_location_id { get; set; }
        public string modification_date { get; set; }
        public string status { get; set; }
        public int company_id { get; set; }
        public int created_by { get; set; }
         public string locationtype { get; set; }
        public string area { get; set; }
        public string farmer_name { get; set; }

        public string description { get; set; }
        public string industry_id_list { get; set; }

        public string location_code { get; set; }

        public string location_entry { get; set; }
    }
    public class Breed_Master
    {
        public int breed_id { get; set; }
        public string breed_name { get; set; }
        public string industry_type { get; set; }
        public int industry_id { get; set; }
        public string modification_date { get; set; }
        public string status { get; set; }
        public int company_id { get; set; }
        public int created_by { get; set; }
        public string breed_no { get; set; }
    }
    public class Parameter_Master
    {
        public int parameter_id { get; set; }
        public string parameter_name { get; set; }
        public string parameter_type { get; set; }
        public string industry_type { get; set; }
        public int industry_id { get; set; }
        public int lob_id { get; set; }
        public string line_of_business { get; set; }
        public int parameter_type_id { get; set; }
        public string formula_flag { get; set; }
        public string status { get; set; }
        public int company_id { get; set; }
        public int created_by { get; set; }
        public int livestock_flag { get; set; }
        public string parameter_input_type { get; set; }
        public string parameter_input_format { get; set; }
        public string parameter_no { get; set; }
        public string livestock_type { get; set; }
    }
    public class DataEntryType_Master
    {
        public int dataentry_type_id { get; set; }
        public string dataentry_type { get; set; }
        public string nature_of_business { get; set; }
        public int nature_id { get; set; }
        public string line_of_business { get; set; }
        public int lob_id { get; set; }
        public string uom { get; set; }
        public string alternate_uom { get; set; }
        public string status { get; set; }
        public int company_id { get; set; }
        public int created_by { get; set; }
    }
    public class LOB_Master
    {
        public int lob_id { get; set; }
        public string nature_of_business { get; set; }
        public string line_of_business { get; set; }
        public string status { get; set; }
    }
    public class PLAN_MODEL
    {
        public IEnumerable<Plan> plan { get; set; }
        public IEnumerable<Plan_Description> plan_descriptions { get; set; }
    }
    public class Plan {
        public string plan_id { get; set; }
        public string entity { get; set; }
        public int interval { get; set; }
        public string period { get; set; }
        public string status { get; set; }
    }
    public class Plan_Description
    {
        public string id { get; set; }
        public string plan_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string amount { get; set; }
        public string unit_amount { get; set; }
        public string currency { get; set; }
        public string type { get; set; }
        public string unit { get; set; }
        public int tax_inclusive { get; set; }
        public string hsn_code { get; set; }
        public string sac_code { get; set; }
        public string tax_rate { get; set; }
        public string tax_id { get; set; }
        public string tax_group_id { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string status { get; set; }
    }

    public class Animal_Master:BatchLivestock
    {
        public int ANIMAL_REG_ID { get; set; }
        public string Birth_Date { get; set; }
        public string Dad_Number { get; set; }
        public string Mom_Number { get; set; }
        public string Mom_Breed { get; set; }
        public string Livestock_Number { get; set; }
        public string Gender { get; set; }
        public int Breed { get; set; }
        public int Sub_Breed { get; set; }
        public string Shed_Number { get; set; }
        public string Remarks { get; set; }
        public int Age_Day { get; set; }
        public int Age_Month { get; set; }
        public int Age_Year { get; set; }
        public string Death_Date { get; set; }
        public int COMPANY_ID { get; set; }
        public int CREATED_BY { get; set; }
        public string Breed_Name { get; set; }
        public string Sub_Breed_Name { get; set; }
        public string Mom_Breed_Name { get; set; }
        public int LOCATION_ID { get; set; }
        public int SUB_LOCATION_ID { get; set; }
        public string Location_Name { get; set; }
        public string Sub_Location_Name { get; set; }
        public int Item_id { get; set; }
        public string Serial_No { get; set; }
        public string Item_Name { get; set; }
        public string Dad_Breed { get; set; }
        public string Dad_Breed_Name { get; set; }

        public int Pregnancy { get; set; }
        public string Pregnancy_date { get; set; }
        public int Transfer_id { get; set; }
        public string livestock_type { get; set; }
    }

    public class Parameter_Master_Kpi:Parameter_Master
    {
        public int parameter_kpi_id { get; set; }
         public int dataentry_type_id { get; set; }
        public string kpi_type { get; set; }
        public decimal kpi_value { get; set; }


    }
    public class GL_Template
    {
        public int GL_ID { get; set; }
        public int NOB_ID { get; set; }
        public int LOB_ID { get; set; }
        public int ITEM_CATEGORY_ID { get; set; }
        public int PARAMETER_TYPE_ID { get; set; }
        public string GL_CODE { get; set; }
        public int created_by { get; set; }
        public int company_id { get; set; }
        public string WIP_CODE { get; set; }
        public string AC_TYPE { get; set; }
    }

    public class ListItem_Class
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public string attr_1 { get; set; }
        public string area { get; set; }
    }

    public class Breed_Attribute
    {
        //attribute_form(attribute/attribute_item)
        public string attribute_form { get; set; }
        public int breed_id { get; set; }
        public int attribute_id { get; set; }
        public int attribute_item_id { get; set; }
        public string value { get; set; }
        public string name { get; set; }
        public string text { get; set; }
        public string uom { get; set; }
        public int created_by { get; set; }
        public int company_id { get; set; }
        public string status { get; set; }
    }

    public class item_Attribute
    {
        //attribute_form(attribute/attribute_item)
        public string attribute_form { get; set; }
        public int item_id { get; set; }
        public int attribute_id { get; set; }
        public int attribute_item_id { get; set; }
        public string value { get; set; }
        public string name { get; set; }
        public string text { get; set; }
        public string uom { get; set; }
        public int created_by { get; set; }
        public int company_id { get; set; }
        public string status { get; set; }
    }

}
