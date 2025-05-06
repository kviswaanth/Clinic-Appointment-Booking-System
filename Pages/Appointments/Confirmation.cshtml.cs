using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicAppointment.Pages.Appointments
{
    public class ConfirmationModel : PageModel
    {
        public void OnGet()
        {
            ViewData["successfullyMessage"] = "  successfully booking Doctor appointment..";
        }
    }
}
