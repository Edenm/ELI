using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Eli.Models
{
    public class EliManagerDB
    {
        DataConnectionDataContext db;

        private static SqlCommand queryCommand;
        private static SqlDataReader queryCommandReader;
        private static DataTable dataTable;

        public EliManagerDB(){
            db = SQLConnection.GetDataContextInstance();
        }

//-------------------------Tables----------------------------------------------------------
 
        public Table<tblPatient> Patients
        {
            get { return db.tblPatients; }
        }


//-------------------------Add methods----------------------------------------------------------
        public void addPatient(tblPatient patient)
        {
            Patients.InsertOnSubmit(patient);
            db.SubmitChanges();
        }

    }

//-------------------------Remove methods----------------------------------------------------------



}