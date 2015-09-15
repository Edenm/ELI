using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Eli.ViewModel;
using System.Web.Mvc;

namespace Eli.Models
{
    public class EliManagerDB
    {
        DataConnectionDataContext db;

        public EliManagerDB(){
            db = SQLConnection.GetDataContextInstance();
        }

//-------------------------Tables----------------------------------------------------------
 
        public Table<tblPatient> Patients
        {
            get { return db.tblPatients; }
        }

        public Table<tblFinancingFactor> FinancingFactor
        {
            get { return db.tblFinancingFactors; }
        }


        public Table<tblBrotherSister> BrotherSister
        {
            get { return db.tblBrotherSisters; }
        }

        public Table<tblBrotherSisterPatient> BrotherSisterPatient
        {
            get { return db.tblBrotherSisterPatients; }
        }

        public Table<tblReference> Reference
        {
            get { return db.tblReferences; }
        }

        public Table<tblTherapist> Therapist
        {
            get { return db.tblTherapists; }
        }

        public Table<tblTreatment> Treatment
        {
            get { return db.tblTreatments; }
        }

        public Table<tblReferenceTherapist> ReferenceTherapist
        {
            get { return db.tblReferenceTherapists; }
        }

        public Table<tblRefererencePatient> ReferencePatient
        {
            get { return db.tblRefererencePatients; }
        }

        public Table<tblParentPatient> ParentPatient
        {
            get { return db.tblParentPatients; }
        }

        public Table<tblParent> Parent
        {
            get { return db.tblParents; }
        }

        public Table<tblRefererencePatient> RefererencePatients
        {
            get { return db.tblRefererencePatients; }
        }

        public Table<tblEvent> Events
        {
            get { return db.tblEvents; }
        }


//-------------------------Add methods----------------------------------------------------------
        
        /* The method add patient with all the details about him **/
        public void addPatient(Family family)
        {

            var patient = from p in Patients
                          where p.ID.Equals(family.Patient.ID)
                          select p;

            if (patient.Any())
                throw new Exception("המטופל כבר קיים במערכת");

            Patients.InsertOnSubmit(family.Patient);

            family.Reference.ReferenceNumber = Reference.Count() + 1;

            foreach (tblParent p in family.Parents)
            {
                Parent.InsertOnSubmit(p);

                tblParentPatient tbPP = new tblParentPatient()
                {
                    ParentID = p.ParentID,
                    PatientID = family.Patient.ID
                };
                
                ParentPatient.InsertOnSubmit(tbPP);
            }

            foreach (tblBrotherSister bs in family.BrotherSisters)
            {
                BrotherSister.InsertOnSubmit(bs);

                tblBrotherSisterPatient tbBSP = new tblBrotherSisterPatient()
                {
                    BrotherSisterID = bs.BrotherSisterID,
                    PatientID = family.Patient.ID
                };

                BrotherSisterPatient.InsertOnSubmit(tbBSP);
            }

            Reference.InsertOnSubmit(family.Reference);

            tblRefererencePatient tbRP = new tblRefererencePatient()
            {
                PatientID = family.Patient.ID,
                ReferenceNumber = family.Reference.ReferenceNumber
            };

            ReferencePatient.InsertOnSubmit(tbRP);

            tblReferenceTherapist tbRT = new tblReferenceTherapist()
            {
                ReferenceNumber = family.Reference.ReferenceNumber,
                TherapistID=family.Therapist.TherapistID
                
            };

            ReferenceTherapist.InsertOnSubmit(tbRT);

            db.SubmitChanges();
        }

        /* The method add parent to exist patient **/
        public void addParentToPatient(tblParent tp, string patId)
        {
            checkParent(tp);

            var tblParentPatient = new tblParentPatient()
            {
                ParentID = tp.ParentID,
                PatientID = patId
            };

            Parent.InsertOnSubmit(tp);
            ParentPatient.InsertOnSubmit(tblParentPatient);

            db.SubmitChanges();

        }

        /* The method add parent to exist patient **/
        public void addBrotherSisterToPatient(tblBrotherSister tbs, string patId)
        {
            checkBrotherSister(tbs);

            var tblBrotherSisterPatient = new tblBrotherSisterPatient()
            {
                BrotherSisterID = tbs.BrotherSisterID,
                PatientID = patId
            };

            BrotherSister.InsertOnSubmit(tbs);
            BrotherSisterPatient.InsertOnSubmit(tblBrotherSisterPatient);

            db.SubmitChanges();

        }

