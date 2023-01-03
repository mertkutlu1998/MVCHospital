using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Web.WebPages.Html;

namespace MVC_Hospital.Controllers
{
    public class HomeController : Controller
    {
        private Mvc_HospitalEntities db = new Mvc_HospitalEntities();
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public ActionResult StartTemplate()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(password))
            {
                return View("Login");


            }
            var user = db.UserTables.Where(u => u.Email == email && u.Password == password && u.IsVerifed == true).FirstOrDefault();
            if (user != null)
            {
                Session["UserID"] = user.UserID;
                Session["UserTypeID"] = user.UserTypeID;
                Session["UserName"] = user.UserName;
                Session["Password"] = user.Password;
                Session["Email"] = user.Email;
                Session["ContactNo"] = user.ContactNo;
                Session["Description"] = user.Description;
                Session["IsVerifed"] = user.IsVerifed;

                if (user.UserTypeID == 2)//Doctor
                {
                    var doc = db.DoctorTables.Where(u => u.UserID == user.UserID).FirstOrDefault();
                    Session["Doctor"] = doc;
            
                }
                else if (user.UserTypeID == 3)//Lab
                {
                    var lab = db.LabTables.Where(u => u.UserID == user.UserID).FirstOrDefault();
                    Session["Lab"] = lab;

                }

                else if (user.UserTypeID == 4)//Patient
                {
                    var pat = db.PatientTables.Where(u => u.UserID == user.UserID).FirstOrDefault();
                    Session["Patient"] = pat;


                }
                return RedirectToAction("Index");

            }
           user = db.UserTables.Where(u => u.Email == email && u.Password == password && u.IsVerifed == false ).FirstOrDefault();
            if (user!=null)
            {


                ViewBag.Message = "Email is Already registered , Please Profile Details";

                Session["User"] = user;

                if (user.UserTypeID == 2)//Doctor
                {
                    var doc = db.DoctorTables.Where(u => u.UserID == user.UserID).FirstOrDefault();

                    if (doc==null)
                    {
                        return RedirectToAction("AddDoctor");

                    }
                    ViewBag.Message = "Account is under Review!";
                }
                else if (user.UserTypeID == 3)//Lab
                {
                    var doc = db.LabTables.Where(u => u.UserID == user.UserID).FirstOrDefault();

                    if (doc == null)
                    {
                        return RedirectToAction("AddLab");

                    }
                    ViewBag.Message = "Account is under Review!";
                }

                else if (user.UserTypeID == 4)//Patient
                {
                    var doc = db.PatientTables.Where(u => u.UserID == user.UserID).FirstOrDefault();

                    if (doc == null)
                    {
                        return RedirectToAction("AddPatient");

                    }
                    ViewBag.Message = "Account is under Review!";
                }
            }
            else
            {
                ViewBag.message = "Username and password is incorrect!";

            }
            Logout();

