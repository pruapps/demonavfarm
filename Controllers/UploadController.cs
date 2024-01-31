using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using FarmIT_Api.database_accesslayer;
using FarmIT_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FarmIT_Api.Controllers
{
    [ApiController]
    [Authorize]
    [RESTAuthorizeAttribute]
    public class UploadController : ControllerBase
    {
        private IConfigDB _configdb;
        public IConfiguration configuration;
        public UploadController(IConfigDB configDB, IConfiguration iconfig)
        {
            _configdb = configDB;
            configuration = iconfig;
        }
        
        [Route("api/upload_files")]
        [HttpPost]
        public async Task<IActionResult> PostFiles([FromForm]UploadFilesModel uploadFilesModel)
        {
            ResponseModel obj = new ResponseModel();
            try
            {            
                var batch_id = uploadFilesModel.id;
                var files = uploadFilesModel.files;
                if(batch_id != 0)
                {
                    DataTable fileTable = new DataTable("TBL_FILES");
                    fileTable.Columns.Add("BATCH_ID", typeof(int));
                    fileTable.Columns.Add("FILE_NAME", typeof(string));

                    foreach (var formFile in files)
                    {
                        string ImageName = "";
                        if (formFile.Length > 0)
                        {
                            //Set Key Name
                            ImageName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);

                            //Get url To Save
                            string filepath = configuration.GetSection("UploadFilePath").GetSection("FilePath").Value;

                            string SavePath = Path.Combine(filepath, "uploadfiles", ImageName);
                            IFormFile file = formFile;

                            using (var stream = new FileStream(SavePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                        }

                        DataRow dr = null;
                        dr = fileTable.NewRow();
                        dr["BATCH_ID"] = batch_id;
                        dr["FILE_NAME"] = ImageName;
                        fileTable.Rows.Add(dr);
                    }
                    if (fileTable.Rows.Count > 0)
                    {
                        string res = _configdb.Insert_Batch_files(fileTable);
                        if (res == "success")
                        {
                            obj.Status = "success";
                            obj.Message = "File uploaded";
                            obj.Data = new { };
                        }
                        else
                        {
                            obj.Status = "failure";
                            obj.Message = res;
                            obj.Data = new { };
                        }
                    }
                    else
                    {
                        obj.Status = "failure";
                        obj.Message = "no files";
                        obj.Data = new { };
                    }
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = "Batch details can't be blank.";
                    obj.Data = new { };
                }                                
            }
            catch (Exception exp)
            {
                obj.Status = "failure";
                obj.Message = "Exception generated when uploading file - " + exp.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }

        [Route("api/upload_profile_picture")]
        [HttpPost]
        public async Task<IActionResult> ProfileFilePost([FromForm]ProfilePictureModel profilePictureModel)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                var user_id = profilePictureModel.user_id;
                var file = profilePictureModel.file;

                string ImageName = "";
                if (file.Length > 0)
                {
                    //Set Key Name
                    ImageName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    //Get url To Save
                    string filepath = configuration.GetSection("UploadFilePath").GetSection("FilePath").Value;

                    string SavePath = Path.Combine(filepath, "ProfilePictures", ImageName);                   

                    using (var stream = new FileStream(SavePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                string[] res = (_configdb.Insert_Profile_Picture(ImageName,user_id)).Split(',');
                if (res[0] == "success")
                {
                    obj.Status = "success";
                    obj.Message = res[1];
                    obj.Data = new { ImageName };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = res[1];
                    obj.Data = new { };
                }
            }
            catch (Exception exp)
            {
                obj.Status = "failure";
                obj.Message = "Exception generated when uploading file - " + exp.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }

        [Route("api/upload_voucher_files")]
        [HttpPost]
        public async Task<IActionResult> Upload_Voucher_Files([FromForm]UploadFilesModel uploadFilesModel)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                var batch_id = uploadFilesModel.id;
                var files = uploadFilesModel.files;

                DataTable fileTable = new DataTable("TBL_FILES");
                fileTable.Columns.Add("BATCH_ID", typeof(int));
                fileTable.Columns.Add("FILE_NAME", typeof(string));

                foreach (var formFile in files)
                {
                    string ImageName = "";
                    if (formFile.Length > 0)
                    {
                        //Set Key Name
                        ImageName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);

                        //Get url To Save
                        string filepath = configuration.GetSection("UploadFilePath").GetSection("FilePath").Value;

                        string SavePath = Path.Combine(filepath, "VoucherFiles", ImageName);
                        IFormFile file = formFile;

                        using (var stream = new FileStream(SavePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }

                    DataRow dr = null;
                    dr = fileTable.NewRow();
                    dr["BATCH_ID"] = batch_id;
                    dr["FILE_NAME"] = ImageName;
                    fileTable.Rows.Add(dr);
                }
                if (fileTable.Rows.Count > 0)
                {
                    string res = _configdb.Insert_Voucher_files(fileTable);
                    if (res == "success")
                    {
                        obj.Status = "success";
                        obj.Message = "File uploaded";
                        obj.Data = new { };
                    }
                    else
                    {
                        obj.Status = "failure";
                        obj.Message = res;
                        obj.Data = new { };
                    }
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = "no files";
                    obj.Data = new { };
                }
            }
            catch (Exception exp)
            {
                obj.Status = "failure";
                obj.Message = "Exception generated when uploading file - " + exp.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }
        [Route("api/upload_resource_files")]
        [HttpPost]
        public async Task<IActionResult> PostResourceFiles([FromForm] UploadFilesModel uploadFilesModel)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                var batch_id = uploadFilesModel.id;
                var files = uploadFilesModel.files;
                var file_name = "";
                if (batch_id != 0)
                {


                    //foreach (var formFile in files)
                    //{
                    var formFile = files[0];
                        string ImageName = "";
                        if (formFile.Length > 0)
                        {
                            //Set Key Name
                            ImageName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);

                            //Get url To Save
                            string filepath = configuration.GetSection("UploadFilePath").GetSection("FilePath").Value;

                            string SavePath = Path.Combine(filepath, "uploadfiles", ImageName);
                            IFormFile file = formFile;

                            using (var stream = new FileStream(SavePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                        //}



                        file_name= ImageName;
                         
                    }
                    //if (fileTable.Rows.Count > 0)
                    //{
                    //    string res = _configdb.Insert_Batch_files(fileTable);
                    //    if (res == "success")
                    //    {
                    //        obj.Status = "success";
                    //        obj.Message = "File uploaded";
                    //        obj.Data = new { };
                    //    }
                    //    else
                    //    {
                    //        obj.Status = "failure";
                    //        obj.Message = res;
                    //        obj.Data = new { };
                    //    }
                    //}
                    //else
                    //{
                    //    obj.Status = "failure";
                    //    obj.Message = "no files";
                    //    obj.Data = new { };
                    //}

                    obj.Status = "success";
                    obj.Message = "File uploaded";
                    obj.Data = new { };
                }
                else
                {
                    obj.Status = "failure";
                    obj.Message = "Batch details can't be blank.";
                    obj.Data = new { };
                }
            }
            catch (Exception exp)
            {
                obj.Status = "failure";
                obj.Message = "Exception generated when uploading file - " + exp.Message;
                obj.Data = new { };
            }
            return Ok(obj);
        }

    }
}