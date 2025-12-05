using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OrderManagementSystemDomain.Models;
using OrderManagementSystemInfrastructure.Data;

namespace OrderManagementSystemInfrastructure.Authorization
{
    public class PermissionBaseAuthrizationFilter(AppDbContext _dbcontext) : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var attribute = (CheckPermissionAttribute)context.ActionDescriptor.EndpointMetadata
                .FirstOrDefault(x => x is CheckPermissionAttribute);

            if (attribute != null)
            {
                var clamimsIdintity = context.HttpContext.User.Identity as ClaimsIdentity;
                if (clamimsIdintity == null || !clamimsIdintity.IsAuthenticated)
                {
                    context.Result = new ForbidResult();
                }
                else
                {
                    var userId = int.Parse(clamimsIdintity.FindFirst(ClaimTypes.NameIdentifier).Value);
                    var haspermission = _dbcontext.Set<Userpermission>().Any(x => x.UserId == userId &&
                    x.PermissionId == attribute.Permission);
                    if (!haspermission)
                    {
                        context.Result = new ForbidResult();
                    }
                }

            }
        }
    }
}

