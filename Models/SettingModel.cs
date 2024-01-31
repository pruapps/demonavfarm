using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmIT_Api.Models
{
    public class SettingModel
    {
        public int location_id { get; set; }
        public string location_name { get; set; }
    }
    public class RoleModel
    {
        public int ROLE_ID { get; set; }
        public string ROLE_NAME { get; set; }
        public int ROLEACTIVEINDEX { get; set; }
    }
    public class ModuleModel
    {
        public int MODULE_ID { get; set; }
        public string MODULE_NAME { get; set; }
        public string STATUS { get; set; }
        public int ROLEACTIVEINDEX { get; set; }
    }
    public class PagesPermissionModel
    {
        public int ROLE_PERMISSION_ID { get; set; }
        public int ROLE_ID { get; set; }
        public int PAGE_ID { get; set; }
        public int PERMISSION_ID { get; set; }
        public int MODULE_ID { get; set; }
        public string STATUS { get; set; }
        public string PAGE_NAME { get; set; }
        public string MODULE_NAME { get; set; }
        public string PERMISSION_NAME { get; set; }
        public string UPSTATUS { get; set; }
    }
    public class RolePermissionsStatus
    {
        public int ROLE_PERMISSION_ID { get; set; }
        public int ROLE_ID { get; set; }
        public string CHECKED_STATUS { get; set; }
    }
    public class SystemUser
    {
        public int User_Id { get; set; }
        public int sub_location_id { get; set; }
        public string Location { set; get; }  
        public string Created_By { get; set; }
        public List<UserPermissionsStatus> userPermissions { set; get; }
    }
    public class UserPermissionsStatus
    {
        public Int64 USER_ID { get; set; }
        public int ROLE_PERMISSION_ID { get; set; }
        public int ROLE_ID { get; set; }
        public String CHECKED_STATUS { get; set; }
    }
    public class UserLocations
    {
        public int user_id { get; set; }
        public string location { get; set; }
        public int sub_location_id { get; set; }
    }
}
