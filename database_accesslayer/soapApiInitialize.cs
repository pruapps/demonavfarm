using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace FarmIT_Api.database_accesslayer
{
    public class soapApiInitialize
    {
       /* 
        //public HttpClient InitializeApi()
        //{
        //    var client = new HttpClient();
        //    client.BaseAddress = new Uri("http://soapapi.navfarm.com/api/");

        //    //string _ContentType = "application/json";
        //    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_ContentType));
        //    //var _CredentialBase64 = "dGVzdDp0ZXN0";
        //    //client.DefaultRequestHeaders.Add("Authorization", String.Format("Basic {0}", _CredentialBase64));
        //    //var apiKey = "MyRandomApiKeyValue";
        //    //// You can actually also set the User-Agent via a built-in property
        //    //client.DefaultRequestHeaders.Add("X-ApiKey", apiKey);
        //    return client;
        //}
        public string Call_StockUpdate(string Location_code ,int comp_id)
        {
            try
            {
                var s1 = new WebClient();
                
                // var byteArray = Encoding.ASCII.GetBytes("TASHI:TASHI@15");
                //string base64 = Convert.ToBase64String(byteArray);
                //string authorization = String.Concat("Basic ", base64);
                // s1.Headers.Add("authorization", authorization);
                // s1.Headers.Add("X-ApiKey", "MyRandomApiKeyValue");
                if (comp_id==56) // for Unnat 
                {
                
                    var a = s1.DownloadString("http://soapapi.navfarm.com/api/stock_update?Locationcode=" + Location_code);
                    var R = Newtonsoft.Json.JsonConvert.DeserializeObject(a);
                    return R.ToString();
                }
                else if (comp_id==81) // for BPIL item stock is updating using service for bpil
                {  //old code
                   //  var a = s1.DownloadString("http://soapapi.navfarm.com/api/bpil_stock_update?Locationcode=" + Location_code);
                   //  var a = s1.DownloadString("https://localhost:44354/api/bpil_stock_update?Locationcode=" + Location_code);
                   //   var R = Newtonsoft.Json.JsonConvert.DeserializeObject(a);
                   //New code

                    // for  BPIL now stock is updating  using  service 
                    //new code 
                    //System.Uri _uri = new System.Uri("http://soapapi.navfarm.com");
                    //ExtendedWebClient sNew1 = new ExtendedWebClient(_uri);
                    //var apiurl = "http://soapapi.navfarm.com/api/bpil_stock_update?Locationcode=" + Location_code;
                    //var ss= sNew1.DownloadString(apiurl);
                    //var R = Newtonsoft.Json.JsonConvert.DeserializeObject(ss);

                    var R = "";
                   
                    return R.ToString();
                }
                else
                {
                    return "";
                }
          

           
            }
            catch(Exception ex)
            {

                return "error";
            }
           
        }

        public string Call_postitem(int dataentry_id ,int company_id)
        {
            try
            {
                var s1 = new WebClient();
                // var byteArray = Encoding.ASCII.GetBytes("TASHI:TASHI@15");
                //string base64 = Convert.ToBase64String(byteArray);
                //string authorization = String.Concat("Basic ", base64);
                // s1.Headers.Add("authorization", authorization);
                // s1.Headers.Add("X-ApiKey", "MyRandomApiKeyValue");
                if (company_id==56)
                {
                    var a = s1.DownloadString("http://soapapi.navfarm.com/api/post_item?dataentry_id=" + dataentry_id + "&company_id=" + company_id);
                    var R = Newtonsoft.Json.JsonConvert.DeserializeObject(a);
                    return R.ToString();
                } else if (company_id == 81 || company_id == 239 || company_id == 246)
                {
                    var a = s1.DownloadString("http://soapapi.navfarm.com/api/postitemledger_bpil?dataentry_id=" + dataentry_id + "&company_id=" + company_id);
                    //var a = s1.DownloadString("https://localhost:44354/api/postitemledger_bpil?dataentry_id=" + dataentry_id + "&company_id=" + company_id);
                    var R = Newtonsoft.Json.JsonConvert.DeserializeObject(a);
                    return R.ToString();
                }
                else
                {
                    return "";
                }
            }
            catch
            {

                return "error";
            }

        }

        public string Call_postitem_adjustment(int dataentry_id, int company_id)
        {
            try
            {
                var s1 = new WebClient();
                // var byteArray = Encoding.ASCII.GetBytes("TASHI:TASHI@15");
                //string base64 = Convert.ToBase64String(byteArray);
                //string authorization = String.Concat("Basic ", base64);
                // s1.Headers.Add("authorization", authorization);
                // s1.Headers.Add("X-ApiKey", "MyRandomApiKeyValue");
                if (company_id == 81 || company_id == 239 || company_id == 246)
                {
                       var a = s1.DownloadString("http://soapapi.navfarm.com/api/postitemledger_adjustment_bpil?dataentry_id=" + dataentry_id + "&company_id=" + company_id);
                 //  var a = s1.DownloadString("https://localhost:44354/api/postitemledger_adjustment_bpil?dataentry_id=" + dataentry_id + "&company_id=" + company_id);
                    var R = Newtonsoft.Json.JsonConvert.DeserializeObject(a);
                    return R.ToString();
                }
                else
                {
                    return "";
                }
            }
            catch
            {

                return "error";
            }

        }
      */


    }

    public class ExtendedWebClient : WebClient
    {

        private int timeout;
        public int Timeout
        {
            get
            {
                return timeout;
            }
            set
            {
                timeout = value;
            }
        }
        public ExtendedWebClient(Uri address)
        {
            this.timeout = 600000;//In Milli seconds
            var objWebClient = GetWebRequest(address);
        }
        protected override WebRequest GetWebRequest(Uri address)
        {
            var objWebRequest = base.GetWebRequest(address);
            objWebRequest.Timeout = this.timeout;
            return objWebRequest;
        }
    }
}
