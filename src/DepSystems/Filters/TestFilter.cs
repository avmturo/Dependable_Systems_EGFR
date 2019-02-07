using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DepSystems.Filters
{
    public class TestFilterAttribute : ResultFilterAttribute
    {
        private readonly UserType _userType;
        private readonly int _userTypeInt;

        private readonly string _routePath;

        public TestFilterAttribute(UserType userType, string routePath = "")
        {
            _userType = userType;
            _userTypeInt = (int)_userType;
            _routePath = routePath;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            UserType userType = Controllers.SessionController.Authenticate(context.HttpContext);
            int userTypeInt = (int)userType;

            if(userTypeInt < _userTypeInt)
            {
                context.Result = new RedirectToActionResult("Login", "Session", _routePath);
            }
            base.OnResultExecuting(context);
        }
    }
}