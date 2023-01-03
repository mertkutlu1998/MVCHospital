using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace MVC_Hospital.Controllers
{
    public class LabSettingController : Controller
    {
        // GET: LabSetting

        private Mvc_HospitalEntities db = new Mvc_HospitalEntities();

        public ActionResult LabAllTest()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
        
           if (Session["Lab"] == null)
            {
                return RedirectToAction("Login", "Home");

            }


            var lab =(LabTable)Session["Lab"];

            var textlist = db.LabTestTables.Where(l => l.LabID == lab.LabID).ToList();
            return View(textlist);
        }  
        public ActionResult GetAllTest()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
        
          

            var textlist = db.LabTestTables.ToList();
            return View(textlist);
        }
        public ActionResult AddTest()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddTest(LabTestTable test)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            else if (Session["Lab"] == null)
            {
                return RedirectToAction("Login", "Home");

            }
            var lab = (LabTable)Session["Lab"];
            test.LabID = lab.LabID;
            if (ModelState.IsValid)
            {
                var findlab = db.LabTestTables.Where(a => a.Name == test.Name).FirstOrDefault();
                if (findlab==null)
                {
                    db.LabTestTables.Add(test);
                    db.SaveChanges();
                    return RedirectToAction("LabAllTest");
                }
                else
                {
                    ViewBag.Message = "Already Registered";
                }
            }
            return View(test);
        }
        public ActionResult TestDetails(int? id)
        {
            Session["LabTestID"] = id;
            var detailslist = db.LabTestDetailsTables.Where(I => I.LabTestID == id).ToList();
            return View(detailslist);
        }
        public ActionResult AddTestDetails(int? id)
        {

          
            return View();
        }

        [HttpPost]
        public ActionResult AddTestDetails(LabTestDetailsTable testDetails)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            else if (Session["Lab"] == null)
            {
                return RedirectToAction("Login", "Home");

            }
            var lab = (LabTable)Session["Lab"];
            testDetails.LabID = lab.LabID;
            int testid = Convert.ToInt32(Convert.ToString(Session["LabTestID"]));
            testDetails.LabTestID = testid;


            if (ModelState.IsValid)
            {
                var finddetails = db.LabTestDetailsTables.Where(a => a.Name == testDetails.Name).FirstOrDefault();
                if (finddetails == null)
                {
                    db.LabTestDetailsTables.Add(testDetails);
                    db.SaveChanges();
                    return RedirectToAction("TestDetails",new { id= testid });
                }
                else
                {
                    ViewBag.Message = "Already Registered";
                }
            }
            return View(testDetails);

        }
    
        public ActionResult EditTest(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var labtest=db.LabTestTables.Find(id);//id yakaladık
            return View(labtest);
        }

        [HttpPost]
        public ActionResult EditTest(LabTestTable test)
        {//14D 4.18
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            else if (Session["Lab"] == null)
            {
                return RedirectToAction("Login", "Home");

            }
           
            if (ModelState.IsValid)
            {
                var findlab = db.LabTestTables.Where(a => a.Name == test.Name&&a.LabTestID!=test.LabTestID).FirstOrDefault();
                if (findlab == null)
                {
                    db.Entry(test).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("LabAllTest");
                }
                else
                {
                    ViewBag.Message = "Already Registered";
                }
            }
            return View(test);
        }

        public ActionResult EditTestDetails(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var details=db.LabTestDetailsTables.Find(id);
            return View(details);
        }

        [HttpPost]
        public ActionResult EditTestDetails(LabTestDetailsTable testDetails)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            else if (Session["Lab"] == null)
            {
                return RedirectToAction("Login", "Home");

            }
     
            if (ModelState.IsValid)
            {
                var finddetails = db.LabTestDetailsTables.Where(a => a.Name == testDetails.Name&&a.LabTestDetailID!=testDetails.LabTestDetailID).FirstOrDefault();
                if (finddetails == null)
                {
                    db.Entry(testDetails).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("TestDetails", new { id = testDetails.LabTestID });
                }
                else
                {
                    ViewBag.Message = "Already Registered";
                }
            }
            return View(testDetails);
        }
    }
}