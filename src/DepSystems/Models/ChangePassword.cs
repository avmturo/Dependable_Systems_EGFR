using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepSystems.Models
{
    public class ChangePassword
    {
        public string CurrentPassword { get; set; }
        public string ConfirmCurrentPassword { get; set; }
        public string NewPassword { get; set; }
        
    }
}
