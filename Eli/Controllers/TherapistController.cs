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

                therapist.Add(new tblTherapist());

                ViewBag.type = "success";
                if (submit.Equals("צור")){
                    try
                    {
                            db.addTherapist(tt);
                            ViewBag.operate = "מטפל התווסף בהצלחה";
                            
                    }
                    catch (Exception e)
                    {
                        ViewBag.operate = e.Message;
                        ViewBag.type = "danger";
                    }
                }
                
                else{
                    db.EditTherapist(tt);
                    ViewBag.operate = "פרטי מטפל התעדכנו בהצלחה";
                }
                

                return View(therapist);
            
        }

        public IView therapist { get; set; }
    }
}
