using ClinicAppointment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicAppointment.Controllers
{
    [Route("api/patients")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            return Ok(await _context.Patients.ToListAsync());
        }

        [HttpGet("fileNo")]
        public async Task<IActionResult> GetPatientByFileNo(string fileNo)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.FileNo == fileNo);
            if (patient == null)
                return NotFound();
            return Ok(patient);
        }
    }
}
