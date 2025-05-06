using ClinicAppointment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicAppointment.Controllers
{
    [Route("api/doctors")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DoctorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {
            return Ok(await _context.Doctors.ToListAsync());
        }

        [HttpGet("doctorId/availabletimes")]
        public async Task<IActionResult> GetAvailableTimes(int doctorId, [FromQuery] DateTime date)
        {
            var appointments = await _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.AppointmentDateTime.Date == date.Date)
                .ToListAsync();

            var startHour = 9;
            var endHour = 17;
            var duration = 30;
            var slots = new List<DateTime>();

            for (int hour = startHour; hour < endHour; hour++)
            {
                for (int min = 0; min < 60; min += duration)
                {
                    var slot = new DateTime(date.Year, date.Month, date.Day, hour, min, 0);
                    if (!appointments.Any(a => slot < a.AppointmentDateTime.AddMinutes(a.DurationInMinutes) && slot.AddMinutes(duration) > a.AppointmentDateTime))
                    {
                        slots.Add(slot);
                    }
                }
            }

            return Ok(slots);
        }
        [HttpPost]
        public async Task<IActionResult> AddDoctor([FromBody] Doctor doctor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(AddDoctor), new { id = doctor.Id }, doctor);
        }
       
    }
}
