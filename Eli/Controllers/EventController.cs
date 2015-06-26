using Eli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eli.Controllers
{
    public class EventController : Controller
    {
        /* The Method displays all the events in the system- this is the main form of events  **/
        [HttpGet]
        public ActionResult IndexEvent(string operate, string type)
        {
            EliManagerDB db = new EliManagerDB();

            List<tblEvent> events = db.Events.ToList();

            events.Add(new tblEvent());

            ViewBag.operate = operate;
            ViewBag.type = type;

            return View(events);
        }


        /* This post Method add/edit Events  **/
        [HttpPost]
        public ActionResult IndexEvent(tblEvent ev, string submit)
        {
            EliManagerDB db = new EliManagerDB();

            ViewBag.type = "success";
            try
            {
                if (submit.Equals("צור"))
                {
                    db.addEvent(ev);
                    ViewBag.operate = "אירוע התווסף בהצלחה";
                }
                else 
                {
                    db.EditEvent(ev);
                    ViewBag.operate = "פרטי אירוע התעדכנו בהצלחה";
                }
            }
            catch (Exception e)
            {
                ViewBag.operate = e.Message;
                ViewBag.type = "danger";
            }

            return RedirectToAction("IndexEvent", new { operate = ViewBag.operate, type = ViewBag.type });
        }


        /* This post Method remove Events  **/
        [HttpPost]
        public ActionResult RemoveEvent(int evNum)
        {
            EliManagerDB db = new EliManagerDB();

            ViewBag.type = "success";
            try
            {

                db.RemoveEvent(evNum);
                    ViewBag.operate = "אירוע נמחק בהצלחה";
            }
            catch (Exception e)
            {
                ViewBag.operate = e.Message;
                ViewBag.type = "danger";
            }

            return RedirectToAction("IndexEvent", new { operate = ViewBag.operate, type = ViewBag.type });
        }
    }
}
