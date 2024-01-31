using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmIT_Api.Models
{
    public class UploadFilesModel
    {
        public int id { get; set; }
        public List<IFormFile> files { get; set; }
    }
    public class ProfilePictureModel
    {
        public int user_id { get; set; }
        public IFormFile file { get; set; }
    }
}
