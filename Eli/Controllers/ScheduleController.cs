using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eli.Models;
using Eli.ViewModel;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using System.Text;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using System.Security.Cryptography.X509Certificates;

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

