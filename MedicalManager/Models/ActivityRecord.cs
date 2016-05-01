using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalManager.Models
{
    public class ActivityRecord
    {
        public int ActivityRecordId { get; set; }

        public int? PatientId { get; set; }
        public int? ActivityId { get; set; }

        public virtual Patient Patient {get;set;}
        public virtual Activity Activity { get; set; }
    }
}