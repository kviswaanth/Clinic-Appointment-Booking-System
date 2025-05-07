using ClinicAppointment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAppointment.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginData model)
        {
            if (model.Username == "admin" && model.Password == "Welcome123")
            {
                return Ok(new { message = "Login successful" });
            }
            return Unauthorized(new { message = "Invalid credentials" });
        }
    }
     [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); // Clear all session data
        return Ok(new { message = "Logout successful" });
    }
  }
