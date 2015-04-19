using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eli.Models;
using Eli.ViewModel;


namespace Eli.Controllers
{
    public class WizardFormPatientController : Controller
    {
        //
        // GET: /WizardFormPatient/
        public static int countGlobal = 0;
        public static int countP = 0;
        public static int countBS= 0;

        public ActionResult Step1()
        {
            return View(new tblPatient());
        }


        public ActionResult Step2(tblPatient pat, string nextBtn)
        {
            if (nextBtn != null)
            {
                //if (ModelState.IsValid)
                {
                    Family obj = GetFamily();
                    obj.Patient = pat;
                    //obj.Patient.ID = pat.ID;
                    /*
                    obj.Patient.PatientFirstName = pat.PatientFirstName;
                    obj.Patient.PatientSurName = pat.PatientSurName;
                    obj.Patient.StatusPatient = pat.StatusPatient;
                    obj.Patient.Gender = pat.Gender;
                    obj.Patient.BirthDate = pat.BirthDate;
                    obj.Patient.Address = pat.Address;
                    obj.Patient.ContcatPhoneNumber = pat.ContcatPhoneNumber;
                    obj.Patient.EducationalFramework = pat.EducationalFramework;
                     */

                    return View(obj);
                }
            }
            return View("Step1");
        }

        public ActionResult Step3(FormCollection famlyForm, string prevBtn, string nextBtn, string add, string remove)
        {
            Family fam = GetFamily();
            /*
                tblPatient tp = new tblPatient();
                //tp.ID = fam.Patient.ID;
                tp.PatientFirstName = fam.Patient.PatientFirstName;
                tp.PatientSurName = fam.Patient.PatientSurName;
                tp.StatusPatient = fam.Patient.StatusPatient;
                tp.Gender = fam.Patient.Gender;
                tp.BirthDate = fam.Patient.BirthDate;
                tp.Address = fam.Patient.Address;
                tp.ContcatPhoneNumber = fam.Patient.ContcatPhoneNumber;
                tp.EducationalFramework = fam.Patient.EducationalFramework;*/

//--------------------------------Add/Remove Parent-------------------------------------------------------
            if (add != null && add == "AddParent")
            {
                tblParent parent = new tblParent();
                parent.ParentFirstName = famlyForm.GetValues("ParentFirstName")[countP];
                parent.ParentSurName = famlyForm.GetValues("ParentSurName")[countP];
                //parent.ParentAddress = famlyForm.GetValues("ParentAddress")[countP];
                parent.ParentPhoneNumber = famlyForm.GetValues("ParentPhoneNumber")[countP];
                //parent.Explain = famlyForm.GetValues("Explain")[countP];
                parent.IsWorking = famlyForm.GetValues("IsWorking")[countP];
                parent.ParentMail = famlyForm.GetValues("ParentMail")[countP];
                parent.ParentGender = famlyForm.GetValues("ParentGender")[countP];
                //parent.ParentBirthDate =Convert.ToDateTime(famlyForm.GetValues("ParentBirthDate")[countP]);
                fam.Parents.Add(parent);
                countP++;
                countGlobal++;
                return View("Step2",fam);
            }

            if (remove != null && remove == "RemoveParent")
            {
                fam.Parents.RemoveAt(fam.Parents.Count() - 1);
                countP--;
                countGlobal--;
                return View("Step2", fam);
            }

//--------------------------------Add/Remove BrotherSister----------------------------------------------

            if (add != null && add == "AddBrotherSister")
            {
                tblBrotherSister broSis = new tblBrotherSister();
                broSis.BrotherSisterFirstName = famlyForm.GetValues("BrotherSisterFirstName")[countGlobal];
                broSis.BrotherSisterSurName = famlyForm.GetValues("BrotherSisterSurName")[countP];
                broSis.BrotherSisterStudyFramework = famlyForm.GetValues("BrotherSisterStudyFramework")[countBS];
                //broSis.BrotherSisterGender = famlyForm.GetValues("BrotherSisterGender")[countP];
                broSis.BrotherSisterBirthDate = Convert.ToDateTime(famlyForm.GetValues("BrotherSisterBirthDate")[countBS]);
                fam.BrotherSisters.Add(broSis);
                countGlobal++;
                countBS++;
                return View("Step2", fam);
            }

            if (remove != null && remove == "RemoveBrotherSister")
            {
                fam.BrotherSisters.RemoveAt(fam.BrotherSisters.Count() - 1);
                countGlobal--;
                countBS--;
                return View("Step2", fam);
            }

//--------------------------------next/prev BrotherSister----------------------------------------------
            if (prevBtn != null)
            {
                return View("Step1",fam.Patient );
            }
            if (nextBtn != null)
            {
                //if (ModelState.IsValid)
                {
                    /*
                    for (int i = 0; i <famlyForm.GetValue("IsWorking").AttemptedValue.Count() ; i++)
                    {
                        tblParent parent= new tblParent();
                        parent.FirstName = famlyForm.GetValues("FirstName")[i];
                        parent.SurName = famlyForm.GetValues("SurName")[i];
                        //parent.Address = famlyForm.GetValues("Address")[i];
                        parent.ContcatPhoneNumber = famlyForm.GetValues("ContcatPhoneNumber")[i];
                        //parent.Explain = famlyForm.GetValues("Explain")[i];
                        parent.IsWorking = famlyForm.GetValues("IsWorking")[i];
                        parent.ContactMail = famlyForm.GetValues("ContactMail")[i];
                        //parent.Gender = famlyForm.GetValues("Gender")[i]; 
                        //parent.BirthDate =Convert.ToDateTime(famlyForm.GetValues("BirthDate")[i]);
                        fam.Parents.Add(parent);
                    }
                    int countBSin=famlyForm.GetValue("IsWorking").AttemptedValue.Count();
                    for (int j = 0; j <famlyForm.GetValue("StudyFramework").AttemptedValue.Count() ; j++)
                    {
                        countBSin += j;
                        tblBrotherSister broSis = new tblBrotherSister();
                        broSis.FirstName = famlyForm.GetValues("FirstName")[j];
                        broSis.SurName = famlyForm.GetValues("SurName")[j];
                        broSis.StudyFramework = famlyForm.GetValues("StudyFramework")[countBSin];
                        //broSis.Gender = famlyForm.GetValues("Gender")[j];
                        broSis.BirthDate = Convert.ToDateTime(famlyForm.GetValues("BirthDate")[j]);
                        fam.BrotherSisters.Add(broSis);
                    }*/

                    return View(new tblReference());
                }
            }
            return View("Step2", fam);
        }

        public ActionResult EndOfWizard(tblReference refe, string finBtn, string prevBtn)
        {
            Family fam = GetFamily();

            if (prevBtn != null)
            {
                return View("Step2", fam);
            }

            if (finBtn != null)
            {
                //if (ModelState.IsValid)
                {
                    Family obj = GetFamily();
                    obj.Reference = refe;
                    return View();
                }
            }
            return View("Step3",fam.Reference);
        }


        /** The method return family from Session*/
        private Family GetFamily()
        {
            if (Session["family"] == null)
            {
                Session["family"] = new Family();
            }
            return (Family)Session["family"];
        }

        /** The method remove family from Session*/
        private void RemoveFamily()
        {
            Session.Remove("family");
        }
    }
}