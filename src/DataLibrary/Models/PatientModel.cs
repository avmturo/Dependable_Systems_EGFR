namespace DataLibrary.Models
{
    public class PatientModel
    {
        /// <summary>
        /// The required length of a patients NHS Number
        /// </summary>
        public const int NHS_NUMBER_LENGTH = 10;

        /// <summary>
        /// The required length of a patients password
        /// </summary>
        public const int PASSWORD_LENGTH = 7;

        public int Id { get; set; }
        public string NHSNumber { get; set; }
        public string Password { get; set; }
        public int? Details { get; set; }
        public string NewPassword { get; set; }
    }
}