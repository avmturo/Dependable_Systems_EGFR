using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using DepSystems.Enums;

namespace DepSystems.Filters
{
    public class CustomValidateAttribute : ResultFilterAttribute
    {
        private readonly UserType _userType;
        private readonly int _userTypeInt;

        private readonly string _routePath;

        public CustomValidateAttribute(UserType userType, string routePath = "")
        {
            _userType = userType;
            _userTypeInt = (int)_userType;
            _routePath = routePath;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            UserType userType = Controllers.SessionController.GetLoggedInUserType(context.HttpContext.Session);
            int userTypeInt = (int)userType;
            string redirect = _routePath;

            if(userTypeInt != _userTypeInt)
            {
                // The user is actually logged in and trying to access a page without the correct privileges
                if(userType != UserType.None)
                {
                    context.Result = new RedirectToActionResult("NotAuthorised", "Session", null);
                }
                else if (_userType == UserType.Admin)
                {
                    context.Result = new RedirectToActionResult("Login", "Admin", new { redirect });
                }
                else
                {
                    context.Result = new RedirectToActionResult("Login", "Session", new { redirect });
                }
            }
            base.OnResultExecuting(context);
        }
    }
}