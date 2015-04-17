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

        private List<tblParent> parents;

        private List<tblBrotherSister> brotherSisters;

        private EliManagerDB db;

        public Family(String PID)
        {
            db = new EliManagerDB();

            patient = db.getPatientById(PID);
            brotherSisters = db.getAllBrotherSisterByPatient(PID);
            parents = db.getAllParentsByPatient(PID);

        }

        public tblPatient Patient
        {
            get { return patient; }
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