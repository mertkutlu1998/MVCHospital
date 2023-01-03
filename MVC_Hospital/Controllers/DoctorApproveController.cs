using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Hospital.Controllers
{
    public class DoctorApproveController : Controller
    {
        private Mvc_HospitalEntities db = new Mvc_HospitalEntities();

        // GET: DoctorApprove
        public ActionResult PendingAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");

            }
            var doc = (DoctorTable)Session["Doctor"];
            var pendingappointment = db.DoctorAppointTables.Where(d => d.DoctorID == doc.DoctorID && d.IsChecked == false && d.IsFeeSubmit == false && string.IsNullOrEmpty(d.DoctorComment) == true);
            return View(pendingappointment);
        }
        public ActionResult CompleteAppointment()//**
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");

            }
            var doc = (DoctorTable)Session["Doctor"];
            var pendingappointment = db.DoctorAppointTables.Where(d => d.DoctorID == doc.DoctorID && d.IsChecked == true && d.IsFeeSubmit == true && string.IsNullOrEmpty(d.DoctorComment) != true);
            return View(pendingappointment);
        }
        public ActionResult AllAppoint()//**
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");

            }
            var doc = (DoctorTable)Session["Doctor"];
            var pendingappointment = db.DoctorAppointTables.Where(d => d.DoctorID == doc.DoctorID);
            return View(pendingappointment);
        }
        public ActionResult ChangeStatus(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");

            }
            var appoint = db.DoctorAppointTables.Find(id);
            ViewBag.DoctorTimeSlotID=new SelectList( db.DoctorTimeSlotTables.Where(d=>d.DoctorID==appoint.DoctorID), "DoctorTimeSlotID","Name",appoint.DoctorTimeSlotID);
            return View(appoint);
        }
        [HttpPost]
        public ActionResult ChangeStatus(DoctorAppointTable app)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");

            }
            if (ModelState.IsValid)
            {
               
                db.Entry(app).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PendingAppoint");

            }
            ViewBag.DoctorTimeSlotID = new SelectList(db.DoctorTimeSlotTables.Where(d => d.DoctorID == app.DoctorID), "DoctorTimeSlotID", "DoctorTimeSlotID",app.DoctorTimeSlotID);

            return View(app);
        }
        public ActionResult CurrentAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");

            }
            var doc = (DoctorTable)Session["Doctor"];
            var currentappointment = db.DoctorAppointTables.Where(d => d.DoctorID == doc.DoctorID && d.IsChecked == false && d.IsFeeSubmit == true && string.IsNullOrEmpty(d.DoctorComment) == true);
            return View(currentappointment);
        }
        public ActionResult ProcessAppointment(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");

            }

            var appoint = db.DoctorAppointTables.Find(id);


            return View(appoint);
        }

        [HttpPost]
        public ActionResult ProcessAppointment(DoctorAppointTable app)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");

            }
            if (ModelState.IsValid)
            {
                db.Entry(app).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PendingAppoint");

            }
            return View(app);
        }
    }
}