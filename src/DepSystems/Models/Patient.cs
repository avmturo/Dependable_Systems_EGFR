using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using DepSystems.Enums;

namespace DepSystems.Models
{
    public class Patient
    {
        [Display(Name = "NHS Number")]
        public string NHSNumber { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public PatientDetails Details { get; set; }
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
