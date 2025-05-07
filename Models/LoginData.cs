using System.ComponentModel.DataAnnotations;

namespace ClinicAppointment.Models
{
    public class LoginData
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
