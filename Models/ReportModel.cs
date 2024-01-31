using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmIT_Api.Models
{
    public class ReportModel
    {
        public string batch_no { get; set; }
        public string week_no { get; set; }
        //public object Labels { get; set; }
        public List<ReportValueData> details { get; set; }
    }
    public class ReportValueData
    {
        public object Values { get; set; }
    }
    public class Breeding_Laying_Report_Part1
    {
        public string Batch_No { get; set; }
        public string User_Name { get; set; }
        public string Start_Date { get; set; }
        public string Entry_Date { get; set; }
        public int Week { get; set; }
        public string Breed { get; set; }
        public string Location { get; set; }
        public int age_day { get; set; }
        public int age_week { get; set; }
        public int opening_balance { get; set; }
        public int remaining_birds { get; set; }        
        public string hatching_date { get; set; }        
        public int mortality { get; set; }
        public int culls { get; set; }
        public int sex_error { get; set; }
        public decimal mortality_per { get; set; }
        public int feed_per_day { get; set; }
        public decimal fcr { get; set; }
        //public decimal body_weight_gm { get; set; }
        //public decimal body_weight_kg { get; set; }
        //public decimal growth_per { get; set; }
    }
    public class Breeding_Laying_Report_Part2
    {
        public string Batch_No { get; set; }
        public string User_Name { get; set; }
        public string Start_Date { get; set; }
        public string Entry_Date { get; set; }
        public int Week { get; set; }
        public string Breed { get; set; }
        public string Location { get; set; }
        public decimal total_feed_cost { get; set; }
        public decimal total_feed_consumed { get; set; }
        public int commercial_eggs_qty { get; set; }
        public int hatchable_eggs_qty { get; set; }
        public int crack_egg { get; set; }
        public decimal water_cost { get; set; }
        public decimal electricity_cost { get; set; }
        public decimal labour_cost { get; set; }
        public decimal electricity_units { get; set; }
        public decimal water_units { get; set; }
        public decimal vaccine_cost { get; set; }
        public decimal vaccine_units { get; set; }
        public decimal batch_performance { get; set; }
    }
    public class CBF_Report_Part1
    {
        public string Batch_No { get; set; }
        public string User_Name { get; set; }
        public string Start_Date { get; set; }
        public string Entry_Date { get; set; }
        public int Week { get; set; }
        public string Breed { get; set; }
        public string Location { get; set; }
        public string hatching_date { get; set; }
        public int age_day { get; set; }
        public int age_week { get; set; }
        public int opening_balance { get; set; }
        public int remaining_birds { get; set; }

        public int mortality { get; set; }
        public int culls { get; set; }
        public decimal mortality_per { get; set; }
        //public decimal doc_output { get; set; }
        public decimal body_weight_kg { get; set; }
        public decimal growth_per { get; set; }
        public decimal Output { get; set; }
        public decimal total_feed { get; set; }
        public decimal comm_feed { get; set; }
        public decimal FCR { get; set; }
        public decimal KPI { get; set; }
        public decimal Sale_qty { get; set; }
        public decimal Sale_amount { get; set; }



    }
    public class CBF_Report_Part2
    {
        public string Batch_No { get; set; }
        public string User_Name { get; set; }
        public string Start_Date { get; set; }
        public string Entry_Date { get; set; }
        public int Week { get; set; }
        public string Breed { get; set; }
        public string Location { get; set; }
        public decimal total_feed_cost { get; set; }
        public decimal total_feed_consumed { get; set; }
        public decimal fcr { get; set; }
        public decimal pef { get; set; }
        public decimal water_cost { get; set; }
        public decimal electricity_cost { get; set; }
        public decimal labour_cost { get; set; }
        public decimal electricity_units { get; set; }
        public decimal water_units { get; set; }
        public decimal vaccine_cost { get; set; }
        public decimal vaccine_units { get; set; }
        public decimal batch_performance { get; set; }
    }
    public class CBF_Report_SUMM
    {
        
        public int opening_balance { get; set; }
        public int remaining_birds { get; set; }

        public int mortality { get; set; }
        
         public decimal body_weight_kg { get; set; }
         public decimal Output { get; set; }
        public int culls { get; set; }
        public decimal total_feed { get; set; }
        public decimal FCR { get; set; }
        public decimal Age { get; set; }

        public decimal Cost_Bird { get; set; }
        public decimal Sale_Qty { get; set; }
        public decimal Sale_Amount { get; set; }

    }
    public class HATCHERY_REPORT_PART1
    {
        public string Batch_No { get; set; }
        public string Supervisor_Name { get; set; }
        public string Egg_Setting_Date { get; set; }
        public string Entry_Date { get; set; }
        public int Week { get; set; }
        public string Breed { get; set; }
        public string Location { get; set; }
        public string Egg_Laying_Date { get; set; }
        public int Setable_hatching_Egg { get; set; }
        public int Crack_Eggs { get; set; }
        public int Opening_Egg { get; set; }
        public int DOC_Produced_M { get; set; }
        public int DOC_Produced_F { get; set; }
        public int Culls_Chicks { get; set; }
        public decimal DOC_COST { get; set; }

        public decimal SaleQty { get; set; }
        public decimal SaleAmount { get; set; }

    }
    public class HATCHERY_REPORT_SUMM
    {
        public int Opening_Egg { get; set; }
        public decimal Opening_Amount { get; set; }
        
        public int Crack_Eggs { get; set; }
        public int Remaining { get; set; }

        public int DOC_Produced_M { get; set; }
        public int DOC_Produced_F { get; set; }
        public int Culls_Chicks { get; set; }
        public decimal SaleQty { get; set; }
        public decimal SaleAmount { get; set; }
        public decimal DOC_COST { get; set; }
    }
    public class HATCHERY_REPORT_PART2
    {
        public string Batch_No { get; set; }
        public string Supervisor_Name { get; set; }
        public string Egg_Setting_Date { get; set; }
        public string Entry_Date { get; set; }
        public int Week { get; set; }
        public string Breed { get; set; }
        public string Location { get; set; }
        public decimal Total_Water_Cost { get; set; }
        public decimal Total_Electricity_Cost { get; set; }
        public decimal Total_Labour_Cost { get; set; }
        public decimal Total_Electricity_Units { get; set; }
        public decimal Total_Water_Consumed_Units { get; set; }
        public decimal Total_Vaccine_Cost { get; set; }
        public decimal Total_Vaccine_Units { get; set; }
        public decimal Batch_Performance_Amount { get; set; }
    }
    public class Flock_Wise_Fcr_Report
    {
        public string Batch_No { get; set; }
        public string Breed { get; set; }
        public string Location_name { get; set; }
        public string Placement_Date { get; set; }
        public string Depletion_Date { get; set; }
        public int Age { get; set; }
        public int Age_in_days { get; set; }
        public int Chicks_Placed { get; set; }
        public decimal Mortality { get; set; }
        public decimal Previous_day_Mortality { get; set; }
        public decimal DBY_Mortaltiy { get; set; }
        public decimal Total_Culls { get; set; }
        public decimal Mortality_Per { get; set; }
        public decimal Remaining_Chicks { get; set; }
        public decimal Output { get; set; }
        public decimal Average_Weight { get; set; }
        public decimal Feed_consumption { get; set; }
        public decimal Per_Bird_Feed_Consume { get; set; }
        public decimal Total_Body_Weight { get; set; }
        public decimal Final_Avg_body_weight { get; set; }
        public decimal Fcr { get; set; }
        public decimal Livability_Per { get; set; }
        public decimal PEF { get; set; }
      }
    public class Flock_Wise_Fcr_Report2
    {
        public string Batch_No { get; set; }
        public int Chicks_Placed { get; set; }
        public decimal Cracked_EGGS { get; set; }
        public decimal Cracked_per { get; set; }
        public decimal Setable_EGGS { get; set; }
        public string Egg_Setting_Date { get; set; }
        public string Hatch_transfer_date { get; set; }
        public decimal Infertile_EGGS { get; set; }
        public decimal Infertile_EGGS_per { get; set; }
        public decimal MDOC { get; set; }
        public decimal FDOC { get; set; }
        public decimal CULL { get; set; }
 
    }
    public class Flock_Wise_Fcr_Report3
    {
        public string Batch_No { get; set; }

        public int Age { get; set; }
        public int Age_in_days { get; set; }
        public int Opening_M { get; set; }
        public int Opening_F { get; set; }
        public int Closing_M { get; set; }
        public int Closing_F { get; set; }
        public int Hatch_Egg { get; set; }
        public int Table_Egg { get; set; }
        public decimal FCR { get; set; }
        public decimal Feed_consumption { get; set; }
        public decimal Per_Bird_Feed_Consume { get; set; }
        public decimal Per_Egg_Feed { get; set; }
        public decimal Avg_Egg_Weight { get; set; }

    }
    public class Feed_Report
    {
        public string Batch_Date { get; set; }
        public string Batch_No { get; set; }
        public string Formulation_Name { get; set; }
        public string Plant_Incharge { get; set; }        
        public string Breed { get; set; }
        public string Location_name { get; set; }
        //public string FG_Item_Name { get; set; }
        //public string FG_Item { get; set; }
        public decimal OP_Qty { get; set; }
        public decimal FG_Item_Qty { get; set; }
        public decimal Batch_Performance_Amount { get; set; }
        public string RM { get; set; }
        public decimal RM_Qty { get; set; }
        public int RM_Standard_Yield { get; set; }
        public decimal RM_Cost { get; set; }
   
      
    }
    public class Slaughter_Report
    {
        public string Batch_Date { get; set; }
        public string Batch_No { get; set; }
        public string Plant_Incharge { get; set; }
        public string Breed { get; set; }
        public string Location_name { get; set; }
       
        //public string FG_Item { get; set; }
        public decimal OP_Qty { get; set; }
        public string FG_Item_Name { get; set; }
        public decimal FG_Item_Qty { get; set; }

        public decimal Batch_Performance_Amount { get; set; }
        public string RM { get; set; }
        public decimal RM_Qty { get; set; }
        public int RM_Standard_Yield { get; set; }
        public decimal RM_Cost { get; set; }
      
    }
    public class Laying_Report
    {
        public string Entry_Date { get; set; }
        public int Male { get; set; }
        public int Female { get; set; }
        public int Culls_M { get; set; }
        public int Culls_F { get; set; }
        public int Sex_error { get; set; }
        public int Comu_Mortality { get; set; }
        public decimal Comu_Mortality_Per { get; set; }
        public int Rem_Birds { get; set; }
        public decimal Weight_Male { get; set; }
        public decimal Weight_Female { get; set; }
        public decimal Growth_Per_M { get; set; }
        public decimal Growth_Per_F { get; set; }
        public decimal Feed_Consum_M { get; set; }
        public decimal Feed_Consum_F { get; set; }
        public decimal Comu_Feed_Consum { get; set; }
        public decimal FCR { get; set; }
        public int Age_Day { get; set; }
        public int Age_Week { get; set; }
        public decimal Eggs_Collection { get; set; }
        public decimal Table_Collection { get; set; }
        public decimal Creck_Eggs { get; set; }
        public decimal Jumbo_Eggs { get; set; }
        //public decimal Pullet_Eggs { get; set; }
        public decimal Current_Feed_Kgs { get; set; }
        public decimal Current_Feed_Bird_grams { get; set; }
        public decimal Current_Feed_Egg_grams { get; set; }
        public decimal Cumulative_Feed_Egg_grams { get; set; }

        public decimal Total_Cost { get; set; }
    }
    public class Laying_Report_SUMM
    {
        public int Male { get; set; }
        public int Female { get; set; }
        public int Culls_M { get; set; }
        public int Culls_F { get; set; }
        public int Sex_error { get; set; }
        public int Rem_Birds { get; set; }
        public decimal Weight_Male { get; set; }
        public decimal Weight_Female { get; set; }
        public decimal Feed_Consum_M { get; set; }
        public decimal Feed_Consum_F { get; set; }
        public decimal Eggs_Collection { get; set; }
        //public decimal Table_Collection { get; set; }
        //public decimal Creck_Eggs { get; set; }
        //public decimal Jumbo_Eggs { get; set; }
        //public decimal Pullet_Eggs { get; set; }
        public decimal Total_Cost { get; set; }
    }
    public class Rearing_Report
    {
        public string Entry_Date { get; set; }
        public int Male { get; set; }
        public int Female { get; set; }
        public int Culls_M { get; set; }
        public int Culls_F { get; set; }
        public int Comu_Mortality { get; set; }
        public decimal Comu_Mortality_Per { get; set; }
        public int Rem_Birds { get; set; }
        public int Weight_Male { get; set; }
        public int Weight_Female { get; set; }
        public decimal Growth_Per { get; set; }
        public int Feed_Consum_M { get; set; }
        public int Feed_Consum_F { get; set; }
        public int Comu_Feed_Consum { get; set; }
        public decimal FCR { get; set; }
        public int Age_Day { get; set; }
        public int Age_Week { get; set; }
    }
    public class Farm_Performance_Location_report
    {
        public string NOB { get; set; }
        public string Location_Name { get; set; }
        public string Batch_No { get; set; }
        public int Opening_Qty { get; set; }
        public decimal Output_Qty { get; set; }
        //public decimal Mortality_Per { get; set; }
        //public decimal FCR { get; set; }
        //public decimal Growth_Per { get; set; }
        public decimal Electricity_Cost { get; set; }
        public decimal Electricity_Units { get; set; }
        public decimal Water_Cost { get; set; }
        public decimal Water_Units { get; set; }
        public decimal Labour_Cost { get; set; }
        public decimal Vaccine_Cost { get; set; }
        public decimal Vaccine_Units { get; set; }
        public decimal Expense_Amount { get; set; }
        public decimal Feed_Cost { get; set; }
        public decimal Feed_Consumed { get; set; }
        public decimal Running_Cost { get; set; }
    }
    public class Farm_Performance_Wk_Report
    {
        public int Week_No { get; set; }
        public int Total_Running_Batch { get; set; }
        public string USER_NAME { get; set; }
        //public decimal WATER_COST { get; set; }
        //public decimal ELECTRICITY_COST { get; set; }
        //public decimal LABOUR_COST { get; set; }
        //public decimal ELECTRICITY_UNITS { get; set; }
        //public decimal WATER_UNITS { get; set; }
        //public decimal VACCINE_COST { get; set; }
        //public decimal VACCINE_UNITS { get; set; }
        //public decimal FEED_COST { get; set; }
        //public decimal FEED_UNITS { get; set; }
        public decimal Total_Running_Cost { get; set; }
        public decimal Misc_Expenses { get; set; }
        public decimal Payments { get; set; }
        public decimal Recipets { get; set; }
        public decimal FarmPL { get; set; }
    }
    public class Batch_Output_Performance_Report
    {
        public string LOB { get; set; }
        public string Batch_No { get; set; }
        public int Week_No { get; set; }
     
        public decimal Unit_Cost { get; set; }
        public decimal Output_Item_Qty { get; set; }
        public decimal Total_Running_Cost { get; set; }
    }
    public class Cattle_Report_Part1
    {
        public string Batch_No { get; set; }
        //public string Supervisor_Name { get; set; }
        public string Breed { get; set; }
       // public string Location { get; set; }
        public string Start_Date { get; set; }
        public string Entry_Date { get; set; }
        public int Age_in_Days { get; set; }
        public int Opening_Balance { get; set; }
        //  public decimal Milk_Extracted { get; set; }
        public decimal Mortality_M { get; set; }
        public decimal Mortality_F { get; set; }

        public decimal Body_Weight_M { get; set; }
        public decimal Body_Weight_F { get; set; }

        public decimal FEED { get; set; }
        public decimal Growth { get; set; }
        public decimal Vaccine { get; set; }
        public decimal Medicine { get; set; }
        public decimal FCR { get; set; }
        public decimal Livability_Per { get; set; }
    }
    public class Cattle_Report_Part2
    {
        public string Batch_No { get; set; }
        public int Opening_Balance { get; set; }
        public string Supervisor_Name { get; set; }
        public string Breed { get; set; }
        public string Location { get; set; }
        public string Start_Date { get; set; }
        public string Entry_Date { get; set; }
        public decimal Total_Water_Cost { get; set; }
        public decimal Total_Feed_Cost { get; set; }
        public decimal Total_Feed_Consumed { get; set; }
        public decimal Total_Electricity_Cost { get; set; }
        public decimal Total_Labour_Cost { get; set; }
        public decimal Total_Electricity_Units { get; set; }
        public decimal Total_Water_Consumed { get; set; }
        public decimal Total_Vaccine_Cost { get; set; }
        public decimal Total_Vaccine_Units { get; set; }
        public decimal Batch_Performance { get; set; }
        public decimal Body_Weight { get; set; }
        public decimal Growth { get; set; }
    }

    public class Laying_Mortality
    {
        public decimal Mortality_Per { get; set; }
        public decimal Previous_WK_Mortality { get; set; }
        public decimal Prevoius_Mon_Mortaltiy { get; set; }
        public int Today_Mortaltiy { get; set; }

    }
    public class Cattle_Report_Dairy
    {
        public string Batch_No { get; set; }
        public string Enrty_date { get; set; }
        public int Batch_Quantity { get; set; }
        public decimal YM { get; set; }
        public decimal YE { get; set; }

        public decimal Total_Yield { get; set; }
        public decimal CM { get; set; }

        public decimal CE { get; set; }
        public decimal Total_Calves { get; set; }
        public decimal Avg_Per_day { get; set; }
        public decimal Total_Processing { get; set; }
    }
    public class Batch_Least_Performance_Report
    {
        public string Batch_No { get; set; }
        public string Start_Date { get; set; }
        public string Last_entry_Date { get; set; }
        public decimal Opening_Stock { get; set; }
        public decimal Remaning_Stock { get; set; }
        public decimal total_Output_Per { get; set; }
    }
}
