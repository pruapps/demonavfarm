using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FarmIT_Api.Models
{
    public class BatchModel
    {
        public BatchHeader batchHeader { get; set; }
        public IEnumerable<BatchLivestock> batchlivestock { get; set; }
        public IEnumerable<BatchItems> batchitems { get; set; }
        public IEnumerable<BatchMachine> batchmachine { get; set; }
    }
    public class BatchHeader
    {
        public int BATCH_ID { get; set; }
        public int COMPANY_ID { get; set; }
        public int NOB_ID { get; set; }
        public string NATURE_OF_BUSINESS { get; set; }
        public int LOB_ID { get; set; }
        public string LINE_OF_BUSINESS { get; set; }
        public int TEMPLATE_ID { get; set; }
        public string TEMPLATE_NAME { get; set; }
        public int BREED_ID { get; set; }
        public string BREED_NAME { get; set; }
        public int MALE_BIRD { get; set; }
        public int FEMALE_BIRD { get; set; }
        public int BATCH_QUANTITY { get; set; }
        public int LOCATION_ID { get; set; }
        public string LOCATION_NAME { get; set; }
        public DateTime? START_DATE { get; set; }
        public DateTime? END_DATE { get; set; }
        [DisplayName("START_DATE")]
        public string S_DATE { get; set; }
        [DisplayName("END_DATE")]
        public string E_DATE { get; set; }
        public DateTime? HATCHING_DATE { get; set; }
        [DisplayName("HATCHING_DATE")]
        public string H_DATE { get; set; }
        public string BATCH_NO { get; set; }
        public string STATUS { get; set; }
        public string REMARKS { get; set; }
        public int CREATED_BY { get; set; }
        public string LOCATION { get; set; }
        public string CURRENT_LOCATION { get; set; }
        public int ITEM_ID_M { get; set; }
        public int ITEM_ID_F { get; set; }
        public string ITEM_NAME_M { get; set; }
        public string ITEM_NAME_F { get; set; }
        public decimal Remaining_Stock_M { get; set; }
        public decimal Remaining_Stock_F { get; set; }
        public int Flag_M { get; set; }
        public int Flag_F { get; set; }
        public decimal BATCH_CAPACITY { get; set; }

    }
    public class BatchLivestock
    {
        public int batch_ls_id { get; set; }
        public int batch_id { get; set; }
        public int animal_reg_id { get; set; }
        public int item_id { get; set; }
    }
    public class BatchItems
        {
        public int Batch_item_id { get; set; }
        public int Batch_id { get; set; }
        public int Item_id { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rem_Qty { get; set; }
        public string Original_batch_no { get; set; }
        public string Remarks { get; set; }
        public string Item_name { get; set; }
        public string UOM { get; set; }
        public string INVENTORY_TYPE { get; set; }
        public string Flag { get; set; }
        public decimal Aw_gms { get; set; }
        public decimal Biomass { get; set; }
        public string date { get; set; }
        public decimal Slaughter_Weight { get; set; }
        public string Original_type { get; set; }
    }

    public class BatchMachine
    {
        public int batch_machine_id { get; set; }
        public int batch_id { get; set; }
        public int resource_card_id { get; set; }
        public decimal capacity { get; set; }
        public decimal quantity { get; set; }
        public decimal allocated { get; set; }
        public decimal remaining { get; set; }
        public string resource_card_name { get; set; }
    }
    public class Batch_Summary
    {
        public string nob_id { get; set; }
        public string nature_of_business { get; set; }
        public string line_of_business { get; set; }
        public string lob_id { get; set; }
        public string location_name { get; set; }
        public List<Batch_Details> batches { get; set; }
    }
    public class Batch_Details
    {
        public int batch_id { get; set; }
        public string batch_no { get; set; }
        public string start_date { get; set; }
        public int opening_stocks { get; set; }
        public string remark { get; set; }
        public string status { get; set; }
        public int ITEM_ID_M { get; set; }
        public int ITEM_ID_F { get; set; }
        public string ITEM_NAME_M { get; set; }
        public string ITEM_NAME_F { get; set; }
        public string Location_name { get; set; }
    }
    public class Batch_Details_Web
    {
        public int batch_id { get; set; }
        public string batch_no { get; set; }
        public string start_date { get; set; }
        public int opening_stocks { get; set; }
        public string nature_of_business { get; set; }
        public string line_of_business { get; set; }
        public string status { get; set; }
    }
    public class File_Details
    {
        public int file_id { get; set; }
        public string file_name { get; set; }
    }
}
