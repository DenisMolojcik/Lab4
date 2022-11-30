using lab4.Data;
using lab4.Models;
using lab4.ViewsModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace lab4.Controllers
{
    public class TherapyController : Controller
    {
        private readonly Context _context;
        public TherapyController(Context context)
        {
            _context = context;
        }
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 280)]
        public ActionResult Index(SortState sortTherapy, string searchDoctorName, string searchMedicianName)
        {
            ViewBag.SearchDoctorName = searchDoctorName;
            ViewBag.SearchMedicianName = searchMedicianName;
            IEnumerable<TherapyView> therapyViews = from t in _context.Therapies.ToList()
                                                    join d in _context.Diseases
                                                    on t.DiseaseId equals d.DiseaseId
                                                    join m in _context.Medicianes
                                                    on t.MedicianId equals m.MedicianId
                                                    join doc in _context.Doctors
                                                    on t.DoctorId equals doc.DoctorId
                                                    join p in _context.Patients
                                                    on t.PatientId equals p.PatientId
                                                    select new TherapyView
                                                    {
                                                        Id = t.Id,
                                                        DiseaseName = d.Name,
                                                        MedicianName = m.Name,
                                                        DoctorName = doc.Name,
                                                        PatientName = p.Name,
                                                        Date = t.Date
                                                    };
            therapyViews = _Search(therapyViews, searchDoctorName, searchMedicianName);
            therapyViews = _Sort(therapyViews, sortTherapy);
            return View(therapyViews.ToList());
        }
        private IEnumerable<TherapyView> _Sort(IEnumerable<TherapyView> therapyViews, SortState sortTherapy)
        {
            ViewData["DiseaseName"] = sortTherapy == SortState.DiseaseNameAsc ? SortState.DiseaseNameDesc : SortState.DiseaseNameAsc;
            ViewData["MedicianName"] = sortTherapy == SortState.MedicianNameAsc ? SortState.MedicianNameDesc : SortState.MedicianNameAsc;
            ViewData["DoctorName"] = sortTherapy == SortState.DoctorNameAsc ? SortState.DoctorNameDesc : SortState.DoctorNameAsc;
            therapyViews = sortTherapy switch
            {
                SortState.DiseaseNameAsc => therapyViews.OrderBy(t => t.DiseaseName),
                SortState.DiseaseNameDesc => therapyViews.OrderByDescending(t => t.DiseaseName),
                SortState.MedicianNameAsc => therapyViews.OrderBy(t => t.MedicianName),
                SortState.MedicianNameDesc => therapyViews.OrderByDescending(t => t.MedicianName),
                SortState.DoctorNameAsc => therapyViews.OrderBy(t => t.DoctorName),
                _ => therapyViews.OrderByDescending(t => t.DoctorName),
            };
            return therapyViews;
        }
        private IEnumerable<TherapyView> _Search(IEnumerable<TherapyView> therapyViews, string searchDoctorName, string searchMedicianName)
        {
            if (!String.IsNullOrEmpty(searchDoctorName) && !String.IsNullOrEmpty(searchMedicianName))
            {
                therapyViews = therapyViews.Where(s => s.DoctorName.Contains(searchDoctorName)
                & s.MedicianName.Contains(searchMedicianName));
            }
            return therapyViews;
        }
    }
}
