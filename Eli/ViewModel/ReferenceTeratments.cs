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

        public List<tblReference> getAllReferencesByPatientId(string id)
        {
            var referencesList = referencePatients.Where(p => p.PatientID == id).Select(re=> re.ReferenceNumber).ToList();

            var refe = references.Where(r => referencesList.Contains(r.ReferenceNumber)).ToList();

            return refe;
        }

        public List<tblReferenceTherapist> getAllReferencesTherapistByReferenceNumber(int refNum)
        {
            var refTher = referenceTherapists.Where(rt => rt.ReferenceNumber == refNum).ToList();

            return refTher;
        }

        public List<tblTreatment> getAllTreatmentByReferenceNumberAndTherapistId(int refNum, string id)
        {

            var treat = treatments.Where(t => t.ReferenceNumber == refNum && t.TherapistID==id).ToList();

            return treat;
        }

        public List<tblTreatment> getAllTreatmentByReferenceNumber(int refNum)
        {
           
            var treat = treatments.Where(t => t.ReferenceNumber==refNum).ToList();

            return treat;
        }
    }
}
