using ClinicAppointment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using System.Text;

namespace ClinicAppointment.Pages.Appointments
{
    public class BookAppointmentModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public BookAppointmentModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

 

        [BindProperty]
        public Appointment Appointment { get; set; } = new Appointment();
        public string? Message { get; set; }
        public List<SelectListItem> DoctorList { get; set; } = new();
        public List<SelectListItem> PatientList { get; set; } = new();

        public async Task OnGetAsync()
        {
            var client = _clientFactory.CreateClient();
            var doctors = await client.GetFromJsonAsync<List<Doctor>>("https://localhost:44355/api/doctors");
            var patients = await client.GetFromJsonAsync<List<Patient>>("https://localhost:44355/api/patients");

            DoctorList = doctors?.Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.Id.ToString()
            }).ToList() ?? new();

            PatientList = patients?.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()
            }).ToList() ?? new();
    
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            
            var fullDateTime = Appointment.AppointmentDateTime;
            var DurationInMinutes = Appointment.DurationInMinutes;
            var appointmentToSend = new Appointment
            {
                DoctorId = Appointment.DoctorId,
                PatientId = Appointment.PatientId,
                AppointmentDateTime = fullDateTime,
                DurationInMinutes = Appointment.DurationInMinutes
            };

            var client = _clientFactory.CreateClient();
            var response = await client.PostAsync("https://localhost:44355/api/appointments",
                new StringContent(JsonSerializer.Serialize(appointmentToSend), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Appointment Book successfully!";
                return Page();

            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"Error: {error}");
            ViewData["ErrorMessage"] = "This time slot is already booked for the doctor.";
            return Page();
        }
    }
}
