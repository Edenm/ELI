using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eli.Models;

namespace Eli.ViewModel
{
    public partial class Family
    {
        private tblPatient patient;

        private tblReference reference;

        private tblTreatment treatment;

        private tblTherapist therapist;

        private List<tblParent> parents;

        private List<tblBrotherSister> brotherSisters;

        private EliManagerDB db;

        public Family()
        {
            patient = new tblPatient();
            reference = new tblReference();
            parents = new List<tblParent>();
            brotherSisters = new List<tblBrotherSister>();
        }

        public Family(String PID)
        {
            db = new EliManagerDB();

            patient = db.getPatientById(PID);
            brotherSisters = db.getAllBrotherSisterByPatient(PID);
            parents = db.getAllParentsByPatient(PID);
            
            treatment = db.getNextTreatmentByPatient(PID);
            if (treatment!=null)
            {
                therapist = db.getTherapistByTreatmentNumber(treatment.TreatmentNumber);
            }
                
        }

        public tblPatient Patient
        {
            get { return patient; }
            set { patient = value; }
        }

        public tblReference Reference
        {
            get { return reference; }
            set { reference = value; }
        }

        public tblTreatment NextTreatment
        {
            get { return treatment; }
            set { treatment = value; }
        }

        public tblTherapist Therapist
        {
            get { return therapist; }
            set { therapist = value; }
        }

        public List<tblParent> Parents
        {
            get { return parents; }
        }

        public List<tblBrotherSister> BrotherSisters
        {
            get { return brotherSisters; }
        }
    }
}