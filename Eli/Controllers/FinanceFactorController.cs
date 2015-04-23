using Eli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;

using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;

namespace Eli.Controllers
{
    public class FinanceFactorController : Controller
    {
        [HttpGet]
        public ActionResult IndexFinancingFactor()
        {
            EliManagerDB db = new EliManagerDB();

            List<tblFinancingFactor> finfac = db.FinancingFactor.ToList();

            finfac.Add(new tblFinancingFactor());

            return View(finfac);

          
        }


        /* This post Method add/edit finance factor  **/
        [HttpPost]
        public ActionResult IndexFinancingFactor(tblFinancingFactor ff, string submit)
        {
            EliManagerDB db = new EliManagerDB();


           
            if (submit.Equals("צור"))
                db.addFinanceFactor(ff);

            else if (submit.Equals("שמור"))
                db.EditFinanceFactor(ff);

            List<tblFinancingFactor> finfac = db.FinancingFactor.ToList();

            finfac.Add(new tblFinancingFactor());

            return View(finfac);
        }

        /* The Methods updates finance factor details  **/
        [HttpPost]
        public ActionResult EditFinanceFactor(tblFinancingFactor t)
        {
            var manager = new EliManagerDB();
            manager.EditFinanceFactor(t);
            return RedirectToAction("MainFinancingFactor");
        }

        [HttpGet]
        public ActionResult EditFinanceFactor(int id)
        {
            EliManagerDB db = new EliManagerDB();
            tblFinancingFactor ff = db.FinancingFactor.ToArray().Single(p => p.FinancingFactorNumber == (id));
            return View(ff);
        }

//-------------------------------------------------------------------------------------------------------------------------------//



        public ActionResult ExportToExcel()
        {

            EliManagerDB db = new EliManagerDB();
            List<tblFinancingFactor> finfac = db.FinancingFactor.ToList();

            var grid = new GridView();
            grid.DataSource = from p in finfac
                              select new
                              {
                                  Id = p.FinancingFactorNumber,
                                  Name = p.FinancingFactorName
                              };
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Default;



            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return View();
        }

    }
}
