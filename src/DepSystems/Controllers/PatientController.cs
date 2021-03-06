﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DepSystems.Filters;
using DepSystems.Enums;
using DepSystems.Models;
using DataLibrary.Models;
using DataLibrary.BusinessLogic;
using System;

namespace DepSystems.Controllers
{
    public class PatientController : Controller
    {
        /// <summary>
        /// Displays the landing page that the patient sees when initially logging in
        /// </summary>
        /// <returns></returns>
        [CustomValidate(UserType.Patient, routePath : @"/Patient")]
        public IActionResult Index()
        {
            var details = SessionController.GetPatientDetails(HttpContext.Session);
            if(details != null)
            {
                Calculation calculation = new Calculation
                {
                    Age = details.DateOfBirth.GetAge(),
                    Gender = (Gender)details.Gender,
                    Ethnicity = (Ethnicity)details.Ethnicity
                };
                
                return View(calculation);
            }
            return View();
        }

        /// <summary>
        /// Displays the calculator for the patients current details
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CustomValidate(UserType.Patient, routePath: @"/Patient")]
        public IActionResult Index(Calculation calculation)
        {
            return View(calculation);
        }


        [HttpGet]
        [CustomValidate(UserType.Patient, routePath: @"/Patient/Details")]
        public IActionResult Details()
        {
            PatientDetailsModel patientDetailsModel = SessionController.GetPatientDetails(HttpContext.Session);
            PatientDetails patientDetails = null;

            // Check that patient details were extracted from the session controller
            // Null details means that the logged on patient does not have details saved yet
            if(patientDetailsModel != null)
            {
                patientDetails = new PatientDetails
                {
                    DateOfBirth = patientDetailsModel.DateOfBirth,
                    Gender = (Gender)patientDetailsModel.Gender,
                    Ethnicity = (Ethnicity)patientDetailsModel.Ethnicity
                };
            }

            return View(patientDetails);
        }
        //[HttpPost]
        //[CustomValidate(UserType.Patient, routePath: @"/Patient/Details")]
        //public IActionResult UpdatePassword(Patient patient)
        //{
        //    PatientModel patientModel = PatientProcessor.LoadPatient((int)patientModel.Details);
        //    PatientDetails patientDetails = null;

        //    // Check that patient details were extracted from the session controller
        //    // Null details means that the logged on patient does not have details saved yet
        //    if (patientDetailsModel != null)
        //    {
        //        patientDetails = new PatientDetails
        //        {
        //            DateOfBirth = patientDetailsModel.DateOfBirth,
        //            Gender = (Gender)patientDetailsModel.Gender,
        //            Ethnicity = (Ethnicity)patientDetailsModel.Ethnicity
        //        };
        //    }

        //    return View(patientDetails);
        //}

        // change password action
        [CustomValidate(UserType.Patient, routePath: @"/Patient/Details")]
        public IActionResult ChangePasswordPage()
        {

            return View("ChangePassword");
        }
        [CustomValidate(UserType.Patient, routePath: @"/Patient/Details")]
        public IActionResult DeleteAccountPage()
        {

            return View("DeleteAccountConfirm");
        }
        [CustomValidate(UserType.Patient, routePath: @"/Patient/DeleteAccountConfirm")]
        public IActionResult DoNotDelete()
        {

            return View("Details");
        }
        [HttpPost]
        [CustomValidate(UserType.Patient, routePath: @"/Patient/DeleteAccountConfirm")]
        public IActionResult DeleteAccountConfirm()
        {
            var details = SessionController.GetPatientDetails(HttpContext.Session);
            int id = details.Id;
            PatientProcessor.DeletePatient(id);
            SessionController.Logout(HttpContext.Session);
            return View("Index");
        }

        [HttpPost]
        [CustomValidate(UserType.Patient, routePath: @"/Patient/ChangePassword")]
        public IActionResult ChangePassword(PatientModel patientM)
        {
            int id = SessionController.GetId(HttpContext.Session);
            PatientModel currentPatient = PatientProcessor.LoadPatient(id);
            String currentPassword = currentPatient.Password;
            if (patientM.Password == currentPassword)
            {
                PatientProcessor.UpdatePatientPassword(id, patientM.NewPassword);
                return PartialView("_StatusMessagePartial", new Tuple<bool, string>(false, $"Password Successfully Updated"));
            }
            else
            {
                return PartialView("_StatusMessagePartial", new Tuple<bool, string>(false, $"Current Password did Not match, Password Not Updated"));
            }

            
        }

        [HttpPost]
        [CustomValidate(UserType.Patient, routePath: @"/Patient/Details")]
        public IActionResult Details(PatientDetails patientDetails)
        {
            PatientDetailsModel sessionDetails = SessionController.GetPatientDetails(HttpContext.Session);
            int result = UpdateDetails(patientDetails, sessionDetails);

            // Add validation
            // Insert details into db, either updating the details or creating a new row in the db
            if (result == 0)
            {
                ViewData["ErrorMessage"] = "There was a problem saving your details.";
            }
            else
            {
                // Update session details
                ViewData["SuccessMessage"] = "Your details have been updated.";
            }
            return View(patientDetails);
        }

        [HttpGet]
        [CustomValidate(UserType.Patient, routePath: @"/Patient/History")]
        public IActionResult History()
        {
            return null;
        }

        private int UpdateDetails(PatientDetails newPatientDetails, PatientDetailsModel sessionDetails)
        {
            int result;
            if (sessionDetails != null)
            {
                sessionDetails.DateOfBirth = newPatientDetails.DateOfBirth;
                sessionDetails.Ethnicity = (int)newPatientDetails.Ethnicity;
                sessionDetails.Gender = (int)newPatientDetails.Gender;
                result = PatientProcessor.UpdatePatientDetails(sessionDetails);
            }
            else
            {
                sessionDetails = new PatientDetailsModel
                {
                    DateOfBirth = newPatientDetails.DateOfBirth,
                    Ethnicity = (int)newPatientDetails.Ethnicity,
                    Gender = (int)newPatientDetails.Gender
                };
                result = PatientProcessor.SavePatientDetails(SessionController.GetId(HttpContext.Session), sessionDetails);
            }

            if(result == 1)
            {
                SessionController.UpdatePatientDetails(HttpContext.Session, sessionDetails);
            }
            return result;
        }
    }
}