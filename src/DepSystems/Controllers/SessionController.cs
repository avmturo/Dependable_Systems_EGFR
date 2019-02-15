using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DepSystems.Models;
using DataLibrary.BusinessLogic;
using DataLibrary.Models;
using DepSystems.Enums;
using System;

namespace DepSystems.Controllers
{
    public class SessionController : Controller
    {
        private const string LOGIN_STATUS = "LOGIN_STATUS";
        private const string LOGIN_ID = "LOGIN_ID";
        private const string PATIENT_DETAILS = "PATIENT_DETAILS";

        public static void Login(ISession session, PatientModel patient, PatientDetailsModel patientDetails)
        {
            session.Set(LOGIN_STATUS, new byte[] { (byte)UserType.Patient } );
            session.Set(LOGIN_ID, BitConverter.GetBytes(patient.Id));

            if (patientDetails != null)
            {
                session.Set(PATIENT_DETAILS, patientDetails.Serialize());
            }
        }

        public static void Logout(ISession session)
        {
            session.Remove(LOGIN_STATUS);
            session.Remove(LOGIN_ID);
            session.Remove(PATIENT_DETAILS);
        }

        public static UserType GetLoggedInUserType(ISession session)
        {
            UserType user = UserType.None;
            byte[] loginStatus = session.Get(LOGIN_STATUS);

            if (loginStatus != null)
            {
                user = (UserType)loginStatus[0];
            }

            return user;
        }

        public static int GetId(ISession session)
        {
            if (GetLoggedInUserType(session) == UserType.None)
            {
                // Throw exception instead?
                return -1;
            }
            
            return BitConverter.ToInt32(session.Get(LOGIN_ID));
        }

        public static PatientDetailsModel GetPatientDetails(ISession session)
        {
            if(GetLoggedInUserType(session) != UserType.Patient)
            {
                return null;
            }

            byte[] patientBytes;
            if(!session.TryGetValue(PATIENT_DETAILS, out patientBytes))
            {
                return null;
            }

            return PatientDetailsModel.Deserialize(patientBytes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginDetails loginDetails)
        {
            PatientModel patient = PatientProcessor.AuthorisePatient(loginDetails.Id, loginDetails.Password);
            if(patient != null)
            {
                PatientDetailsModel patientDetails = null;
                if(patient.Details != null)
                {
                    patientDetails = PatientProcessor.LoadPatientDetails((int)patient.Details);
                }

                Login(HttpContext.Session, patient, patientDetails);
                return RedirectToAction("Index", "Patient");
            }

            ViewData["ErrorMessage"] = "Your login credentials did not match our records. Please try again.";
            return Login(redirect: null);
        }

        [HttpGet]
        public IActionResult Login(string redirect)
        {
            if(redirect != null)
            {
                ViewData["ErrorMessage"] = "You must be logged in to view that.";
                ViewData["Redirect"] = (string)redirect;
            }
            return View();
        }

        public IActionResult Logout()
        {
            Logout(HttpContext.Session);
            return RedirectToAction("Index", "Home");
        }
    }
}