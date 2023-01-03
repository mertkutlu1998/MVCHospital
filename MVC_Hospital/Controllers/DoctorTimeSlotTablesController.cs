using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DatabaseLayer;

namespace MVC_Hospital.Controllers
{
    public class DoctorTimeSlotTablesController : Controller
    {
        private Mvc_HospitalEntities db = new Mvc_HospitalEntities();

        // GET: DoctorTimeSlotTables
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var doc = (DoctorTable)Session["Doctor"];
            var doctorTimeSlotTables = db.DoctorTimeSlotTables.Include(d => d.DoctorTable).Where(d => d.DoctorID == doc.DoctorID);
            return View(doctorTimeSlotTables.ToList());
        }

        // GET: DoctorTimeSlotTables/Details/5
        public ActionResult Details(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorTimeSlotTable doctorTimeSlotTable = db.DoctorTimeSlotTables.Find(id);
            if (doctorTimeSlotTable == null)
            {
                return HttpNotFound();
            }
            return View(doctorTimeSlotTable);
        }

        // GET: DoctorTimeSlotTables/Create
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            ViewBag.DoctorID = new SelectList(db.DoctorTables, "DoctorID", "Name");
            return View();
        }

        // POST: DoctorTimeSlotTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DoctorTimeSlotTable doctorTimeSlotTable)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var doc = (DoctorTable)Session["Doctor"];
            doctorTimeSlotTable.DoctorID = doc.DoctorID;
            if (ModelState.IsValid)
            {
                var find = db.DoctorTimeSlotTables.Where(x => x.DoctorID == doc.DoctorID && x.FromTime == doctorTimeSlotTable.FromTime && x.ToTime == doctorTimeSlotTable.ToTime).FirstOrDefault();

                if (find == null)
                {
                    db.DoctorTimeSlotTables.Add(doctorTimeSlotTable);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Message = "Already List Please Checek";
                }

                db.DoctorTimeSlotTables.Add(doctorTimeSlotTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(doctorTimeSlotTable);
        }

        // GET: DoctorTimeSlotTables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorTimeSlotTable doctorTimeSlotTable = db.DoctorTimeSlotTables.Find(id);
            if (doctorTimeSlotTable == null)
            {
                return HttpNotFound();
            }
            return View(doctorTimeSlotTable);
        }

        // POST: DoctorTimeSlotTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( DoctorTimeSlotTable doctorTimeSlotTable)
        {


            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var doc = (DoctorTable)Session["Doctor"];
            doctorTimeSlotTable.DoctorID = doc.DoctorID;
            if (ModelState.IsValid)
            {
                var find = db.DoctorTimeSlotTables.Where(x => x.DoctorID == doc.DoctorID && x.FromTime == doctorTimeSlotTable.FromTime && x.ToTime == doctorTimeSlotTable.ToTime && x.DoctorTimeSlotID != doctorTimeSlotTable.DoctorTimeSlotID).FirstOrDefault();

                if (find == null)
                {
                    db.Entry(doctorTimeSlotTable).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Message = "Already List Please Checek";
                }


            

            }
            return View(doctorTimeSlotTable);
        }

        // GET: DoctorTimeSlotTables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorTimeSlotTable doctorTimeSlotTable = db.DoctorTimeSlotTables.Find(id);
            if (doctorTimeSlotTable == null)
            {
                return HttpNotFound();
            }
            return View(doctorTimeSlotTable);
        }

        // POST: DoctorTimeSlotTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            DoctorTimeSlotTable doctorTimeSlotTable = db.DoctorTimeSlotTables.Find(id);
            db.DoctorTimeSlotTables.Remove(doctorTimeSlotTable);
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
