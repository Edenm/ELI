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

            List<tblTherapist> therapist = db.Therapist.ToList();

            therapist.Add(new tblTherapist());

            return View(therapist);
        }


        [HttpPost]
        public ActionResult IndexTherapist(tblTherapist tt, string submit)
        {
            EliManagerDB db = new EliManagerDB();
            String gender = Request.Form["gender"];
            tt.Gender = gender;
            if (submit.Equals("צור"))
                db.addTherapist(tt);

            else
                db.EditTherapist(tt);

            List<tblTherapist> therapist = db.Therapist.ToList();

            therapist.Add(new tblTherapist());

            return View(therapist);
        }
    }
}
