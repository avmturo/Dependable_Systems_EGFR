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

        public static void Login(ISession session, ClinicianModel clinician)
        {
            session.Set(LOGIN_STATUS, new byte[] { (byte)UserType.Clinician });
            session.Set(LOGIN_ID, BitConverter.GetBytes(clinician.Id));
        }

        public static void Login(ISession session, AdminModel admin)
        {
            session.Set(LOGIN_STATUS, new byte[] { (byte)UserType.Admin });
            session.Set(LOGIN_ID, BitConverter.GetBytes(admin.Id));

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

            if(!session.TryGetValue(PATIENT_DETAILS, out byte[] patientBytes))
            {
                return null;
            }

            return PatientDetailsModel.Deserialize(patientBytes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PatientLogin(Patient patient)
        {
            PatientModel patientModel = PatientProcessor.AuthorisePatient(patient.NHSNumber, patient.Password);
            if (patientModel != null)
            {
                PatientDetailsModel patientDetails = null;
                if (patientModel.Details != null)
                {
                    patientDetails = PatientProcessor.LoadPatientDetails((int)patientModel.Details);
                }

                Login(HttpContext.Session, patientModel, patientDetails);
                return RedirectToAction("Index", "Patient");
            }

            ViewData["ErrorMessage"] = "Your login credentials did not match our records. Please try again.";
            return Login(redirect: null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClinicianLogin(Clinician clinician)
        {
            ClinicianModel clinicianModel = ClinicianProcessor.AuthoriseClinician(clinician.HCPId, clinician.ClinicianPassword);
            if (clinicianModel != null)
            {
                Login(HttpContext.Session, clinicianModel);
                return RedirectToAction("Index", "Clinician");
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