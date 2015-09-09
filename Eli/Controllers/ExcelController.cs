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
            htw.Write("<table><tr><td colspan='3'><h2><u><b>רשימת מטופלים<b></u></h2></td></tr>");

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return View();


          
        }

        public ActionResult PatientByFinance(String name,String type)
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
                                      
                                       תז_מטופל = p.ID,

                                       שם_מטופל = p.FirstName + " " + p.SurName,

                                   }).ToList().Distinct(); grid.DataBind();
            }
            else
            {
                return RedirectToAction("allPatientByFinance");

            }

            Response.ClearContent();
            Response.Buffer = true;
            
            
                Response.AddHeader("content-disposition", "attachment; filename=מטופלים של  " + name + ".xls");
            
            
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            String s = "שם גורם מממן= " + name + " ,סוג גורם מממן = " + type ;

            htw.Write("<table><tr><td colspan='3'><h2><u><b>" + s + "<b></u></h2></td></tr>");
            
           
            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return View();
        }

        public ActionResult allPatientByFinance()
        {
            EliManagerDB db = new EliManagerDB();
            List<tblPatient> pat = db.Patients.ToList();
            List<tblRefererencePatient> refpat = db.ReferencePatient.ToList();
            List<tblReferenceTherapist> refterapist = db.ReferenceTherapist.ToList();
            List<tblTherapist> ter = db.Therapist.ToList();
            List<tblTreatment> treat = db.Treatment.ToList();
            List<tblFinancingFactor> fin = db.FinancingFactor.ToList();
            Double totall = db.totatlNotPaidTreat();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=מטופלים לפי גורמים מממנים.xls");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            Response.Charset = "";
            var data = db.Treatment.Where(d => d.IsPaid == "לא");

            for (int i = 0; i < fin.Count(); i++)
            {

                var mail = db.getFinanceMailByName(fin.ElementAt(i).FinancingFactorName);

                var grid = new GridView();
                grid.DataSource = (from p in pat
                                   join rp in refpat on p.ID equals rp.PatientID
                                   join rt in refterapist on rp.ReferenceNumber equals rt.ReferenceNumber
                                   join t in ter on rt.TherapistID equals t.TherapistID
                                   join tr in treat on rt.ReferenceNumber equals tr.ReferenceNumber
                                   join f in fin on tr.FinancingFactorNumber equals f.FinancingFactorNumber
                                   where f.FinancingFactorName == fin.ElementAt(i).FinancingFactorName.ToString()
                                   select new
                                   {

                                       תז_מטופל = p.ID,

                                       שם_מטופל = p.FirstName + " " + p.SurName,

                                   }).ToList().Distinct(); grid.DataBind();

                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                if (i == 0)
                {
                    htw.Write("<table><tr><td colspan='3'><h1><u><b> מטופלים לפי גורמים ממנים <b></u></h1></td></tr>");

                }
                if (db.getNumPatientPerFinance(fin.ElementAt(i).FinancingFactorName) == 0)
                {
                    htw.Write("<br><table><tr><td colspan='3'><h3><u><b>" + fin.ElementAt(i).FinancingFactorName + "<b></u></h3></td></tr>");
                    htw.Write("<table><tr><td colspan='3'><b> אין מטופלים עבור גורם מממן  " + fin.ElementAt(i).FinancingFactorName + "  מייל= " + mail + "<b></td></tr>");
                    grid.RenderControl(htw);

                    Response.Output.Write(sw.ToString());
                    continue;

                }
                htw.Write("<table><tr><td colspan='3'><h3><u><b>" + fin.ElementAt(i).FinancingFactorName + "<b><u></h3></td></tr>");
                htw.Write("<table><tr><td colspan='3'><b> מטופלים עבור גורם מממן " + fin.ElementAt(i).FinancingFactorName + "  מייל= " + mail + "<b></td></tr>");
                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
            }

            Response.Flush();
            Response.End();

            return View();
        }

        public ActionResult PaymentByFinance(String name)
        {

            EliManagerDB db = new EliManagerDB();
            var data = db.Treatment.Where(d => d.IsPaid == "לא");
            var totallSum = data.Sum(d => d.Cost);


            List<tblPatient> pat = db.Patients.ToList();
            List<tblRefererencePatient> refpat = db.ReferencePatient.ToList();
            List<tblReferenceTherapist> refterapist = db.ReferenceTherapist.ToList();
            List<tblTherapist> ter = db.Therapist.ToList();
            List<tblTreatment> treat = db.Treatment.ToList();
            List<tblFinancingFactor> fin = db.FinancingFactor.ToList();
            var financeNum = db.getFinanceNumByName(name);
            var mail = db.getFinanceMailByName(name);
            var allFinance = (from t in treat
                              where t.FinancingFactorNumber.ToString() == financeNum && t.IsPaid=="לא"
                              select new { t.Cost }
                  ).ToList();
            var Financesum = allFinance.Select(c => c.Cost).Sum();
            var grid = new GridView();
            if (name != "הכל")
            {
                grid.DataSource = (from p in pat
                                   join rp in refpat on p.ID equals rp.PatientID
                                   join rt in refterapist on rp.ReferenceNumber equals rt.ReferenceNumber
                                   join t in ter on rt.TherapistID equals t.TherapistID
                                   join tr in treat on rt.ReferenceNumber equals tr.ReferenceNumber
                                   join f in fin on tr.FinancingFactorNumber equals f.FinancingFactorNumber
                                   where f.FinancingFactorName == name && tr.IsPaid=="לא"
                                   select new
                                   {

                                       תאריך = tr.TreatmentDate,

                                       שם_מטופל = p.FirstName + " " + p.SurName,
                                       עלות = tr.Cost,


                                   }).ToList().Distinct(); grid.DataBind();
            }
            else
            {
                return RedirectToAction("allPaymentFinance");
            }

            Response.ClearContent();
            Response.Buffer = true;

           
                Response.AddHeader("content-disposition", "attachment; filename=חובות של  " + name + ".xls");
           
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            String s = "שם גורם מממן= " + name + " ,סוג גורם מממן = " ;
            htw.Write("<table><tr><td colspan='3'><b><u><h2> טיפולים שלא שולמו עבור גורם מממן " + name + "  מייל= " + mail + "</h2></u><b></td></tr>");
            htw.Write("<table><tr><td colspan='3'><b><u><h3>סהכ חובות עבור כל גורמים הממנים:" + totallSum + "</h3></u><b></td></tr>");
            htw.Write("<table><tr><td colspan='3'><b><u><h3>חובות עבור גורם מממן " + name + " " + Financesum + "</h3></u><b></td></tr>");


           
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
            htw.Write("<table><tr><td colspan='3'><u>אנשי קשר של מטופלים</u></td></tr>");

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return View();
        }


        public ActionResult ContactList()
        {

            EliManagerDB db = new EliManagerDB();
            List<tblPatient> pat = db.Patients.ToList();
            
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=אנשי קשר של מטופלים.xls");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            Response.Charset = "";

            for (int i = 0; i < pat.Count(); i++)
            {

                var grid = new GridView();
                grid.DataSource = (from p in pat
                                   
                                   where p.ID.ToString() == pat.ElementAt(i).ID.ToString()
                                   select new
                                   {

                                       שם_איש_קשר1 = p.ContactName1,
                                       תפקיד1 = p.ContactProfession1,
                                       טלפון1 = "*" + p.ContactPhone1,
                                       מייל1 = p.ContactMail1,
                                       שם_איש_קשר2 = p.ContactName2,
                                       תפקיד2 = p.ContactProfession2,
                                       טלפון2 = "*" + p.ContactPhone2,
                                       מייל2 = p.ContactMail2,
                                       שם_איש_קשר3 = p.ContactName3,
                                       תפקיד3 = p.ContactProfession3,
                                       טלפון3 = "*" + p.ContactPhone3,
                                       מייל3 = p.ContactMail3,

                                   }).ToList().Distinct(); grid.DataBind();

                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                if (i == 0)
                {
                    htw.Write("<table><tr><td colspan='3'><h1><u><b> אנשי קשר של מטופלים <b></u></h1></td></tr>");

                }

                htw.Write("<table><tr><td colspan='3'><h3><u><b>שם:" + pat.ElementAt(i).FirstName + " " + pat.ElementAt(i).SurName +" תז: "+pat.ElementAt(i).ID+ "<b></u></h3></td></tr>");
                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
            }

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


        public ActionResult allPaymentFinance()
        {

            EliManagerDB db = new EliManagerDB();
            List<tblPatient> pat = db.Patients.ToList();
            List<tblRefererencePatient> refpat = db.ReferencePatient.ToList();
            List<tblReferenceTherapist> refterapist = db.ReferenceTherapist.ToList();
            List<tblTherapist> ter = db.Therapist.ToList();
            List<tblTreatment> treat = db.Treatment.ToList();
            List<tblFinancingFactor> fin = db.FinancingFactor.ToList();
            Double totall = db.totatlNotPaidTreat();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=חובות לפי גורמים מממנים.xls");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            Response.Charset = "";
            var data = db.Treatment.Where(d => d.IsPaid == "לא");
            
            for (int i = 0; i < fin.Count(); i++)
            {
                
                var financeNum = db.getFinanceNumByName(fin.ElementAt(i).FinancingFactorName);
                var mail = db.getFinanceMailByName(fin.ElementAt(i).FinancingFactorName);
                var allFinance = (from t in treat
                                  where t.FinancingFactorNumber.ToString() == financeNum && t.IsPaid == "לא"
                                  select new { t.Cost }
                      ).ToList();
                var Financesum = allFinance.Select(c => c.Cost).Sum();
                var grid = new GridView();
                grid.DataSource = (from p in pat
                                   join rp in refpat on p.ID equals rp.PatientID
                                   join rt in refterapist on rp.ReferenceNumber equals rt.ReferenceNumber
                                   join t in ter on rt.TherapistID equals t.TherapistID
                                   join tr in treat on rt.ReferenceNumber equals tr.ReferenceNumber
                                   join f in fin on tr.FinancingFactorNumber equals f.FinancingFactorNumber
                                   where f.FinancingFactorName == fin.ElementAt(i).FinancingFactorName && tr.IsPaid == "לא"
                                   select new
                                   {

                                       תאריך = tr.TreatmentDate,

                                       שם_מטופל = p.FirstName + " " + p.SurName,
                                       עלות = tr.Cost,


                                   }).ToList().Distinct();
                grid.DataBind();

                
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                if(i==0)
                {
                    htw.Write("<table><tr><td colspan='3'><b><u><h1> חובות לפי גורמים מממנים </h1></u><b></td></tr>");

                    htw.Write("<table><tr><td colspan='3'><b><u><h3> סהכ חובות עבור כל הגורמים מממנים " + totall + "</h3></u><b></td></tr>");

                }
                if (db.getNotPaidTreatmentByName(fin.ElementAt(i).FinancingFactorName) == 0)
                {
                    htw.Write("<table><tr><td colspan='3'><b><u><h4>" + fin.ElementAt(i).FinancingFactorName + "</h4></u><b></td></tr>");
                    htw.Write("<table><tr><td colspan='3'><b> אין חובות עבור גורם מממן " + fin.ElementAt(i).FinancingFactorName + "  מייל= " + mail + "<b></td></tr>");
                    grid.RenderControl(htw);

                    Response.Output.Write(sw.ToString());
                    continue;

                }
                htw.Write("<table><tr><td colspan='3'><u><h4><b>" + fin.ElementAt(i).FinancingFactorName + "<b></h4></u></td></tr>");
                htw.Write("<table><tr><td colspan='3'><b> טיפולים שלא שולמו עבור גורם מממן " + fin.ElementAt(i).FinancingFactorName + "  מייל= " + mail + "<b></td></tr>");
                htw.Write("<table><tr><td colspan='3'>חובות עבור גורם מממן " + fin.ElementAt(i).FinancingFactorName + " " + Financesum + "</td></tr>");
                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
            }

            Response.Flush();
            Response.End();

            return View();


        }



    }
}
