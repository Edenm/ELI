using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eli.Models;

namespace Eli.Controllers
{
    public class ReferenceController : Controller
    {
        //
        // GET: /Treatment/

        [HttpGet]
        public ActionResult IndexReference(string pid, string operate, string type)
        {
            EliManagerDB db= new EliManagerDB();

            tblTherapist ther = (tblTherapist)Session["Therapist"];
            var refe = db.getAllReferencesByPatientAndTherapist(pid, ther.TherapistID);

            refe.Add(new tblReference());

            var pat = db.getPatientById(pid);

            ViewBag.Id = pat.ID;
            ViewBag.Name = pat.FirstName + " " + pat.SurName;
            ViewBag.operate = operate;
            ViewBag.type=type;

            return View(refe);
        }


        [HttpPost]
        public ActionResult IndexReference(tblReference refe, string submit, string pid)
        {
            EliManagerDB db = new EliManagerDB();

            ViewBag.type = "success";
            if (submit.Equals("צור"))
            {
                tblTherapist ther = (tblTherapist)Session["Therapist"];
                db.addReference(refe, pid, ther.TherapistID);
                ViewBag.operate = "ההפנייה התווספה בהצלחה";
            }

            else
            {
                db.EditReference(refe);
                ViewBag.operate = "פרטי ההפנייה התעדכנו בהצלחה";
            }

            return RedirectToAction("IndexReference", new { pid = pid, operate = ViewBag.operate, type = ViewBag.type });
        }


    }
}
