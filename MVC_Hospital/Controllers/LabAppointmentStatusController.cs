using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Hospital.Controllers
{
    public class LabAppointmentStatusController : Controller
    {
        private Mvc_HospitalEntities db = new Mvc_HospitalEntities();

        public ActionResult PendingAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var patient = (PatientTable)Session["Patient"];
            var pendingappointment = db.LabAppointTables.Where(d => d.PatientID == patient.PatientID && d.IsComplete == false && d.IsFeeSubmit == false);
            return View(pendingappointment);
        }



        public ActionResult CurrentAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var patient = (PatientTable)Session["Patient"];
            var currentappointment = db.LabAppointTables.Where(d => d.PatientID == patient.PatientID && d.IsComplete == false && d.IsFeeSubmit == true);
            return View(currentappointment);
        }

        public ActionResult CompletedAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var patient = (PatientTable)Session["Patient"];
            var completeappointment = db.LabAppointTables.Where(d => d.PatientID == patient.PatientID && d.IsComplete == true && d.IsFeeSubmit == true);
            return View(completeappointment);
        }
        public ActionResult CancelAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var patient = (PatientTable)Session["Patient"];
            var cancelappointment = db.LabAppointTables.Where(d => d.PatientID == patient.PatientID && (d.IsComplete == false || d.IsFeeSubmit == false)&& d.AppointDate>DateTime.Now);
            return View(cancelappointment);
        }
        public ActionResult AllAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var patient = (PatientTable)Session["Patient"];
            var allappointment = db.LabAppointTables.Where(d => d.PatientID == patient.PatientID );
            return View(allappointment);
        }
    }
}