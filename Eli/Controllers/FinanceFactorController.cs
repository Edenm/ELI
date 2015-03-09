using Eli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eli.Controllers
{
    public class FinanceFactorController : Controller
    {
        public ActionResult AddFinanceFactor()
        {
            EliManagerDB db = new EliManagerDB();

            String name = Request.Form["Name"];
            String type = Request.Form["type"];
            String contactname = Request.Form["contactname"];
            String phone = Request.Form["phone"];
            String mail = Request.Form["mail"];



            var pat = db.FinancingFactor.ToArray();
            int lastNumber = pat.ElementAt(pat.Length - 1).FinancingFactorNumber + 1;
            tblFinancingFactor tp = new tblFinancingFactor()
            {
                FinancingFactorNumber = lastNumber,
                Name = name,
                FinancingFactorType = type,
                ContactName = contactname,
                ContcatPhoneNumber = phone,
                ContactMail = mail,





            };

            // db.addPatient(tp);



            return View(tp);
        }


        public ActionResult NewFinanceFactor()
        {
            return View();
        }
        public ActionResult FinancingFactorDetails(int id)
        {
            EliManagerDB db = new EliManagerDB();


            tblFinancingFactor ff = db.FinancingFactor.ToArray().Single(p => p.FinancingFactorNumber == (id));



            return View(ff);

        }
        public ActionResult FinancingFactor()
        {


            EliManagerDB db = new EliManagerDB();

            var pat = db.FinancingFactor.ToArray();

            return View(pat);
        }
        public ActionResult Index()
        {
            return View();
        }

    }
}