        /* Check if patient is exist**/
        public void checkPatient(tblPatient pat)
        {
            var patient = from p in Patients
                          where p.ID.Equals(pat.ID)
                          select p;
            if (patient.Any())
                throw new Exception("המטופל שהזנת כבר קיים במערכת");
        }

        /* Check if BrotherSister is exist**/
        public void checkParent(tblParent tp)
        {
            var patParent = from p in Parent
                            where p.ParentID.Equals(tp.ParentID)
                            select p;
            if (patParent.Any())
                throw new Exception("הורה זה כבר קיים במערכת");
        }

        /* Check if BrotherSister is exist**/
        public void checkBrotherSister(tblBrotherSister tBS)
        {
            var patBS = from p in BrotherSister
                        where p.BrotherSisterID.Equals(tBS.BrotherSisterID)
                        select p;
            if (patBS.Any())
                throw new Exception("אח זה כבר קיים במערכת");
        }

        /* Add Therapist to data base **/
        public void addTherapist(tblTherapist tt)
        {
            var therapist = from d in Therapist
                            where d.TherapistID.Equals(tt.TherapistID)
                            select d;

            if (therapist.Any())
                throw new Exception("המטפל שהזנת כבר קיים במערכת");

            therapist = from t in Therapist
                        where t.TherapistMail.Equals(tt.TherapistMail)
                        select t;

            if (therapist.Any())
                throw new Exception("כבר קיים במערכת " + tt.UserName + " :לא ניתן להוסיף את המטפל כיוון שהמייל");

            therapist = from t in Therapist
                       where t.UserName.Equals(tt.UserName)
                       select t;

            if (therapist.Any())
               throw new Exception("כבר קיים במערכת " + tt.UserName + " :לא ניתן להוסיף את המטפל כיוון ששם המשתמש");

            Therapist.InsertOnSubmit(tt);
            db.SubmitChanges();
        }

        /* Add FinancingFactor to data base **/
        public void addFinanceFactor(tblFinancingFactor ff)
        {
            var financeFactor = from f in FinancingFactor
                                where f.FinancingFactorName.Equals(ff.FinancingFactorName)
                                select f;

            if (financeFactor.Any())
                throw new Exception("למערכת, קיים גורם מממן עם שם זהה" +ff.FinancingFactorName+ " לא ניתן להוסיף את");

            ff.FinancingFactorNumber = FinancingFactor.Count()+1;

            FinancingFactor.InsertOnSubmit(ff);
            db.SubmitChanges();
        }

        /* Add Treatment to data base **/
        public void addReference(tblReference re, string pid , string tid)
        {
            re.ReferenceNumber = Reference.Count() + 1;

            tblRefererencePatient refPat= new tblRefererencePatient();

            refPat.PatientID=pid;
            refPat.ReferenceNumber=re.ReferenceNumber;
            ReferencePatient.InsertOnSubmit(refPat);

            Reference.InsertOnSubmit(re);

            tblReferenceTherapist refTher = new tblReferenceTherapist();

            refTher.ReferenceNumber = re.ReferenceNumber;
            refTher.TherapistID = tid;

            ReferenceTherapist.InsertOnSubmit(refTher);

            db.SubmitChanges();
        }

        /* Add Treatment to data base **/
        public void addTreatment(tblTreatment tr)
        {
            tr.TreatmentNumber = Treatment.Count() + 1;

            Treatment.InsertOnSubmit(tr);
            db.SubmitChanges();
        }

        /* Add Event to data base **/
        public void addEvent(tblEvent ev)
        {
            if (Events.Count() == 0)
            {
                ev.EventNum = 1;
            }
            else
            {
                int eve = Events.ToArray().Last().EventNum;
                ev.EventNum = eve+1;
            }
            

            Events.InsertOnSubmit(ev);
            db.SubmitChanges();
        }


        //-------------------------Edit methods----------------------------------------------------------

