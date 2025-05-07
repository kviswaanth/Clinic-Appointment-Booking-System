using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LogoutModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsync("https://localhost:44355/api/Auth/logout", null);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Login");
            }

            // Optionally show an error message
            return RedirectToPage("/Error");
        }
    }
}

