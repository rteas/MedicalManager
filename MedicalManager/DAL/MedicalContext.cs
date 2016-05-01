using MedicalManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace MedicalManager.DAL
{
    public class MedicalContext : DbContext
    {
        public MedicalContext() : base("MedicalContext")
        {

        }

        // Set entity classes
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }

        public System.Data.Entity.DbSet<MedicalManager.Models.ActivityRecord> ActivityRecords { get; set; }
    }
}