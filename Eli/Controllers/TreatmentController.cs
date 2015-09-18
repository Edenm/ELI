using Eli.Models;
using Eli.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eli.Controllers
{
    [Authorize]
    public class TreatmentController : Controller
    {

        [HttpGet]
        public ActionResult IndexTreatment(int rid, string pid, string operate, string type)
        {
            EliManagerDB db = new EliManagerDB();
           
            List<Treatment> treatments = new List<Treatment>();

            tblTherapist ther = (tblTherapist)Session["Therapist"];

            List<tblTreatment> treat = null;

            if (ther.UserName != "admin")
            {
                treat = db.getAllTreatmentByReferenceAndTherapist(rid, ther.TherapistID);
            }
            else
            {
                treat = db.getAllTreatmentByReference(rid);
            }
 

            foreach (tblTreatment t in treat)
            {
                treatments.Add(new Treatment(t.TreatmentNumber));
            }

            Treatment tr= new Treatment();
            tr.treatment.ReferenceNumber=rid;
            tr.treatment.TherapistID = ther.TherapistID;

            treatments.Add(tr);

            var pat = db.getPatientById(pid);

            ViewBag.Id = pat.ID;
            ViewBag.Name = pat.FirstName + " " + pat.SurName;
            ViewBag.Ref = db.getReferenceByReferenceNumber(rid).ReasonReference;
            ViewBag.operate = operate;
            ViewBag.type = type;

            return View(treatments);
        }


        [HttpPost]
        public ActionResult IndexTreatment(tblTreatment treat, string submit, string pid)
        {
            EliManagerDB db = new EliManagerDB();

            tblTherapist ther = (tblTherapist)Session["Therapist"];
            treat.TherapistID = ther.TherapistID;

            ViewBag.type = "success";
            if (submit.Equals("צור"))
            {
                db.addTreatment(treat);
                ViewBag.operate = "הטיפול התווספף בהצלחה";
            }

            else
            {
                db.EditTreatment(treat);
                ViewBag.operate = "פרטי הטיפול התעדכנו בהצלחה";
            }

            var refId = treat.ReferenceNumber;
            var patId = pid;

            return RedirectToAction("IndexTreatment", new { rid = refId, pid = patId, operate = ViewBag.operate, type = ViewBag.type });
        }

    }
}