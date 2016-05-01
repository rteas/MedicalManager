using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MedicalManager.Models
{
    public class Room
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RoomId { get; set; }
        public int RoomPhoneNumber { get; set; }

        public virtual ICollection<Patient> Patients { get; set; }
    }
}