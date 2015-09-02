using Eli.Models;
using Eli.ViewModel;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        static string connectionString = SQLConnection.GetConnectionString();

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
            Response.AddHeader("content-disposition", "attachment; filename=רשימת מטופלים.xls");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.Write("<table><tr><td colspan='3'>רשימת מטופלים</td></tr>");

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return View();


          
        }

        public ActionResult PatientByFinance(String name)
        {

            EliManagerDB db = new EliManagerDB();
            List<tblPatient> pat = db.Patients.ToList();
            List<tblRefererencePatient> refpat = db.ReferencePatient.ToList();
            List<tblReferenceTherapist> refterapist = db.ReferenceTherapist.ToList();
            List<tblTherapist> ter = db.Therapist.ToList();
            List<tblTreatment> treat = db.Treatment.ToList();
            List<tblFinancingFactor> fin = db.FinancingFactor.ToList();
            var grid = new GridView();
            if (name != "הכל")
            {
                grid.DataSource = (from p in pat
                                   join rp in refpat on p.ID equals rp.PatientID
                                   join rt in refterapist on rp.ReferenceNumber equals rt.ReferenceNumber
                                   join t in ter on rt.TherapistID equals t.TherapistID
                                   join tr in treat on rt.ReferenceNumber equals tr.ReferenceNumber
                                   join f in fin on tr.FinancingFactorNumber equals f.FinancingFactorNumber
                                   where f.FinancingFactorName == name
                                   select new
                                   {
                                       שם_גורם_מממן = f.FinancingFactorName,
                                       סוג_גורם_מממן = f.FinancingFactorType,
                                       תז_מטופל = p.ID,

                                       שם_מטופל = p.FirstName + " " + p.SurName,

                                   }).ToList().Distinct(); grid.DataBind();
            }
            else
            {
                grid.DataSource = (from p in pat
                                   join rp in refpat on p.ID equals rp.PatientID
                                   join rt in refterapist on rp.ReferenceNumber equals rt.ReferenceNumber
                                   join t in ter on rt.TherapistID equals t.TherapistID
                                   join tr in treat on rt.ReferenceNumber equals tr.ReferenceNumber
                                   join f in fin on tr.FinancingFactorNumber equals f.FinancingFactorNumber
                                   select new
                                   {
                                       שם_גורם_מממן = f.FinancingFactorName,
                                       סוג_גורם_מממן = f.FinancingFactorType,
                                       תז_מטופל = p.ID,

                                       שם_מטופל = p.FirstName + " " + p.SurName,

                                   }).ToList().Distinct(); grid.DataBind();
            }

            Response.ClearContent();
            Response.Buffer = true;
            
            if (name != "הכל")
            {
                Response.AddHeader("content-disposition", "attachment; filename=מטופלים של  " + name + ".xls");
            }
            else
            {
                Response.AddHeader("content-disposition", "attachment; filename= מטופלים לפי גורמים מממנים.xls");

            }
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            if (name != "הכל")
            {
                htw.Write("<table><tr><td colspan='3'>"+ name+ "</td></tr>");
            }
            else
            {
                htw.Write("<table><tr><td colspan='3'>מטופלים לפי גורם מממן</td></tr>");
            }
            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return View();
        }


        public ActionResult TreatExcel(String name)
        {
            EliManagerDB db = new EliManagerDB();
            List<PatientByFinanceFactor> Result = getPatientByFinanceFactor(name);

            var grid = new GridView();

            grid.DataSource = from p in Result

                              select new
                              {
                                  שם_מטופל = p.FinancingFactor,
                                  ש=p.Patients
                                 

                              };
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=PatientContactList.xls");
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
            Response.AddHeader("content-disposition", "attachment; filename=אנשי קשר.xls");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());


            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.Write("<table><tr><td colspan='3'>אנשי קשר של מטופלים</td></tr>");

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return View();
        }



        // display PatientByFinanceFactorReport
        public List<PatientByFinanceFactor> getPatientByFinanceFactor(string FinancingFactorName)
        {
            List<PatientByFinanceFactor> Result = new List<PatientByFinanceFactor>();
            List<tblPatient> pats = new List<tblPatient>();
            tblPatient tempPatient = new tblPatient();
            tblFinancingFactor tempFinanceNext = new tblFinancingFactor();
            PatientByFinanceFactor temp = new PatientByFinanceFactor();
            Boolean flagJustOne = false;
            int NextId = 0;
            int CurId = 0;

            string Command = "select max(IsNull(finan.FinancingFactorNumber,'')) as FinancingFactorNumber,IsNull(finan.FinancingFactorName,'') as FinancingFactorName,IsNull(finan.FinancingFactorType,'') as FinancingFactorType ,ID,FirstName,SurName " +
                    "from tblPatient left outer join tblRefererencePatient refPat on ID=refPat.PatientID " +
                    "left outer join tblReferenceTherapist refTher on refPat.ReferenceNumber=refTher.ReferenceNumber " +
                    "left outer join tblTherapist ther on refTher.TherapistID=ther.TherapistID " +
                    "left outer join tblTreatment treat on refTher.ReferenceNumber=treat.ReferenceNumber and refTher.TherapistID=treat.TherapistID " +
                    "left outer join tblFinancingFactor finan on treat.FinancingFactorNumber=finan.FinancingFactorNumber ";

            if (FinancingFactorName != "הכל")
            {
                Command += "where finan.FinancingFactorName='" + FinancingFactorName + "' ";
                flagJustOne = true;
            }
            Command += "group by ID,FirstName,SurName,FinancingFactorName,FinancingFactorType order by FinancingFactorNumber desc";

            using (SqlConnection mConnection = new SqlConnection(connectionString))
            {
                mConnection.Open();
                using (SqlCommand cmd = new SqlCommand(Command, mConnection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            while (true)
                            {
                                CurId = (Int32)reader[0];
                                NextId = (Int32)reader[0];

                                tblFinancingFactor tempFinance = new tblFinancingFactor()
                                {
                                    FinancingFactorName = (string)reader[1],
                                    FinancingFactorType = (string)reader[2]
                                };

                                tempPatient = new tblPatient()
                                {
                                    ID = (string)reader[3],
                                    FirstName = (string)reader[4],
                                    SurName = (string)reader[5]
                                };

                                pats.Add(tempPatient);

                                while (NextId == CurId && reader.Read())
                                {

                                    NextId = (Int32)reader[0];

                                    tempPatient = new tblPatient()
                                    {
                                        ID = (string)reader[3],
                                        FirstName = (string)reader[4],
                                        SurName = (string)reader[5]
                                    };

                                    if (NextId == CurId)
                                    {
                                        pats.Add(tempPatient);
                                    }
                                    else
                                    {
                                        temp = new PatientByFinanceFactor()
                                        {
                                            Patients = pats,
                                            FinancingFactor = tempFinance
                                        };

                                        Result.Add(temp);
                                        pats = new List<tblPatient>();
                                        pats.Add(tempPatient);

                                        tempFinanceNext = new tblFinancingFactor()
                                        {
                                            FinancingFactorName = (string)reader[1],
                                            FinancingFactorType = (string)reader[2]
                                        };

                                    }
                                }
                                if (flagJustOne)
                                {
                                    temp = new PatientByFinanceFactor()
                                    {
                                        Patients = pats,
                                        FinancingFactor = tempFinance
                                    };
                                    Result.Add(temp);
                                    break;
                                }
                                if (!reader.Read())
                                {
                                    temp = new PatientByFinanceFactor()
                                    {
                                        Patients = pats,
                                        FinancingFactor = tempFinanceNext
                                    };
                                    Result.Add(temp);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return Result;
        }


    }
}
