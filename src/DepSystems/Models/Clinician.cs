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

        //[MaxLength(ClinicianModel.PASSWORD_LENGTH, ErrorMessage = "The password you provided is too short.")]
        //[MinLength(ClinicianModel.PASSWORD_LENGTH, ErrorMessage = "The password you provided is too long.")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Provide")]
        public string ClinicianPassword { get; set; }
    }
}
