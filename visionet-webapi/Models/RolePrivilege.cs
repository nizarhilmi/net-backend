using visionet_webapi.Common.Enum;

namespace visionet_webapi.Models
{
    public class RolePrivilege
    {
        public int Id { get; set; }
        public Role Role { get; set; }
        public int? RoleId { get; set; }
        public PrivilegeType Privilege { get; set; }
    }
}
