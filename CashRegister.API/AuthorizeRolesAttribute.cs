using CashRegister.Application.Services.Authentification;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CashRegister.API
{
    public class AuthorizeRolesAttribute : Attribute, IAsyncActionFilter
    {
        private readonly List<string> _roles;

        public AuthorizeRolesAttribute(params string[] roles)
        {
            _roles = roles.ToList();
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var authService = context.HttpContext.RequestServices.GetService<IAuthService>();

            var userName = context.HttpContext.User.Identity.Name;

            if (string.IsNullOrEmpty(userName))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

           
            var user = await authService.GetByUsername(userName);

            if (user == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userRoles = await authService.GetUserRoles(user);
            if (!_roles.Any(role => userRoles.Contains(role)))
            {
                context.Result = new ForbidResult();
                return;
            }

            await next();
        }
    }
}
