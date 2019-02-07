using System;
using System.Collections.Generic;
using DataLibrary.Models;

namespace DataLibrary.BusinessLogic
{
    public static class PatientProcessor
    {
        public static int CreatePatient(string nhsNumber, string password, DateTime dateOfBirth, 
            int gender, int ethnicity)
        {
            PatientModel data = new PatientModel
            {
                NHSNumber = nhsNumber,
                Password = password,
                DateOfBirth = dateOfBirth,
                Gender = gender,
                Ethnicity = ethnicity
            };

            return SqlDataAccess.Save<PatientModel>
            (
                @"INSERT into dbo.Patient (NHSNumber, Password)
                    values (@NHSNumber, @Password);", 
                data
            );
        }

        public static List<PatientModel> LoadPatients()
        {
            return SqlDataAccess.Load<PatientModel>
            (
                @"SELECT NHSNumber, Password
                    FROM dbo.Patient"  
            );
        }

        public static PatientModel AuthorisePatient(string nhsNumber, string password)
        {
            PatientModel data = new PatientModel
            {
                NHSNumber = nhsNumber,
                Password = password
            };

            List<PatientModel> patientModels = SqlDataAccess.Load<PatientModel>
            (
                @"SELECT *
                    FROM dbo.Patient
                    WHERE NHSNumber = @NHSNumber AND Password = @Password",
                data
            );

            if(patientModels.Count == 0)
            {
                return null;
            }
            return patientModels[0];
        }
    }
}
