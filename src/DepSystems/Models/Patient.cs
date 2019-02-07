using System;

namespace DepSystems.Models
{
    // Front-end Model
    public class Patient
    {
        public string NHSNumber { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Gender { get; set; }
        public int Ethnicity { get; set; }
    }
}
