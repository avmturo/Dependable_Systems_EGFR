using System.ComponentModel.DataAnnotations;
using DataLibrary.Models;

namespace DepSystems.Models
{
    public class Clinician
    {
        [Required]
        [Display(Name = "HCP ID")]
        [MaxLength(ClinicianModel.HCP_LENGTH, ErrorMessage = "The HCP ID you provided is too short.")]
        [MinLength(ClinicianModel.HCP_LENGTH, ErrorMessage = "The HCP ID you provided is too long.")]
        public string HCPId { get; set; }

        [DataType(DataType.Password)]
        [MaxLength(ClinicianModel.PASSWORD_LENGTH, ErrorMessage = "The password you provided is too short.")]
        [MinLength(ClinicianModel.PASSWORD_LENGTH, ErrorMessage = "The password you provided is too long.")]
        [Required(ErrorMessage = "Provide")]
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
    }
}
