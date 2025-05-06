using System.ComponentModel.DataAnnotations;

namespace ClinicAppointment.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public  Doctor? Doctor { get; set; }

        public int PatientId { get; set; }
        public  Patient? Patient { get; set; }

        [Required]
        [Display(Name = "Appointment Date & Time")]
        [DataType(DataType.DateTime)]
        public DateTime AppointmentDateTime { get; set; }
        [Required]
        public int DurationInMinutes { get; set; }
    }
}
