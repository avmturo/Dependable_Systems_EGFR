using System.ComponentModel.DataAnnotations;
using DepSystems.Enums;

namespace DepSystems.Models
{
    public class LoginDetails
    {
        [Display(Name = "Login As")]
        [Range((int)UserType.Patient, (int)UserType.Clinician)]
        public UserType UserType { get; set; }

        [Display(Name = "NHS#/HCP ID")]
        [Required(ErrorMessage = "Please provide your Login ID")]
        public string Id { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please provide your password")]
        public string Password { get; set; }

        public LoginDetails()
        {
            UserType = UserType.Patient;
        }
    }
}
