using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eli.Models;

namespace Eli.Controllers
{
    [Authorize]
    public class ReferenceController : Controller
    {
        //
        // GET: /Treatment/

        [HttpGet]
        public ActionResult IndexReference(string pid, string operate, string type)
        {
            EliManagerDB db= new EliManagerDB();

            tblTherapist ther = (tblTherapist)Session["Therapist"];

            List<tblReference> refe=null;

            if (ther.UserName != "admin")
            {
                refe = db.getAllReferencesByPatientAndTherapist(pid, ther.TherapistID);
            }
            else
            {
                refe = db.getAllReferencesByPatient(pid);
            }

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
            if(refe.EndDateReference<refe.StartDateReference)
            {
                ViewBag.operate = "תאריך התחלה צריך להיות לפני תאריך סיום";
                return RedirectToAction("IndexReference", new { pid = pid, operate = ViewBag.operate, type = ViewBag.type });

            }
            ViewBag.type = "success";
            try
            {
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
            }
            catch (Exception e)
            {
                ViewBag.operate = e.Message;
                ViewBag.type = "danger";
            }
            

            return RedirectToAction("IndexReference", new { pid = pid, operate = ViewBag.operate, type = ViewBag.type });
        }

        [HttpPost]
        public ActionResult ShareReference(tblReference refe, string submit, string pid, string therID)
        {
            EliManagerDB db = new EliManagerDB();

            ViewBag.type = "success";

            try { 
                if (submit.Equals("שתף"))
                {
                    db.ShareReferenceToAnotherFinanceFactor(refe.ReferenceNumber, therID);
                    ViewBag.operate = "ההפנייה שותפה בהצלחה";
                }
            }
            catch (Exception e)
            {
                ViewBag.operate = e.Message;
                ViewBag.type = "danger";
            }

            return RedirectToAction("IndexReference", new { pid = pid, operate = ViewBag.operate, type = ViewBag.type });
        }

    }
}
