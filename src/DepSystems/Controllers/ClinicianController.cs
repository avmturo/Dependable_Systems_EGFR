using Microsoft.AspNetCore.Mvc;
using DepSystems.Filters;
using DepSystems.Enums;
using DepSystems.Models;
using System.Collections.Generic;

namespace DepSystems.Controllers
{
    public class ClinicianController : Controller
    {
        ////https://www.ryadel.com/en/asp-net-mvc-fix-ambiguous-action-methods-errors-multiple-action-methods-action-name-c-sharp-core/

        /// <summary>
        /// Displays the landing page that a clinician sees when logging in
        /// </summary>
        /// <returns></returns>
        [CustomValidate(UserType.Clinician, "/Clinician")]
        public IActionResult Index()
        {
            return View();
        }

        [CustomValidate(UserType.Clinician, "/Clinician/ImportPatients")]
        public IActionResult ImportPatients()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ImportPatientCredentials(ImportPatientCredentials importPatientCredentials)
        {
            // if all goes well, redirect to import patients with a success message
            // otherwise, r
            if(!CsvProcessor.IsCsv(importPatientCredentials.File))
            {
                ViewData["ErrorMessage"] = "Could not upload patients, the file provided was not a CSV file.";
                return View(viewName: "ImportPatients");
            }

            var errorMessages = CsvProcessor.GetPatientCredentials(importPatientCredentials.File, out List<Patient> parsedPatients);
            if(errorMessages.Count != 0)
            {
                ViewData["ErrorMessages"] = errorMessages;
            }

            // No patients parsed then return with errors. If there are patients parsed but the user does not want to upload
            // if there are errors, then simply return with errors
            if(parsedPatients.Count == 0 || !importPatientCredentials.UploadWithErrors)
            {
                ViewData["ErrorMessage"] = "No patients were uploaded, check the file matches the format required.";
                return View(viewName: "ImportPatients");
            }

            // TODO: Update the database

            ViewData["SuccessMessage"] = $"{parsedPatients.Count} Patients were uploaded successfully.";
            return View(viewName: "ImportPatients");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult ManualPatientCredentials(ManualPatientCredentials manualPatientCredentials)
        //{

        //    return View(viewName: "ImportPatients");
        //}



        [CustomValidate(UserType.Clinician, "/Clinician/CalculateMultiple")]
        public IActionResult CalculateMultiple()
        {
            return View();
        }
    }
}