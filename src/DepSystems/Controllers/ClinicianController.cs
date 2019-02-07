﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DepSystems.Controllers
{
    public class ClinicianController : Controller
    {
        /// <summary>
        /// Displays the landing page that a clinician sees when logging in
        /// </summary>
        /// <returns></returns>
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