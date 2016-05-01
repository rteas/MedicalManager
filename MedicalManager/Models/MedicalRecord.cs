using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalManager.Models
{
    public class MedicalRecord
    {
        public int MedicalRecordId { get; set; }

        public int MedicineTakeTime { get; set; }
        public string MedicalConditions { get; set; }

        public int? PatientId { get; set; }
        public int? MedicineId { get; set; }

        public virtual Patient Patient { get; set; }
        public virtual Medicine Medicine { get; set; }
    }
}