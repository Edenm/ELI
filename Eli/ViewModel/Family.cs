using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eli.Models;

namespace Eli.ViewModel
{
    public partial class Family
    {
        public String PatientID;

        public List<tblParent> Parent;

        public List<tblBrotherSister> BrotherSister;

        EliManagerDB db;

        public Family(String PID)
        {
            db = new EliManagerDB();

            PatientID = PID;
            BrotherSister = db.getAllBrotherSisterByPatient(PID);
            Parent = db.getAllParentsByPatient(PID);

        }
    }
}