using Eli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eli.Controllers
{
    public class PdfController : Controller
    {
        //
        // GET: /Pdf/

        public ActionResult Index()
        {
            EliManagerDB db = new EliManagerDB();

            List<tblTherapist> t = db.Therapist.ToList();
            return View(t);
        }

        public ActionResult GeneratePDF()
        {
            return new Rotativa.ActionAsPdf("Index");
        }

        

    }
}
