using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using FarmIT_Api.Models;

namespace FarmIT_Api
{
    public class FCM_HELPER
    {
        public static readonly string[] FullMonths = {"",  "Jan", "Feb", "Mar", "Apr", "May", "Jun",
                                            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        public static string PushNotification( string To_Token, string massage,string title,string screen_name ,string month="",string year="0" ,int sender_emp_id = 0 ,int receiver_emp_id =0)
        {
            string res = "";
            try
            {
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";

                var objNotification = new
                {
                    to = To_Token,
                    notification = new
                    {
                        title = title,
                        body = massage,
                        //  icon = "/firebase-logo.png",
                        sound = true,
                        image = "",//"https://static.pexels.com/photos/4825/red-love-romantic-flowers.jpg",
                        screen_name = screen_name,
                        month = month,
                        year = year,
                        emp_id = sender_emp_id

                    },
                    data = new                 
                    {
                        title = title,
                        body = massage,
                        //  icon = "/firebase-logo.png",
                        sound = true,
                        image = "",//"https://static.pexels.com/photos/4825/red-love-romantic-flowers.jpg",
                        screen_name = screen_name,
                        month = month,
                        year= year,
                        emp_id= sender_emp_id

                    }
                    
            };
                string jsonNotificationFormat = Newtonsoft.Json.JsonConvert.SerializeObject(objNotification);

                    Byte[] byteArray = Encoding.UTF8.GetBytes(jsonNotificationFormat);
                //android
                //tRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAAHTQQP0g:APA91bGKkNnI-8KdU-z82Zvz6rdxdu6A6XTzjli4Cswh7rfVNnB1CiWB77sfGDsREXXxBPtE41t2CBEorPqGz_UHazCXKeOHczci4a9VnvC2BUaFaOWSKPb6RSN-e9Nrf5zvYV-GVrvQ"));
                //tRequest.Headers.Add(string.Format("Sender: id={0}", "125427531592"));

                //ios
                // tRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAAx1r7bks:APA91bFbtMESBrD_ruIadQnTPOU1KFQKQNX1-CuTmzDcAn2d0moU2cfIiJdlTmrHKFkVpEjVAyhez0K_582vzFHt0bs1uI-eAwain7eRi5QoIKdYR96OxjDupUZj2lPbO31X7BCmiVpZ"));
                tRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAAVWALUz4:APA91bF-_DEbMQLD0rLU4T5jzCMS0NHZEAs3f2ESBAzgrKuIwEWi0j-_EBeUcd5ROL7BdyIyi-3_gX6-mwH6UW-MuV7Yixy8Z8C_rHWi3s8PKvmJpHISGTmtg1_fDI7T1rbBCcaiVRjk"));
                //  tRequest.Headers.Add(string.Format("Sender: id={0}", "856224919115"));
                tRequest.ContentLength = byteArray.Length;
                    tRequest.ContentType = "application/json";
                    using (Stream dataStream = tRequest.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);

                        using (WebResponse tResponse = tRequest.GetResponse())
                        {
                            using (Stream dataStreamResponse = tResponse.GetResponseStream())
                            {
                                using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {
                                    string responseFromFirebaseServer = tReader.ReadToEnd();

                                    FCMResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<FCMResponse>(responseFromFirebaseServer);
                                    res = response.results[0].message_id;
                                    //database_Access_Layer.db db_layer = new database_Access_Layer.db();
                                    //db_layer.save_Notification(title, massage, screen_name, month, Convert.ToInt32(year), sender_emp_id, receiver_emp_id);
                                    //if (response.success == 1)
                                    //{
                                    //    Console.WriteLine("succeeded");
                                    //}
                                    //else if (response.failure == 1)
                                    //{
                                    //    Console.WriteLine("failed");
                                    //}
                                }
                            }

                        }
                    } 
            }
            catch(Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
    }

    public class FCMResponse
    {
        public long multicast_id { get; set; }
        public int success { get; set; }
        public int failure { get; set; }
        public int canonical_ids { get; set; }
        public List<FCMResult> results { get; set; }
    }
    public class FCMResult
    {
        public string message_id { get; set; }
    }
}
