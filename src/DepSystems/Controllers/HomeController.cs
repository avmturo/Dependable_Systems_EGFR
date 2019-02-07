using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DepSystems.Models;
using DataLibrary.Models;
using DataLibrary.BusinessLogic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace DepSystems.Controllers
{
	// Things that can be done on the basic home pages...
	// Index
	// Calculation Index
	// Login
	// Logout
	public class HomeController : Controller
	{
		[HttpGet]
		public IActionResult Index()
		{
            // Create the session string
            HttpContext.Session.SetString("blah", "hello world");

            // Get the session string
            ViewData["blah"] = HttpContext.Session.GetString("blah");
			return View();
		}

		// This will be called when a HttpPost is posted with the post containing calculation details
		[HttpPost]
		public IActionResult Index(Calculation postedCalculation)
		{
			return View(postedCalculation);
		}

		public IActionResult Login()
		{
            return Index();
            // This is how we would redirect
			return RedirectToAction("Index", "Patient");
		}

        public IActionResult CreatePatient()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreatePatient(Patient patient)
        {
            PatientProcessor.CreatePatient(patient.NHSNumber, patient.Password, patient.DateOfBirth, patient.Gender, patient.Ethnicity);
            return RedirectToAction("PatientDetails");
        }

        public IActionResult PatientDetails(int id)
        {
            var allPatients = PatientProcessor.LoadPatients();
            Patient patient;
            if (allPatients.Count == 0)
            {
                patient = new Patient();
            }
            else
            {
                patient = new Patient()
                {
                    NHSNumber = allPatients[0].NHSNumber,
                    Password = allPatients[0].Password,
                    DateOfBirth = allPatients[0].DateOfBirth,
                    Gender = allPatients[0].Gender,
                    Ethnicity = allPatients[0].Ethnicity
                };
            }
            return View(patient);
        }

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
