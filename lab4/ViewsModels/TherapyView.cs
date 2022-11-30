using lab4.Models;
using System;

namespace lab4.ViewsModels
{
    public class TherapyView
    {
        public int Id { get; set; }
        public string DiseaseName { get; set; } 
        public string MedicianName { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public DateTime Date { get; set; }
    }
}
