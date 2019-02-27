using System.ComponentModel.DataAnnotations;

namespace DepSystems.Models
{
    public class ManualPatientCredentials
    {
        [Required]
        public Patient[] Patients { get; set; }
    }
}
