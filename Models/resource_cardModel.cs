namespace FarmIT_Api.Models
{
    public class resource_cardModel
    {
        public int resource_id { get; set; }
        public string resource_no { get; set; }
        public string resource_name { get; set; }
        public string uom { get; set; }
        public decimal direct_unit_cost { get; set; }
        public decimal in_direct_unit_cost_per { get; set; }
        public decimal unit_cost { get; set; }
        public decimal unit { get; set; }
        public decimal capacity { get; set; }
        public string status { get; set; }
        public int company_id { get; set; }
        public int created_by { get; set; }
        public string type { get; set; }
        public string maintanance_frequency { get; set; }
        public string last_maintanance_dt { get; set; }
        public string maintanance_start_dt { get; set; }
        public string next_due_date { get; set; }
        public string under_maintenance { get; set; }

    }

    public class resource_card_list : resource_cardModel
    {
        public decimal allocated { get; set; }
    }
}
