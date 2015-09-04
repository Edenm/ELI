using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eli.Models;

namespace Eli.ViewModel
{
    public class TreatmentByFinanceFactor
    {
        private tblFinancingFactor financeFactor;
        private List<TreatmentPatient> treatmentPatient;
        private double totalDebateFinance;
        private double total;

        public tblFinancingFactor FinancingFactor
        {
            get { return financeFactor; }
            set { financeFactor = value; }
        }

        public List<TreatmentPatient> TreatmentPatient
        {
            get { return treatmentPatient; }
            set { treatmentPatient = value; }
        }

        public double TotalDebateFinance
        {
            get { return totalDebateFinance; }
            set { totalDebateFinance = value; }
        }

        public double Total
        {
            get { return total; }
            set { total = value; }
        }
    }

    public class TreatmentPatient
    {
        private tblTreatment treatment;
        private tblPatient patient;

        public tblTreatment Treatment
        {
            get { return treatment; }
            set { treatment = value; }
        }

        public tblPatient Patient
        {
            get { return patient; }
            set { patient = value; }
        }
    }
}