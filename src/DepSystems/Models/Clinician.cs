using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataLibrary.Models;

namespace DepSystems.Models
{
    public class Clinician
    {
        [Required(ErrorMessage = "Please provide your HCP ID.")]
        [Display(Name = "HCP ID")]
        [MinLength(ClinicianModel.HCP_LENGTH, ErrorMessage = "The HCP ID you provided is too short.")]
        [MaxLength(ClinicianModel.HCP_LENGTH, ErrorMessage = "The HCP ID you provided is too long.")]
        public string HCPId { get; set; }

        [Required(ErrorMessage = "Please provide your password.")]
        [Display(Name = "HCP ID")]
        [DataType(DataType.Password)]
        [MinLength(ClinicianModel.PASSWORD_LENGTH, ErrorMessage = "The password you provided is too short.")]
        [MaxLength(ClinicianModel.PASSWORD_LENGTH, ErrorMessage = "The password you provided is too long.")]
        public string ClinicianPassword { get; set; }

        public static bool IsValidHCPId(string hcpId)
        {
            return hcpId.Length == ClinicianModel.HCP_LENGTH;
        }

        public static bool IsValidPassword(string password)
        {
            return password.Length == ClinicianModel.PASSWORD_LENGTH;
        }

        public static Clinician Convert(ClinicianModel clinicianModel)
        {
            return new Clinician
            {
                HCPId = clinicianModel.HCPId,
                ClinicianPassword = clinicianModel.Password
            };
        }

        public static ClinicianModel Convert(Clinician clinician)
        {
            return new ClinicianModel
            {
                HCPId = clinician.HCPId,
                Password = clinician.ClinicianPassword
            };
        }

        public static List<ClinicianModel> Convert(List<Clinician> clinicians)
        {
            List<ClinicianModel> models = new List<ClinicianModel>();
            foreach(var clinician in clinicians)
            {
                models.Add(Convert(clinician));
            }
            return models;
        }

        public static List<Clinician> Convert(List<ClinicianModel> models)
        {
            List<Clinician> clinicians = new List<Clinician>();
            foreach (var model in models)
            {
                clinicians.Add(Convert(model));
            }
            return clinicians;
        }
    }
}
