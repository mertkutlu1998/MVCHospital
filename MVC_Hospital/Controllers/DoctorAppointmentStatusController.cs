using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Hospital.Controllers
{
    public class DoctorAppointmentStatusController : Controller
    {
        private Mvc_HospitalEntities db = new Mvc_HospitalEntities();

        // GET: DoctorAppointmentStatus
        public ActionResult PendingAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
        
            var patient = (PatientTable)Session["Patient"];
            var pendingappointment=db.DoctorAppointTables.Where(d=>d.PatientID ==patient.PatientID &&d.IsChecked==false &&d.IsFeeSubmit==false &&d.DoctorComment.Trim().Length==0);
            return View(pendingappointment);
        }



        public ActionResult CurrentAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var patient = (PatientTable)Session["Patient"];
            var currentappointment = db.DoctorAppointTables.Where(d => d.PatientID == patient.PatientID && d.IsChecked == true && d.IsFeeSubmit == false && d.DoctorComment.Trim().Length == 0);
            return View(currentappointment);
        }

        public ActionResult CompletedAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var patient = (PatientTable)Session["Patient"];
            var completeappointment = db.DoctorAppointTables.Where(d => d.PatientID == patient.PatientID && d.IsChecked == true && d.IsFeeSubmit == true && d.DoctorComment.Trim().Length> 0);
            return View(completeappointment);
        }
        public ActionResult CancelAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var patient = (PatientTable)Session["Patient"];
            var cancelappointment = db.DoctorAppointTables.Where(d => d.PatientID == patient.PatientID && d.DoctorComment.Trim().Length > 0);
            return View(cancelappointment);
        }
        public ActionResult AllAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var patient = (PatientTable)Session["Patient"];
            var allappointment = db.DoctorAppointTables.Where(d => d.PatientID == patient.PatientID );
            return View(allappointment);
        }
    }
}