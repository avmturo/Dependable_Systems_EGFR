using DataLibrary.BusinessLogic;
using DataLibrary.Models;
using DepSystems.Enums;
using DepSystems.Filters;
using Microsoft.AspNetCore.Mvc;
using DepSystems.Models;
using System.Collections.Generic;
using System;

namespace DepSystems.Controllers
{
    public class AdminController : Controller
    {
        /// <summary>
        /// The landing page for when the Admin logs in, requires user validation of type Admin to access.
        /// The landing page retrieves all of the stored Clinicians and displays them in a searchable table
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomValidate(UserType.Admin, "/Admin")]
        public IActionResult Index()
        {
            //Clinician
            ClinicianManagerModel clinicianManagerModel = new ClinicianManagerModel
            {
                Clinicians = ConvertModels(ClinicianProcessor.GetAllClinicians()),
            };

            return View(clinicianManagerModel);
        }

        /// <summary>
        /// The view when an Admin posts a file with clinician credentials to the form. Processes the file
        /// and adds the processed clinician credentials to the clinician repository. Outputs a custom message
        /// based on the status of the import action.
        /// </summary>
        /// <param name="clinicianCredentials">The clinician credentials configuration to process</param>
        /// <returns></returns>
        [HttpPost]
        [CustomValidate(UserType.Admin, "//Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ImportClinicianCredentials clinicianCredentials)
        {
            ClinicianManagerModel clinicianManagerModel = new ClinicianManagerModel();

            if(clinicianCredentials.File == null)
            {
                ViewData["ErrorMessage"] = "Could not upload clinicians, no file was provided.";
                clinicianManagerModel.Clinicians = ConvertModels(ClinicianProcessor.GetAllClinicians());
                return View(clinicianManagerModel);
            }

            if (!CsvProcessor.IsCsv(clinicianCredentials.File))
            {
                ViewData["ErrorMessage"] = "Could not upload clinicians, the file provided was not a CSV file.";
                clinicianManagerModel.Clinicians = ConvertModels(ClinicianProcessor.GetAllClinicians());
                return View(clinicianManagerModel);
            }

            var errorMessages = CsvProcessor.GetClinicianCredentials(clinicianCredentials.File, out List<Clinician> clinicians);

            if(clinicians.Count == 0 && !clinicianCredentials.UploadWithErrors)
            {
                ViewData["ErrorMessage"] = "No clinicians were uploaded, check the file matches the format required.";
                clinicianManagerModel.Clinicians = ConvertModels(ClinicianProcessor.GetAllClinicians());
                return View(clinicianManagerModel);
            }

            int successfulInserts = ClinicianProcessor.SaveClinicians(ConvertToModels(clinicians), errorMessages);

            if(errorMessages.Count != 0)
            {
                ViewData["ErrorMessages"] = errorMessages;
            }

            ViewData["SuccessMessage"] = $"{successfulInserts} Clinicians were uploaded successfully.";
            clinicianManagerModel.Clinicians = ConvertModels(ClinicianProcessor.GetAllClinicians());
            return View(clinicianManagerModel);
        }


        [HttpPost]
        [CustomValidate(UserType.Admin)]
        public IActionResult DeleteClinicianCredentials(string HCPId)
        {
            if(ClinicianProcessor.DeleteClinician(HCPId) == 1)
            {
                return PartialView("_StatusMessagePartial", new Tuple<bool, string>(true, $"Successfully deleted {HCPId}"));
            }
            return PartialView("_StatusMessagePartial", new Tuple<bool, string>(false, $"Could not delete {HCPId}"));
        }

        /// <summary>
        /// A simple login page for the admin
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Processes the Admin login action and redirects to the appropriate view
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Login(Admin admin)
        {
            AdminModel adminModel = AdminProcessor.AuthoriseAdmin(admin.Username, admin.Password);
            if(adminModel != null)
            {
                SessionController.Login(HttpContext.Session, adminModel);
                return RedirectToAction("Index");
            }

            ViewData["ErrorMessage"] = "Incorrect login details";
            return View();
        }

        private List<Clinician> ConvertModels(List<ClinicianModel> models)
        {
            List<Clinician> clinicians = new List<Clinician>();
            foreach(var clinicianModel in models)
            {
                clinicians.Add(Clinician.Convert(clinicianModel));
            }

            return clinicians;
        }

        private List<ClinicianModel> ConvertToModels(List<Clinician> clinicians)
        {
            List<ClinicianModel> models = new List<ClinicianModel>();
            foreach(var clinician in clinicians)
            {
                models.Add(Clinician.Convert(clinician));
            }

            return models;
        }
    }
}