        /* The method is editing exist Finance Factor **/
        public void EditFinanceFactor(tblFinancingFactor ff)
        {
            var d = FinancingFactor.First(x => x.FinancingFactorNumber == ff.FinancingFactorNumber);
            d.FinancingFactorName = ff.FinancingFactorName;
            d.FinancingFactorType = ff.FinancingFactorType;
            d.FinancingFactorContactName = ff.FinancingFactorContactName;
            d.FinancingFactorContactMail = ff.FinancingFactorContactMail;
            d.FinancingFactorContcatPhoneNumber = ff.FinancingFactorContcatPhoneNumber;
            d.FinancingFactorAddress = ff.FinancingFactorAddress;
            db.SubmitChanges();
        }

        /* The method is editing exist Therapist**/
        public void EditTherapist(tblTherapist tt)
        {
            var t = Therapist.First(x => x.TherapistID == tt.TherapistID);
           
            t.TherapistFirstName = tt.TherapistFirstName;
            t.TherapistSurName = tt.TherapistSurName;
            t.TherapistBirthDate = tt.TherapistBirthDate;
            t.TherapistAddress = tt.TherapistAddress;
            t.TherapistGender = tt.TherapistGender;
            t.Passcode = tt.Passcode;
            t.UserName = tt.UserName;
            t.TherapistMail = tt.TherapistMail;
            t.TherapistPhoneNumber = tt.TherapistPhoneNumber;
            db.SubmitChanges();
        }

        /* The method is editing exist Patient**/
        public void EditPatient(tblPatient pat)
        {
            var patient = Patients.First(p => p.ID == pat.ID);
            patient.FirstName = pat.FirstName;
            patient.SurName = pat.SurName;
            patient.PatientAddress = pat.PatientAddress;
            patient.BirthDate = pat.BirthDate;
            patient.PhoneNumber = pat.PhoneNumber;
            patient.EducationalFramework = pat.EducationalFramework;
            patient.Gender = pat.Gender;
            patient.PatientStatus = pat.PatientStatus;

            db.SubmitChanges();
        }

        /* The method is editing exist Treatment**/
        public void EditReference(tblReference re)
        {
            var refe = Reference.First(r => r.ReferenceNumber == re.ReferenceNumber);
            refe.ReasonReference = re.ReasonReference;
            refe.ReferenceSource = re.ReferenceSource;
            refe.StartDateReference = re.StartDateReference;
            refe.EndDateReference = re.EndDateReference;
            refe.OtherStatus = re.OtherStatus;
            refe.AbuseType = re.AbuseType;
            refe.StatusReference = re.StatusReference;
            refe.OtherReference = re.OtherReference;

            db.SubmitChanges();
        }

        /* The method is editing exist Treatment**/
        public void EditTreatment(tblTreatment tr)
        {
            var treat = Treatment.First(t => t.TreatmentNumber == tr.TreatmentNumber);
            treat.TreatmentGoal = tr.TreatmentGoal;
            treat.TreatmentDescription = tr.TreatmentDescription;
            treat.TreatmentDate = tr.TreatmentDate;
            treat.TreatmentSummary = tr.TreatmentSummary;
            treat.TreatmentStartTime = tr.TreatmentStartTime;
            treat.TreatmentStatusPatient = tr.TreatmentStatusPatient;
            treat.IsPaid = tr.IsPaid;
            treat.NextTreatment = tr.NextTreatment;
            treat.TreatmentPlace = tr.TreatmentPlace;
            treat.TreatmentSubject = tr.TreatmentSubject;
            treat.Cost = tr.Cost;
            treat.IsArrived = tr.IsArrived;

            treat.FinancingFactorNumber = tr.FinancingFactorNumber;
            treat.TherapistID = tr.TherapistID;
            treat.ReferenceNumber = tr.ReferenceNumber;

            db.SubmitChanges();
        }

