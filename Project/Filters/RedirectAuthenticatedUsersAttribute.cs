using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;


namespace Project.Filters
{
    public class RedirectAuthenticatedUsersAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // If the user is authenticated, redirect them to the home page or any other page
                filterContext.Result = new RedirectResult("~/Home/Index");
            }
        }
    }
}