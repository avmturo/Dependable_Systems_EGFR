using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DepSystems.Models;
using DepSystems.Enums;
using DataLibrary.Models;

namespace DepSystems.ModelValidation
{
    public class LoginUsernameAttribute : ValidationAttribute
    {
        public LoginUsernameAttribute() {}

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            LoginDetails details = (LoginDetails)validationContext.ObjectInstance;
            if(details.UserType == UserType.Patient)
            {
                if(details.Id.Length < PatientModel.NHS_NUMBER_LENGTH)
                {
                    return new ValidationResult($"The NHS Number provided is too short.");
                }
                else if(details.Id.Length > PatientModel.NHS_NUMBER_LENGTH)
                {
                    return new ValidationResult($"The NHS Number provided is too long.");
                }
            }
            else if(details.UserType == UserType.Clinician)
            {

            }
            return new ValidationResult($"You can only login as a patient or clinician.");
        }
    }
}
