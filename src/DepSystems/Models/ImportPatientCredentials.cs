using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DepSystems.Models
{
    public class ImportPatientCredentials
    {
        [Required]
        [Display(Name = "Patient File")]
        public IFormFile File { get; set; }

        [Display(Name = "Tick to upload valid patients even if others are invalid")]
        public bool UploadWithErrors { get; set; }
    }
}
