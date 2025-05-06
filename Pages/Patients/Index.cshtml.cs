using ClinicAppointment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicAppointment.Pages.Patients
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Patient> Patients { get; set; }
        public string FileNo { get; set; }
        public void OnGet(string fileNo)
        {
            FileNo = fileNo;

            // Filter the patients by FileNo if it's provided
            if (!string.IsNullOrEmpty(fileNo))
            {
                Patients = _context.Patients
                    .Where(p => p.FileNo.Contains(fileNo))
                    .ToList();
            }
            else
            {
                // If no filter is applied, show all patients
                Patients = _context.Patients.ToList();
            }
        }
    }
}
