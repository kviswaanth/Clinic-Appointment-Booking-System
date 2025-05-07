using ClinicAppointment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Numerics;

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
        [BindProperty]
        public Doctor Doctor { get; set; } = new();
        public List<Doctor> Doctors { get; set; } = new List<Doctor>();
        public async Task OnGetAsync()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:44355/api/Doctors");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Doctors = System.Text.Json.JsonSerializer.Deserialize<List<Doctor>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            Doctors = await _context.Doctors.ToListAsync();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!string.IsNullOrWhiteSpace(Doctor.Name) && !string.IsNullOrWhiteSpace(Doctor.Specialization))
            {
                _context.Doctors.Add(Doctor);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Doctor data saved successfully";
                return RedirectToPage("./Index");

            }
            ModelState.AddModelError(string.Empty, "Doctor Name and Specialization are required.");
            return Page();
        }

    }
}
