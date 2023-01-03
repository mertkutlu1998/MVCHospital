using DatabaseLayer;
using MVC_Hospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MVC_Hospital.Controllers
{
    public class LabApproveController : Controller
    {
        // GET: LabApprove
        private Mvc_HospitalEntities db = new Mvc_HospitalEntities();


        public ActionResult PendingAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");

            }
            var lab = (LabTable)Session["Lab"];
            var pendingappoint = db.LabAppointTables.Where(d => d.LabID == lab.LabID && d.IsComplete == false && d.IsFeeSubmit == false);
            return View(pendingappoint);
        }
        public ActionResult CompleteAppointment()//**
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");

            }
            var lab = (LabTable)Session["Lab"];
            var completeappointment = db.LabAppointTables.Where(d => d.LabID == lab.LabID && d.IsComplete == true && d.IsFeeSubmit == true);
            return View(completeappointment);
        }
        public ActionResult AllAppoint()//**
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");

            }
            var lab = (LabTable)Session["Lab"];
            var allappointment = db.LabAppointTables.Where(d => d.LabID == lab.LabID);
            return View(allappointment);
        }


        public ActionResult CurrentAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");

            }
            var lab = (LabTable)Session["Lab"];
            var completeappointment = db.LabAppointTables.Where(d => d.LabID == lab.LabID && d.IsComplete == false && d.IsFeeSubmit == true);
            return View(completeappointment);
        }

        public ActionResult ChangeStatus(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");

            }
            var appoint = db.LabAppointTables.Find(id);
            ViewBag.LabTimeSlotID = new SelectList(db.LabTimeSlotTables.Where(d => d.LabID == appoint.LabID), "LabTimeSlotID", "Name", appoint.LabTimeSlotID);
            return View(appoint);
        }
        [HttpPost]
        public ActionResult ChangeStatus(LabAppointTable app)
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
            ViewBag.LabTimeSlotID = new SelectList(db.LabTimeSlotTables.Where(d => d.LabID == app.LabID), "LabTimeSlotID", "Name", app.LabTimeSlotID);

            return View(app);
        }

        public ActionResult ProccessAppointment(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");

            }
            List<PatientAppointmentReportMV> detaillist = new List<PatientAppointmentReportMV>();
            var appoint = db.LabAppointTables.Find(id);
            var testdetails = db.LabTestDetailsTables.Where(d => d.LabTestID == appoint.LabTestID);
            foreach (var item in testdetails)
            {
                var details = new PatientAppointmentReportMV()
                {
                    DetailName = item.Name,
                    LabAppointID = appoint.LabAppointID,
                    LabTestDetailID = item.LabTestDetailID,
                    MaxValue = item.MaxValue,
                    MinValue = item.MinValue,
                    PatientValue = 0

                };
                detaillist.Add(details);

            }

            ViewBag.TestName = appoint.LabTestTable.Name;
            //ViewBag.LabTimeSlotID = new SelectList(db.LabTimeSlotTables.Where(d => d.LabID == appoint.LabID), "LabTimeSlotID", "Name", appoint.LabTimeSlotID);
            return View(detaillist);
        }

        [HttpPost]
        public ActionResult ProccessAppointment(FormCollection collection)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");

            }
            string[] LabTestDetailID = collection["item.LabTestDetailID"].Split(',');
            string[] LabAppointID = collection["item.LabAppointID"].Split(',');
            string[] DetailName = collection["item.DetailName"].Split(',');
            string[] MaxValue = collection["item.MaxValue"].Split(',');
            string[] MinValue = collection["item.MinValue"].Split(',');
            string[] PatientValue = collection["item.PatientValue"].Split(',');
            List<PatientAppointmentReportMV> detaillist = new List<PatientAppointmentReportMV>();
            bool issubmitted = false;
            for (int i = 0; i < LabTestDetailID.Length; i++)
            {
                var details = new PatientAppointmentReportMV()
                {
                    DetailName = DetailName[i],
                    LabAppointID = Convert.ToInt32(LabAppointID[i]),
                    LabTestDetailID = Convert.ToInt32(LabTestDetailID[i]),
                    MaxValue = Convert.ToInt32(MaxValue[i]),
                    MinValue = Convert.ToInt32(MinValue[i]),
                    PatientValue = Convert.ToInt32(PatientValue[i]),

                };
                if (details.PatientValue > 0)
                {
                    issubmitted = true;

                }
                detaillist.Add(details);
            }
            if (issubmitted == true)
            {


                foreach (var item in detaillist)
                {
                    var tbdetails = new PatientTestDetailTable()
                    {
                        LabAppointID = item.LabAppointID,
                        LabTestDetailID = item.LabTestDetailID,
                        PatientValue = item.PatientValue,


                    };
                    db.PatientTestDetailTables.Add(tbdetails);
                    db.SaveChanges();
                }
                int appointid = Convert.ToInt32(LabAppointID[0]);
                var appointment = db.LabAppointTables.Find(appointid);
                appointment.IsComplete = true;
                db.Entry(appointment).State = System.Data.Entity.EntityState.Modified; db.SaveChanges();
                return RedirectToAction("PendingAppoint");
            }
            else
            {
                ViewBag.Message = "Please Enter Patient Test Details!";
            }
            return View(detaillist);
        }

        public ActionResult ViewDetails(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");

            }
            ViewBag.TestName = db.LabAppointTables.Find(id).LabTestTable.Name;
            ViewBag.Patient = db.LabAppointTables.Find(id).PatientTable.Name;
            ViewBag.AppointmentNo = db.LabAppointTables.Find(id).TransectionID;
            ViewBag.Lab = db.LabAppointTables.Find(id).LabTable.Name;
            ViewBag.LabLogo = db.LabAppointTables.Find(id).LabTable.Photo;
            ViewBag.PatientPhoto = db.LabAppointTables.Find(id).PatientTable.Photo;



            return View(db.PatientTestDetailTables.Where(p => p.LabAppointID == id).ToList());
        }

    }
}