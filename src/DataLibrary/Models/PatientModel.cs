using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class PatientModel
    {
        public int Id { get; set; }
        public string NHSNumber { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Gender { get; set; }
        public int Ethnicity { get; set; }
    }
}