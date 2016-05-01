using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalManager.Models
{
    public class Medicine
    {
        public int MedicineId { get; set; }

        public string MedicineType { get; set; }
        public int MedicineQuantity { get; set; }

        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; }
    }
}