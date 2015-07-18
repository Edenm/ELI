using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eli.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/

        public ActionResult IndexReport()
        {
            
                return View();
        }
    }
}
