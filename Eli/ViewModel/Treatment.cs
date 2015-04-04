using Eli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eli.ViewModel
{
    public class Treatment
    {
        public tblTreatment treatment;

        public tblPatient patient;

        public tblReference reference;

        public tblTherapist therapist;

        public tblFinancingFactor financingFactor;
        
        EliManagerDB db;


        public Treatment() 
        {
            treatment = new tblTreatment();
        }

        public Treatment(int treatNum)
        {

            db= new EliManagerDB();

            treatment = db.getTreatmentByNumber(treatNum);

            patient = db.getPatientByTreatmentNumber(treatNum);

            reference = db.getReferenceByTreatmentNumber(treatNum);

            therapist = db.getTherapistByTreatmentNumber(treatNum);

            financingFactor = db.getFinancingFactorByTreatmentNumber(treatNum);
        }


    }
}