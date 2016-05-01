using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MedicalManager.DAL;
using MedicalManager.Models;

namespace MedicalManager.Controllers
{
    [Authorize]
    public class MedicalRecordsController : Controller
    {
        private MedicalContext db = new MedicalContext();

        // GET: MedicalRecords
        public ActionResult Index()
        {
            var medicalRecords = db.MedicalRecords.Include(m => m.Medicine).Include(m => m.Patient);
            return View(medicalRecords.ToList());
        }

        // GET: MedicalRecords/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalRecord medicalRecord = db.MedicalRecords.Find(id);
            if (medicalRecord == null)
            {
                return HttpNotFound();
            }
            return View(medicalRecord);
        }

        // GET: MedicalRecords/Create
        public ActionResult Create()
        {
            ViewBag.MedicineId = new SelectList(db.Medicines, "MedicineId", "MedicineType");
            ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "FirstName");
            return View();
        }

        // POST: MedicalRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MedicalRecordId,MedicineTakeTime,MedicalConditions,PatientId,MedicineId")] MedicalRecord medicalRecord)
        {
            if (ModelState.IsValid)
            {
                db.MedicalRecords.Add(medicalRecord);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MedicineId = new SelectList(db.Medicines, "MedicineId", "MedicineType", medicalRecord.MedicineId);
            ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "FirstName", medicalRecord.PatientId);
            return View(medicalRecord);
        }

        public ActionResult PatientMedRec(int? id)
        {
            var patients = db.Patients.Where(p => p.PatientId == id);
            Patient patient = patients.FirstOrDefault();
            if (patient != null) {
                ViewBag.PatientName = patient.LastName + ", " + patient.FirstName;
                ViewBag.PatientId = new SelectList(patients, "PatientId", "FirstName");
                ViewBag.MedicineId = new SelectList(db.Medicines, "MedicineId", "MedicineType");
                return View();
            }
            return Redirect("/patients");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PatientMedRec([Bind(Include = "MedicalRecordId,MedicineTakeTime,MedicalConditions,PatientId,MedicineId")] MedicalRecord medicalRecord)
        {
            if (ModelState.IsValid)
            {
                db.MedicalRecords.Add(medicalRecord);
                db.SaveChanges();
                return Redirect("/patients/details/" + medicalRecord.PatientId); ;
            }
            ViewBag.MedicineId = new SelectList(db.Medicines, "MedicineId", "MedicineType", medicalRecord.MedicineId);
            return View(medicalRecord);
        }
        

        // GET: MedicalRecords/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalRecord medicalRecord = db.MedicalRecords.Find(id);
            if (medicalRecord == null)
            {
                return HttpNotFound();
            }
            ViewBag.MedicineId = new SelectList(db.Medicines, "MedicineId", "MedicineType", medicalRecord.MedicineId);
            ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "FirstName", medicalRecord.PatientId);
            return View(medicalRecord);
        }
       

        // POST: MedicalRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MedicalRecordId,MedicineTakeTime,MedicalConditions,PatientId,MedicineId")] MedicalRecord medicalRecord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medicalRecord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MedicineId = new SelectList(db.Medicines, "MedicineId", "MedicineType", medicalRecord.MedicineId);
            ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "FirstName", medicalRecord.PatientId);
            return View(medicalRecord);
        }

        // GET: MedicalRecords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalRecord medicalRecord = db.MedicalRecords.Find(id);
            if (medicalRecord == null)
            {
                return HttpNotFound();
            }
            return View(medicalRecord);
        }

        // POST: MedicalRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MedicalRecord medicalRecord = db.MedicalRecords.Find(id);
            int patientId = (int)medicalRecord.PatientId;
            db.MedicalRecords.Remove(medicalRecord);
            db.SaveChanges();
            return Redirect("/patients/details/" + patientId);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
