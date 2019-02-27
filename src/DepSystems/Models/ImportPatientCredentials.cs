using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DepSystems.Models
{
    public class ImportPatientCredentials
    {
        [Required]
        [Display(Name = "Patient File")]
        public IFormFile File { get; set; }
    }
}
