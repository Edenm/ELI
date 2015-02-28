using Eli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        public ActionResult NewPatient()
        {
            return View();
        }

        public ActionResult AddPatient()
        {
            EliManagerDB db = new EliManagerDB();

            String Id = Request.Form["Id"];
            String fName = Request.Form["firstName"];
            String lName = Request.Form["lastName"];
            String gender = Request.Form["gender"];
            String education = Request.Form["education"];
            String phone1 = Request.Form["phone1"];
            String phone2 = Request.Form["phone2"];
            String city = Request.Form["city"];
            String street = Request.Form["street"];

            int homeNumber;
            int.TryParse(Request.Form["homeNumber"], out homeNumber);

            String mail = Request.Form["mail"];

            tblPatient tp = new tblPatient(){
                ID=Id,
                PatientFirstName=fName,
                PatientSurName=lName,
                Gender=gender,
                EducationalFramework=education,
                ContcatPhoneNumber1=phone1,
                ContcatPhoneNumber2=phone2,
                City=city,
                Street=street,
                HomeNumber=homeNumber,
                ContactMail=mail,
            };

            db.addPatient(tp);

            var pat = db.Patients.ToArray();

            return View(pat);
        }

        public ActionResult childs()
        {
            EliManagerDB db = new EliManagerDB();

            var pat = db.Patients.ToArray();

            return View(pat);
        }

        public ActionResult MainPatients()
        {


            EliManagerDB db = new EliManagerDB();

            var pat = db.Patients.ToArray();

            return View(pat);
        }


        public ActionResult PatientDetails(String id)
        {
            
            EliManagerDB db = new EliManagerDB();
            
            var pat = db.Patients.ToArray();
            tblPatient patient = db.Patients.ToArray().Single(p => p.ID.ToString()==(id));

            return View(patient);
        }
        
    }
}
