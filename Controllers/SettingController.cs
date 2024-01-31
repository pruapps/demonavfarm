using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FarmIT_Api.database_accesslayer;
using FarmIT_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FarmIT_Api.Controllers
{
    [ApiController]
    [Authorize]
    [RESTAuthorizeAttribute]
    public class SettingController : ControllerBase
    {
        private readonly ISettingDB _settingDB;
        public SettingController(ISettingDB settingDB)
        {
            _settingDB = settingDB;
        }

        [Route("api/get_roles")]
        [HttpGet]
        public IActionResult Get_Roles()
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                int Id = 1;
                DataTable dt = _settingDB.Get_Roles();
                if (dt.Rows.Count > 0)
                {
                    var roles = (from DataRow dr in dt.Rows
                                      select new RoleModel()
                                      {
                                          ROLE_ID = Convert.ToInt32(dr["ROLE_ID"].ToString()),
                                          ROLE_NAME = dr["ROLE_NAME"].ToString(),
                                          ROLEACTIVEINDEX = (Id == Convert.ToInt32(dr["ROLE_ID"].ToString())) ? 1 : 0
                                      }).ToList();

                    obj.Status = "success";
                    obj.Message = "Roles Data.";
                    obj.Data = new { roles };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = "Data is not available.";
                    obj.Data = new { };
                }
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }

        [Route("api/get_modulepagepermission")]
        [HttpGet]
        public IActionResult Get_ModulePagePermission(int Role_Id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                DataSet ds = _settingDB.GetModulePagePermissions(Role_Id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var modules = (from DataRow dr in ds.Tables[0].Rows
                                        select new ModuleModel()
                                        {
                                            MODULE_ID = Convert.ToInt32(dr["MODULE_ID"].ToString()),
                                            MODULE_NAME = dr["MODULE_NAME"].ToString(),
                                            STATUS = dr["STATUS"].ToString(),
                                            ROLEACTIVEINDEX = Convert.ToInt32(dr["ISACTIVE"].ToString())
                                        }).ToList();
                    DataTable dtPagePermission = ds.Tables[1];

                    #region Create table with columns

                    DataTable dtfilterPagePermission = new DataTable();

                    dtfilterPagePermission.Columns.Add("ROLE_PERMISSION_ID");
                    dtfilterPagePermission.Columns.Add("ROLE_ID");
                    dtfilterPagePermission.Columns.Add("PAGE_ID");

                    dtfilterPagePermission.Columns.Add("PERMISSION_ID");
                    dtfilterPagePermission.Columns.Add("MODULE_ID");
                    dtfilterPagePermission.Columns.Add("STATUS");
                    dtfilterPagePermission.Columns.Add("PAGE_NAME");
                    dtfilterPagePermission.Columns.Add("MODULE_NAME");

                    int noOfColumns = dtPagePermission.Columns.Count - 1;
                    int noOfColumnsDynamic = 0;

                    DataView view = new DataView(dtPagePermission);
                    DataTable distinctValues = view.ToTable(true, "PERMISSION_NAME");
                    foreach (DataRow pname in distinctValues.Rows)
                    {
                        dtfilterPagePermission.Columns.Add(pname["PERMISSION_NAME"].ToString());
                        noOfColumnsDynamic += 1;
                    }

                    #endregion

                    var pageName = "";
                    var moduleName = "";
                    int fillColumns = 0;
                    DataRow row = dtfilterPagePermission.NewRow();

                    foreach (DataRow pp in dtPagePermission.Rows)
                    {

                        if (pageName == "" || (pageName != pp["PAGE_NAME"].ToString()) || (moduleName != pp["MODULE_NAME"].ToString()))
                        {
                            pageName = pp["PAGE_NAME"].ToString();
                            moduleName = pp["MODULE_NAME"].ToString();
                            fillColumns = 1;
                            row = dtfilterPagePermission.NewRow();

                            row["ROLE_PERMISSION_ID"] = pp["ROLE_PERMISSION_ID"].ToString();
                            row["ROLE_ID"] = pp["ROLE_ID"].ToString();
                            row["PAGE_ID"] = pp["PAGE_ID"].ToString();
                            row["PERMISSION_ID"] = pp["PERMISSION_ID"].ToString();
                            row["MODULE_ID"] = pp["MODULE_ID"].ToString();
                            row["STATUS"] = pp["STATUS"].ToString();
                            row["PAGE_NAME"] = pp["PAGE_NAME"].ToString();
                            row["MODULE_NAME"] = pp["MODULE_NAME"].ToString();
                            row[noOfColumns] = pp["ROLE_PERMISSION_ID"] + "~" + pp["STATUS"].ToString();
                        }
                        else if (pageName == pp["PAGE_NAME"].ToString() && moduleName == pp["MODULE_NAME"].ToString())
                        {
                            row[noOfColumns + fillColumns] = pp["ROLE_PERMISSION_ID"] + "~" + pp["STATUS"].ToString();
                            fillColumns += 1;
                        }
                     //   If original value including dynamic column
                        if (noOfColumnsDynamic == fillColumns)
                        {
                            dtfilterPagePermission.Rows.Add(row);
                        }
                    }

                    obj.Status = "success";
                    obj.Message = "Module/Page Permission Data.";
                    obj.Data = new { modules, pagePermission = dtfilterPagePermission, fixedColumns = noOfColumns };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = "Data is not available.";
                    obj.Data = new { };
                }
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }

        [Route("api/updaterolepermission")]
        [HttpPost]
        public IActionResult UpdateRolePermission(List<RolePermissionsStatus> rolePermissions)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
               string res = _settingDB.UpdateRolePermission(rolePermissions);
               if(res == "success")
                {
                    obj.Status = "success";
                    obj.Message = "Successfully assign permission.";
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = res;
                    obj.Data = new { };
                }
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }

        [Route("api/getPagePermissionsForUser")]
        [HttpGet]
        public IActionResult GetPagePermissionsForUser(int Role_Id,int User_Id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                var dt = _settingDB.GetPagePermissionsForUser(Role_Id,User_Id);
                if (dt.Rows.Count > 0)
                {
                    var page_permission = (from DataRow dr in dt.Rows
                                           select new PagesPermissionModel()
                                           {
                                               ROLE_PERMISSION_ID = Convert.ToInt32(dr["ROLE_PERMISSION_ID"].ToString()),
                                               ROLE_ID = Convert.ToInt32(dr["ROLE_ID"].ToString()),
                                               PAGE_ID = Convert.ToInt32(dr["PAGE_ID"].ToString()),
                                               PERMISSION_ID = Convert.ToInt32(dr["PERMISSION_ID"].ToString()),
                                               MODULE_ID = Convert.ToInt32(dr["MODULE_ID"].ToString()),
                                               STATUS = dr["STATUS"].ToString(),
                                               PAGE_NAME = dr["PAGE_NAME"].ToString(),
                                               MODULE_NAME = dr["MODULE_NAME"].ToString(),
                                               PERMISSION_NAME = dr["PERMISSION_NAME"].ToString(),
                                               UPSTATUS = dr["UPSTATUS"].ToString()
                                           }).ToList();

                    obj.Status = "success";
                    obj.Message = "User Page Permission Data.";
                    obj.Data = new { page_permission };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = "Data is not available."; 
                    obj.Data = new { };
                }
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }

        [Route("api/getUser_Details")]
        [HttpGet]
        public IActionResult GetUser_Details(int User_Id, int Company_Id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                var ds = _settingDB.GetUser_Details(User_Id, Company_Id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var user_details = (from DataRow dr in ds.Tables[0].Rows
                                           select new User_List()
                                           {
                                               user_id = Convert.ToInt32(dr["USER_ID"].ToString()),
                                               user_name = dr["USER_NAME"].ToString(),
                                               company_name = dr["COMPANY_NAME"].ToString(),
                                               mobile_no = dr["MOBILE_NO"].ToString(),
                                               email = dr["EMAIL"].ToString(),
                                               location_name = dr["LOCATION_NAME"].ToString(),
                                               location_id = Convert.ToInt32(dr["LOCATION_ID"].ToString()),
                                               role_name = dr["ROLE_NAME"].ToString(),
                                               role_id = Convert.ToInt32(dr["ROLE_ID"].ToString()),
                                               status = dr["STATUS"].ToString(),
                                               sub_location_name = dr["SUB_LOCATION_NAME"].ToString(),
                                               sub_location_id = Convert.ToInt32(dr["SUB_LOCATION_ID"].ToString()),
                                           }).ToList();

                    var roles = (from DataRow dr in ds.Tables[1].Rows
                                        select new RoleModel()
                                        {
                                            ROLE_ID = Convert.ToInt32(dr["ROLE_ID"].ToString()),
                                            ROLE_NAME = dr["ROLE_NAME"].ToString()
                                        }).ToList();

                    var locations = (from DataRow dr in ds.Tables[2].Rows
                                 select new SettingModel()
                                 {
                                     location_id = Convert.ToInt32(dr["LOCATION_ID"].ToString()),
                                     location_name = dr["LOCATION_NAME"].ToString()
                                 }).ToList();

                    obj.Status = "success";
                    obj.Message = "User Data.";
                    obj.Data = new { roles,locations,user_details };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = "Data is not available.";
                    obj.Data = new { };
                }
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }

        [Route("api/updateUser_Details")]
        [HttpPost]
        public IActionResult UpdateUser_Details(SystemUser su)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                string res = _settingDB.UpdateSystemUser(su.User_Id,su.Location,su.sub_location_id);
                if (res == "success")
                {
                    string resPermission = "Not updated";
                    if (su.userPermissions != null)
                    {                        
                        resPermission = _settingDB.UpdateUserRolePermission(su.userPermissions.AsEnumerable(),su.Created_By);
                    }
                    obj.Status = "success";
                    obj.Message = "Successfully assign location.";
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = res;
                    obj.Data = new { };
                }
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }

        [Route("api/getUserPermissionByRoleId")]
        [HttpGet]
        public IActionResult GetUserPermissionByRoleId(int Role_Id, int User_Id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                #region Create user permission

                DataTable dtPagePermission = _settingDB.GetPagePermissionsForUser(Role_Id, User_Id);

                #region Create table with columns

                DataTable PagePermission = new DataTable();

                PagePermission.Columns.Add("ROLE_PERMISSION_ID");
                PagePermission.Columns.Add("ROLE_ID");
                PagePermission.Columns.Add("PAGE_ID");
                PagePermission.Columns.Add("PERMISSION_ID");
                PagePermission.Columns.Add("MODULE_ID");
                PagePermission.Columns.Add("STATUS");
                PagePermission.Columns.Add("UPSTATUS");
                PagePermission.Columns.Add("PAGE_NAME");
                PagePermission.Columns.Add("MODULE_NAME");

                int noOfColumns = dtPagePermission.Columns.Count - 1;
                int noOfColumnsDynamic = 0;

                DataView view = new DataView(dtPagePermission);
                DataTable distinctValues = view.ToTable(true, "PERMISSION_NAME");
                foreach (DataRow pname in distinctValues.Rows)
                {
                    PagePermission.Columns.Add(pname["PERMISSION_NAME"].ToString());
                    noOfColumnsDynamic += 1;
                }

                #endregion

                var pageName = "";
                var Module_id = "";
                int fillColumns = 0;
                DataRow row = PagePermission.NewRow();

                foreach (DataRow pp in dtPagePermission.Rows)
                {

                    if (pageName == "" || (pageName != pp["PAGE_NAME"].ToString()))
                    {
                        pageName = pp["PAGE_NAME"].ToString();
                        Module_id= pp["MODULE_ID"].ToString();
                        fillColumns = 1;
                        row = PagePermission.NewRow();

                        row["ROLE_PERMISSION_ID"] = pp["ROLE_PERMISSION_ID"].ToString();
                        row["ROLE_ID"] = pp["ROLE_ID"].ToString();
                        row["PAGE_ID"] = pp["PAGE_ID"].ToString();
                        row["PERMISSION_ID"] = pp["PERMISSION_ID"].ToString();
                        row["MODULE_ID"] = pp["MODULE_ID"].ToString();
                        row["STATUS"] = pp["STATUS"].ToString();
                        row["PAGE_NAME"] = pp["PAGE_NAME"].ToString();
                        row["MODULE_NAME"] = pp["MODULE_NAME"].ToString();
                        row[noOfColumns] = pp["ROLE_PERMISSION_ID"] + "~" + pp["STATUS"].ToString() + "~" + pp["UPSTATUS"].ToString();
                    }
                    else if (pageName == pp["PAGE_NAME"].ToString() && Module_id==pp["MODULE_ID"].ToString())
                    {
                        row[noOfColumns + fillColumns] = pp["ROLE_PERMISSION_ID"] + "~" + pp["STATUS"].ToString() + "~" + pp["UPSTATUS"].ToString();
                        fillColumns += 1;
                    }
                    //If original value including dynamic column 
                    if (noOfColumnsDynamic == fillColumns)
                    {
                        PagePermission.Rows.Add(row);
                    }
                }

                obj.Status = "success";
                obj.Message = "User Page Permission Data.";
                obj.Data = new { PagePermission, fixedColumns = noOfColumns };
                #endregion
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }

        [Route("api/updateuser_locations")]
        [HttpPost]
        public IActionResult UpdateUser_Locations(List<UserLocations> userLocations)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                string res = _settingDB.UpdateUserLocation(userLocations);
                if (res == "success")
                {
                    obj.Status = "success";
                    obj.Message = "Successfully assign location.";
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = res;
                    obj.Data = new { };
                }
            }
            catch (Exception ex)
            {
                obj.Status = "error";
                obj.Message = ex.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }
    }
}