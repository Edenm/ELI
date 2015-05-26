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
    [Authorize]
    public class FinanceFactorController : Controller
    {
        [HttpGet]
        public ActionResult IndexFinancingFactor(string operate, string type)
        {
            EliManagerDB db = new EliManagerDB();

            List<tblFinancingFactor> finfac = db.FinancingFactor.ToList();

            finfac.Add(new tblFinancingFactor());

            ViewBag.operate = operate;
            ViewBag.type = type;

            return View(finfac);

        }

        

        /* This post Method add/edit finance factor  **/
        [HttpPost]
        public ActionResult IndexFinancingFactor(tblFinancingFactor ff, string submit)
        {
            EliManagerDB db = new EliManagerDB();

            ViewBag.type = "success";
            try{
                if (submit.Equals("צור"))
                {
                    db.addFinanceFactor(ff);
                    ViewBag.operate = "גורם מממן התווסף בהצלחה";
                }
                else
                {
                    db.EditFinanceFactor(ff);
                    ViewBag.operate = "פרטי גורם מממן התעדכנו בהצלחה";
                }
            }catch(Exception e){
                ViewBag.operate = e.Message;
                ViewBag.type = "danger";
            }

            return RedirectToAction("IndexFinancingFactor", new { operate = ViewBag.operate, type = ViewBag.type });
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
