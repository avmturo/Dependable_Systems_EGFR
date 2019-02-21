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
    }
}
