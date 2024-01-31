using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FarmIT_Api.Models
{
    public class ResponseModel
    {
        public string Status { set; get; }
        public string Message { set; get; }
        public object Data { set; get; }
    }
    public class ResponseDataModel
    {
        public string Status { set; get; }
        public string Message { set; get; }
        public DataTable Data { set; get; }
    }
    public class DeviceTokenModel
    {
        public int USER_ID { get; set; }
        public string DEVICE_TOKEN { get; set; }
        public string DEVICE_TYPE { get; set; }
    }
    public class Notification_Status
    {
        public int ID { get; set; }
        public int IS_READ { get; set; }
        public int CLEAR { get; set; }
        public int USER_ID { get; set; }
        public int STATUS { get; set; }
    }
    public class Notifications
    {
       public int notification_id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public string screenName { get; set; }
        public string web_screenName { get; set; }
        public int batch_id { get; set; }
        public int remaing_qyt { get; set; }
        public int opening_qyt { get; set; }
        public string batch_stauts { get; set; }
        public string created_date { get; set; }
        public int is_read { get; set; }
        public string current_date_status { get; set; }
    }
}
