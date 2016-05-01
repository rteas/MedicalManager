using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalManager.Models
{
    public class Activity
    {
        public int ActivityId { get; set; }

        public string ActivityType { get; set; }
        public int ActivityTime { get; set; }

        public virtual ICollection<ActivityRecord> ActivityRecords { get; set; }
    }
}