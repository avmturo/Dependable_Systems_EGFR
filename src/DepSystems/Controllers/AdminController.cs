using DataLibrary.BusinessLogic;
using DataLibrary.Models;
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
            return View();
        }

        [HttpPost]
        public IActionResult Login(Admin admin)
        {
            AdminModel adminModel = AdminProcessor.AuthoriseAdmin(admin.Username, admin.Password);
            if(adminModel != null)
            {
                SessionController.Login(HttpContext.Session, adminModel);
                return View(viewName: "Index");
                // Login
                // return to the index view
            }

            ViewData["ErrorMessage"] = "Incorrect login details";
            return View();
        }
    }
}