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
        //static string connectionString = "Data Source=Eden-PC\\SQLEXPRESS1;Initial Catalog=Eli;Integrated Security=True";
        static string connectionString = "Data Source=SHAHAR-PC\\SQLEXPRESS;Initial Catalog=Eli;Integrated Security=True";


        public ActionResult IndexReport()
        {       
                return View();
        }

        public ActionResult SelectReport(string reportName, string ffParam)
        {
            switch (reportName)
            {
                case "0": return RedirectToAction("PatientReport");
                case "1": return RedirectToAction("PatientByFinanceFactorReport", new { FinancingFactorName = ffParam });
                case "2": return RedirectToAction("IndexReport");
                case "3": return RedirectToAction("ContactsPatientsReport");
            }
            return RedirectToAction("PatientReport");
        }

        public ActionResult PatientByFinanceFactorReport(string reportName, string FinancingFactorName)
        {
            List<PatientByFinanceFactor> Result = getPatientByFinanceFactor(FinancingFactorName);

            return View(Result);
        }

        // display PatientReport
        public ActionResult PatientReport()
        {
            return View(new EliManagerDB().getAllPatients());
            /*
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
             */
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

            string Command = null;

            if (FinancingFactorName != "הכל")
            {
                Command = "select max(IsNull(finan.FinancingFactorNumber,'')) as FinancingFactorNumber,IsNull(finan.FinancingFactorName,'') as FinancingFactorName,IsNull(finan.FinancingFactorType,'') as FinancingFactorType ,ID,FirstName,SurName " +
                    "from tblPatient left outer join tblRefererencePatient refPat on ID=refPat.PatientID "+
                    "left outer join tblReferenceTherapist refTher on refPat.ReferenceNumber=refTher.ReferenceNumber "+ 
                    "left outer join tblTherapist ther on refTher.TherapistID=ther.TherapistID "+  
                    "left outer join tblTreatment treat on refTher.ReferenceNumber=treat.ReferenceNumber and refTher.TherapistID=treat.TherapistID "+ 
                    "left outer join tblFinancingFactor finan on treat.FinancingFactorNumber=finan.FinancingFactorNumber "+ 
                    "where finan.FinancingFactorName='" + FinancingFactorName + "' " + 
                    "group by ID,FirstName,SurName,FinancingFactorName,FinancingFactorType order by FinancingFactorNumber desc";
                flagJustOne = true;
            }
            else
                Command = "select max(IsNull(finan.FinancingFactorNumber,'')) as FinancingFactorNumber,IsNull(finan.FinancingFactorName,'') as FinancingFactorName,IsNull(finan.FinancingFactorType,'') as FinancingFactorType ,ID,FirstName,SurName "+ 
                    "from tblPatient left outer join tblRefererencePatient refPat on ID=refPat.PatientID " +
                    "left outer join tblReferenceTherapist refTher on refPat.ReferenceNumber=refTher.ReferenceNumber "+ 
                    "left outer join tblTherapist ther on refTher.TherapistID=ther.TherapistID "+ 
                    "left outer join tblTreatment treat on refTher.ReferenceNumber=treat.ReferenceNumber and refTher.TherapistID=treat.TherapistID "+ 
                    "left outer join tblFinancingFactor finan on treat.FinancingFactorNumber=finan.FinancingFactorNumber "+
                    "group by ID,FirstName,SurName,FinancingFactorName,FinancingFactorType order by FinancingFactorNumber desc";

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
