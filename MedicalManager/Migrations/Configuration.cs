namespace MedicalManager.Migrations
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MedicalManager.DAL.MedicalContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MedicalManager.DAL.MedicalContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            Patient patient1 = new Patient()
            {
                PatientId = 1,
                FirstName = "John",
                LastName = "Smith",
                EmergencyContact = 1234567890,
                MedicalRecords = new List<MedicalRecord>(),
                ActivityRecords = new List<ActivityRecord>()
            };

            Patient patient2 = new Patient()
            {
                PatientId = 2,
                FirstName = "Alexander",
                LastName = "Great",
                EmergencyContact = 1234567891,
                MedicalRecords = new List<MedicalRecord>(),
                ActivityRecords = new List<ActivityRecord>()
            };

            Patient patient3 = new Patient()
            {
                PatientId = 3,
                FirstName = "Jane",
                LastName = "Doe",
                EmergencyContact = 1234567892,
                MedicalRecords = new List<MedicalRecord>(),
                ActivityRecords = new List<ActivityRecord>()
            };

            Room room1 = new Room() { RoomId = 1, RoomPhoneNumber = 1111111 };
            Room room2 = new Room() { RoomId = 2, RoomPhoneNumber = 2222222 };
            Room room3 = new Room() { RoomId = 3, RoomPhoneNumber = 3333333 };

            Activity activity1 = new Activity() { ActivityId = 1, ActivityType = "Bingo", ActivityTime = 1200 };
            Activity activity2 = new Activity() { ActivityId = 2, ActivityType = "Yoga", ActivityTime = 1200 };

            Medicine medicine1 = new Medicine() { MedicineId = 1, MedicineType = "Aspirin", MedicineQuantity = 50 };
            Medicine medicine2 = new Medicine() { MedicineId = 2, MedicineType = "Antihistamine", MedicineQuantity = 100 };

            MedicalRecord mr1 = new MedicalRecord() { MedicalRecordId = 1, MedicineTakeTime = 1200, MedicalConditions = "Headache", Medicine = medicine1 };
            MedicalRecord mr2 = new MedicalRecord() { MedicalRecordId = 2, MedicineTakeTime = 1200, MedicalConditions = "Allergy", Medicine = medicine2 };
            MedicalRecord mr3 = new MedicalRecord() { MedicalRecordId = 3, MedicineTakeTime = 1200, MedicalConditions = "Headache", Medicine = medicine1 };
            MedicalRecord mr4 = new MedicalRecord() { MedicalRecordId = 4, MedicineTakeTime = 1200, MedicalConditions = "Allergy", Medicine = medicine2 };

            ActivityRecord ar1 = new ActivityRecord() { ActivityRecordId = 1, ActivityId = 1, PatientId = 1, Activity = activity1 };
            ActivityRecord ar2 = new ActivityRecord() { ActivityRecordId = 2, ActivityId = 1, PatientId = 2, Activity = activity1 };
            ActivityRecord ar3 = new ActivityRecord() { ActivityRecordId = 3, ActivityId = 2, PatientId = 3, Activity = activity2 };

            // Link

            mr1.MedicineId = medicine1.MedicineId;
            mr2.MedicineId = medicine2.MedicineId;
            mr3.MedicineId = medicine1.MedicineId;
            mr4.MedicineId = medicine2.MedicineId;

            mr1.PatientId = patient1.PatientId;
            mr2.PatientId = patient2.PatientId;
            mr3.PatientId = patient3.PatientId;
            mr4.PatientId = patient3.PatientId;

            patient1.Room = room1;
            patient2.Room = room2;
            patient3.Room = room3;

            patient1.RoomId = room1.RoomId;
            patient2.RoomId = room2.RoomId;
            patient3.RoomId = room3.RoomId;

            patient1.ActivityRecords.Add(ar1);
            patient2.ActivityRecords.Add(ar2);
            patient3.ActivityRecords.Add(ar3);

            patient1.MedicalRecords.Add(mr1);
            patient2.MedicalRecords.Add(mr2);
            patient3.MedicalRecords.Add(mr3);
            patient3.MedicalRecords.Add(mr4);

            context.Patients.Add(patient1);
            context.Patients.Add(patient2);
            context.Patients.Add(patient3);
        }
    }
}