        /* The method is editing exist patient**/
        public void EditFamily(FormCollection family, string pid)
        {
            Family fam = new Family(pid);
            int countP=0;
            foreach (tblParent tp in fam.Parents)
            {
                var parent = Parent.First(bs => bs.ParentID == tp.ParentID);
                parent.ParentFirstName = family.GetValues("ParentFirstName")[countP];
                parent.ParentSurName = family.GetValues("ParentSurName")[countP];
                parent.ParentAddress = family.GetValues("ParentAddress")[countP];
                parent.ParentPhoneNumber = family.GetValues("ParentPhoneNumber")[countP];
                parent.Explain = family.GetValues("Explain")[countP];
                parent.IsWorking = family.GetValues("IsWorking")[countP];
                parent.ParentMail = family.GetValues("ParentMail")[countP];
                parent.ParentGender = family.GetValues("ParentGender")[countP]; 
                parent.ParentBirthDate =Convert.ToDateTime(family.GetValues("ParentBirthDate")[countP]);
                countP++;
            }
            int countBS = 0;
            foreach (tblBrotherSister tbs in fam.BrotherSisters){
                var broSis = BrotherSister.First(bs => bs.BrotherSisterID == tbs.BrotherSisterID);
                broSis.BrotherSisterFirstName = family.GetValues("BrotherSisterFirstName")[countBS];
                broSis.BrotherSisterSurName = family.GetValues("BrotherSisterSurName")[countBS];
                broSis.BrotherSisterStudyFramework = family.GetValues("BrotherSisterStudyFramework")[countBS];
                broSis.BrotherSisterGender = family.GetValues("BrotherSisterGender")[countBS];
                broSis.BrotherSisterBirthDate = Convert.ToDateTime(family.GetValues("BrotherSisterBirthDate")[countBS]);
                countBS++;
            }

            var patient = Patients.First(p => p.ID == pid);

            patient.ContactName1 = family.GetValues("ContactName1")[0];
            patient.ContactMail1 = family.GetValues("ContactMail1")[0];
            patient.ContactProfession1 = family.GetValues("ContactProfession1")[0];
            patient.ContactPhone1 = family.GetValues("ContactPhone1")[0];

            patient.ContactName2 = family.GetValues("ContactName2")[0];
            patient.ContactMail2 = family.GetValues("ContactMail2")[0];
            patient.ContactProfession2 = family.GetValues("ContactProfession2")[0];
            patient.ContactPhone2 = family.GetValues("ContactPhone2")[0];

            patient.ContactName3 = family.GetValues("ContactName3")[0];
            patient.ContactMail3 = family.GetValues("ContactMail3")[0];
            patient.ContactProfession3 = family.GetValues("ContactProfession3")[0];
            patient.ContactPhone3 = family.GetValues("ContactPhone3")[0];
            
            db.SubmitChanges();
        }

        public void EditEvent(tblEvent ev)
        {
            var eve = Events.First(e => e.EventNum == ev.EventNum);
            eve.EventDate = ev.EventDate;
            eve.EventTime = ev.EventTime;
            eve.Description = ev.Description;

            db.SubmitChanges();
        }

        //-----------------------remove Method----------------------------------------------------------------

        public void RemoveEvent(int evNum)
        {
            var eve = Events.First(e => e.EventNum == evNum);

            Events.DeleteOnSubmit(eve);

            db.SubmitChanges();
        }


        //-----------------------get Method----------------------------------------------------------------

        public tblPatient getPatientById(String pId)
        {
            var p = (Patients.Where(pat => pat.ID == pId)).FirstOrDefault();

            return p;
        }

        public tblPatient getPatientByReferencNumber(int rid)
        {
            var patId = (ReferencePatient.Where(rp => rp.ReferenceNumber == rid)).Select(rp => rp.PatientID).First();

            var pat = Patients.Where(p => p.ID == patId).First();

            return pat;
        }

        public tblPatient getPatientByTreatmentNumber(int tnum)
        {
            var refNum = getTreatmentByNumber(tnum).ReferenceNumber;

            var patId = RefererencePatients.Where(rp => rp.ReferenceNumber == refNum).Select(rp=> rp.PatientID).First();

            var pat = Patients.Where(p => p.ID == patId).First();

            return pat;
        }

        public tblTreatment getTreatmentByNumber(int tnum)
        {
            var tr = (Treatment.Where(t => t.TreatmentNumber == tnum)).First();

            return tr;
        }

        public tblReference getReferenceByReferenceNumber(int rnum)
        {
            return Reference.Where(r => r.ReferenceNumber == rnum).First();

        }

        public tblReference getReferenceByTreatmentNumber(int tnum)
        {
            var refNum = getTreatmentByNumber(tnum).ReferenceNumber;

            var refer = Reference.Where(r => r.ReferenceNumber == refNum).First() ;

            return refer;
        }

        public tblTherapist getTherapistByTreatmentNumber(int tnum)
        {
            var therId = getTreatmentByNumber(tnum).TherapistID;

            var ther = Therapist.Where(t => t.TherapistID == therId).First();

            return ther;
        }

