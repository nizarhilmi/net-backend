using Microsoft.AspNetCore.Mvc.Filters;
using visionet_webapi.Common.Enum;
using visionet_webapi.Exceptions;
using visionet_webapi.Repository;

namespace visionet_webapi.Controllers.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AuthorizeRoleAttribute : Attribute, IAuthorizationFilter
    {
        private readonly PrivilegeType privilege;

        public AuthorizeRoleAttribute(PrivilegeType privilege)
        {
            this.privilege = privilege;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous) return;

            var roleId = context.HttpContext.Items["RoleId"];
            if (roleId == null)
                throw new UnauthorizedException();

            var db = context.HttpContext.RequestServices.GetService(typeof(DataContext)) as DataContext;
            if (!ValidatePrivilege(Convert.ToInt32(roleId), db))
                throw new ForbiddenException();
        }

        private bool ValidatePrivilege(int? roleId, DataContext? db)
        {
            if (roleId == null || db == null) return false;

            var roleExist = db.Role.Any(x => x.Id == roleId);
            if (!roleExist) return false;

            return db.RolePrivilege
                .Any(x => x.RoleId == roleId && x.Privilege == privilege);
        }
    }
}
