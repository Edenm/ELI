using Eli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eli.ViewModel
{
    public class TeratmentReferencePatient    
    {
        public tblPatient Patient;

        public List<tblReference> references;

        public List<tblReferenceTherapist> referenceTherapists;

        public List<tblReferenceTherapistTreatment> referenceTherapistTreatments;

        public List<tblTreatment> treatments;

        EliManagerDB db;

        public TeratmentReferencePatient(string id)
        {

            db= new EliManagerDB();

            Patient = db.getPatientById(id);

            references = db.Reference.ToList();

            referenceTherapists = db.ReferenceTherapist.ToList();

            referenceTherapistTreatments = db.ReferenceTherapistTreatment.ToList();

            treatments = db.Treatment.ToList();

        }

        public List<tblReference> getAllReferencesByPatients(string id)
        {
            var refe = references.Where(r => r.PatientID == id).ToList();

            return refe;
        }

        public List<tblReferenceTherapist> getAllReferencesTherapistByReferenceNumber(int refNum)
        {
            var refTher = referenceTherapists.Where(rt => rt.ReferenceNumber == refNum).ToList();

            return refTher;
        }

        public List<tblTreatment> getAllTreatmentByReferenceNumberAndTherapistId(int refNum, string id)
        {
            int tretNum=referenceTherapistTreatments.Where(rtt => rtt.ReferenceNumber == refNum && rtt.TherapistID == id).Select(rtt=>rtt.TreatmentNumber).First();
            var treat = treatments.Where(t => t.TreatmentNumber==tretNum).ToList();

            return treat;
        }

        public List<tblTreatment> getAllTreatmentByReferenceNumber(int refNum)
        {
            var tretNum = referenceTherapistTreatments.Where(rtt => rtt.ReferenceNumber == refNum).Select(rtt => rtt.TreatmentNumber).ToList();
            var treat = treatments.Where(t => tretNum.Contains(t.TreatmentNumber)).ToList();

            return treat;
        }
    }
}
