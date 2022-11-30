using lab4.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace lab4.Controllers
{
    public class PatientController : Controller
    {
        private readonly Context _context;
        public PatientController(Context context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            return View(_context.Patients.ToList());
        }
    }
}