        public tblTherapist getTherapistByMail(string tmail)
        {
            
            return Therapist.Where(t => t.TherapistMail == tmail).FirstOrDefault();
        }

        public String getFinanceNumByName(string name)
        {
            if (name == "הכל")
                return "";
            List<tblFinancingFactor> fin = db.tblFinancingFactors.ToList();
            var financeNum = (from f in fin
                  where f.FinancingFactorName == name
                  select new { f.FinancingFactorNumber}
                  ).ToList().First();
            return (financeNum.FinancingFactorNumber.ToString());
        }

        public double getTotallDebatorByRange(String name,DateTime fDate,DateTime tDate)
        {
            var financeNum = getFinanceNumByName(name);
            List<tblTreatment> t = db.tblTreatments.ToList();
            double totall = 0;
            for(int i=0;i<t.Count();i++)
            {
                if(t.ElementAt(i).FinancingFactorNumber.ToString()==financeNum.ToString() && t.ElementAt(i).IsPaid=="לא" && 
                    t.ElementAt(i).TreatmentDate>=fDate && t.ElementAt(i).TreatmentDate<=tDate)
                {
                    totall = totall + (Double)t.ElementAt(i).Cost;
                }
            }
            return (totall);

        }


        public String getFinanceMailByName(string name)
        {
            
            List<tblFinancingFactor> fin = db.tblFinancingFactors.ToList();
            for (int i = 0; i < fin.Count();i++ )
            {
                if (fin.ElementAt(i).FinancingFactorName == name && fin.ElementAt(i).FinancingFactorContactMail!=null && fin.ElementAt(i).FinancingFactorContactMail!="")
                    return (fin.ElementAt(i).FinancingFactorContactMail.ToString());
            }
            return("לא קיים מייל");

               
        }

         public Double totatlNotPaidTreat(DateTime dFrom,DateTime dTo)
        {
            Double sum = 0;
            List<tblTreatment> treat = db.tblTreatments.ToList();
            for (int i = 0; i < treat.Count(); i++)
            {
                if (treat.ElementAt(i).IsPaid == "לא" && dFrom <= treat.ElementAt(i).TreatmentDate && treat.ElementAt(i).TreatmentDate <= dTo)
                    sum = sum + (Double)treat.ElementAt(i).Cost;
            }
            return (sum);


        }

         public int getNotPaidTreatmentByName(String name)
        {
            int count = 0;
            List<tblFinancingFactor> fin = db.tblFinancingFactors.ToList();
            var financeNum = getFinanceNumByName(name);
            List<tblTreatment> t = db.tblTreatments.ToList();
             for(int i=0;i<t.Count();i++)
             {
                 if (t.ElementAt(i).FinancingFactorNumber.ToString() == financeNum)
                     count++;
             }
             return count;


        }

         public List<tblPatient> inizializePatientNullValues(List<tblPatient> arr)
         {
             for(int i=0;i<arr.Count();i++)
             {
                 if(arr.ElementAt(i).ContactMail1==null || arr.ElementAt(i).ContactMail1.ToString()=="")
                 {
                     arr.ElementAt(i).ContactMail1 = "מידע לא זמין";
                 }
                 if (arr.ElementAt(i).ContactMail2 == null || arr.ElementAt(i).ContactMail2.ToString() == "")
                 {
                     arr.ElementAt(i).ContactMail2 = "מידע לא זמין";
                 }
                 if (arr.ElementAt(i).ContactMail3 == null || arr.ElementAt(i).ContactMail3.ToString() == "")
                 {
                     arr.ElementAt(i).ContactMail3 = "מידע לא זמין";
                 }
                 if (arr.ElementAt(i).ContactName1==null || arr.ElementAt(i).ContactName1.ToString() == "")
                 {
                     arr.ElementAt(i).ContactName1 = "מידע לא זמין";
                 }
                 if (arr.ElementAt(i).ContactName2 == null || arr.ElementAt(i).ContactName2.ToString() == "")
                 {
                     arr.ElementAt(i).ContactName2 = "מידע לא זמין";
                 }
                 if (arr.ElementAt(i).ContactName3 == null || arr.ElementAt(i).ContactName3.ToString() == "")
                 {
                     arr.ElementAt(i).ContactName3 = "מידע לא זמין";
                 }
                 if (arr.ElementAt(i).ContactPhone1==null || arr.ElementAt(i).ContactPhone1.ToString() == "")
                 {
                     arr.ElementAt(i).ContactPhone1 = "מידע לא זמין";
                 }
                 if (arr.ElementAt(i).ContactPhone2 == null || arr.ElementAt(i).ContactPhone2.ToString() == "")
                 {
                     arr.ElementAt(i).ContactPhone2 = "מידע לא זמין";
                 }
                 if (arr.ElementAt(i).ContactPhone3 == null || arr.ElementAt(i).ContactPhone3.ToString() == "")
                 {
                     arr.ElementAt(i).ContactPhone3 = "מידע לא זמין";
                 }
                 if (arr.ElementAt(i).ContactProfession1==null || arr.ElementAt(i).ContactProfession1.ToString() == "")
                 {
                     arr.ElementAt(i).ContactProfession1 = "מידע לא זמין";
                 }
                 if (arr.ElementAt(i).ContactProfession2 == null || arr.ElementAt(i).ContactProfession2.ToString() == "")
                 {
                     arr.ElementAt(i).ContactProfession2 = "מידע לא זמין";
                 }
                 if (arr.ElementAt(i).ContactProfession3 == null || arr.ElementAt(i).ContactProfession3.ToString() == "")
                 {
                     arr.ElementAt(i).ContactProfession3 = "מידע לא זמין";
                 }
             }
             return arr;
         }

