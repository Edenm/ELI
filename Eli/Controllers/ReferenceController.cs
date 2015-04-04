using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eli.Models;

namespace Eli.Controllers
{
    public class ReferenceController : Controller
    {
        //
        // GET: /Treatment/

        [HttpGet]
        public ActionResult IndexReference(string pid)
        {
            EliManagerDB db= new EliManagerDB();

            var refe = db.getAllReferencesByPatientId(pid);

            var pat = db.getPatientById(pid);

            ViewBag.Id = pat.ID;
            ViewBag.Name = pat.PatientFirstName + " " + pat.PatientSurName;

            return View(refe);
        }



    }
}
