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
using PagedList;
using MedicalManager.ViewModels;
using System.Data.Entity.Infrastructure;

namespace MedicalManager.Controllers
{
    [Authorize]
    public class PatientsController : Controller
    {
        private MedicalContext db = new MedicalContext();

        [AllowAnonymous]
        public ActionResult GuestSearch(string id, string firstName, string lastName)
        {
            if (!String.IsNullOrEmpty(lastName))
            {
                Patient toFind = db.Patients.Where(p => 
                p.PatientId.ToString().Equals(id) && 
                p.FirstName.Equals(firstName) && 
                p.LastName.Equals(lastName)).FirstOrDefault();
                if(toFind != null)
                {
                    return View(toFind);
                }
                else
                {
                    ModelState.AddModelError("", "Unable to find patient. Please re-validate your information.");
                }
            }
            return View();
        }

        // GET: Patients
        public ActionResult Index(string sortOrder, string searchName)
        {
            // link parameters that are changable by clicking from view
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.FirstNameSortParam = sortOrder == "FirstName" ? "firstname_desc" : "FirstName";
            ViewBag.RoomSortParam = sortOrder == "Room" ? "room_desc" : "Room";
            ViewBag.IdSortParam = sortOrder == "Id" ? "id_desc" : "Id";

            ViewBag.CurrentFilter = searchName;

            var patients = from p in db.Patients
                           select p;
            // Search for matching first/last name
            if (!String.IsNullOrEmpty(searchName))
            {
                patients = patients.
                    Where(p => p.LastName.Contains(searchName));
            }
            // Sorts by one attribute, desc/asc depending on parameter value
            switch (sortOrder)
            {
                case "name_desc":
                    patients = patients.OrderByDescending(p => p.LastName);
                    break;
                case "Room":
                    patients = patients.OrderBy(p => p.RoomId);
                    break;
                case "room_desc":
                    patients = patients.OrderByDescending(p => p.RoomId);
                    break;
                case "FirstName":
                    patients = patients.OrderBy(p => p.FirstName);
                    break;
                case "firstname_desc":
                    patients = patients.OrderByDescending(p => p.FirstName);
                    break;
                case "Id":
                    patients = patients.OrderBy(p => p.PatientId);
                    break;
                case "id_desc":
                    patients = patients.OrderByDescending(p => p.PatientId);
                    break;
                default:
                    patients = patients.OrderBy(p => p.LastName);
                    break;
            }
            return View(patients.ToList());
        }

        // GET: Patients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // GET: Patients/Create
        public ActionResult Create()
        {
            var patient = new Patient();
            patient.ActivityRecords = new List<ActivityRecord>();
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "RoomId");
            PopulateAssignedActivityData(patient);
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PatientId,FirstName,LastName,EmergencyContact,RoomId")] Patient patient, string[] selectedActivities)
        {
            var activities = db.Activities;
            if(selectedActivities != null)
            {
                patient.ActivityRecords = new List<ActivityRecord>();
                foreach(var activity in selectedActivities)
                {
                    var actToAdd = db.Activities.Find(int.Parse(activity));
                    if(actToAdd != null)
                    {
                        var act = new ActivityRecord
                        {
                            PatientId = patient.PatientId,
                            Patient = patient,
                            ActivityId = actToAdd.ActivityId,
                            Activity = actToAdd
                        };
                        patient.ActivityRecords.Add(act);
                    }
                }
            }
            if (ModelState.IsValid)
            {
                db.Patients.Add(patient);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = patient.PatientId });
            }

            PopulateAssignedActivityData(patient);
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "RoomId", patient.RoomId);
            return View(patient);
        }

        // GET: Patients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            PopulateAssignedActivityData(patient);
            if (patient == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "RoomId", patient.RoomId);
            return View(patient);
        }

        private void PopulateAssignedActivityData(Patient patient)
        {
            var allActivities = db.Activities;
            var viewModel = new List<AssignedActivityData>();
            foreach (var activity in allActivities)
            {
                // Check if the activity is already present in patient
                List<ActivityRecord> actRecords = patient.ActivityRecords.ToList();
                bool contains = false;
                foreach (var acts in actRecords)
                {
                    if (acts.ActivityId == activity.ActivityId)
                    {
                        contains = true;
                        break;
                    }
                }

                int actId = activity.ActivityId;
                viewModel.Add(new AssignedActivityData {
                    ActivityId = activity.ActivityId,
                    ActivityType = activity.ActivityType,
                    ActivityTime = activity.ActivityTime,
                    Assigned = contains
                });
            }
            ViewBag.Activities = viewModel;
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, string[] selectedActivities)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var patientToUpdate = db.Patients.Find(id);
            if (TryUpdateModel(patientToUpdate, "",
                new string[] { "FirstName", "LastName", "EmergencyContact", "RoomId" }))
            {
                try
                {
                    patientToUpdate.Room = db.Rooms.Where(r => r.RoomId == patientToUpdate.RoomId).First();
                    UpdatePatientActivities(selectedActivities, patientToUpdate);
                    db.Entry(patientToUpdate).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (RetryLimitExceededException /*dex*/)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "RoomId", patientToUpdate.RoomId);
            // return View(patientToUpdate);
            return Redirect("/patients/details/"+id);
        }

        private void UpdateMedicalActivities(string[] selectedMedicines, Patient patientToUpdate)
        {

        }

        private void UpdatePatientActivities(string[] selectedActivites, Patient patientToUpdate)
        {
            if(selectedActivites == null)
            {
                patientToUpdate.ActivityRecords = new List<ActivityRecord>();
                return;
            }

            var selectedActs = new HashSet<string>(selectedActivites);
            var patientActs = patientToUpdate.ActivityRecords;

            var allActRecords = db.ActivityRecords;

            foreach (var activity in db.Activities)
            {
                bool contains = false;
                // Add activity if selected and does not exist with the patient
                if (selectedActs.Contains(activity.ActivityId.ToString()))
                {
                    foreach(var actRec in patientActs)
                    {
                        if(actRec.ActivityId == activity.ActivityId)
                        {
                            contains = true;
                            break;
                        }
                    }
                    if (!contains)
                    {
                        var newActRec = new ActivityRecord {
                            Activity = activity,
                            ActivityId = activity.ActivityId,
                            Patient = patientToUpdate,
                            PatientId = patientToUpdate.PatientId
                        };
                        patientToUpdate.ActivityRecords.Add(newActRec);
                    }
                }
                // Remove activity because not selected
                else
                {
                    
                    foreach(var actRec in patientActs)
                    {
                        if(actRec.ActivityId == activity.ActivityId)
                        {
                            patientToUpdate.ActivityRecords.Remove(actRec);
                            allActRecords.Remove(actRec);
                            break;
                        }
                    }
                    
                }
            }

        }

        // GET: Patients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient patient = db.Patients.Find(id);
            db.Patients.Remove(patient);

            // Additonal deletion from activity records necessary
            var actRecords = db.ActivityRecords
                .Where(a => a.PatientId == id);

            db.ActivityRecords.RemoveRange(actRecords);

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
