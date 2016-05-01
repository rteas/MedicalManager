using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalManager.ViewModels
{
    public class AssignedActivityData
    {
        public int ActivityId { get; set; }
        public string ActivityType { get; set; }
        public int ActivityTime { get; set; }
        public bool Assigned { get; set; }
    }
}