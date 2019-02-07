using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DepSystems.Filters;

namespace DepSystems.Controllers
{
    public class PatientController : Controller
    {
        /// <summary>
        /// Displays the landing page that the patient sees when initially logging in
        /// </summary>
        /// <returns></returns>
        [TestFilter(UserType.Patient, "/Patient")]
        public IActionResult Index()
        {
            //return new RedirectToActionResult("Login", "Session", "/Patient");
            return View();
        }

        /// <summary>
        /// Displays the calculator for the patients current details
        /// </summary>
        /// <returns></returns>
        public IActionResult Calculate()
        {
            return View();
        }
    }
}