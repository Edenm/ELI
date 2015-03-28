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
        tblPatient Patients;

        public List<tblReference> References;

        EliManagerDB db;

        public TeratmentReferencePatient()
        {
            
        }
    }
}
