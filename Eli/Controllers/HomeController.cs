﻿using Eli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eli.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
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

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
