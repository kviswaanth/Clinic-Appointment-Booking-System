using ClinicAppointment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ClinicAppointment.Pages.Patients
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }
        [BindProperty]
        public Patient Patient { get; set; }
        [BindProperty]
        public List<Patient> Patients { get; set; } = new();
        public string FileNo { get; set; }
        public async Task OnGetAsync(string fileNo)
        {
            Patients = new List<Patient>();
            FileNo = fileNo;

            var client = _httpClientFactory.CreateClient();
            if (!string.IsNullOrEmpty(fileNo))
            {
                var response = await client.GetAsync($"https://localhost:44355/api/Patients/fileNo?fileNo={fileNo}");
                if (response.IsSuccessStatusCode)
                {
                    var patient = await response.Content.ReadFromJsonAsync<Patient>();
                    if (patient != null)
                        Patients.Add(patient); // Add single patient to list for Razor display
                }
            }
            else
            {
                // If no filter is applied, show all patients
                Patients = _context.Patients.ToList();
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!string.IsNullOrWhiteSpace(Patient.Name) &&
        !string.IsNullOrWhiteSpace(Patient.PhoneNumber) &&
        !string.IsNullOrWhiteSpace(Patient.FileNo))
            {
                // Check if FileNo already exists
                var exists = await _context.Patients.AnyAsync(p => p.FileNo == Patient.FileNo);
                if (exists)
                {
                    TempData["ErrorMessage"] = "A patient with this File Number already exists.";
                    return Page();
                }

                _context.Patients.Add(Patient);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Patient added successfully!";
                return RedirectToPage();
            }

            TempData["ErrorMessage"] = "Please fill in all fields.";
            return Page();
        }
    }
}