         public int getNumPatientPerFinance(String name)
         {
             List<tblPatient> pat = Patients.ToList();
             List<tblRefererencePatient> refpat = ReferencePatient.ToList();
             List<tblReferenceTherapist> refterapist = ReferenceTherapist.ToList();
             List<tblTherapist> ter = Therapist.ToList();
             List<tblTreatment> treat = Treatment.ToList();
             List<tblFinancingFactor> fin = FinancingFactor.ToList();
             var temp = (from p in pat
                                   join rp in refpat on p.ID equals rp.PatientID
                                   join rt in refterapist on rp.ReferenceNumber equals rt.ReferenceNumber
                                   join t in ter on rt.TherapistID equals t.TherapistID
                                   join tr in treat on rt.ReferenceNumber equals tr.ReferenceNumber
                                   join f in fin on tr.FinancingFactorNumber equals f.FinancingFactorNumber
                                   where f.FinancingFactorName == name
                                   select new
                                   {

                                       תז_מטופל = p.ID,


                                   }).ToList().Distinct();
            
             return temp.Count();


         }
        public tblFinancingFactor getFinancingFactorByTreatmentNumber(int tnum)
        {
            var financeNum = getTreatmentByNumber(tnum).FinancingFactorNumber;

            var finfac = FinancingFactor.Where(f => f.FinancingFactorNumber == financeNum).First();

            return finfac;
        }

        public List<tblPatient> getAllPatientsByTherapist(String tid)
        {
            var therRrfe = (ReferenceTherapist.Where(rt => rt.TherapistID == tid).Select(rt => rt.ReferenceNumber)).ToList();

            var refePat = (RefererencePatients.Where(rp => therRrfe.Contains(rp.ReferenceNumber))).Select(rp => rp.PatientID).ToList();

            var pats = (Patients.Where(p => refePat.Contains(p.ID))).ToList();

            return  pats;
        }

        public List<tblBrotherSister> getAllBrotherSisterByPatient(String PID)
        {
            var BSP= (BrotherSisterPatient.Where(bs => bs.PatientID==PID).Select(bs=> bs.BrotherSisterID));

            var BS= (BrotherSister.Where(bs=> BSP.Contains(bs.BrotherSisterID))).ToList();
         
            return BS;
        }

        public List<tblParent> getAllParentsByPatient(String PID)
        {
            var PP = (ParentPatient.Where(p => p.PatientID == PID).Select(p=> p.ParentID));

            var P = (Parent.Where(bs => PP.Contains(bs.ParentID))).ToList();

            return P;
        }

        public List<tblTreatment> getAllTreatmentByReferenceAndTherapist(int refNum, string tid)
        {
            var treat = Treatment.Where(t => t.ReferenceNumber == refNum && t.TherapistID == tid).ToList();

            return treat;
        }

        public List<tblTreatment> getAllTreatmentByTherapist(string tid)
        {
            var treat = Treatment.Where(t => t.TherapistID == tid).Where(t => t.TreatmentDate >= DateTime.Now).OrderBy(t => t.TreatmentDate).ToList();

            return treat;
        }

