using System.ComponentModel.DataAnnotations;

namespace ClinicAppointment.Models
{
    public class Patient
    {
        public int Id { get; set; }
        [Required]
        public string FileNo { get; set; } = string.Empty; // Unique Identifier
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        public string PhoneNumber { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
