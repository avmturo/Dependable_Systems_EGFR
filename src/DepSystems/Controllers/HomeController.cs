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
			return View();
		}

		// This will be called when a HttpPost is posted with the post containing calculation details
		[HttpPost]
		public IActionResult Index(Calculation postedCalculation)
		{
			return View(postedCalculation);
		}

		//public IActionResult PatientDetails(int id)
		//{
		//	var allPatients = PatientProcessor.LoadPatients();
		//	Patient patient;
		//	if (allPatients.Count == 0)
		//	{
		//		patient = new Patient();
		//	}
		//	else
		//	{
		//		patient = new Patient()
		//		{
		//			NHSNumber = allPatients[0].NHSNumber,
		//			Password = allPatients[0].Password,
		//		};
		//	}
		//	return View(patient);
		//}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
