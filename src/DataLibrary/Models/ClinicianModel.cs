namespace DataLibrary.Models
{
    public class ClinicianModel
    {
        public const int HCP_LENGTH = 7;
        public const int PASSWORD_LENGTH = 10;

        public int Id { get; set; }
        public string HCPId { get; set; }
        public string Password { get; set; }
    }
}
