﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

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

        public Table<tblReferenceTherapistTreatment> ReferenceTherapistTreatment
        {
            get { return db.tblReferenceTherapistTreatments; }
        }

        public Table<tblParentPatient> ParentPatient
        {
            get { return db.tblParentPatients; }
        }

        public Table<tblParent> Parent
        {
            get { return db.tblParents; }
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

        public void addFinanceFactor(tblFinancingFactor ff)
        {
            FinancingFactor.InsertOnSubmit(ff);
            db.SubmitChanges();
        }


        //-------------------------Remove methods----------------------------------------------------------

        public void removeFinanceFactor(tblFinancingFactor ff)
        {


            var FinanceFactores = from t in Treatment
                                  where t.FinancingFactorNumber.Equals(ff.FinancingFactorNumber)
                                  select t;
            if (FinanceFactores.Any())
                throw new Exception("You cant delete  Finance factor id=" + ff.FinancingFactorNumber + "name=" + ff.Name + "because its conected to treatment");
            var d = FinancingFactor.First(x => x.FinancingFactorNumber == ff.FinancingFactorNumber);

            FinancingFactor.DeleteOnSubmit(d);
            db.SubmitChanges();

            return;

        }

        //-------------------------Edit methods----------------------------------------------------------

        public void EditFinanceFactor(tblFinancingFactor ff)
        {
            var d = FinancingFactor.First(x => x.FinancingFactorNumber == ff.FinancingFactorNumber);
            d.ContactName = ff.ContactName;
            d.ContactMail = ff.ContactMail;
            d.ContcatPhoneNumber = ff.ContcatPhoneNumber;
            db.SubmitChanges();
        }
        


    }

    




}