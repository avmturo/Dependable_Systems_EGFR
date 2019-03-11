using System;
using System.IO;

namespace DataLibrary.Models
{
    public class PatientDetailsModel
    {
        public int Id { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Gender { get; set; }
        public int Ethnicity { get; set; }

        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(Id);
                    writer.Write(DateOfBirth.ToBinary());
                    writer.Write(Gender);
                    writer.Write(Ethnicity);
                }
                return m.ToArray();
            }
        }

        public static PatientDetailsModel Deserialize(byte[] data)
        {
            PatientDetailsModel patientDetailsModel = new PatientDetailsModel();
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    patientDetailsModel.Id = reader.ReadInt32();
                    patientDetailsModel.DateOfBirth = DateTime.FromBinary(reader.ReadInt64());
                    patientDetailsModel.Gender = reader.ReadInt32();
                    patientDetailsModel.Ethnicity = reader.ReadInt32();
                }
            }

            return patientDetailsModel;
        }
    }
}
