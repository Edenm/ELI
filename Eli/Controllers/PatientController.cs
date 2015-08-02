using Eli.Models;
using Eli.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;


namespace Eli.Controllers
{
    [Authorize]
    public class PatientController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Test()
        {
            return View(new tblPatient());
        }

        public ActionResult WebForm1()
        {

            return View();
        }

        /* The Method displays all the patients in the system-this is the main form of patients  **/
        [HttpGet]
        public ActionResult IndexPatients(string operate,string type)
        {
            EliManagerDB db = new EliManagerDB();

            tblTherapist ther = (tblTherapist)Session["Therapist"];
            var pat = db.getAllPatientsByTherapist(ther.TherapistID);

            List<Family> fams= new List<Family>();

            foreach (tblPatient f in pat){
                fams.Add(new Family(f.ID));
            }

            ViewBag.operate = operate;
            ViewBag.type = type;

            return View(fams);
        }


        /* The method get a patient details and update the patient **/
        [HttpPost]
        public ActionResult IndexPatients(tblPatient pat)
        {
            EliManagerDB db = new EliManagerDB();
            string type = "success", operate = "פרטי מטופל עודכנו בהצלחה";
            String gender = Request.Form["gender"];
            pat.Gender = gender;
            String status = Request.Form["status"];
            pat.PatientStatus = status;
            db.EditPatient(pat);

            return RedirectToAction("IndexPatients",new { operate = operate, type = type });
        }

        
        /* The method update family obj siblings and parents **/
        [HttpPost]
        public ActionResult _EditFamily(FormCollection family, string pid)
        {
            EliManagerDB db = new EliManagerDB();
            string type = "success", operate="פרטי משפחה עודכנו בהצלחה";

            Family fam = new Family(pid);

            db.EditFamily(family, pid);

            return RedirectToAction("IndexPatients", new { id = pid, operate = operate, type = type });
        }

        
        [HttpPost]
        public ActionResult _AddParentToPatient(tblParent tpar, string pid)
        {
            EliManagerDB db = new EliManagerDB();
            string type = "success", operate;
            try {
                db.addParentToPatient(tpar, pid);
                operate = "ההורה התווסף בהצלחה";
            }
            catch (Exception e)
            {
                operate = e.Message;
                type = "danger";
            }
            return RedirectToAction("IndexPatients", new { id = pid, operate = operate, type = type });
        }

        [HttpPost]
        public ActionResult _AddBrotherSisterToPatient(tblBrotherSister tbs, string pid)
        {
            EliManagerDB db = new EliManagerDB();
            string type = "success", operate;
            try {
                db.addBrotherSisterToPatient(tbs, pid);
                operate = "האח התווסף בהצלחה";
            }
            catch (Exception e)
            {
                operate = e.Message;
                type = "danger";
            }

            return RedirectToAction("IndexPatients", new { id = pid,operate = operate, type = type });
        }
       



            public ActionResult childs()
            {
                EliManagerDB db = new EliManagerDB();

                var pat = db.Patients.ToArray();

                return View(pat);
            }

            

        }
}



