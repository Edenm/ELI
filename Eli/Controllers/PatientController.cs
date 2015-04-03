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
            EliManagerDB db = new EliManagerDB();

            var pat = db.Patients.ToArray();

            return View(pat);
        }

        /* The Method displays all the patients in the system-this is the main form of patients  **/
        [HttpGet]
        public ActionResult IndexPatients()
        {
            EliManagerDB db = new EliManagerDB();

            var pat = db.Patients.ToArray();

            return View(pat);
        }


        /* The method get a patient details and update the patient **/
        [HttpPost]
        public ActionResult IndexPatients(tblPatient obj)
        {
            EliManagerDB db = new EliManagerDB();

            Type type = obj.GetType();
            String str = type.Name;

            if (obj.GetType() == typeof(tblPatient))
                db.EditPatient((tblPatient)obj);

            //if (obj is Family)
            // db.EditFamily((Family)obj);

            var pat = db.Patients.ToArray();

            return View(pat);
        }


        public ActionResult _EditFamily(String id)
        {
            EliManagerDB db = new EliManagerDB();

            Family objFam = new Family(id);
            return PartialView(objFam);
        }

        public ActionResult NewPatient()
        {
            return View();
        }





        /* The Method get parameters of patient and add him to system  **/

        public ActionResult AddPatient()
        {

            EliManagerDB db = new EliManagerDB();


       //-----------personal details of patients--------------------------

            tblPatient tbP = new tblPatient()
            {
                ID = Request.Form["id"],
                PatientFirstName = Request.Form["fname"],
                PatientSurName = Request.Form["lname"],
                Gender = Request.Form["gender"],
                EducationalFramework = Request.Form["education"],
                ContcatPhoneNumber = Request.Form["phone"],
                Address = Request.Form["address"]
            };


      //-----------more details of patients parents--------------------------

            tblParent tbPF = new tblParent()
            {
                ID = Request.Form["idf"],
                FirstName = Request.Form["fnamef"],
                SurName = Request.Form["lnamef"],
                //BirthDate = Convert.ToDateTime(birthdatef),
                ContcatPhoneNumber = Request.Form["phonef"],
                Address = Request.Form["adressf"],
                ContactMail = Request.Form["mailf"],
                IsWorking = Request.Form["isworkf"],
                Explain=Request.Form["explainf"]
            };

            tblParent tbPM = new tblParent()
            {
                 ID = Request.Form["idm"],
                FirstName = Request.Form["fnamem"],
                SurName = Request.Form["lnamem"],
                //BirthDate = Convert.ToDateTime(birthdatef),
                ContcatPhoneNumber = Request.Form["phonem"],
                Address = Request.Form["adressm"],
                ContactMail = Request.Form["mailm"],
                IsWorking = Request.Form["isworkm"],
                Explain=Request.Form["explainm"]
            };


       //-----------more details of patients brothers and sisters--------------------------

            List<tblBrotherSister> arrBS = new List<tblBrotherSister>();

            for (int i = 1; Request.Form["id" + i] == ""; i++)
            {
                tblBrotherSister tbBS = new tblBrotherSister()
                {
                    ID = Request.Form["id" + i],
                    FirstName = Request.Form["fname" + i],
                    SurName = Request.Form["lname" + i],
                    // BirthDate=Request.Form["birthdate"],
                    Gender = Request.Form["gender" + i],
                    StudyFramework = Request.Form["education" + i]
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



