using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eli.Models;

namespace Eli.ViewModel
{
    public class PatientByFinanceFactor
    {
        private tblFinancingFactor financeFactor;
        private List<tblPatient> patients;

        public tblFinancingFactor FinancingFactor
        {
            get { return financeFactor; }
            set { financeFactor = value; }
        }

        public List<tblPatient> Patients
        {
            get { return patients; }
            set { patients = value; }
        }
    }
}