        public List<tblReference> getAllReferencesByPatientAndTherapist(string pid, string tid)
        {
            var referencesListByPatient = ReferencePatient.Where(p => p.PatientID == pid).Select(re => re.ReferenceNumber).ToList();
            var referencesListByTherapist = ReferenceTherapist.Where(t => t.TherapistID == tid).Select(re => re.ReferenceNumber).ToList();

            var refe = Reference.Where(r => referencesListByPatient.Contains(r.ReferenceNumber) && referencesListByTherapist.Contains(r.ReferenceNumber)).ToList();

            return refe;
        }

        public List<tblFinancingFactor> getAllFinanceFactor()
        {
            return FinancingFactor.ToList();
        }

        public List<tblEvent> getAllEvents()
        {
            return Events.ToList();
        }

        public List<tblPatient> getAllPatients()
        {
            return Patients.ToList();
        }


//-------------------------------Help Method-----------------------------------------

        /*the function return all StatusReference strings*/
        public List<String> getAllStatusReference()
        {
            List<String> list = new List<String>();
            list.Add("פוגע");
            list.Add("נפגע");
            list.Add("פוגע ונפגע");
            list.Add("משבר");
            list.Add("טראומה");
            list.Add("טיפול זוגי");
            list.Add("טיפול משפחתי");
            list.Add("ילדים-בעיות בבית ספר");
            list.Add("ילדים -צרכים מיוחדים");
            list.Add("אחר");

            return list;
        }

        /*the function return all AbuseType strings*/
        public List<String> getAllAbuseType()
        {
            List<String> list = new List<String>();
            list.Add("פיזית");
            list.Add("הזנחה");
            list.Add("רגשית");
            list.Add("מינית");
            list.Add("לא רלונטי");

            return list;
        }

        /*the function return all ReferenceSource strings*/
         public List<String> getAllReferenceSource()
        {
            List<String> list = new List<String>();
            list.Add("הסברה");
            list.Add("איש מקצוע");
            list.Add("מדיה");
            list.Add("איש חינוך");
            list.Add("אישיות דתית");
            list.Add("אחר");

           

            return list;
        }

         /*the function return all FinancingFactorType strings*/
           public List<String> getAllFinancingFactorType()
           {
              List<String> list = new List<String>();
              list.Add("פרטי");
              list.Add("עירייה");
              list.Add("קרן");
              list.Add("מערכת חינוך");
              list.Add("פרויקט");
              list.Add("פנימייה");
              list.Add("עצמי");

              return list;
           }

        public String getTypeByFinanceName(String name)
           {
               var fin = FinancingFactor.ToList();
                for(int i=0;i<fin.Count();i++)
                {
                    if(fin.ElementAt(i).FinancingFactorContactName==name)
                    {
                        return (fin.ElementAt(i).FinancingFactorType);
                    }
                }
                return "";

           }


           /*the function return all treatment hours strings*/
           public List<String> getAllHours()
           {
               List<String> list = new List<String>();
               list.Add("08:00");
               list.Add("08:30");
               list.Add("09:00");
               list.Add("09:30");
               list.Add("10:30");
               list.Add("11:00");
               list.Add("11:30");
               list.Add("12:00");
               list.Add("12:30");
               list.Add("13:00");
               list.Add("13:30");
               list.Add("14:00");
               list.Add("14:30");
               list.Add("15:00");
               list.Add("15:30");
               list.Add("16:00");
               list.Add("16:30");
               list.Add("17:00");
               list.Add("17:30");
               list.Add("18:00");
               list.Add("18:30");
               list.Add("19:00");
               list.Add("19:30");
               list.Add("20:00");
               return list;
           }

           /*the function return all the names of the reports*/
           public List<String> getAllReportNames()
           {
               List<String> list = new List<String>();
               list.Add("רשימת מטופלים");
               list.Add("מטופלים עפ''י גורמים מממנים");
               list.Add("תשלומים עבור גורם מממן");
               list.Add("אנשי קשר של מטפלים");

               return list;
           }

    //----------------------------LOGIN-------------------------------------------------------------------
        
        public tblTherapist isUserValid(User user)
        {
            var u = Therapist.Where(t => t.UserName == user.UserName && t.Passcode == user.Password).FirstOrDefault();

            return u;
        }
    }

}