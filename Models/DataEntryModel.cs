using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmIT_Api.Models
{
    public class DataEntryModel
    {
        public DataEntry_Header header { get; set; }
        public IEnumerable<DataEntry_Line> lines { get; set; }
        public IEnumerable<DataEntry_Line_LiveStock> livestock { get; set; }

    }
    public class DataEntryModel_Bulk
    {
        public DataEntry_Header header { get; set; }
        public IEnumerable<DataEntry_Line_Bulk> lines { get; set; }
        

    }
    public class DataEntry_Header
    {
        public int DATAENTRY_ID { get; set; }
        public int COMPANY_ID { get; set; }
        public int NOB_ID { get; set; }
        public string NATURE_OF_BUSINESS { get; set; }
        public int LOB_ID { get; set; }
        public string LINE_OF_BUSINESS { get; set; }
        public int BATCH_ID { get; set; }
        public string BATCH_NO { get; set; }
        public string BREED_NAME { get; set; }
        public string TEMPLATE_NAME { get; set; }
        public int TEMPLATE_ID { get; set; }
        public string LOCATION_NAME { get; set; }
        public DateTime POSTING_DATE { get; set; }
        public string P_DATE { get; set; }
        public int AGE_DAYS { get; set; }
        public int AGE_WEEK { get; set; }
        public int OPENING_QTY { get; set; }
        public DateTime START_DATE { get; set; }
        public string S_DATE { get; set; }
        public decimal RUNNING_COST { get; set; }
        public int REMAINING_QTY { get; set; }
        public string STATUS { get; set; }
        public int CREATED_BY { get; set; }
        public string CURRENT_LOCATION { get; set; }
        public string LOCATION { get; set; }
        public string ENTRY_FROM { get; set; }
        public string CHK_in_lat { get; set; }
        public string CHK_in_long { get; set; }
        public string END_DATE { get; set; }
        public string REMARK { get; set; }
    }
    public class DataEntry_Line
    {
        public int LINE_ID { get; set; }
        public int PARAMETER_TYPE_ID { get; set; }
        public string PARAMETER_TYPE { get; set; }
        public string PARAMETER_ID { get; set; }
        public string PARAMETER_NAME { get; set; }
        public string FORMULA_FLAG { get; set; }
        public decimal ACTUAL_VALUE { get; set; }
        public decimal UNIT_COST { get; set; }
        public int DATAENTRY_TYPE_ID { get; set; }
        public string DATAENTRY_TYPE { get; set; }
        public string DATAENTRY_UOM { get; set; }
        public string OCCURRENCE { get; set; }
        public int FREQUENCY_START_DATE { get; set; }
        public int FREQUENCY_END_DATE { get; set; }
        public string F_START_DATE { get; set; }
        public string  F_END_DATE { get; set; }
        public string ITEM_NAME { get; set; }
        public decimal LINE_AMOUNT { get; set; }
        public int Batch_ID { get; set; }
        public int ITEM_ID { get; set; }
        public int Livestock_flag { get; set; }
        public string Status { get; set; }
        public decimal Stock { get; set; }
        public string Parameter_input_type { get; set; }
        public string Parameter_input_format { get; set; }
        public string Parameter_input_value { get; set; }
        public int Company_id { get; set; }

        public IEnumerable<DataEntry_Line_LiveStock> livestock { get; set; }
    }

    public class DataEntry_Line_Bulk: DataEntry_Line
    {
        public string Batch_No { get; set; }
    }
        public class DataEntry_Line_LiveStock
    {
        public int DataEntry_id { get; set; }
        public decimal Total_Units { get; set; }
        public string Livestock_No { get; set; }
        public int Parameter_id { get; set; }
        public int item_id { get; set; }
        public int Batch_id { get; set; }
        public DateTime Posting_date { get; set; }
    }
    public class DataEntry_Summary
    {
        public string nob_id { get; set; }
        public string nature_of_business { get; set; }
        public string line_of_business { get; set; }
        public string lob_id { get; set; }
        public List<DataEntry_Details> batches { get; set; }
    }
    public class DataEntry_Details
    {
        public int batch_id { get; set; }
        public string batch_no { get; set; }
        public string start_date { get; set; }
        public int opening_stocks { get; set; }
        public int remaining_stocks { get; set; }
        public string last_entry_date { get; set; }
        public string remark { get; set; }
        public string status { get; set; }
    }
    public class DataEntry_History
    {
        public int dataentry_id { get; set; }
        public string posting_date { get; set; }
    }
}
