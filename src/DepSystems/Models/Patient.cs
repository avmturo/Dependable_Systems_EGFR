using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataLibrary.Models;
using DepSystems.Enums;

namespace DepSystems.Models
{
    public class Patient
    {
        // GRRRR Need to find a workaround, otherwise our error messages arent helpful enough
        //private const string MAX_LENGTH_ERROR = "The password you provided was too short. Password length must be " + PatientModel.PASSWORD_LENGTH;

        [Required]
        [Display(Name = "NHS Number")]
        [MinLength(PatientModel.NHS_NUMBER_LENGTH, ErrorMessage = "The NHS Number you provided is too short.")]
        [MaxLength(PatientModel.NHS_NUMBER_LENGTH, ErrorMessage = "The NHS Number you provided is too long.")]
        public string NHSNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(PatientModel.PASSWORD_LENGTH, ErrorMessage = "The password you provided is too short.")]
        [MaxLength(PatientModel.PASSWORD_LENGTH, ErrorMessage = "The password you provided is too long.")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(PatientModel.PASSWORD_LENGTH, ErrorMessage = "The password you provided is too short.")]
        [MaxLength(PatientModel.PASSWORD_LENGTH, ErrorMessage = "The password you provided is too long.")]
        public string NewPassword { get; set; }

        public PatientDetails Details { get; set; }

        public string RedirectLink { get; set; }

        public static bool IsValidNHSNumber(string nhsNumber)
        {
            // Need to check if all nhs numbers begin with a 2?
            return nhsNumber.IsNumeric() && nhsNumber.Length == PatientModel.NHS_NUMBER_LENGTH;
        }

        public static bool IsValidPassord(string password)
        {
            // Need to check if the rule for patient passwords is that they all start with a p?
            return password.Length == PatientModel.PASSWORD_LENGTH;
        }

        public static Patient Convert(PatientModel patientModel)
        {
            return new Patient
            {
                NHSNumber = patientModel.NHSNumber,
                Password = patientModel.Password
            };
        }

        public static PatientModel Convert(Patient patient)
        {
            return new PatientModel
            {
                NHSNumber = patient.NHSNumber,
                Password = patient.Password
            };
        }

        public static List<Patient> Convert(List<PatientModel> models)
        {
            List<Patient> patients = new List<Patient>();
            foreach(var model in models)
            {
                patients.Add(Convert(model));
            }
            return patients;
        }

        public static List<PatientModel> Convert(List<Patient> patients)
        {
            List<PatientModel> models = new List<PatientModel>();
            foreach(var patient in patients)
            {
                models.Add(Convert(patient));
            }
            return models;
        }
    }

    public class PatientDetails
    {
        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Please provide your date of birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please provide your gender")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Please provide your ethnicity")]
        public Ethnicity Ethnicity { get; set; }

        public PatientDetails()
        {
            Gender = Gender.Female;
            Ethnicity = Ethnicity.Black;
        }

        public int Age()
        {
            return -1;
        }

        //// Reference:
        //// https://stackoverflow.com/questions/1446547/how-to-convert-an-object-to-a-byte-array-in-c-sharp
        //public byte[] Serialize()
        //{
        //    using (MemoryStream m = new MemoryStream())
        //    {
        //        using (BinaryWriter writer = new BinaryWriter(m))
        //        {
        //            writer.Write(DateOfBirth.ToBinary());
        //            writer.Write(Gender);
        //            writer.Write(Ethnicity);
        //        }
        //        return m.ToArray();
        //    }
        //}

        //public static PatientDetails Deserialize(byte[] data)
        //{
        //    PatientDetails patientDetails = new PatientDetails();
        //    using (MemoryStream m = new MemoryStream(data))
        //    {
        //        using (BinaryReader reader = new BinaryReader(m))
        //        {
        //            string dob = reader.ReadString();
        //            patientDetails.DateOfBirth = DateTime.FromBinary(reader.ReadInt64());
        //            patientDetails.Gender = reader.ReadInt32();
        //            patientDetails.Ethnicity = reader.ReadInt32();
        //        }
        //    }

        //    return patientDetails;
        //}
    }
}
