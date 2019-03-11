using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DepSystems.Models
{
    /// <summary>
    /// Import Clinician Credentials is a model that is used to store the clinician 
    /// credentials file and the upload configuration.
    /// </summary>
    public class ImportClinicianCredentials
    {
        /// <summary>
        /// File is the clinician credentials file that will be uploaded
        /// </summary>
        [Required]
        [Display(Name = "Clinician File")]
        public IFormFile File { get; set; }

        /// <summary>
        /// Determines whether to process any valid clinician credentials from the file even
        /// after an error is encountered with other credentials
        /// </summary>
        [Display(Name = "Tick to upload valid clinicians even if others are invalid")]
        public bool UploadWithErrors { get; set; }
    }
}
