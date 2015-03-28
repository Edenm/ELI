using Eli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.IO;
namespace Eli.Controllers
{
    public class FinanceFactorController : Controller
    {
        [HttpGet]
        public ActionResult IndexFinancingFactor()
        {
            EliManagerDB db = new EliManagerDB();

           // var finfac = db.FinancingFactor.ToArray();

            List<tblFinancingFactor> finfac = db.FinancingFactor.ToList();

            finfac.Add(new tblFinancingFactor());
            
            return View(finfac);

          
        }


        /* This post Method add/edit finance factor  **/
        [HttpPost]
        public ActionResult IndexFinancingFactor(tblFinancingFactor ff, string submit)
        {
            EliManagerDB db = new EliManagerDB();
            String type = Request.Form["type"];
            ff.FinancingFactorType = type;
            if (submit.Equals("צור"))
                db.addFinanceFactor(ff);

            else
                db.EditFinanceFactor(ff);

            List<tblFinancingFactor> finfac = db.FinancingFactor.ToList();

            finfac.Add(new tblFinancingFactor());

            return View(finfac);
        }






//-------------------------------------------------------------------------------------------------------------------------------//







        /* The Method adds a new finance factor to system  **/

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

            db.addFinanceFactor(tp);

            var temp = db.FinancingFactor.ToArray();

            return RedirectToAction("MainFinancingFactor");




        }

        /* The Method opens a new form of finance factors  **/

        public ActionResult NewFinanceFactor()
        {
            return View();
        }


        /* The Methods updates finance factor details  **/

        [HttpPost]
        public ActionResult EditFinanceFactor(tblFinancingFactor t)
        {
            var manager = new EliManagerDB();
            manager.EditFinanceFactor(t);
            return RedirectToAction("MainFinancingFactor");
        }

        [HttpGet]
        public ActionResult EditFinanceFactor(int id)
        {
            EliManagerDB db = new EliManagerDB();


            tblFinancingFactor ff = db.FinancingFactor.ToArray().Single(p => p.FinancingFactorNumber == (id));

            return View(ff);
        }


        /* The Method get parameters finance factor and removes its from system  **/

        public ActionResult DeleteFinancingFactor(int id)
        {

            
                EliManagerDB db = new EliManagerDB();


                tblFinancingFactor ff = db.FinancingFactor.ToArray().Single(p => p.FinancingFactorNumber == (id));
                var manager = new EliManagerDB();
                manager.removeFinanceFactor(ff);




                return RedirectToAction("MainFinancingFactor");
           
            
        }



        /* The Method get parameters of finance factor and displays his details  **/

        public ActionResult FinancingFactorDetails(int id)
        {
            EliManagerDB db = new EliManagerDB();


            tblFinancingFactor ff = db.FinancingFactor.ToArray().Single(p => p.FinancingFactorNumber == (id));



            return View(ff);

        }

        /* The Method displays all the financefactor in the system-this is the main form of finance factor  **/

        public ActionResult MainFinancingFactor()
        {


            EliManagerDB db = new EliManagerDB();

            var pat = db.FinancingFactor.ToArray();

            return View(pat);
        }
       

    }
}
