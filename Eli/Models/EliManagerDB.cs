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

        private static SqlCommand queryCommand;
        private static SqlDataReader queryCommandReader;
        private static DataTable dataTable;

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

//-------------------------Add methods----------------------------------------------------------
        
        /* The method add patient with all the details about him **/
        public void addPatient(Family family)
        {
            Patients.InsertOnSubmit(family.Patient);

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

            db.SubmitChanges();
        }


        /* Add Therapist to reference **/
        public void addTherapisttoRef(string tid, int refid)
        {
            var therapist = from r in ReferenceTherapist
                            where r.ReferenceNumber.Equals(refid)
                            select r;
            foreach(var item in therapist)
            {
                if (item.TherapistID.Equals(tid))
                    return;
            }

            tblReferenceTherapist tbRT = new tblReferenceTherapist()
            {
                ReferenceNumber = refid,
                TherapistID = tid
            };

            ReferenceTherapist.InsertOnSubmit(tbRT);
        }
       
        /* Add Therapist to data base **/
        public void addTherapist(tblTherapist tt)
        {
            var therapist = from d in Therapist
                            where d.TherapistID.Equals(tt.TherapistID)
                            select d;

            if (therapist.Any() || therapist.Any())
                throw new Exception("This therapist is already exsit");
            Therapist.InsertOnSubmit(tt);
            db.SubmitChanges();
        }

        /* Add FinancingFactor to data base **/
        public void addFinanceFactor(tblFinancingFactor ff)
        {
            ff.FinancingFactorNumber = FinancingFactor.Count()+1;

            FinancingFactor.InsertOnSubmit(ff);
            db.SubmitChanges();
        }

        /* Add Treatment to data base **/
        public void addReference(tblReference re, string pid)
        {
            re.ReferenceNumber = Reference.Count() + 1;

            tblRefererencePatient refPat= new tblRefererencePatient();
            refPat.PatientID=pid;
            refPat.ReferenceNumber=re.ReferenceNumber;
            ReferencePatient.InsertOnSubmit(refPat);

            Reference.InsertOnSubmit(re);
            db.SubmitChanges();
        }

        /* Add Treatment to data base **/
        public void addTreatment(tblTreatment tr)
        {
            tr.TreatmentNumber = Treatment.Count() + 1;

            Treatment.InsertOnSubmit(tr);
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
            db.SubmitChanges();
        }

        /* The method is editing exist Therapist**/
        public void EditTherapist(tblTherapist tt)
        {
            var t = Therapist.First(x => x.TherapistID == tt.TherapistID);
            t.TherapistID = tt.TherapistID;
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
            patient.ID = pat.ID;
            patient.FirstName = pat.FirstName;
            patient.SurName = pat.SurName;
            patient.PatientAddress = pat.PatientAddress;
            patient.BirthDate = pat.BirthDate;
            patient.PhoneNumber = pat.PhoneNumber;
            patient.EducationalFramework = pat.EducationalFramework;

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
                //parent.ParentAddress = family.GetValues("ParentAddress")[countP];
                parent.ParentPhoneNumber = family.GetValues("ParentPhoneNumber")[countP];
                //parent.Explain = family.GetValues("Explain")[countP];
                parent.IsWorking = family.GetValues("IsWorking")[countP];
                parent.ParentMail = family.GetValues("ParentMail")[countP];
                //parent.ParentGender = family.GetValues("ParentGender")[countP]; 
                //parent.ParentBirthDate =Convert.ToDateTime(family.GetValues("ParentBirthDate")[countP]);
                countP++;
            }
            int countBS = 0;
            foreach (tblBrotherSister tbs in fam.BrotherSisters){
                var broSis = BrotherSister.First(bs => bs.BrotherSisterID == tbs.BrotherSisterID);
                broSis.BrotherSisterFirstName = family.GetValues("BrotherSisterFirstName")[countBS];
                broSis.BrotherSisterSurName = family.GetValues("BrotherSisterSurName")[countBS];
                broSis.BrotherSisterStudyFramework = family.GetValues("BrotherSisterStudyFramework")[countBS];
                //broSis.BrotherSistersGender = family.GetValues("BrotherSisterGender")[countBS];
                broSis.BrotherSisterBirthDate = Convert.ToDateTime(family.GetValues("BrotherSisterBirthDate")[countBS]);
                countBS++;
            }
            
            db.SubmitChanges();
        }


        //-----------------------get Method----------------------------------------------------------------

        public tblPatient getPatientById(String pId)
        {
            var p = (Patients.Where(pat => pat.ID == pId)).First();

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

        public tblFinancingFactor getFinancingFactorByTreatmentNumber(int tnum)
        {
            var financeNum = getTreatmentByNumber(tnum).FinancingFactorNumber;

            var finfac = FinancingFactor.Where(f => f.FinancingFactorNumber == financeNum).First();

            return finfac;
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

        public List<tblTreatment> getAllTreatmentByReferenceNumber(int refNum)
        {
            var treat = Treatment.Where(t => t.ReferenceNumber==refNum).ToList();

            return treat;
        }

        public List<tblReference> getAllReferencesByPatientId(string id)
        {
            var referencesList = ReferencePatient.Where(p => p.PatientID == id).Select(re => re.ReferenceNumber).ToList();

            var refe = Reference.Where(r => referencesList.Contains(r.ReferenceNumber)).ToList();

            return refe;
        }
       
    //----------------------------LOGIN-------------------------------------------------------------------
        
        public tblTherapist isUserValid(User user)
        {
            var u = Therapist.Where(t => t.UserName == user.UserName && t.Passcode == user.Password).FirstOrDefault();

            return u;
        }



       
    }

}