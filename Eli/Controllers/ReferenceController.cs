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
        public ActionResult IndexReference(string pid)
        {
            EliManagerDB db= new EliManagerDB();

            var refe = db.getAllReferencesByPatientId(pid);

            refe.Add(new tblReference());

            var pat = db.getPatientById(pid);

            ViewBag.Id = pat.ID;
            ViewBag.Name = pat.PatientFirstName + " " + pat.PatientSurName;

            return View(refe);
        }


        [HttpPost]
        public ActionResult IndexReference(tblReference refe, string submit, string pid)
        {
            EliManagerDB db = new EliManagerDB();
            String StatusReference = Request.Form["StatusReference"];
            String AbuseType = Request.Form["AbuseType"];
            String ReferenceSource = Request.Form["ReferenceSource"];
            refe.StatusReference = StatusReference;
            refe.AbuseType = AbuseType;
            refe.ReferenceSource = ReferenceSource;

            if (submit.Equals("צור"))
                db.addReference(refe, pid);

            else
                db.EditReference(refe);

            return RedirectToAction("IndexReference", new { pid = pid });
        }


        public ActionResult Index()
        {
            EliManagerDB db = new EliManagerDB();

            List<SelectListItem> listItem = new List<SelectListItem>();
            var a = db.Patients.ToList();
            for (int i = 0; i < a.Count(); i++)
            {
                listItem.Add(new SelectListItem() { Value = a.ElementAt(i).ID, Text = a.ElementAt(i).PatientFirstName.ToString() + " " + a.ElementAt(i).PatientSurName.ToString() });
            }

            ViewBag.value = new SelectList(listItem, "Value", "Text");
            return View();

        }

    }
}
