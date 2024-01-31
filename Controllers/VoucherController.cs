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
    public class VoucherController : ControllerBase
    {
        private IVoucherDB _voucherDB;
        public VoucherController(IVoucherDB voucherDB)
        {
            _voucherDB = voucherDB;
        }

        [Route("api/insert_voucher")]
        [HttpPost]
        public IActionResult Insert_Voucher(VoucherModel vm)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if(vm.status.ToLower() == "active")
                {
                    if (vm.type == null)
                    {
                        obj.Status = "failure";
                        obj.Message = "Page type can not blank.";
                        obj.Data = new { };
                        return Ok(obj);
                    }
                    if (vm.expense_date == null)
                    {
                        obj.Status = "failure";
                        obj.Message = "Date can not blank.";
                        obj.Data = new { };
                        return Ok(obj);
                    }
                    if (vm.expense_amount == 0)
                    {
                        obj.Status = "failure";
                        obj.Message = "Amount can not blank.";
                        obj.Data = new { };
                        return Ok(obj);
                    }
                    if (vm.description == null)
                    {
                        obj.Status = "failure";
                        obj.Message = "Description can not blank.";
                        obj.Data = new { };
                        return Ok(obj);
                    }
                    if (vm.company_id == 0)
                    {
                        obj.Status = "failure";
                        obj.Message = "Company can not blank.";
                        obj.Data = new { };
                        return Ok(obj);
                    }
                }
                else
                {
                    if (vm.expense_id == 0)
                    {
                        obj.Status = "failure";
                        obj.Message = "expense id can not blank.";
                        obj.Data = new { };
                        return Ok(obj);
                    }
                }
                
                string[] str = (_voucherDB.insert_voucher(vm.type,vm.expense_id,vm.expense_date,vm.expense_amount,vm.description,vm.status,vm.company_id,vm.created_by,vm.batch_id,vm.quantity,vm.item_id,vm.unit_cost)).Split(',');
                if (str[0] == "success")
                {
                    var expense_id = str[2];
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { expense_id };
                }
                else if (str[0] == "update")
                {
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { };
                }
                else if (str[0] == "delete")
                {
                    obj.Status = "success";
                    obj.Message = str[1];
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = str[1];
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

        [Route("api/get_voucher_summary")]
        [HttpGet]
        public IActionResult Voucher_Summary(int Company_Id,string type)
        {
            ResponseModel obj = new ResponseModel();
            List<Voucher_Summary> summarry = new List<Voucher_Summary>();
            try
            {
                var dt = _voucherDB.get_voucher_summary(Company_Id,type);
                if (dt.Rows.Count > 0)
                {
                    string voucher_id = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        Voucher_Summary vs = new Voucher_Summary();
                        if (voucher_id != dr["VOUCHER_ID"].ToString())
                        {
                            voucher_id = dr["VOUCHER_ID"].ToString();

                            vs.voucher_id = Convert.ToInt32(dr["VOUCHER_ID"].ToString());
                            vs.voucher_date = dr["VOUCHER_DATE"].ToString();
                            vs.voucher_amount = Convert.ToDecimal(dr["VOUCHER_AMOUNT"].ToString());
                            vs.description = dr["DESCRIPTION"].ToString();
                            vs.batch_id = Convert.ToInt32(dr["batch_id"].ToString());
                            vs.item_id = Convert.ToInt32(dr["item_id"].ToString());
                            vs.quantity = Convert.ToDecimal(dr["quantity"].ToString());
                            vs.item_name = dr["Item_name"].ToString();
                            vs.batch_no = dr["batch_no"].ToString();
                            vs.uom = dr["UOM"].ToString();
                            vs.unit_cost = Convert.ToDecimal(dr["UNIT_COST"].ToString());
                            List<File_Details> files = new List<File_Details>();
                            foreach (DataRow dr1 in dt.Select("VOUCHER_ID = '" + voucher_id + "'"))
                            {
                                File_Details fd = new File_Details();
                                if(Convert.ToInt32(dr1["FILE_ID"].ToString())!=0)
                                {
                                    fd.file_id = Convert.ToInt32(dr1["FILE_ID"].ToString());
                                    fd.file_name = dr1["FILE_NAME"].ToString();

                                    files.Add(fd);
                                }                                
                            }
                            vs.files = files;

                            summarry.Add(vs);
                        }
                    }

                    obj.Status = "success";
                    obj.Message = "Voucher summary data.";
                    obj.Data = new { summarry };
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

        [Route("api/delete_voucher_file")]
        [HttpPost]
        public IActionResult Voucher_File_Delete(int File_Id)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (File_Id == 0)
                {
                    obj.Status = "failure";
                    obj.Message = "File id can not blank.";
                    obj.Data = new { };
                    return Ok(obj);
                }

                string res = _voucherDB.Delete_Voucher_File(File_Id, "voucher");

                if (res == "success")
                {
                    obj.Status = "success";
                    obj.Message = "successfully deleted file.";
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