using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmIT_Api.Models
{
    public class BillingInfoModel
    {
        public int BILLING_INFO_ID { get; set; }
        public int USER_ID { get; set; }
        public string FULL_NAME { get; set; }
        public string EMAIL { get; set; }
        public string PHONE_NO { get; set; }
        public string ADDRESS { get; set; }
        public int ZIP_CODE { get; set; }
        public string COUNTRY { get; set; }
        public string CURRENCY { get; set; }
    }
}
