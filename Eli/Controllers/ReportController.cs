using Eli.Models;
using Eli.ViewModel;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eli.Controllers
{
    public class ReportController : Controller
    {
        static string connectionString = "Data Source=Eden-PC\\SQLEXPRESS1;Initial Catalog=Eli;Integrated Security=True";

        public ActionResult IndexReport()
        {       
                return View();
        }

        public ActionResult SelectReport(string reportName)
        {
            switch (reportName)
            {
                case "0": return RedirectToAction("PatientReport");
                case "1": return RedirectToAction("PatientByFinanceFactorReport");
                case "2": return RedirectToAction("IndexReport");
                case "3": return RedirectToAction("ContactsPatientsReport");
            }
            return RedirectToAction("PatientReport");
        }

        // display PatientReport
        public ActionResult PatientReport()
        {
            List<tblPatient> Result = new List<tblPatient>();
            string Command = "select * from tblPatient";
            using (SqlConnection mConnection = new SqlConnection(connectionString))
            {
                mConnection.Open();
                using (SqlCommand cmd = new SqlCommand(Command, mConnection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tblPatient temp = new tblPatient()
                            {
                                ID = (string)reader[0],
                                FirstName = (string)reader[1],
                                SurName = (string)reader[2],
                                Gender = (string)reader[4],
                                PhoneNumber = (string)reader[7]
                            };

                            Result.Add(temp);
                        }
                    }
                }
            }
            return View(Result);
        }

        // display PatientByFinanceFactorReport
        public ActionResult PatientByFinanceFactorReport(string FinancingFactorName)
        {
            List<PatientByFinanceFactor> Result = new List<PatientByFinanceFactor>();
            List<tblPatient> pats = new List<tblPatient>();
            int count = 1;

            string Command = "select count(finan.FinancingFactorNumber),finan.FinancingFactorName,finan.FinancingFactorType,pat.ID,pat.FirstName,pat.SurName"
                            + "from tblPatient pat inner join tblRefererencePatient refPat on pat.ID=refPat.PatientID"
                            + "inner join tblReferenceTherapist refTher on refPat.ReferenceNumber=refTher.ReferenceNumber"
                            + "inner join tblTherapist ther on refTher.TherapistID=ther.TherapistID"
                            + "inner join tblTreatment treat on refTher.ReferenceNumber=treat.ReferenceNumber and refTher.TherapistID=treat.TherapistID"
                            + "inner join tblFinancingFactor finan on treat.FinancingFactorNumber=finan.FinancingFactorNumber"
                            + "where finan.FinancingFactorName=@" + FinancingFactorName
                            + "group by ID,FirstName,SurName,FinancingFactorName,FinancingFactorType";
            using (SqlConnection mConnection = new SqlConnection(connectionString))
            {
                mConnection.Open();
                using (SqlCommand cmd = new SqlCommand(Command, mConnection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int amount = Int16.Parse((string)reader[0]);

                            tblPatient tempPatient = new tblPatient()
                            {
                                ID = (string)reader[3],
                                FirstName = (string)reader[4],
                                SurName = (string)reader[5]
                            };
                            pats.Add(tempPatient);

                            if (count>=amount)
                            {
                                tblFinancingFactor tempFinance = new tblFinancingFactor()
                                {
                                    FinancingFactorContactName = (string)reader[1],
                                    FinancingFactorType = (string)reader[2]
                                };

                                PatientByFinanceFactor temp = new PatientByFinanceFactor()
                                {
                                    Patients = pats,
                                    FinancingFactor = tempFinance
                                };

                                Result.Add(temp);
                                count = 1;
                            }
                            count++;
                        }
                    }
                }
            }
            return View(Result);
        }

        // display FinanceFactorDebatorsReport
        public ActionResult FinanceFactorDebatorsReport()
        {
            return View();
        }

        // display ContactsPatientsReport
        public ActionResult ContactsPatientsReport()
        {
            EliManagerDB db = new EliManagerDB();
            return View(db.getAllPatients());
        }

        
        /*
        public ActionResult GeneratePDF()
        {
            return new Rotativa.ActionAsPdf("PatientReport");

        }*/


    }
}
