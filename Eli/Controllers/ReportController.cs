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
        static string connectionString = SQLConnection.GetConnectionString();

        public ActionResult IndexReport()
        {       
                return View(new DateModel());
        }

        public ActionResult SelectReport(string reportName, string ffParam, DateModel dm)
        {
            switch (reportName)
            {
                case "0": return RedirectToAction("PatientReport");
                case "1": return RedirectToAction("PatientByFinanceFactorReport", new { FinancingFactorName = ffParam });
                case "2": return RedirectToAction("FinanceFactorDebatorsReport", new { FinancingFactorName = ffParam, from = dm.StartDate, to = dm.EndDate });
                case "3": return RedirectToAction("ContactsPatientsReport");
            }
            return RedirectToAction("PatientReport");
        }

        // display PatientReport
        public ActionResult PatientReport()
        {
            return View(new EliManagerDB().getAllPatients());
        }

        // display PatientByFinanceFactorReport
        public ActionResult PatientByFinanceFactorReport(string FinancingFactorName)
        {
            EliManagerDB db = new EliManagerDB();
            List<PatientByFinanceFactor> Result = getPatientByFinanceFactor(FinancingFactorName);
            ViewBag.type = db.getTypeByFinanceName(FinancingFactorName);

            ViewBag.FinancingFactorName = FinancingFactorName;

            return View(Result);
        }

        // display FinanceFactorDebatorsReport
        public ActionResult FinanceFactorDebatorsReport(DateTime from, DateTime to, string FinancingFactorName)
        {
            List<TreatmentByFinanceFactor> Result = getFinanceFactorDebatorByTreat(from, to, FinancingFactorName);

            ViewBag.FinancingFactorName = FinancingFactorName;

            ViewBag.From = from;
            ViewBag.To = to;

            return View(Result);
        }

        // display ContactsPatientsReport
        public ActionResult ContactsPatientsReport()
        {
            EliManagerDB db = new EliManagerDB();
            return View(db.inizializePatientNullValues(db.getAllPatients()));
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


        // display PatientByFinanceFactorReport
        public List<TreatmentByFinanceFactor> getFinanceFactorDebatorByTreat(DateTime from, DateTime to, string FinancingFactorName)
        {
            List<TreatmentByFinanceFactor> Result = new List<TreatmentByFinanceFactor>();
            List<TreatmentPatient> treatPats = new List<TreatmentPatient>();
            TreatmentPatient tempTreatmentPatient = new TreatmentPatient();
            tblTreatment tempTreatment = new tblTreatment();
            tblPatient tempPatient = new tblPatient();
            TreatmentByFinanceFactor temp = new TreatmentByFinanceFactor();
            Boolean flagJustOne = false;
            int NextId = 0;
            int CurId = 0;
            double totalFinance = 0;
            double total = 0;

            int dayTo = to.Day;
            int monthTo = to.Month;
            int yearTo = to.Year;
            int dayFrom = from.Day;
            int monthFrom = from.Month;
            int yearFrom = from.Year;
            String dFrom = from.Year.ToString() + "/" + from.Month.ToString() + "/" + from.Day.ToString();
            String dTo = to.Year.ToString() + "/" + to.Month.ToString() + "/" + to.Day.ToString();

            string Command = "select ff.FinancingFactorNumber,FinancingFactorName,FinancingFactorContactMail, TreatmentDate, tr.IsPaid,Cost ,p.FirstName,p.SurName , deb.SumCost as totalByFinance , bSum.SumCost as total "
                        + "from tblFinancingFactor ff inner join tblTreatment tr on ff.FinancingFactorNumber=tr.FinancingFactorNumber "
                        + "inner join tblReferenceTherapist rt on rt.ReferenceNumber=tr.ReferenceNumber and rt.TherapistID=tr.TherapistID "
                        + "inner join tblRefererencePatient rp on rp.ReferenceNumber=rt.ReferenceNumber "
                        + "inner join tblPatient p on p.ID=rp.PatientID "
                        + "inner join (select finf.FinancingFactorNumber, SUM(Cost) as SumCost "
                                       + "from tblFinancingFactor finf inner join tblTreatment tr on finf.FinancingFactorNumber=tr.FinancingFactorNumber "
                                       + "where IsPaid='לא' and TreatmentDate between ('" + dFrom + "') and ('" + dTo + "') "
                                       + "group by finf.FinancingFactorNumber) as deb on ff.FinancingFactorNumber=deb.FinancingFactorNumber "
                           + "inner join (select SUM(Cost) as SumCost, IsPaid "
                                       + "from tblFinancingFactor ff inner join tblTreatment tr on ff.FinancingFactorNumber=tr.FinancingFactorNumber "
                                       + "where IsPaid='לא' and TreatmentDate between ('" + dFrom + "') and ('" + dTo + "') "
                                       + "group by IsPaid) as bSum on bSum.IsPaid=tr.IsPaid "
                                       + "where tr.IsPaid='לא'  and TreatmentDate between ('" + dFrom + "') and ('" + dTo + "') ";
            
            if (FinancingFactorName != "הכל")
            {
                Command += "and FinancingFactorName='" + FinancingFactorName + "' ";
                flagJustOne = true;
            }
            
            Command +="order by FinancingFactorContactName, TreatmentDate";

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

                                totalFinance=(double)reader[8];
                                total = (double)reader[9];

                                tblFinancingFactor tempFinance = new tblFinancingFactor()
                                {
                                    FinancingFactorName = (string)reader[1],
                                    FinancingFactorContactMail = (string)reader[2]
                                };
                                
                                tempTreatment = new tblTreatment()
                                {
                                    TreatmentDate = (DateTime)reader[3],
                                    Cost = (double)reader[5],
                                };
                                
                                tempPatient= new tblPatient(){
                                    FirstName = (string)reader[6],
                                    SurName = (string)reader[7],
                                };
                                

                                tempTreatmentPatient = new TreatmentPatient()
                                {
                                    Treatment = tempTreatment,
                                    Patient= tempPatient
                                };

                                treatPats.Add(tempTreatmentPatient);

                                while (NextId == CurId && reader.Read())
                                {

                                    NextId = (Int32)reader[0];
                                    
                                    tempTreatment = new tblTreatment()
                                    {
                                        TreatmentDate = (DateTime)reader[3],
                                        Cost = (double)reader[5],
                                    };
                                    
                                    tempPatient = new tblPatient()
                                    {
                                        FirstName = (string)reader[6],
                                        SurName = (string)reader[7],
                                    };


                                    tempTreatmentPatient = new TreatmentPatient()
                                    {
                                        Treatment = tempTreatment,
                                        Patient = tempPatient
                                    };

                                    if (NextId == CurId)
                                    {
                                        treatPats.Add(tempTreatmentPatient);
                                    }
                                    else
                                    {
                                        temp = new TreatmentByFinanceFactor()
                                        {
                                            FinancingFactor = tempFinance,
                                            TreatmentPatient=treatPats,
                                            TotalDebateFinance = totalFinance,
                                            Total = total
                                        };

                                        Result.Add(temp);
                                        treatPats = new List<TreatmentPatient>();
                                        treatPats.Add(tempTreatmentPatient);

                                        tempFinance = new tblFinancingFactor()
                                        {
                                            FinancingFactorName = (string)reader[1],
                                            FinancingFactorContactMail = (string)reader[2]
                                        };

                                    }
                                }
                                if (flagJustOne)
                                {
                                    temp = new TreatmentByFinanceFactor()
                                    {
                                        FinancingFactor = tempFinance,
                                        TreatmentPatient = treatPats,
                                        TotalDebateFinance = totalFinance,
                                        Total = total
                                    };
                                    Result.Add(temp);
                                    break;
                                }
                                if (!reader.Read())
                                {
                                    temp = new TreatmentByFinanceFactor()
                                    {
                                        FinancingFactor = tempFinance,
                                        TreatmentPatient = treatPats,
                                        TotalDebateFinance = totalFinance,
                                        Total = total
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
