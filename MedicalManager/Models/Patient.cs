using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MedicalManager.Models
{
    public class Patient
    {

        public int PatientId { get; set; }

        [Required, StringLength(50), Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required, StringLength(50), Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Emergency Contact")]
        public int EmergencyContact { get; set; }

        public int? RoomId { get; set; }
        // Navigation/Relation properties
        public virtual Room Room {get;set;}
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; }
        public virtual ICollection<ActivityRecord> ActivityRecords { get; set; }

    }
}