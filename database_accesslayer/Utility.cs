using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace FarmIT_Api.Models
{
    public static class Utility
    {
        private const string _alg = "HmacSHA256";
        private const string _salt = "rz8LuOtFBXphj9WQfvFh"; // Generated at https://www.random.org/strings 3AJYZlghsmJgdA7iIluM
        public static void IgnoreBadCertificates()
        {
           ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
        }

        private static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        #region PDF Generation

        //public static string GeneratePDF(string path,string html)
        //{
        //    //Create a byte array that will eventually hold our final PDF
        //    Byte[] bytes;

        //    //Boilerplate iTextSharp setup here
        //    //Create a stream that we can write to, in this case a MemoryStream
        //    using (var ms = new MemoryStream())
        //    {

        //        //Create an iTextSharp Document which is an abstraction of a PDF but **NOT** a PDF
        //        using (var doc = new Document())
        //        {

        //            //Create a writer that's bound to our PDF abstraction and our stream
        //            using (var writer = PdfWriter.GetInstance(doc, ms))
        //            {

        //                //Open the document for writing
        //                doc.Open();

        //                //Our sample HTML and CSS
        //                var example_html = html;
        //                //var example_css = @".headline{font-size:200%}";

        //                /**************************************************
        //                 * Example #1                                     *
        //                 *                                                *
        //                 * Use the built-in HTMLWorker to parse the HTML. *
        //                 * Only inline CSS is supported.                  *
        //                 * ************************************************/

        //                //Create a new HTMLWorker bound to our document
        //                using (var htmlWorker = new iTextSharp.text.html.simpleparser.HTMLWorker(doc))
        //                {

        //                    //HTMLWorker doesn't read a string directly but instead needs a TextReader (which StringReader subclasses)
        //                    using (var sr = new StringReader(example_html))
        //                    {

        //                        //Parse the HTML
        //                        htmlWorker.Parse(sr);
        //                    }
        //                }

        //                /**************************************************
        //                 * Example #2                                     *
        //                 *                                                *
        //                 * Use the XMLWorker to parse the HTML.           *
        //                 * Only inline CSS and absolutely linked          *
        //                 * CSS is supported                               *
        //                 * ************************************************/

        //                //XMLWorker also reads from a TextReader and not directly from a string
        //                using (var srHtml = new StringReader(example_html))
        //                {

        //                    //Parse the HTML
        //                    iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, srHtml);
        //                }

        //                /**************************************************
        //                 * Example #3                                     *
        //                 *                                                *
        //                 * Use the XMLWorker to parse HTML and CSS        *
        //                 * ************************************************/

        //                //In order to read CSS as a string we need to switch to a different constructor
        //                //that takes Streams instead of TextReaders.
        //                //Below we convert the strings into UTF8 byte array and wrap those in MemoryStreams
        //                //using (var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_css)))
        //                //{
        //                //    using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_html)))
        //                //    {

        //                //        //Parse the HTML
        //                //        iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml, msCss);
        //                //    }
        //                //}



        //                    using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_html)))
        //                    {

        //                        //Parse the HTML
        //                        iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml);
        //                    }



        //                doc.Close();
        //            }
        //        }

        //        //After all of the PDF "stuff" above is done and closed but **before** we
        //        //close the MemoryStream, grab all of the active bytes from the stream
        //        bytes = ms.ToArray();
        //    }

        //    //Now we just need to do something with those bytes.
        //    //Here I'm writing them to disk but if you were in ASP.Net you might Response.BinaryWrite() them.
        //    //You could also write the bytes to a database in a varbinary() column (but please don't) or you
        //    //could pass them to another function for further PDF processing.
        //    var testFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.pdf");
        //    System.IO.File.WriteAllBytes(testFile, bytes);
        //}

        #endregion

        public static string GenerateRandomNumber(int noOfDigit)
        {
            Random random = new Random();
            string r = "";
            int i;
            for (i = 1; i < noOfDigit; i++)
            {
                r += random.Next(0, 9).ToString();
            }
            return r;
        }
        
        public static string SendResetPassword(string PhoneNo)
        {
            string msg = "";
            try
            {

            }
            catch(Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }

        public static string GenerateToken(string username, string password, string ip, string userAgent, long ticks)
        {
            string hash = string.Join(":", new string[] { username, ip, userAgent, ticks.ToString() });
            string hashLeft = "";
            string hashRight = "";
            using (HMAC hmac = HMACSHA256.Create(_alg))
            {
                hmac.Key = Encoding.UTF8.GetBytes(GetHashedPassword(password));
                hmac.ComputeHash(Encoding.UTF8.GetBytes(hash));
                hashLeft = Convert.ToBase64String(hmac.Hash);
                hashRight = string.Join(":", new string[] { username, ticks.ToString() });
            }
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Join(":", hashLeft, hashRight)));
        }
        public static string GetHashedPassword(string password)
        {
            string key = string.Join(":", new string[] { password, _salt });
            using (HMAC hmac = HMACSHA256.Create(_alg))
            {
                // Hash the key.
                hmac.Key = Encoding.UTF8.GetBytes(_salt);
                hmac.ComputeHash(Encoding.UTF8.GetBytes(key));
                return Convert.ToBase64String(hmac.Hash);
            }
        }

        public static HttpClient InitialPaymentGetWayPlan()
        {

            var userName = "rzp_test_MTEvxK5Wq3Uz5y";
            var passwd = "m2z086pwXfeYrfbSBIXmXAi6";

            var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.razorpay.com/v1/");
            var authToken = Encoding.ASCII.GetBytes($"{userName}:{passwd}");

            string _ContentType = "application/json";
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_ContentType));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
            Convert.ToBase64String(authToken));

            return client;
        }
    }

    public static class EmailService
    {
        public static string Send_Email_Cancellation(string user_name, string EmailId, string date,string template_path,string SenderEmail,string SenderEmail_Password,string SenderMailText,string SMTPServer,int SMTPPort, string IsDefaultCredentials,string EnableSSL_Credentials)
        {
            string strFinalHtml = "";
            StreamReader s = File.OpenText(template_path + "Email_Navfarm_Cancellation.html");
            string read = null;
            while ((read = s.ReadLine()) != null)
            {
                strFinalHtml += read;
            }
            s.Close();

            strFinalHtml = strFinalHtml.Replace("{User}", user_name);
            strFinalHtml = strFinalHtml.Replace("{date}", date);

            string MailDeliver = "";
            try
            {
                MailMessage myMessage = new MailMessage();
                myMessage.IsBodyHtml = true;
                myMessage.To.Add(EmailId);
                myMessage.From = new MailAddress(SenderEmail, SenderMailText);
                myMessage.Subject = "Plan Cancellation";
                myMessage.Body = strFinalHtml;
                myMessage.Priority = MailPriority.Normal;
                SmtpClient smtp = new SmtpClient(SMTPServer);
                if (IsDefaultCredentials.ToUpper() == "TRUE")
                {
                    smtp.UseDefaultCredentials = true;
                }
                else
                {
                    NetworkCredential basicAuthenticationInfo = new NetworkCredential(SenderEmail, SenderEmail_Password);
                    smtp.UseDefaultCredentials = false;
                    smtp.Port = SMTPPort;
                    smtp.EnableSsl = Convert.ToBoolean(EnableSSL_Credentials);
                    smtp.Credentials = basicAuthenticationInfo;
                }

                smtp.Send(myMessage);
                MailDeliver = "Sent";
            }
            catch (Exception ex)
            {
                MailDeliver = ex.Message;
            }
            return MailDeliver;
        }

        public static string Send_Email_Upgradation(string user_name, string EmailId, string date, string template_path, string SenderEmail, string SenderEmail_Password, string SenderMailText, string SMTPServer, int SMTPPort, string IsDefaultCredentials,string EnableSSL_Credentials)
        {
            string strFinalHtml = "";
            StreamReader s = File.OpenText(template_path + "Email_Navfarm_Upgrade.html");
            string read = null;
            while ((read = s.ReadLine()) != null)
            {
                strFinalHtml += read;
            }
            s.Close();

            strFinalHtml = strFinalHtml.Replace("{User}", user_name);
            strFinalHtml = strFinalHtml.Replace("{date}", date);

            string MailDeliver = "";
            try
            {
                MailMessage myMessage = new MailMessage();
                myMessage.IsBodyHtml = true;
                myMessage.To.Add(EmailId);
                myMessage.From = new MailAddress(SenderEmail, SenderMailText);
                myMessage.Subject = "Plan Upgradation";
                myMessage.Body = strFinalHtml;
                myMessage.Priority = MailPriority.Normal;
                SmtpClient smtp = new SmtpClient(SMTPServer);
                if (IsDefaultCredentials.ToUpper() == "TRUE")
                {
                    smtp.UseDefaultCredentials = true;
                }
                else
                {
                    NetworkCredential basicAuthenticationInfo = new NetworkCredential(SenderEmail, SenderEmail_Password);
                    smtp.UseDefaultCredentials = false;
                    smtp.Port = SMTPPort;
                    smtp.EnableSsl = Convert.ToBoolean(EnableSSL_Credentials);
                    smtp.Credentials = basicAuthenticationInfo;
                }

                smtp.Send(myMessage);
                MailDeliver = "Sent";
            }
            catch (Exception ex)
            {
                MailDeliver = ex.Message;
            }
            return MailDeliver;
        }
    }

    public class AesOperation
    {
        public static string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string cipherText)
        {
            string key = "b14ca5898a4e4133bbce2ea2315a1918";
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}