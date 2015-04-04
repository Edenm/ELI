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
        //
        // GET: /Treatment/
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

            var pat = db.getPatientById(pid);

            ViewBag.Id = pat.ID;
            ViewBag.Name = pat.PatientFirstName + " " + pat.PatientSurName;

            return View(treatments);
        }

    }
}
