using ClinicAppointment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ClinicAppointment.Pages.Appointments
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

        public List<Appointment> Appointments { get; set; }
        [BindProperty]
        public int AppointmentId { get; set; }
        public void OnGet()
        {
            Appointments = _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var action = Request.Form["action"];
            var appointmentId = Request.Form["AppointmentId"];

            if (action == "Cancel" && !string.IsNullOrEmpty(appointmentId))
            {
                var client = _clientFactory.CreateClient();
                var response = await client.DeleteAsync($"https://localhost:44355/api/Appointments/{appointmentId}");


                if (response.IsSuccessStatusCode)
                {
                    ViewData["ErrorMessage"] = "Appointment canceled successfully.";
                    
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ViewData["ErrorMessage"]= "Appointment not found.";
                }
                else
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Error canceling appointment: {errorMsg}");
                }
            }

            return RedirectToPage();
        }
    }
}
