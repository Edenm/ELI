using Eli.Models;
using Eli.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;


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


        /* The Method get parameters of patient and add him to system  **/

        public ActionResult AddPatient()
        {

            EliManagerDB db = new EliManagerDB();


       //-----------personal details of patients--------------------------

            tblPatient tbP = new tblPatient()
            {
                ID = Request.Form["id"],
                PatientFirstName = Request.Form["fname"],
                PatientSurName = Request.Form["lname"],
                Gender = Request.Form["gender"],
                EducationalFramework = Request.Form["education"],
                ContcatPhoneNumber = Request.Form["phone"],
                Address = Request.Form["address"]
            };


      //-----------more details of patients parents--------------------------

            tblParent tbPF = new tblParent()
            {
                ID = Request.Form["idf"],
                FirstName = Request.Form["fnamef"],
                SurName = Request.Form["lnamef"],
                //BirthDate = Convert.ToDateTime(birthdatef),
                ContcatPhoneNumber = Request.Form["phonef"],
                Address = Request.Form["adressf"],
                ContactMail = Request.Form["mailf"],
                IsWorking = Request.Form["isworkf"],
                Explain=Request.Form["explainf"]
            };

            tblParent tbPM = new tblParent()
            {
                 ID = Request.Form["idm"],
                FirstName = Request.Form["fnamem"],
                SurName = Request.Form["lnamem"],
                //BirthDate = Convert.ToDateTime(birthdatef),
                ContcatPhoneNumber = Request.Form["phonem"],
                Address = Request.Form["adressm"],
                ContactMail = Request.Form["mailm"],
                IsWorking = Request.Form["isworkm"],
                Explain=Request.Form["explainm"]
            };


       //-----------more details of patients brothers and sisters--------------------------

            List<tblBrotherSister> arrBS = new List<tblBrotherSister>();

            for (int i = 1; Request.Form["id" + i] == ""; i++)
            {
                tblBrotherSister tbBS = new tblBrotherSister()
                {
                    ID = Request.Form["id" + i],
                    FirstName = Request.Form["fname" + i],
                    SurName = Request.Form["lname" + i],
                    // BirthDate=Request.Form["birthdate"],
                    Gender = Request.Form["gender" + i],
                    StudyFramework = Request.Form["education" + i]
                };

                arrBS.Add(tbBS);
            }

      //-----------more details of patients reference--------------------------

            tblReference tbR = new tblReference()
            {
                StatusReference = Request.Form["status"],
                OtherStatus = Request.Form["description"],
                AbuseType = Request.Form["type"],
                ReferenceSource = Request.Form["source"],
                PatientID = Request.Form["id"]
            };
            
            db.addPatient(tbP, tbPF, tbPM, tbR,arrBS);

            return View(tbPF);

        }


       
            public ActionResult childs()
            {
                EliManagerDB db = new EliManagerDB();

                var pat = db.Patients.ToArray();

                return View(pat);
            }

            /* The Method displays all the patients in the system-this is the main form of patients  **/
            [HttpGet]
            public ActionResult MainPatients()
            {
                EliManagerDB db = new EliManagerDB();

                var pat = db.Patients.ToArray();

                return View(pat);
            }


            /* The method get a patient details and update the patient **/
            [HttpPost]
            public ActionResult MainPatients(tblPatient obj)
            {
                EliManagerDB db = new EliManagerDB();

                Type type = obj.GetType();
                String str = type.Name;

                if (obj.GetType()==typeof(tblPatient))
                    db.EditPatient((tblPatient)obj);

                //if (obj is Family)
                   // db.EditFamily((Family)obj);

                var pat = db.Patients.ToArray();

                return View(pat);
            }


            public ActionResult _EditFamily(String id)
            {
                EliManagerDB db = new EliManagerDB();

                Family objFam = new Family(id);
                return PartialView(objFam);
            }




            /* The Method get parameters of patient and displays all his refernces  **/
            public ActionResult Reference(String id)
            {


                EliManagerDB db = new EliManagerDB();

                List<tblTherapist> Therapist = new List<tblTherapist>();
                List<tblTreatment> Treatment = new List<tblTreatment>();
                List<tblReference> reference = db.Reference.ToArray().Where(a => a.PatientID.ToString() == (id)).ToList();
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
            /* The Method get parameters of patient and reference and displays all treatments of specific reference of patients  **/

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


            /* The Method get parameters of patient and displays  his full details  **/

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



