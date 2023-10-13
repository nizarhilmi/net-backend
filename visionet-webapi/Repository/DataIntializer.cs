using visionet_webapi.Common.Enum;
using visionet_webapi.Models;

namespace visionet_webapi.Repository
{
    public static class DataIntializer
    {
        public static void Run(DataContext db)
        {
            if (db.User.Any()) return;

            // create role
            var adminRole = new Role
            {
                Name = RoleType.Administrator
            };
            db.Role.Add(adminRole);

            var operatorRole = new Role
            {
                Name = RoleType.Operator
            };
            db.Role.Add(operatorRole);

            db.SaveChanges();


            // create privileges
            db.RolePrivilege.Add(new RolePrivilege
            {
                RoleId = adminRole.Id,
                Privilege = PrivilegeType.CreateUser
            });

            db.RolePrivilege.Add(new RolePrivilege
            {
                RoleId = adminRole.Id,
                Privilege = PrivilegeType.ReadUser
            });

            db.RolePrivilege.Add(new RolePrivilege
            {
                RoleId = operatorRole.Id,
                Privilege = PrivilegeType.ReadUser
            });

            db.SaveChanges();
        }
    }
}
