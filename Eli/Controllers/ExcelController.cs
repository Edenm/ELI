using Eli.Models;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eli.Controllers
{
    public class ExcelController : Controller
    {
        //
        // GET: /Excel/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PatientExcel()
        {

            EliManagerDB db = new EliManagerDB();
            List<tblPatient> patient = db.Patients.ToList();

            var grid = new GridView();
            grid.DataSource = from p in patient
                              
                              select new
                              {
                                  תז=p.ID,
                                  מין=p.Gender,
                                  שם = p.FirstName+" "+p.SurName,
                                   
                                  טלפון="*"+p.PhoneNumber
                              };
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=PatientList.xls");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return View();


          
        }



        public ActionResult TreatExcel()
        {
            EliManagerDB db = new EliManagerDB();
            List<tblPatient> patient = db.Patients.ToList();
            WebGrid grid = new WebGrid(source: patient, canPage: false, canSort: false);
            string gridHtml = grid.GetHtml(
                    columns: grid.Columns(
                    grid.Column("ID", "Id"),
                    grid.Column("Gender", "Gender")
                   )).ToString();

            string exportData = String.Format("<html><head>{0}</head><body>{1}</body></html>", "<style>table{ border-spacing: 10px; border-collapse: separate; }</style>", gridHtml);
            var bytes = System.Text.Encoding.Default.GetBytes(exportData);
            using (var input = new MemoryStream(bytes))
            {
                var output = new MemoryStream();
                var document = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 50);
                var writer = PdfWriter.GetInstance(document, output);
                writer.CloseStream = false;
                document.Open();

                var xmlWorker = iTextSharp.tool.xml.XMLWorkerHelper.GetInstance();
                xmlWorker.ParseXHtml(writer, document, input, System.Text.Encoding.Default);
                document.Close();
                output.Position = 0;

                return new FileStreamResult(output, "application/pdf");
            }
        }


        public ActionResult ContactPatientExcel()
        {

            EliManagerDB db = new EliManagerDB();
            List<tblPatient> patient = db.Patients.ToList();

            var grid = new GridView();
            grid.DataSource = from p in patient

                              select new
                              {
                                  שם_מטופל = p.FirstName + " " + p.SurName,
                                  שם_איש_קשר1=p.ContactName1,
                                  תפקיד1 = p.ContactProfession1,
                                  טלפון1 = "*"+p.ContactPhone1,
                                  מייל1 = p.ContactMail1,
                                  שם_איש_קשר2 = p.ContactName2,
                                  תפקיד2 = p.ContactProfession2,
                                  טלפון2 = "*"+p.ContactPhone2,
                                 מייל2 = p.ContactMail2,
                                  שם_איש_קשר3 = p.ContactName3,
                                  תפקיד3 = p.ContactProfession3,
                                  טלפון3 = "*"+p.ContactPhone3,
                                  מייל3 = p.ContactMail3,



                                  
                              };
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=PatientContactList.xls");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.UTF8;



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
