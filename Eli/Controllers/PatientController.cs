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

        /* The Method displays all the patients in the system-this is the main form of patients  **/
        [HttpGet]
        public ActionResult IndexPatients()
        {
            EliManagerDB db = new EliManagerDB();

            var pat = db.Patients.ToList();
            List<Family> fams= new List<Family>();

            foreach (tblPatient f in pat){
                fams.Add(new Family(f.ID));
            }

            return View(fams);
        }


        /* The method get a patient details and update the patient **/
        [HttpPost]
        public ActionResult IndexPatients(tblPatient pat)
        {
            EliManagerDB db = new EliManagerDB();
            String gender = Request.Form["gender"];
            pat.Gender = gender;
            String status = Request.Form["status"];
            pat.PatientStatus = status;
            db.EditPatient(pat);

            return RedirectToAction("IndexPatients");
        }

        
        /* The method update family obj siblings and parents **/
        [HttpPost]
        public ActionResult _EditFamily(FormCollection family, string pid)
        {
            EliManagerDB db = new EliManagerDB();
            var name = family.GetValues("IsWorking");

            Family fam = new Family(pid);

            db.EditFamily(family, pid);

            return RedirectToAction("IndexPatients", new { id = pid });
        }

        public ActionResult NewPatient()
        {
            return View(new tblPatient());
        }

        /* The Method get parameters of patient and add him to system  **/

        public ActionResult AddPatient()
        {

            EliManagerDB db = new EliManagerDB();


       //-----------personal details of patients--------------------------

            tblPatient tbP = new tblPatient()
            {
                ID = Request.Form["id"],
                FirstName = Request.Form["fname"],
                SurName = Request.Form["lname"],
                Gender = Request.Form["gender"],
                EducationalFramework = Request.Form["education"],
                PhoneNumber = Request.Form["phone"],
                PatientAddress = Request.Form["address"]
            };


      //-----------more details of patients parents--------------------------

            tblParent tbPF = new tblParent()
            {
                ParentID = Request.Form["idf"],
                ParentFirstName = Request.Form["fnamef"],
                ParentSurName = Request.Form["lnamef"],
                //BirthDate = Convert.ToDateTime(birthdatef),
                ParentPhoneNumber = Request.Form["phonef"],
                ParentAddress = Request.Form["adressf"],
                ParentMail = Request.Form["mailf"],
                IsWorking = Request.Form["isworkf"],
                Explain=Request.Form["explainf"]
            };

            tblParent tbPM = new tblParent()
            {
                ParentID = Request.Form["idm"],
                ParentFirstName = Request.Form["fnamem"],
                ParentSurName = Request.Form["lnamem"],
                //BirthDate = Convert.ToDateTime(birthdatef),
                ParentPhoneNumber = Request.Form["phonem"],
                ParentAddress = Request.Form["adressm"],
                ParentMail = Request.Form["mailm"],
                IsWorking = Request.Form["isworkm"],
                Explain=Request.Form["explainm"]
            };


       //-----------more details of patients brothers and sisters--------------------------

            List<tblBrotherSister> arrBS = new List<tblBrotherSister>();

            for (int i = 1; Request.Form["id" + i] == ""; i++)
            {
                tblBrotherSister tbBS = new tblBrotherSister()
                {
                    BrotherSisterID = Request.Form["id" + i],
                    BrotherSisterFirstName = Request.Form["fname" + i],
                    BrotherSisterSurName = Request.Form["lname" + i],
                    // BirthDate=Request.Form["birthdate"],
                    BrotherSisterGender = Request.Form["gender" + i],
                    BrotherSisterStudyFramework = Request.Form["education" + i]
                };

                arrBS.Add(tbBS);
            }

      //-----------more details of patients reference--------------------------

            tblReference tbR = new tblReference()
            {
                StatusReference = Request.Form["status"],
                OtherStatus = Request.Form["description"],
                AbuseType = Request.Form["type"],
                ReferenceSource = Request.Form["source"],
            };
            
            db.addPatient(tbP, tbPF, tbPM, tbR,arrBS);

            return View(tbPF);

        }


       
            public ActionResult childs()
            {
                EliManagerDB db = new EliManagerDB();

                var pat = db.Patients.ToArray();

                return View(pat);
            }

            

        }
}



