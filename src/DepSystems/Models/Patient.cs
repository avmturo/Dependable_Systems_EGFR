using System;
using Microsoft.AspNetCore.Identity;

namespace DepSystems.Models
{
    public class Patient : IdentityUser
    {
        public DateTime DateOfBirth { get; set; }
        public int Gender { get; set; }
        public int Ethnicity { get; set; }
    }
}
