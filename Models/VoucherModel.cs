using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmIT_Api.Models
{
    public class VoucherModel
    {
        public int expense_id { get; set; }
        public DateTime? expense_date { get; set; }
        public decimal expense_amount { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public int company_id { get; set; }
        public int created_by { get; set; }
        public string type { get; set; }
        public int batch_id { get; set; }
        public decimal quantity { get; set; }
        public int item_id { get; set; }
        public decimal unit_cost { get; set; }
    }
    public class Voucher_Summary
    {
        public int voucher_id { get; set; }
        public string voucher_date { get; set; }
        public decimal voucher_amount { get; set; }
        public int batch_id { get; set; }
        public decimal quantity { get; set; }
        public string description { get; set; }
        public int item_id { get; set; }
        public string item_name { get; set; }
        public string batch_no { get; set; }
        public string uom { get; set; }
        public decimal unit_cost { get; set; }
        public List<File_Details> files { get; set; }
    }
}
