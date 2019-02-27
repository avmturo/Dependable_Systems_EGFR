using Microsoft.AspNetCore.Mvc;
using DepSystems.Filters;
using DepSystems.Enums;
using DepSystems.Models;
using System.Collections.Generic;

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
                return RedirectToAction("ImportPatients");
            }

            var errorMessages = CsvProcessor.GetPatientCredentials(importPatientCredentials.File, out List<Patient> parsedPatients);
            if(parsedPatients.Count == 0)
            {
                ViewData["ErrorMessage"] = "No patients were uploaded, check the file matches the format required.";
                ViewData["ImportErrorMessages"] = errorMessages;
                return RedirectToAction("ImportPatients");
            }

            ViewData["SuccessMessage"] = $"{parsedPatients.Count} Patients were uploaded successfully.";
            return RedirectToAction("ImportPatients");
        }



        [CustomValidate(UserType.Clinician, "/Clinician/CalculateMultiple")]
        public IActionResult CalculateMultiple()
        {
            return View();
        }
    }
}