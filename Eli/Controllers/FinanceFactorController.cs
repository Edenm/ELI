using Eli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

           // var finfac = db.FinancingFactor.ToArray();

            List<tblFinancingFactor> finfac = db.FinancingFactor.ToList();

            finfac.Add(new tblFinancingFactor());
            ViewBag.a = "aaa";
            return View(finfac);

          
        }

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


        /* This post Method add/edit finance factor  **/
        [HttpPost]
        public ActionResult IndexFinancingFactor(tblFinancingFactor ff, string submit)
        {
            EliManagerDB db = new EliManagerDB();
            String type = Request.Form["type"];
            ff.FinancingFactorType = type;
            if (submit.Equals("צור"))
                db.addFinanceFactor(ff);

            else
                db.EditFinanceFactor(ff);

            List<tblFinancingFactor> finfac = db.FinancingFactor.ToList();

            finfac.Add(new tblFinancingFactor());

            return View(finfac);
        }






//-------------------------------------------------------------------------------------------------------------------------------//







        /* The Method adds a new finance factor to system  **/

        public ActionResult AddFinanceFactor()
        {
            EliManagerDB db = new EliManagerDB();

            String name = Request.Form["Name"];
            String type = Request.Form["type"];
            String contactname = Request.Form["contactname"];
            String phone = Request.Form["phone"];
            String mail = Request.Form["mail"];



            var pat = db.FinancingFactor.ToArray();
            int lastNumber = pat.ElementAt(pat.Length - 1).FinancingFactorNumber + 1;
            tblFinancingFactor tp = new tblFinancingFactor()
            {
                FinancingFactorNumber = lastNumber,
                FinancingFactorName = name,
                FinancingFactorType = type,
                FinancingFactorContactName = contactname,
                FinancingFactorContcatPhoneNumber = phone,
                FinancingFactorContactMail = mail,





            };

            db.addFinanceFactor(tp);

            var temp = db.FinancingFactor.ToArray();

            return RedirectToAction("MainFinancingFactor");




        }




        //
        // GET: /SendMailer/  
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ViewResult Index(Eli.Models.MailModel _objModelMail)
        {
            if (ModelState.IsValid)
            {
                _objModelMail.From = "margulis.shaharm@gmail.com";
                MailMessage mail = new MailMessage();
                mail.To.Add(_objModelMail.To);
                mail.From = new MailAddress(_objModelMail.From);
                mail.Subject = _objModelMail.Subject;
                string Body = _objModelMail.Body;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential
                ("margulis.shaharm@gmail.com", "smajrubh123");// Enter seders User name and password
                smtp.EnableSsl = true;
                smtp.Send(mail);

                return View("view", _objModelMail);


            }
            else
            {
                return View();
            }
        }
        /* The Method opens a new form of finance factors  **/

        public ActionResult NewFinanceFactor()
        {
            return View();
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



        /* The Method get parameters of finance factor and displays his details  **/

        public ActionResult FinancingFactorDetails(int id)
        {
            EliManagerDB db = new EliManagerDB();


            tblFinancingFactor ff = db.FinancingFactor.ToArray().Single(p => p.FinancingFactorNumber == (id));



            return View(ff);

        }

        /* The Method displays all the financefactor in the system-this is the main form of finance factor  **/

        public ActionResult MainFinancingFactor()
        {


            EliManagerDB db = new EliManagerDB();

            var pat = db.FinancingFactor.ToArray();

            return View(pat);
        }
       

    }
}
