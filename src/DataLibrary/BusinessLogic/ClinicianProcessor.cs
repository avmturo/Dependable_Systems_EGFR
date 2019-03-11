using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.Models;

namespace DataLibrary.BusinessLogic
{
    //public class AdminModel { public int Id; public string Username; public string Password; }
    public static class ClinicianProcessor
    {
        public static int SaveClinician(string hcpId, string password)
        {
            ClinicianModel data = new ClinicianModel
            {
                HCPId = hcpId,
                Password = password
            };

            return SqlDataAccess.Save<ClinicianModel>
            (
                @"INSERT into dbo.Clinician (HCPId, Password)
                    values (@HCPId, @Password)",
                data
            );
        }

        public static int SaveClinicians(List<ClinicianModel> clinicianModels, List<string> errors)
        {
            string sqlStatement = @"INSERT into dbo.Clinician (HCPId, Password)
                    values (@HCPId, @Password)";

            int successfulInserts = SqlDataAccess.SaveList<ClinicianModel>(sqlStatement, clinicianModels);
            if(successfulInserts != clinicianModels.Count)
            {
                errors.Add("Error storing the provided Clinician credentials, all changes were reverted. \nCheck that the HCP Id's provided are unique.");
            }

            // This was an attempt but SaveList returns 0 if any errors are detected, even if rows are inserted
            //int failedIndex = successfulInserts;
            //int lastIndex = clinicianModels.Count - 1;

            //while (failedIndex <= lastIndex)
            //{
            //    errors.Add($"Error storing Clinician {clinicianModels[failedIndex].HCPId}, a record already exists with that ID.");

            //    // If the current index is now the last index, that means the error was on the last index, so break as there is nothing more to process
            //    if(failedIndex == lastIndex)
            //    {
            //        break;
            //    }

            //    ++failedIndex;
            //    int iterativeSuccess = SqlDataAccess.SaveList<ClinicianModel>(sqlStatement, clinicianModels.GetRange(failedIndex, 1 + lastIndex - failedIndex));
            //    successfulInserts += iterativeSuccess;
            //    failedIndex += iterativeSuccess;
            //}

            return successfulInserts;
        }

        public static List<ClinicianModel> GetAllClinicians()
        {
            return SqlDataAccess.Load<ClinicianModel>
            (
                @"SELECT HCPId, Password FROM dbo.Clinician"
            );
        }

        public static ClinicianModel AuthoriseClinician(string hcpId, string password)
        {
            ClinicianModel data = new ClinicianModel
            {
                HCPId = hcpId,
                Password = password
            };

            ClinicianModel clinicianModel = SqlDataAccess.LoadSingle<ClinicianModel>
            (
                @"SELECT Id, HCPId, Password FROM dbo.Clinician
                    WHERE HCPId = @HCPId AND Password = @Password",
                data
            );

            return clinicianModel;
        }

        public static int DeleteClinician(string hcpId)
        {
            return SqlDataAccess.Execute<ClinicianModel>
            (
                @"DELETE FROM dbo.Clinician
                    WHERE HCPId = @HCPId",
                new ClinicianModel { HCPId = hcpId }
            );
        }
    }
}
