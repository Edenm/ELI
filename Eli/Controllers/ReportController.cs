using Eli.Models;
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
                EliManagerDB db = new EliManagerDB();
                
                return View();
        }

        public ActionResult FinanceFactorDebatorsReport()
        {
            EliManagerDB db = new EliManagerDB();

            return View(db.getAllDebators());
        }

        public ActionResult GeneratePDF()
        {
            return new Rotativa.ActionAsPdf("FinanceFactorDebatorsReport");
        }
    }
}
