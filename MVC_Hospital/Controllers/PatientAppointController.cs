using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;

namespace MVC_Hospital.Controllers
{
    public class PatientAppointController : Controller
    {
        private Mvc_HospitalEntities db = new Mvc_HospitalEntities();
        // GET: PatientAppoint
        public ActionResult GetAllDoctors()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var doclist = db.DoctorTables.Where(i => i.UserTable.IsVerifed == true);
            return View(doclist);
        }
        public ActionResult GetAllLabs()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var lablist = db.LabTables.Where(i => i.UserTable.IsVerifed == true);
            return View(lablist);
        }
        public ActionResult GetAllPatients()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var lablist = db.LabTables.Where(i => i.UserTable.IsVerifed == true);
            return View(lablist);
        }


        public ActionResult DoctorAppointment(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            Session["id"] = id;
            ViewBag.DoctorTimeSlotID = new SelectList( db.DoctorTimeSlotTables.Where(d => d.DoctorID == id && d.IsActive == true), "DoctorTimeSlotID","Name","0");
            ViewBag.Doctor = db.DoctorTimeSlotTables.Find(id);

            return View();

        }
        [HttpPost]
        public ActionResult LabAppointment(LabAppointTable appointment)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            appointment.IsComplete = false;
            appointment.IsFeeSubmit = false;
            //Idyı dropdownlıste gondermek ıcın kullanılan alanda ufak bır hata duzeltme yaptım
            ViewBag.LabTimeSlotID = new SelectList(db.LabTimeSlotTables.Where(d => d.LabID == appointment.LabID && d.IsActive == true), "LabTimeSlotID", "Name", "0");
            if (ModelState.IsValid)
            {
                var check = db.LabAppointTables.Where(i => i.TransectionID == appointment.TransectionID).FirstOrDefault();
                if (check == null)
                {
                    var find = db.LabAppointTables.Where(p => p.LabTimeSlotID == appointment.LabTimeSlotID && p.LabID == appointment.LabID && p.AppointDate == appointment.AppointDate).FirstOrDefault();
                    if (find == null)
                    {
                        db.LabAppointTables.Add(appointment);
                        db.SaveChanges();
                        ViewBag.Message = "Appointment submitted Successfully!";
                    }
                    else
                    {
                        ViewBag.Message = "Time Slot is Already Assign!";

                    }
                }
                else
                {
                    ViewBag.Message = "Transection Number is Already Used! ";
                }


            }


            return View(appointment);

        }

        [HttpPost]
        public ActionResult DoctorAppointment(DoctorAppointTable appointment)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            appointment.DoctorComment = string.Empty;
            appointment.IsChecked = false;
            appointment.IsFeeSubmit = false;
            var patient = (PatientTable)Session["Patient"];
            appointment.PatientID = patient.PatientID;
            ViewBag.DoctorTimeSlotID = new SelectList(db.DoctorTimeSlotTables.Where(d => d.DoctorID == appointment.DoctorID && d.IsActive == true), "DoctorTimeSlotID", "Name", "0");

            appointment.DoctorID = int.Parse(Convert.ToString(Session["id"]));
            if (ModelState.IsValid)
            {
                var check = db.DoctorAppointTables.Where(i => i.TransectionID == appointment.TransectionID).FirstOrDefault();
                if (check==null)
                {
                    var find = db.DoctorAppointTables.Where(p => p.DoctorTimeSlotID == appointment.DoctorTimeSlotID && p.DoctorID == appointment.DoctorID && p.AppointDate == appointment.AppointDate).FirstOrDefault();
                    if (find == null)
                    {
                        db.DoctorAppointTables.Add(appointment);
                        db.SaveChanges();
                        //return RedirectToAction("DoctorPendingAppoint");
                        ViewBag.Message = "Appointment submitted Successfully!";

                    }
                    else
                    {
                        ViewBag.Message = "Time Slot is Already Assign!(Another Pattient)";

                    }
                }
                else
                {
                    ViewBag.Message = "Transection Number is Already Used! for Another transection!";
                }
              

            }
           
            return View(appointment);


     

        }

        public ActionResult DoctorProfile(int? id)
        {

            var doc = db.DoctorTables.Find(id);
            return View();
        }

        
        public ActionResult LabTests(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            Session["labid"] = id;
        var laballtest=db.LabTestTables.Where(d=>d.LabID==id);


            return View(laballtest);

        }
        public ActionResult LabAppointment(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int labid = db.LabTestTables.Find(id).LabID;
            var patient = (PatientTable)Session["Patient"];
            Session["labid"] = id;

            var appointment = new LabAppointTable()
            //idleri bu sekilde kullandık
            {
                LabID = labid,
                LabTestID = (int)id,
                PatientID = patient.PatientID,
                LabTimeSlotID= (int)id,

            };
            //ViewBag.LabTestID= new SelectList(db.LabTestTables.Where(d => d.LabID==id ), "LabTimeSlotID", "Name", "0");


            ViewBag.LabTimeSlotID = new SelectList(db.LabTimeSlotTables.Where(d => d.LabID == id && d.IsActive == true), "LabTimeSlotID", "Name", "0");

            return View(appointment);

        }
      

    }
}