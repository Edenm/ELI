using Eli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eli.Controllers
{
    public class ExportPdfController : Controller
    {
        //
        // GET: /ExportPdf/

        public ActionResult getpatients()
        {
            EliManagerDB db = new EliManagerDB();

            List<tblTherapist> t = db.Therapist.ToList();
            return View(t);
        }

        public ActionResult GeneratePDF()
        {
            return new Rotativa.ActionAsPdf("getpatients");
        }

    }
}
