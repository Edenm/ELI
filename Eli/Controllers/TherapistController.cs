using Eli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eli.Controllers
{
    public class TherapistController : Controller
    {

        [HttpGet]
        public ActionResult IndexTherapist()
        {
            EliManagerDB db = new EliManagerDB();

            var therapist = db.Therapist.ToArray();

            return View(therapist);
        }


        [HttpPost]
        public ActionResult IndexTherapist(tblTherapist tt, string submit)
        {
            EliManagerDB db = new EliManagerDB();

            if (submit.Equals("צור"))
                db.addTherapist(tt);

            else
                db.EditTherapist(tt);

            var therapist = db.Therapist.ToArray();

            return View(therapist);
        }
    }
}
