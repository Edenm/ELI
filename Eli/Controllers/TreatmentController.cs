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
            /////////////////////////////////////////////for finance factor

            List<SelectListItem> listItem1 = new List<SelectListItem>();
            var a = db.FinancingFactor.ToList();
            for (int i = 0; i < a.Count(); i++)
            {
                listItem1.Add(new SelectListItem() { Value = a.ElementAt(i).FinancingFactorNumber.ToString(), Text = a.ElementAt(i).FinancingFactorContactName.ToString() });
            }

            ViewBag.value1 = new SelectList(listItem1, "Value", "Text");
            //////////////////////////////////////////////////////////////////////////////////////
            
            /////////////////////////////////////////////for finance factor

            List<SelectListItem> listItem2 = new List<SelectListItem>();
            var b = db.Therapist.ToList();
            for (int i = 0; i < b.Count(); i++)
            {
                listItem2.Add(new SelectListItem() { Value = b.ElementAt(i).TherapistID.ToString(), Text = b.ElementAt(i).TherapistFirstName+" "+b.ElementAt(i).TherapistSurName });
            }

            ViewBag.value2 = new SelectList(listItem2, "Value", "Text");
            //////////////////////////////////////////////////////////////////////////////////////



            List<Treatment> treatments = new List<Treatment>();

            tblTherapist ther = (tblTherapist)Session["Therapist"];

            var treat = db.getAllTreatmentByReferenceAndTherapist(rid, ther.TherapistID);

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

            return View(treatments);
        }


        [HttpPost]
        public ActionResult IndexTreatment(tblTreatment treat, string submit, string pid)
        {
            EliManagerDB db = new EliManagerDB();

            String ispaid = Request.Form["ispaid"];
            treat.IsPaid = ispaid;
            String finance = Request.Form["finance"];
            if (finance != null && finance!="")
            treat.FinancingFactorNumber = Convert.ToInt32(finance);
            int reference = Convert.ToInt32(Request.Form["reference"]);
            treat.ReferenceNumber = reference;
           String therapist = Request.Form["therapist"];
           treat.TherapistID = therapist;
           String time = Request.Form["time"];
           treat.TreatmentStartTime = TimeSpan.Parse(time);

           db.addTherapisttoRef(therapist, reference);
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