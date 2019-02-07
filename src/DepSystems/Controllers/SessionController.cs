using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using DepSystems.Models;
using DataLibrary.BusinessLogic;
using DataLibrary.Models;

namespace DepSystems.Controllers
{
    public class SessionController : Controller
    {
        public static void Verify(Controller sourceController, IActionResult intendedView, out IActionResult actionResult)
        {
            ISession session = sourceController.HttpContext.Session;
            byte[] loginStatus = session.Get("LOGIN_STATUS");

            if (loginStatus == null || loginStatus[0] == 0)
            {
                actionResult = sourceController.RedirectToAction("Login", "Session", intendedView);
                return;
            }
            actionResult = null;
        }

        public static UserType Authenticate(HttpContext httpContext)
        {
            UserType user = UserType.Any;
            ISession session = httpContext.Session;
            byte[] loginStatus = session.Get("LOGIN_STATUS");

            if (loginStatus != null)
            {
                user = (UserType)loginStatus[0];
            }

            return user;
        }

        [HttpPost]
        public IActionResult Login(LoginDetails loginDetails)
        {
            PatientModel patientModel = PatientProcessor.AuthorisePatient(loginDetails.Id, loginDetails.Password);
            if(patientModel != null)
            {
                HttpContext.Session.Set("LOGIN_STATUS", new byte[] { 1 });
                return RedirectToAction("Index", "Patient");
            }

            return Login();
        }

        [HttpGet]
        public IActionResult Login(string redirectRoute = "")
        {
            return View();
        }
    }
}