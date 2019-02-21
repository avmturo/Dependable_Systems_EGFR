using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DepSystems.Enums;
using DepSystems.Filters;
using Microsoft.AspNetCore.Mvc;
using DepSystems.Models;

namespace DepSystems.Controllers
{
    public class AdminController : Controller
    {
        // Login view (get)
        // Login via db (post)
        // create clinician account (get)
        // create clinician account (post) 

        [CustomValidate(UserType.Admin, "/Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginDetails());
        }

        [HttpPost]
        public IActionResult Login(Admin admin)
        {
            return View();
        }
    }
}