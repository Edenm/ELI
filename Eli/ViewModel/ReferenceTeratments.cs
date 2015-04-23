using Eli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eli.ViewModel
{
    public class ReferenceTeratments    
    {
        public tblPatient Patient;

        public List<tblRefererencePatient> referencePatients;

        public List<tblReference> references;

        public List<tblReferenceTherapist> referenceTherapists;

        public List<tblTreatment> treatments;

        EliManagerDB db;

        public ReferenceTeratments(string rid)
        {

            db= new EliManagerDB();

            Patient = db.getPatientById(rid);

            referencePatients = db.RefererencePatients.ToList();

            references = db.Reference.ToList();

            referenceTherapists = db.ReferenceTherapist.ToList();

            treatments = db.Treatment.ToList();

        }
    }
}
