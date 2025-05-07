using ClinicAppointment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Text;

namespace ClinicAppointment.Pages
{
    public class LoginModelPage : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginModelPage(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _clientFactory = clientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public LoginData LoginData { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            var username = LoginData.Username;
            var pssword= LoginData.Password;
            var client = _clientFactory.CreateClient();

            var content = new StringContent(
                JsonSerializer.Serialize(LoginData),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("https://localhost:44355/api/Auth/login", content);


            if (response.IsSuccessStatusCode)
            {
                _httpContextAccessor.HttpContext.Session.SetString("IsAuthenticated", "true");
                return RedirectToPage("/Index");
            }

            TempData["ErrorMessage"] = "Invalid credentials.";
            return Page();
        }

    }
}
