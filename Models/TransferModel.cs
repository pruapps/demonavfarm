using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmIT_Api.Models
{
    public class TransferModel
    {
        public Transfer_Header header { get; set; }
        public IEnumerable<Transfer_Line> lines { get; set; }
        public IEnumerable<DataEntry_Line_LiveStock> livestock { get; set; }
    }
    public class Transfer_Header
    {
        public int TRANSFER_ID { get; set; }
        public string TRANSFER_TYPE { get; set; }
        public int COMPANY_ID { get; set; }
        public int LOCATION_ID { get; set; }
        public int LOCATION_FROM { get; set; }
        public int LOCATION_TO { get; set; }
        public string REMARKS { get; set; }
        public int BATCH_FROM { get; set; }
        public int BATCH_TO { get; set; }
        public string TRANSFER_DATE { get; set; }
        public int CREATED_BY { get; set; }
        public string TRANSFER_NUMBER { get; set; } //THIS for View
        public string ENTRY_FROM { get; set; }  //THIS for View
        public string IS_RECEIPT { get; set; }
        public string IS_SHIP { get; set; }
        public int IS_SERIAL_NO_SYSTEM_GENERATED { get; set; }
    }
    public class Transfer_Line
    {
        public int LINE_ID { get; set; }
        public int TRANSFER_ID { get; set; }
        public int ITEM_ID { get; set; }
        public string UOM { get; set; }
        public decimal UNIT_COST { get; set; }
        public decimal QUANTITY { get; set; }
        public decimal REMAINING_QTY { get; set; }
        public string ITEM_NAME { get; set; }
        public string INVENTORY_TYPE { get; set; }
        public decimal DEAD_ON_ARRIVAL { get; set; }
        public decimal WEIGH_SCALE { get; set; }
        public IEnumerable<DataEntry_Line_LiveStock> livestock { get; set; }
        public int LINE_NO { get; set; }
        public decimal RECEIVED_QUANTITY { get; set; }
        public int IS_POSTED { get; set; }
        public decimal ALTERNATE_QUANTITY { get; set; }
        public string ALTERNATE_UOM { get; set; }
        public int BATCH_FROM { get; set; }
        public int BATCH_TO { get; set; }
        public string BATCH_FROM_NAME { get; set; }
        public string BATCH_TO_NAME { get; set; }
        public string LOT_NO { get; set; }
    }
    public class Transfer_Header_Summary
    {
        public int TRANSFER_ID { get; set; }
        public string TRANSFER_TYPE { get; set; }
        public string TRANSFER_NUMBER { get; set; }
        public string TRANSFER_DATE { get; set; }
        public string LOCATION { get; set; }
        public string REMARKS { get; set; }

        public string ENTRY_FROM { get; set; }
        public string Status { get; set; }
        public int IS_SERIAL_NO_SYSTEM_GENERATED { get; set; }
    }

}

