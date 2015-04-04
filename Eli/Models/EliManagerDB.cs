﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Eli.ViewModel;

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
        public void addPatient(tblPatient patient, tblParent mother, tblParent father, tblReference reference, List<tblBrotherSister> BS)
        {

            Patients.InsertOnSubmit(patient);

            Parent.InsertOnSubmit(mother);
            Parent.InsertOnSubmit(father);

            tblParentPatient tblFP= new tblParentPatient()
            {
                PatientID= patient.ID,
                ParentID = father.ID
            };

            tblParentPatient tblMP= new tblParentPatient()
            {
                PatientID= patient.ID,
                ParentID = mother.ID
            };

            ParentPatient.InsertOnSubmit(tblFP);
            ParentPatient.InsertOnSubmit(tblMP);

            foreach (var bs in BS)
            {
                BrotherSister.InsertOnSubmit(bs);

                tblBrotherSisterPatient tbBSP = new tblBrotherSisterPatient()
                {
                    BrotherSisterID = bs.ID,
                    PatientID = patient.ID
                };

                BrotherSisterPatient.InsertOnSubmit(tbBSP);

            }

            Reference.InsertOnSubmit(reference);

            db.SubmitChanges();

        }

        /* Add Therapist to data base **/
        public void addTherapist(tblTherapist tt)
        {
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
     

        //-------------------------Edit methods----------------------------------------------------------

        /* The method is editing exist Finance Factor **/
        public void EditFinanceFactor(tblFinancingFactor ff)
        {
            var d = FinancingFactor.First(x => x.FinancingFactorNumber == ff.FinancingFactorNumber);
            d.Name = ff.Name;
            d.FinancingFactorType = ff.FinancingFactorType;
            d.ContactName = ff.ContactName;
            d.ContactMail = ff.ContactMail;
            d.ContcatPhoneNumber = ff.ContcatPhoneNumber;
            db.SubmitChanges();
        }

        /* The method is editing exist Therapist**/
        public void EditTherapist(tblTherapist tt)
        {
            var t = Therapist.First(x => x.ID == tt.ID);
            t.ID = tt.ID;
            t.TherapistFirstName = tt.TherapistFirstName;
            t.TherapistSurName = tt.TherapistSurName;
            t.BirthDate = tt.BirthDate;
            t.Address = tt.Address;
            t.Gender = tt.Gender;
            t.Passcode = tt.Passcode;
            t.UserName = tt.UserName;
            t.ContactMail = tt.ContactMail;
            t.ContcatPhoneNumber = tt.ContcatPhoneNumber;
            db.SubmitChanges();
        }

        /* The method is editing exist Patient**/
        public void EditPatient(tblPatient pat)
        {
            var patient = Patients.First(p => p.ID == pat.ID);
            patient.ID = pat.ID;
            patient.PatientFirstName = pat.PatientFirstName;
            patient.PatientSurName = pat.PatientSurName;
            patient.Address = pat.Address;
            patient.BirthDate = pat.BirthDate;
            patient.ContcatPhoneNumber = pat.ContcatPhoneNumber;
            patient.EducationalFramework = pat.EducationalFramework;

            db.SubmitChanges();
        }

        /* The method is editing exist patient**/
        public void EditFamily(Family fam)
        {
            foreach (tblBrotherSister tbs in fam.BrotherSister){
                var broSis = BrotherSister.First(bs => bs.ID == tbs.ID);
                broSis.ID = tbs.ID;
                broSis.FirstName = tbs.FirstName;
                broSis.SurName = tbs.SurName;
                broSis.StudyFramework = tbs.StudyFramework;
                broSis.Gender = tbs.Gender;
                broSis.BirthDate = tbs.BirthDate;
            }

            foreach (tblParent tp in fam.Parent)
            {
                var parent = Parent.First(bs => bs.ID == tp.ID);
                parent.ID = tp.ID;
                parent.FirstName = tp.FirstName;
                parent.SurName = tp.SurName;
                parent.Address = tp.Address;
                parent.ContactMail = tp.ContactMail;
                parent.Explain = tp.Explain;
                parent.IsWorking = tp.IsWorking;
                parent.ContactMail = tp.ContactMail;
                parent.Gender = tp.Gender;
                parent.BirthDate = tp.BirthDate;
            }
            
            db.SubmitChanges();
        }


        //-----------------------get Method----------------------------------------------------------------

        public tblPatient getPatientById(String pId)
        {
            var p = (Patients.Where(pat => pat.ID == pId)).First();

            return p;
        }

        public tblTreatment getTreatmentByNumber(int tnum)
        {
            var tr = (Treatment.Where(t => t.TreatmentNumber == tnum)).First();

            return tr;
        }

        public tblPatient getPatientByTreatmentNumber(int tnum)
        {
            var refNum = getTreatmentByNumber(tnum).ReferenceNumber;

            var patId = RefererencePatients.Where(rp => rp.ReferenceNumber == refNum).Select(rp=> rp.PatientID).First();

            var pat = Patients.Where(p => p.ID == patId).First();

            return pat;
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

            var ther = Therapist.Where(t => t.ID == therId).First();

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

            var BS= (BrotherSister.Where(bs=> BSP.Contains(bs.ID))).ToList();
         
            return BS;
        }

        public List<tblParent> getAllParentsByPatient(String PID)
        {
            var PP = (ParentPatient.Where(p => p.PatientID == PID).Select(p=> p.ParentID));

            var P = (Parent.Where(bs => PP.Contains(bs.ID))).ToList();

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
       

    }

    




}