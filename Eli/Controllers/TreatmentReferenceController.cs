using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eli.ViewModel;

namespace Eli.Controllers
{
    public class TreatmentReferenceController : Controller
    {
        //
        // GET: /Treatment/

        public ActionResult IndexTreatmentReference()
        {
            TeratmentReferencePatient trp = new TeratmentReferencePatient("111111111");

            return View(trp);
        }

    }
}
