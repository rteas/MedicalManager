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
    public class ActivityRecordsController : Controller
    {
        private MedicalContext db = new MedicalContext();

        // GET: ActivityRecords
        public ActionResult Index()
        {
            var activityRecords = db.ActivityRecords.Include(a => a.Activity).Include(a => a.Patient);
            return View(activityRecords.ToList());
        }

        // GET: ActivityRecords/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityRecord activityRecord = db.ActivityRecords.Find(id);
            if (activityRecord == null)
            {
                return HttpNotFound();
            }
            return View(activityRecord);
        }

        // GET: ActivityRecords/Create
        public ActionResult Create()
        {
            ViewBag.ActivityId = new SelectList(db.Activities, "ActivityId", "ActivityType");
            ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "FirstName");
            return View();
        }

        // POST: ActivityRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ActivityRecordId,PatientId,ActivityId")] ActivityRecord activityRecord)
        {
            if (ModelState.IsValid)
            {
                db.ActivityRecords.Add(activityRecord);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ActivityId = new SelectList(db.Activities, "ActivityId", "ActivityType", activityRecord.ActivityId);
            ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "FirstName", activityRecord.PatientId);
            return View(activityRecord);
        }

        // GET: ActivityRecords/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityRecord activityRecord = db.ActivityRecords.Find(id);
            if (activityRecord == null)
            {
                return HttpNotFound();
            }
            ViewBag.ActivityId = new SelectList(db.Activities, "ActivityId", "ActivityType", activityRecord.ActivityId);
            ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "FirstName", activityRecord.PatientId);
            return View(activityRecord);
        }

        // POST: ActivityRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ActivityRecordId,PatientId,ActivityId")] ActivityRecord activityRecord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activityRecord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ActivityId = new SelectList(db.Activities, "ActivityId", "ActivityType", activityRecord.ActivityId);
            ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "FirstName", activityRecord.PatientId);
            return View(activityRecord);
        }

        // GET: ActivityRecords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityRecord activityRecord = db.ActivityRecords.Find(id);
            if (activityRecord == null)
            {
                return HttpNotFound();
            }
            return View(activityRecord);
        }

        // POST: ActivityRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ActivityRecord activityRecord = db.ActivityRecords.Find(id);
            db.ActivityRecords.Remove(activityRecord);
            db.SaveChanges();
            return RedirectToAction("Index");
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
