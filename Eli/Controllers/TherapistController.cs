using Eli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eli.Controllers
{   
    [Authorize]
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
            List<tblTherapist> therapist = db.Therapist.ToList();
            try
            {
              
                
                if (submit.Equals("צור"))
                {
                    db.addTherapist(tt);
                    therapist.Add(tt);

                }

                else
                    db.EditTherapist(tt);



                
                ViewBag.DataExists = false;
                return View(therapist);
            }
            catch (Exception e)
            {
                ViewBag.DataExists = true;
                ViewBag.msg1 = "ההוספה נכשלה";
                ViewBag.msg2 = tt.TherapistID;
                ViewBag.msg3 = "נמצא כבר במערכת";
                return View(therapist);
            }
        }

        public IView therapist { get; set; }
    }
}
