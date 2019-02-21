using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DepSystems.Filters;
using DepSystems.Enums;

namespace DepSystems.Controllers
{
    public class ClinicianController : Controller
    {
        /// <summary>
        /// Displays the landing page that a clinician sees when logging in
        /// </summary>
        /// <returns></returns>
        [CustomValidate(UserType.Clinician, "/Clinician")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ImportPatients()
        {
            return View();
        }

        public IActionResult CalculateMultiple()
        {
            return View();
        }
    }
}