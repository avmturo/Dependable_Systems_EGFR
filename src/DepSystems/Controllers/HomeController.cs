using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DepSystems.Models;

namespace DepSystems.Controllers
{
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

		public IActionResult About()
		{
			ViewData["Message"] = "Your application description page.";

			return View();
		}

		public IActionResult Contact()
		{
			ViewData["Message"] = "Your contact page.";

			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
