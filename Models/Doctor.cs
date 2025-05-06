﻿namespace ClinicAppointment.Models
{
    public class Doctor
    {
       public int Id { get; set; }
       public string Name { get; set; } = string.Empty;
       public string Specialization { get; set; } = string.Empty;

       public ICollection<Appointment> Appointments { get; set; }
    }
}
