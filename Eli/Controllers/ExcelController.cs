﻿using Eli.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
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
                                  שם = p.FirstName+" "+p.SurName,
                                   
                                  טלפון="*"+p.PhoneNumber
                              };
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=PatientList.xls");
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



        public ActionResult TreatExcel()
        {

            EliManagerDB db = new EliManagerDB();
            List<tblPatient> pat = db.Patients.ToList();
             List<tblRefererencePatient> refpat = db.ReferencePatient.ToList();
             List<tblReferenceTherapist> refterapist = db.ReferenceTherapist.ToList();
            List<tblTherapist> ter = db.Therapist.ToList();
            List<tblTreatment> treat = db.Treatment.ToList();
             List<tblFinancingFactor> fin = db.FinancingFactor.ToList();
            var grid = new GridView();
            grid.DataSource = from p in pat  join rp in refpat on p.ID equals rp.PatientID
                              join rt in refterapist on rp.ReferenceNumber equals rt.ReferenceNumber
                              join t in ter on rt.TherapistID equals t.TherapistID 
                              join tr in treat on rt.ReferenceNumber equals tr.ReferenceNumber
                              join f in fin on tr.FinancingFactorNumber equals f.FinancingFactorNumber
                              where tr.IsPaid=="לא" && (rt.TherapistID==tr.TherapistID)
                              select new
                              {
                                  שם_גורם_מממן = f.FinancingFactorName,
                                  שם_איש_קשר=f.FinancingFactorContactName,
                                  טלפון_איש_קשר="*"+f.FinancingFactorContcatPhoneNumber,              
                                  מייל_איש_קשר=f.FinancingFactorContactMail,
                                  תאריך_טיפול=tr.TreatmentDate.ToString().Substring(1,10),
                                  שעת_טיפול=tr.TreatmentStartTime,
                                  שם_מטופל=p.FirstName+" "+p.SurName,
                                  שם_מטפל=t.TherapistFirstName+" "+t.TherapistSurName,
                                  סכום_טיפול=tr.Cost

                              };
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=NotPaidTreatment.xls");
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
