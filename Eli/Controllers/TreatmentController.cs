using Eli.Models;
using Eli.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eli.Controllers
{
    public class TreatmentController : Controller
    {

        [HttpGet]
        public ActionResult IndexTreatment(int rid, string pid)
        {
            EliManagerDB db = new EliManagerDB();

            List<Treatment> treatments = new List<Treatment>();

            var treat = db.getAllTreatmentByReferenceNumber(rid);

            foreach (tblTreatment t in treat)
            {
                treatments.Add(new Treatment(t.TreatmentNumber));
            }

            Treatment tr= new Treatment();
            tr.treatment.ReferenceNumber=rid;
            //tr.treatment.TherapistID=;

            treatments.Add(tr);

            var pat = db.getPatientById(pid);

            ViewBag.Id = pat.ID;
            ViewBag.Name = pat.PatientFirstName + " " + pat.PatientSurName;
            ViewBag.Ref = rid;

            return View(treatments);
        }


        [HttpPost]
        public ActionResult IndexTreatment(tblTreatment treat, string submit, string pid)
        {
            EliManagerDB db = new EliManagerDB();

            String ispaid = Request.Form["ispaid"];
            treat.IsPaid = ispaid;
            if (submit.Equals("צור"))
                db.addTreatment(treat);

            else
                db.EditTreatment(treat);

            var refId = treat.ReferenceNumber;
            var patId = pid;

            return RedirectToAction("IndexTreatment", new { rid = refId, pid = patId });
        }

    }
}