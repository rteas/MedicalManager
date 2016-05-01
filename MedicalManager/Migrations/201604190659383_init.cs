namespace MedicalManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        ActivityId = c.Int(nullable: false, identity: true),
                        ActivityType = c.String(),
                        ActivityTime = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ActivityId);
            
            CreateTable(
                "dbo.ActivityRecords",
                c => new
                    {
                        ActivityRecordId = c.Int(nullable: false, identity: true),
                        PatientId = c.Int(),
                        ActivityId = c.Int(),
                    })
                .PrimaryKey(t => t.ActivityRecordId)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Patients", t => t.PatientId)
                .Index(t => t.PatientId)
                .Index(t => t.ActivityId);
            
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        PatientId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        EmergencyContact = c.Int(nullable: false),
                        RoomId = c.Int(),
                    })
                .PrimaryKey(t => t.PatientId)
                .ForeignKey("dbo.Rooms", t => t.RoomId)
                .Index(t => t.RoomId);
            
            CreateTable(
                "dbo.MedicalRecords",
                c => new
                    {
                        MedicalRecordId = c.Int(nullable: false, identity: true),
                        MedicineTakeTime = c.Int(nullable: false),
                        MedicalConditions = c.String(),
                        PatientId = c.Int(),
                        MedicineId = c.Int(),
                    })
                .PrimaryKey(t => t.MedicalRecordId)
                .ForeignKey("dbo.Medicines", t => t.MedicineId)
                .ForeignKey("dbo.Patients", t => t.PatientId)
                .Index(t => t.PatientId)
                .Index(t => t.MedicineId);
            
            CreateTable(
                "dbo.Medicines",
                c => new
                    {
                        MedicineId = c.Int(nullable: false, identity: true),
                        MedicineType = c.String(),
                        MedicineQuantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MedicineId);
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        RoomId = c.Int(nullable: false),
                        RoomPhoneNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RoomId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Patients", "RoomId", "dbo.Rooms");
            DropForeignKey("dbo.MedicalRecords", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.MedicalRecords", "MedicineId", "dbo.Medicines");
            DropForeignKey("dbo.ActivityRecords", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.ActivityRecords", "ActivityId", "dbo.Activities");
            DropIndex("dbo.MedicalRecords", new[] { "MedicineId" });
            DropIndex("dbo.MedicalRecords", new[] { "PatientId" });
            DropIndex("dbo.Patients", new[] { "RoomId" });
            DropIndex("dbo.ActivityRecords", new[] { "ActivityId" });
            DropIndex("dbo.ActivityRecords", new[] { "PatientId" });
            DropTable("dbo.Rooms");
            DropTable("dbo.Medicines");
            DropTable("dbo.MedicalRecords");
            DropTable("dbo.Patients");
            DropTable("dbo.ActivityRecords");
            DropTable("dbo.Activities");
        }
    }
}
