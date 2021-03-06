﻿using Microsoft.AspNetCore.Mvc;
using DepSystems.Filters;
using DepSystems.Enums;
using DepSystems.Models;
using System.Collections.Generic;
using DataLibrary.BusinessLogic;

namespace DepSystems.Controllers
{
    public class ClinicianController : Controller
    {
        ////https://www.ryadel.com/en/asp-net-mvc-fix-ambiguous-action-methods-errors-multiple-action-methods-action-name-c-sharp-core/

        /// <summary>
        /// Displays the landing page that a clinician sees when logging in
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomValidate(UserType.Clinician, "/Clinician")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Calculation calculation)
        {
            return View(calculation);
        }

        [HttpGet]
        [CustomValidate(UserType.Clinician, "/Clinician/ImportPatients")]
        public IActionResult ImportPatients()
        {
            return View();
        }

        [CustomValidate(UserType.Clinician, "/Clinician/BatchCalculation")]
        public IActionResult BatchCalculation()
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

            List<string> errorMessages = new List<string>();
            List<Patient> patients = CsvProcessor.ReadPatients(importPatientCredentials.File, errorMessages);

            // No patients parsed then return with errors. If there are patients parsed but the user does not want to upload
            // if there are errors, then simply return with errors
            if(patients.Count == 0 || (errorMessages.Count > 0 && !importPatientCredentials.UploadWithErrors))
            {
                ViewData["ErrorMessage"] = "No patients were uploaded, check the file matches the format required.";
                return View(viewName: "ImportPatients");
            }

            int successfulInserts = PatientProcessor.SavePatients(Patient.Convert(patients), errorMessages);

            if(errorMessages.Count != 0)
            {
                ViewData["ErrorMessages"] = errorMessages;
            }

            ViewData["SuccessMessage"] = $"{successfulInserts} Patients were uploaded successfully.";
            return View(viewName: "ImportPatients");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DisplayBatchCalculations(BatchCalculation batchCalculation)
        {
            // if all goes well, redirect to import patients with a success message
            // otherwise, r

            if (!CsvProcessor.IsCsv(batchCalculation.File))
            {
                ViewData["ErrorMessage"] = "Could not upload patients, the file provided was not a CSV file.";
                return View(viewName: "BatchCalculation");
            }
            var errorMessages = CsvProcessor.ReadBatchPatientData(batchCalculation.File, out List<ListCalculations> calculatedPatients);
            if (errorMessages.Count != 0)
            {
                ViewData["ErrorMessages"] = errorMessages;
            }
            return View(viewName: "DisplayBatchCalculations", model: calculatedPatients);
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