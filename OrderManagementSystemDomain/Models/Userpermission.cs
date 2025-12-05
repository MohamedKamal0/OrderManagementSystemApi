using OrderManagementSystemDomain.Enums;

namespace OrderManagementSystemDomain.Models
{
    public class Userpermission
    {
        public int UserId { get; set; }
        public Permission PermissionId { get; set; }
    }
}
