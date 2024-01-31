using FarmIT_Api.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;

namespace FarmIT_Api.database_accesslayer
{
    public class RESTAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private SqlConnection con;

        /// <summary>  
        /// This will Authorize User  
        /// </summary>  
        /// <returns></returns>  
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext != null)
            {
                Microsoft.Extensions.Primitives.StringValues authTokens;
                filterContext.HttpContext.Request.Headers.TryGetValue("authToken", out authTokens);

                var _token = authTokens.FirstOrDefault();

                if (_token != null)
                {
                    string authToken = _token;

                    if (authToken != null)
                    {
                        if (IsValidToken(authToken))
                        {
                            filterContext.HttpContext.Response.Headers.Add("authToken", authToken);
                            filterContext.HttpContext.Response.Headers.Add("AuthStatus", "Authorized");
                            filterContext.HttpContext.Response.Headers.Add("storeAccessiblity", "Authorized");

                            return;
                        }
                        else
                        {
                            filterContext.HttpContext.Response.Headers.Add("authToken", authToken);
                            filterContext.HttpContext.Response.Headers.Add("AuthStatus", "NotAuthorized");

                            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                            filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Not Authorized";
                            filterContext.Result = new JsonResult("NotAuthorized")
                            {
                                Value = new
                                {
                                    status = "error",
                                    message = "Invalid Token",
                                    data = new { }
                                },
                            };
                        }
                    }
                }
                else
                {
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                    filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Please Provide authToken";
                    filterContext.Result = new JsonResult("Please Provide authToken")
                    {
                        Value = new
                        {
                            status = "error",
                            message = "Please Provide authToken",
                            data = new { }
                        },
                    };
                }
            }
        }
        public bool IsValidToken(string authToken)
        {
            string str = ValidateToken(authToken);

            if(str=="valid")
            {
                return true;
            }
            return false;
        }
        private void connection()
        {
            con = new SqlConnection(AesOperation.DecryptString(Startup.ConnectionString_default));
        }
        public string ValidateToken(string token)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_USER_TOKEN_VALIDATE", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TOKEN", token);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 50;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    res = cmd.ExecuteNonQuery().ToString();
                    res = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
    }
}
