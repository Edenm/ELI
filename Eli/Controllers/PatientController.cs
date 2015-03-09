using Eli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eli.Controllers
{
    public class PatientController : Controller
    {
        public ActionResult Index()
        {
            EliManagerDB db = new EliManagerDB();

            var pat = db.Patients.ToArray();

            return View(pat);
        }

        public ActionResult NewPatient()
        {
            return View();
        }



        public ActionResult AddPatient()
        {
            EliManagerDB db = new EliManagerDB();

            String Id = Request.Form["id"];
            String fName = Request.Form["fname"];
            String lName = Request.Form["lname"];
            String gender = Request.Form["gender"];
            String phone = Request.Form["phone"];
            String address = Request.Form["address"];
            String education = Request.Form["education"];

            tblPatient tp = new tblPatient()
            {
                ID = Id,
                PatientFirstName = fName,
                PatientSurName = lName,
                Gender = gender,
                EducationalFramework = education,
                ContcatPhoneNumber = phone
            };

            db.addPatient(tp);

            var pat = db.Patients.ToArray();

            return View(pat);
        }


       
        public ActionResult childs()
        {
            EliManagerDB db = new EliManagerDB();

            var pat = db.Patients.ToArray();

            return View(pat);
        }

        public ActionResult MainPatients()
        {


            EliManagerDB db = new EliManagerDB();

            var pat = db.Patients.ToArray();

            return View(pat);
        }


        





        public ActionResult Reference(String id)
        {


            EliManagerDB db = new EliManagerDB();

            List<tblTherapist> Therapist = new List<tblTherapist>();
            List<tblTreatment> Treatment = new List<tblTreatment>();
            List<tblReference> reference = db.Refernce.ToArray().Where(a => a.PatientID.ToString() == (id)).ToList();
            var pat = db.ReferenceTherapistTreatment.ToArray();

            for (int i = 0; i < reference.Count; i++)
            {
                for (int j = 0; j < pat.Length; j++)
                {

                    int w = reference.ElementAt(i).ReferenceNumber;
                    int z = pat.ElementAt(j).ReferenceNumber;

                    if (w == z)
                    {
                        tblTherapist th = db.Therapist.ToArray().Single(p => p.ID.ToString() == (pat.ElementAt(j).TherapistID));
                        tblTreatment tr = db.Treatment.ToArray().Single(a => a.TreatmentNumber == (pat.ElementAt(j).TreatmentNumber));
                        Therapist.Add(th);
                        Treatment.Add(tr);
                    }
                }
            }
            Reference r = new Reference();
            r.r = reference.ToList();
            r.th = Therapist.ToList();
            r.tr = Treatment.ToList();
            return View(r);
        }

        public ActionResult TreatByReference(int idRef, string pId)
        {

            EliManagerDB db = new EliManagerDB();

            
            List<tblTherapist> Therapist = new List<tblTherapist>();
            List<tblTreatment> Treatment = new List<tblTreatment>();
            List<tblReferenceTherapistTreatment> RTT = db.ReferenceTherapistTreatment.ToArray().Where(a => a.ReferenceNumber == (idRef)).ToList();

            for (int i = 0; i < RTT.Count; i++)
            {
                Therapist.Add(db.Therapist.ToArray().Single(p => p.ID.ToString() == (RTT.ElementAt(i).TherapistID)));
                Treatment.Add(db.Treatment.ToArray().Single(a => a.TreatmentNumber == (RTT.ElementAt(i).TreatmentNumber)));

            }
            TreatByRef r = new TreatByRef();
            r.pId = pId;
            r.rId = idRef;
            r.th = Therapist.ToList();
            r.tr = Treatment.ToList();
            return View(r);
        }
      
        public ActionResult PatientDetails(String id)
        {
            EliManagerDB db = new EliManagerDB();
            BrotherSister BSview = new BrotherSister();
            BSview.BrotherSisters = new List<tblBrotherSister>();

            tblPatient patient = db.Patients.ToArray().Single(p => p.ID.ToString() == (id));


            List<tblBrotherSisterPatient> BSP = db.BrotherSisterPatient.ToArray().Where(a => a.PatientID.ToString() == (id)).ToList();
            var BS = db.BrotherSister.ToArray();

            for (int i = 0; i < BSP.Count; i++)
            {
                for (int j = 0; j < BS.Length; j++)
                {

                    String w = BSP.ToArray().ElementAt(i).BrotherSisterID;
                    String z = BS.ToArray().ElementAt(j).ID;

                    if (string.Compare(w, z) == 0)
                    {


                        BSview.BrotherSisters.Add(BS.ElementAt(j));
                    }
                }
            }

            List<tblParentPatient> parentpatient = db.ParentPatient.ToArray().Where(b => b.PatientID.ToString() == (id)).ToList();
            var parent = db.Parent.ToArray();

            BSview.Parents = new List<tblParent>();
            for (int i = 0; i < parentpatient.Count; i++)
            {
                for (int j = 0; j < parent.Length; j++)
                {
                    int x = 0;
                    int y = 0;

                    Int32.TryParse(parentpatient.ToArray().ElementAt(i).ParentID.ToString(), out x);
                    Int32.TryParse(parent.ToArray().ElementAt(j).ID.ToString(), out y);
                    if (x == y)
                    {
                        BSview.Parents.Add(parent.ToArray().ElementAt(j));
                    }
                }
            }


            BSview.patient = patient;
            return View(BSview);

        }
    }
}



