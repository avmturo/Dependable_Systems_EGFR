﻿using System;
using System.Collections.Generic;
using DataLibrary.Models;

namespace DataLibrary.BusinessLogic
{
    public static class PatientProcessor
    {
        public static int SavePatient(string nhsNumber, string password)
        {
            PatientModel data = new PatientModel
            {
                NHSNumber = nhsNumber,
                Password = password,
            };

            return SqlDataAccess.Save<PatientModel>
            (
                @"INSERT into dbo.Patient (NHSNumber, Password)
                    values (@NHSNumber, @Password);", 
                data
            );
        }

        public static int SavePatients(List<PatientModel> patientModels, List<string> errorMessages)
        {
            string sqlStatement = @"INSERT into dbo.Patient (NHSNumber, Password)
                    values (@NHSNumber, @Password)";

            int successfulInserts = SqlDataAccess.SaveList<PatientModel>(sqlStatement, patientModels);
            if (successfulInserts != patientModels.Count)
            {
                errorMessages.Add("Error storing the provided Patient credentials, all changes were reverted. \nCheck that the NHS Number's provided are unique.");
            }

            return successfulInserts;
        }

        public static PatientModel LoadPatient(int patientId)
        {
            return SqlDataAccess.LoadSingle<PatientModel>
            (
                @"SELECT Id, NHSNumber, Password, FK_PatientDetails_Id as Details FROM dbo.Patient
                    WHERE Id = @Id",
                new PatientModel { Id = patientId }
            );
        }

        public static int UpdatePatientPassword(int id, string newpassword)
        {
            PatientModel patientModel = LoadPatient(id);
            if (patientModel == null)
            {
                return 0;
            }

            patientModel.NewPassword = newpassword;

            return SqlDataAccess.Save<PatientModel>
            (
              @"UPDATE dbo.Patient
                    SET Password = @NewPassword
                    WHERE Id = @Id",
              patientModel
            );
        }
        public static int DeletePatient(int id)
        {
            PatientModel patientModel = LoadPatient(id);
            if (patientModel == null)
            {
                return 0;
            }

            return SqlDataAccess.Save<PatientModel>
            (
              @"DELETE FROM dbo.Patient WHERE Id = @Id",
              patientModel
            );
        }

        public static List<PatientModel> LoadPatients()
        {
            return SqlDataAccess.Load<PatientModel>
            (
                @"SELECT Id, NHSNumber, Password, FK_PatientDetails_Id as Details FROM dbo.Patient"
            );
        }

        public static List<PatientDetailsModel> LoadPatientDetails()
        {
            return SqlDataAccess.Load<PatientDetailsModel>
            (
                @"SELECT * FROM dbo.PatientDetails"
            );
        }

        public static PatientDetailsModel LoadPatientDetails(int id)
        {
            return SqlDataAccess.LoadSingle<PatientDetailsModel>
            (
                @"SELECT * FROM dbo.PatientDetails
                    WHERE Id = @Id",
                new PatientDetailsModel{ Id = id }
            );
        }

        public static PatientModel AuthorisePatient(string nhsNumber, string password)
        {
            if(nhsNumber == null || password == null)
            {
                return null;
            }
            PatientModel data = new PatientModel
            {
                NHSNumber = nhsNumber,
                Password = password
            };

            List<PatientModel> patientModels = SqlDataAccess.Load<PatientModel>
            (
                @"SELECT Id, NHSNumber, Password, FK_PatientDetails_Id as Details FROM dbo.Patient
                    WHERE NHSNumber = @NHSNumber AND Password = @Password",
                data
            );

            if(patientModels.Count == 0)
            {
                return null;
            }
            return patientModels[0];
        }

        public static int SavePatientDetails(int patientId, PatientDetailsModel patientDetailsModel)
        {
            int resultId = SqlDataAccess.SaveReturnId<PatientDetailsModel>
            (
              @"INSERT into dbo.PatientDetails (DateOfBirth, Gender, Ethnicity)
                    VALUES (@DateOfBirth, @Gender, @Ethnicity)",
              patientDetailsModel
            );

            return SqlDataAccess.Save<PatientModel>
            (
              @"UPDATE dbo.Patient
                    SET FK_PatientDetails_Id = @Details
                    WHERE Id = @Id",
              new PatientModel { Id = patientId, Details = resultId }
            );
        }

        public static int UpdatePatientDetails(PatientDetailsModel patientDetailsModel)
        {
            return SqlDataAccess.Save<PatientDetailsModel>
            (
                @"UPDATE dbo.PatientDetails
                    SET DateOfBirth = @DateOfBirth, Gender = @Gender, Ethnicity = @Ethnicity
                    WHERE Id = @Id",
                patientDetailsModel
            );
        }
    }
}
