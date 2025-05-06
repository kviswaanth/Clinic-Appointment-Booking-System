using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicAppointment.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public bool IsAuthenticated { get; set; }
        public IndexModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public void OnGet()
        {
            IsAuthenticated = _httpContextAccessor.HttpContext.Session.GetString("IsAuthenticated") == "true";
        }
        
    }
}
