using System.Web.Mvc;
using AuthorityDomain.DomainLayer;
using AuthorityDomain.ServiceLayer;
using GeneralUtilities.Utilities.Unity;
using MVCWebUIComponent.Models;
using UserDomain.DomainLayer;
using UserDomain.DomainLayer.Entities;
using UserDomain.ServiceLayer;

namespace MVCWebUIComponent.Filter
{
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        public IUserService UserService { get; set; }
        public IAuthorityService AuthorityService { get; set; }

        public UserAuthorizeAttribute()
        {
            UserService = Container.Resolve<UserService>();
            AuthorityService = Container.Resolve<AuthorityService>();
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var userModel = filterContext.HttpContext.Session["CurrentUser"] as UserModel;
            User user = null;
            if (userModel == null)
            {
                // To do: go to login page
            }
            var controller = filterContext.RouteData.Values["controller"].ToString();
            var action = filterContext.RouteData.Values["action"].ToString();
            var isAllowed = IsAllowed(user, controller, action);
            if (!isAllowed)
            {
                filterContext.RequestContext.HttpContext.Response.Write("No access!");
                filterContext.RequestContext.HttpContext.Response.End();
            }
        }

        public bool IsAllowed(User user, string controllerName, string actionName)
        {
            var result = false;
            var role = UserService.GetRoleByUserId(user.UserId);
            var authority = AuthorityService.FindAction(controllerName, actionName);
            if (authority == null)
            {
                authority = AuthorityService.FindController(controllerName);
            }
            if (authority == null)
            {
                return true;
            }
            if (authority.IsAllowedAllRoles)
            {
                if (role != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            result = AuthorityService.IsAccessible(role.RoleId, authority.Id);
            return result;
        }
    }

}