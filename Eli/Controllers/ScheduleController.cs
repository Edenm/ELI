using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eli.Models;
using Eli.ViewModel;

namespace Eli.Controllers
{
    [Authorize]
    public class ScheduleController : Controller
    {
        //
        // GET: /Schedule/

        public ActionResult IndexSchedule()
        {
            EliManagerDB db = new EliManagerDB();

            List<Treatment> treatments = new List<Treatment>();

            tblTherapist ther = (tblTherapist)Session["Therapist"];

            var treat = db.getAllTreatmentByTherapist(ther.TherapistID);

            foreach (tblTreatment t in treat)
            {
                treatments.Add(new Treatment(t.TreatmentNumber));
            }

            return View(treatments);
        }

    }
}
