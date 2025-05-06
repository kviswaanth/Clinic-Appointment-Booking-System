using ClinicAppointment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ClinicAppointment.Pages.Doctors
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _clientFactory;
        public IndexModel(ApplicationDbContext context, IHttpClientFactory clientFactory)
        {
            _context = context;
            _clientFactory = clientFactory;
        }
        public Doctor Doctor { get; set; } = new Doctor();
        public List<Doctor> Doctors { get; set; } = new List<Doctor>();
        public async Task OnGetAsync()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:44355/api/Doctors");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Doctors = JsonSerializer.Deserialize<List<Doctor>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            Doctors = await _context.Doctors.ToListAsync();
        }
        public async Task<IActionResult> OnPostAsync()
        {


            if (!ModelState.IsValid)
                return Page();

            var doctorToSend = new Doctor
            {
                Name = Doctor.Name,
                Specialization = Doctor.Specialization
            };

            var client = _clientFactory.CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(doctorToSend), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:44355/api/doctors", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["successfullyMessage"] = "Doctor added successfully.";
                return RedirectToPage();
            }

            ModelState.AddModelError(string.Empty, "Failed to add doctor.");
            return Page();
        }
    }
}
