﻿using System;
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
        [MaxLength(PatientModel.NHS_NUMBER_LENGTH, ErrorMessage = "The NHS Number you provided is too short.")]
        [MinLength(PatientModel.NHS_NUMBER_LENGTH, ErrorMessage = "The NHS Number you provided is too long.")]
        public string NHSNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MaxLength(PatientModel.PASSWORD_LENGTH, ErrorMessage = "The password you provided is too short.")]
        [MinLength(PatientModel.PASSWORD_LENGTH, ErrorMessage = "The password you provided is too long.")]
        public string Password { get; set; }

        public PatientDetails Details { get; set; }

        public static bool IsValidNHSNumber(string nhsNumber)
        {
            // Need to check if all nhs numbers begin with a 2?
            return nhsNumber.Length == PatientModel.NHS_NUMBER_LENGTH;
        }

        public static bool IsValidPassord(string password)
        {
            // Need to check if the rule for patient passwords is that they all start with a p?
            return password.Length == PatientModel.PASSWORD_LENGTH;
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
