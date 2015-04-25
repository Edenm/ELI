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
        public ActionResult IndexTherapist(string operate, string type)
        {
            EliManagerDB db = new EliManagerDB();

            List<tblTherapist> therapist = db.Therapist.ToList();

            therapist.Add(new tblTherapist());

            ViewBag.operate = operate;
            ViewBag.type = type;
            return View(therapist);
        }


        [HttpPost]
        public ActionResult IndexTherapist(tblTherapist tt, string submit)
        {
                EliManagerDB db = new EliManagerDB();

                ViewBag.type = "success";
                try{
                    if (submit.Equals("צור")){
                   
                            db.addTherapist(tt);
                            ViewBag.operate = "מטפל התווסף בהצלחה";
                    }
                    else{
                            db.EditTherapist(tt);
                            ViewBag.operate = "פרטי מטפל התעדכנו בהצלחה";
                    }
                 }catch (Exception e){
                        ViewBag.operate = e.Message;
                        ViewBag.type = "danger";
                 }

                return RedirectToAction("IndexTherapist", new { operate = ViewBag.operate, type = ViewBag.type });
        }

        public IView therapist { get; set; }
    }

}
