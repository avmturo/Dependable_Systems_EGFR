using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DepSystems.Models
{
    public class BatchCalculation
    {        
        [Required]
        [Display(Name = "Batch File")]
        public IFormFile File { get; set; }

        //[Display(Name = "Tick to upload valid patients even if others are invalid")]
        //public bool UploadWithErrors { get; set; }
    }

}
