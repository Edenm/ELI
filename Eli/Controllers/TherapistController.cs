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
        //
        // GET: /Therapist/

        public ActionResult Index()
        {
            EliManagerDB db = new EliManagerDB();

            var therapist = db.Therapist.ToArray();

            return View(therapist);
        }

    }
}
