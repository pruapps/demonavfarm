using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmIT_Api.Models
{
    public class TemplateSettingsModel
    {
        public Template_Header header { get; set; }
        public IEnumerable<Template_Line> lines { get; set; }
        public IEnumerable<TemplateKpi_Line> KPIlines { get; set; }
        public int Created_by { get; set; }
    }
    public class TemplateKpi_Line
    {
        public int LINE_ID { get; set; }
        public int PARAMETER_TYPE_ID { get; set; }
        public string PARAMETER_TYPE { get; set; }
        public int PARAMETER_ID { get; set; }
        public string PARAMETER_NAME { get; set; }
        public int DATAENTRY_TYPE_ID { get; set; }
        public string DATAENTRY_TYPE { get; set; }
        public string UOM { get; set; }
        public string ALTERNATE_UOM { get; set; }
        public string OCCURRENCE { get; set; }
        public int ITEM_ID { get; set; }
        public string ITEM_NAME { get; set; }
        public int FREQUENCY_START_DATE { get; set; }
        public int FREQUENCY_END_DATE { get; set; }
        public string F_START_DATE { get; set; }
        public string F_END_DATE { get; set; }
        public string STATUS { get; set; }
        public int FREQUENCY_KPI_START_DATE { get; set; }
        public int FREQUENCY_KPI_END_DATE { get; set; }
        public string KPI_TYPE { get; set; }
        public double KPI_VALUE { get; set; }

    }
    public class Template_Header
    {
        public int TEMPLATE_ID { get; set; }
        public int COMPANY_ID { get; set; }
        public int NOB_ID { get; set; }
        public string NATURE_OF_BUSINESS { get; set; }
        public int LOB_ID { get; set; }
        public string LINE_OF_BUSINESS { get; set; }
        public string TEMPLATE_NAME { get; set; }
        public int BREED_ID { get; set; }
        public string BREED_NAME { get; set; }
        public string STATUS { get; set; }
        public string LOCATION { get; set; }
        public string BATCH_START_FROM { get; set; }
    }
    public class Template_Line
    {
        public int LINE_ID { get; set; }
        public int PARAMETER_TYPE_ID { get; set; }
        public string PARAMETER_TYPE { get; set; }
        public int PARAMETER_ID { get; set; }
        public string PARAMETER_NAME { get; set; }
        public int DATAENTRY_TYPE_ID { get; set; }
        public string DATAENTRY_TYPE { get; set; }
        public string UOM { get; set; }
        public string ALTERNATE_UOM { get; set; }
        public string OCCURRENCE { get; set; }
        public int ITEM_ID { get; set; }
        public string ITEM_NAME { get; set; }
        public int FREQUENCY_START_DATE { get; set; }
        public int FREQUENCY_END_DATE { get; set; }
        public string F_START_DATE { get; set; }
        public string F_END_DATE { get; set; }
        public string STATUS { get; set; }
        public List<TemplateKpi_Line> KpiLine { get; set; }
    }
    public class Template_Summary
    {
        public string nob_id { get; set; }
        public string nature_of_business { get; set; }
        public string line_of_business { get; set; }
        public string lob_id { get; set; }
        public List<Template_Details> templates { get; set; }
    }
    public class Template_Details
    {
        public int template_id { get; set; }
        public string template_name { get; set; }
        public int breed_id { get; set; }
        public string breed_name { get; set; }
        public string template_type { get; set; }
        public string status { get; set; }
    }
    public class Breed
    {
        public int breed_id { get; set; }
        public string breed_no { get; set; }
        public string breed_name { get; set; }
    }
    public class DataEntryType
    {
        public int dataentry_id { get; set; }
        public string dataentry_type { get; set; }
        public string uom { get; set; }
        public string alternate_uom { get; set; }
        public int lob_id { get; set; }
        public int nob_id { get; set; }
    }
    public class Item
    {
        public int item_id { get; set; }
        public string item_no { get; set; }
        public string item_name { get; set; }
        public string uom { get; set; }
        public decimal unit_cost { get; set; }
        public string alternate_uom { get; set; }
        public int nature_id { get; set; }
        public decimal remainingstock { get; set; }
        public int flag { get; set; }
        public string INVENTORY_TYPE { get; set; }
        public string class_name { get; set; }
    }
}
