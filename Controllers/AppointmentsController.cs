using ClinicAppointment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicAppointment.Controllers
{
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .ToListAsync();
            return Ok(appointments);
        }
        [HttpPost]
        public async Task<IActionResult> BookAppointment([FromBody] Appointment input)
        {
            if (input == null || input.PatientId == 0 || input.DoctorId == 0 || input.AppointmentDateTime == DateTime.MinValue)
                return BadRequest("Invalid appointment data.");

            // Full DateTime comparison for overlap check
            var appointmentDateTime = input.AppointmentDateTime;
            var appointmentEnd = appointmentDateTime.AddMinutes(input.DurationInMinutes);

            bool isOverlapping = await _context.Appointments.AnyAsync(a =>
                a.DoctorId == input.DoctorId &&
                appointmentDateTime < a.AppointmentDateTime.AddMinutes(a.DurationInMinutes) &&
                appointmentEnd > a.AppointmentDateTime);

            if (isOverlapping)
                return BadRequest("This time slot is already booked for the doctor.");

            var appointment = new Appointment
            {
                DoctorId = input.DoctorId,
                PatientId = input.PatientId,
                AppointmentDateTime = input.AppointmentDateTime,
                DurationInMinutes = input.DurationInMinutes
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return Ok(appointment);
        }



        [HttpDelete("{appointmentId}")]
        public async Task<IActionResult> DeleteAppointment(int appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
                return NotFound();

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