            return View("Login");
        }
        public void Logout()
        {
            Session["UserID"] = string.Empty;
            Session["UserTypeID"] = string.Empty;
            Session["UserName"] = string.Empty;
            Session["Password"] = string.Empty;
            Session["Email"] = string.Empty;
            Session["ContactNo"] = string.Empty;
            Session["Description"] = string.Empty;
            Session["IsVerifed"] = string.Empty;

        }
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string OldPassword, string NewPassword, string ConfirmPassword)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int? userid = Convert.ToInt32(Session["UserID"].ToString());
            UserTable users = db.UserTables.Find(userid);
            if (users.Password == OldPassword)
            {
                if (NewPassword == ConfirmPassword)
                {
                    users.Password = NewPassword;
                    db.Entry(users).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.message = "Change Successfully";
                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    ViewBag.message = "New Password and Confirm not Matched!";
                    return View("ChangePassword");
                }
            }
            else
            {
                ViewBag.message = "Old Password is Incorrect!";
                return View("ChangePassword");
            }
        }



        //Doctor Profile
        public ActionResult CreateUser()
        {

            //Id yerine isim koymak için bu kodu yazdık
            ViewBag.UserTypeID = new SelectList(db.UserTypeTables.Where(u => u.UserTypeID != 1), "UserTypeID", "UserType", "0");

            return View();
        }
        [HttpPost]
        public ActionResult CreateUser(UserTable user)
        {
            if (user != null)
            {
                if (ModelState.IsValid)
                {
                    var finduser = db.UserTables.Where(u => u.Email == user.Email).FirstOrDefault();
                    if (finduser == null)
                    {
                        finduser = db.UserTables.Where(u => u.Email == user.Email && u.IsVerifed == false).FirstOrDefault();
                        if (finduser == null)
                        {
                            if (user.UserTypeID == 2)//Doctor
                            {
                                user.IsVerifed = false;
                            }
                            else if (user.UserTypeID == 3)//Lab
                            {
                                user.IsVerifed = false;
                            }
                            else if (user.UserTypeID == 1)//Admin
                            {
                                user.IsVerifed = false;
                            }
                            else if (user.UserTypeID == 4)//Patient
                            {
                                user.IsVerifed = true;
                            }

                            db.UserTables.Add(user);
                            db.SaveChanges();

                            Session["User"] = user;

                            if (user.UserTypeID == 2)//Doctor
                            {
                                return RedirectToAction("AddDoctor");
                            }
                            else if (user.UserTypeID == 3)//Lab
                            {
                                return RedirectToAction("AddLab");
                            }
                            else if (user.UserTypeID == 1)//Admin
                            {
                                ViewBag.Message = "Account is under Review!";
                            }
                            else if (user.UserTypeID == 4)//Patient
                            {
                                return RedirectToAction("AddPatient");
                            }
                        }
                       
                    }
                    else
                    {
                        ViewBag.Message = "Email is Already registered , Please Profile Details";

                        Session["User"] = finduser;

                        if (finduser.UserTypeID == 2)//Doctor
                        {
                            var doc = db.DoctorTables.Where(u => u.UserID == finduser.UserID).FirstOrDefault();

                            if (doc == null)
                            {
                                return RedirectToAction("AddDoctor");

                            }
                            ViewBag.Message = "Account is under Review!";
                        }
                        else if (finduser.UserTypeID == 3)//Lab
                        {
                            var doc = db.DoctorTables.Where(u => u.UserID == finduser.UserID).FirstOrDefault();

                            if (doc == null)
                            {
                                return RedirectToAction("AddLab");

                            }
                            ViewBag.Message = "Account is under Review!";
                        }

                        else if (finduser.UserTypeID == 4)//Patient
                        {
                            var doc = db.DoctorTables.Where(u => u.UserID == finduser.UserID).FirstOrDefault();

                            if (doc == null)
                            {
                                return RedirectToAction("AddPatient");

                            }
                            ViewBag.Message = "Account is under Review!";
                        }


                    }
                }

            }
            else
            {
                ViewBag.Message = "Please Provide Correct Details!";
            }
            ViewBag.UserTypeID = new SelectList(db.UserTypeTables.Where(u => u.UserTypeID != 1), "UserTypeID", "UserType", "0");

            return View();
        }
        public ActionResult AddDoctor()
        {
            //Gender ıd yerıne gender ısmını yazdırdık oraya
            ViewBag.GenderID = new SelectList(db.GenderTables.ToList(), "GenderID", "Name", "0");
            ViewBag.AccountTypeID = new SelectList(db.AccountTypeTables.ToList(), "AccountTypeID", "Name", "0");



            return View();
        }

        [HttpPost]
        public ActionResult AddDoctor(DoctorTable doctor)
        {
            if (Session["User"] != null)
            {
                var user = (UserTable)Session["User"];
                doctor.UserID = user.UserID;


                if (ModelState.IsValid)
                {

                    var finddoctor = db.DoctorTables.Where(d => d.EmailAddress == doctor.EmailAddress).FirstOrDefault();
                    if (finddoctor == null)
                    {
                        db.DoctorTables.Add(doctor);
                        db.SaveChanges();
                        if (doctor.LogFile != null)
                        {
                            var folder = "~/Content/DoctorImages";
                            var file = string.Format("{0}.png", doctor.DoctorID);
                            var response = FileHelpers.UploadPhoto(doctor.LogFile, folder, file);
                            if (response)
                            {
                                var pic = string.Format("{0}/{1}", folder, file);
                                doctor.Photo = pic;
                                db.Entry(doctor).State = EntityState.Modified;
                                db.SaveChanges();
                                return View("UnderReview");
                            }
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Email Already Registered";
                    }
                   
                }
                else
                {
                    ViewBag.Message = "Don't Refresh Page";
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
            ViewBag.GenderID = new SelectList(db.GenderTables.ToList(), "GenderID", "Name", doctor.GenderID);
            ViewBag.AccountTypeID = new SelectList(db.AccountTypeTables.ToList(), "AccountTypeID", "Name", doctor.AccountTypeID);

            return View(doctor);
        }

        public ActionResult AddLab()
        {
            ViewBag.AccountTypeID = new SelectList(db.AccountTypeTables.ToList(), "AccountTypeID", "Name", "0");

            return View();
        }

        [HttpPost]
        public ActionResult AddLab(LabTable lab)
        {
            if (Session["User"] != null)
            {
                var user = (UserTable)Session["User"];
                lab.UserID = user.UserID;


                if (ModelState.IsValid)
                {

                    var findlab = db.LabTables.Where(d => d.EmailAddress == lab.EmailAddress).FirstOrDefault();
                    if (findlab == null)
                    {
                        db.LabTables.Add(lab);
                        db.SaveChanges();
                        if (lab.LogFile != null)
                        {
                            var folder = "~/Content/LabPhotos";
                            var file = string.Format("{0}.png", lab.LabID);
                            var response = FileHelpers.UploadPhoto(lab.LogFile, folder, file);
                            if (response)
                            {
                                var pic = string.Format("{0}/{1}", folder, file);
                                lab.Photo = pic;
                                db.Entry(lab).State = EntityState.Modified;
                                db.SaveChanges();
                                return View("UnderReview");
                            }
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Email Already Registered";
                    }
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
            ViewBag.AccountTypeID = new SelectList(db.AccountTypeTables.ToList(), "AccountTypeID", "Name", lab.AccountTypeID);
            return View(lab);
        }
        public ActionResult AddPatient()
        {
            ViewBag.GenderID = new SelectList(db.GenderTables.ToList(), "GenderID", "Name", "0");

            return View();
        }

        public ViewDataDictionary GetViewData()
        {
            return ViewData;
        }

        [HttpPost]
        public ActionResult AddPatient(PatientTable patient, ViewDataDictionary viewData)
        {
            if (Session["User"] != null)
            {
                var user = (UserTable)Session["User"];
                patient.UserID = user.UserID;


                if (ModelState.IsValid)
                {

                    var finpatient = db.PatientTables.Where(d => d.Email == patient.Email).FirstOrDefault();
                    if (finpatient == null)
                    {
                        db.PatientTables.Add(patient);

                        db.SaveChanges();
                        if (patient.LogFile != null)
                        {
                            var folder = "~/Content/PatientPhotos";
                            var file = string.Format("{0}.png", patient.PatientID);
                            var response = FileHelpers.UploadPhoto(patient.LogFile, folder, file);
                            if (response)
                            {
                                var pic = string.Format("{0}/{1}", folder, file);
                                patient.Photo = pic;
                                db.Entry(patient).State = EntityState.Modified;
                                db.SaveChanges();

                            }
                        }
                        ViewBag.Messag = "Please Check Confirmation";

                        return RedirectToAction("Login");

                    }
                    else
                    {
                        ViewBag.Message = "Email Already Registered";
                    }
                }
                else
                {
                    ViewBag.Message = "Hata var kayıt olmadı";
                }
               
            }
            else
            {
                return RedirectToAction("Login");
            }


            ViewBag.GenderID = new SelectList(db.GenderTables.ToList(), "GenderID", "Name", patient.GenderID);



            return View(patient);
        }


        public ActionResult UnderReview()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.message = "Your application description page.";

            return View();
        }
        public ActionResult LogoutUser()
        {
            Logout();
            return RedirectToAction("Login");
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Contact";

            return View();
        }
    }
}