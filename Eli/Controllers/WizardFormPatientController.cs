using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eli.Models;
using Eli.ViewModel;


namespace Eli.Controllers
{
    [Authorize]
    public class WizardFormPatientController : Controller
    {
        
        public static int countP = 0;
        public static int countBS= 0;

        public ActionResult Step1()
        {
            return View(new tblPatient());
        }


        public ActionResult Step2(tblPatient pat, string nextBtn)
        {
            EliManagerDB db= new EliManagerDB();
            if (nextBtn != null)
            {
                try{
                    db.checkPatient(pat);
                    Family obj = GetFamily();
                    obj.Patient = pat;
                    return View(obj);
                }
                catch (Exception e)
                {
                    ViewBag.operate = e.Message;
                    ViewBag.type = "danger";
                }
            }
            pat.ID = null;
            return View("Step1", pat);
        }

        public ActionResult Step3(FormCollection famlyForm, string prevBtn, string nextBtn, string add, string remove)
        {
            Family fam = GetFamily();
            EliManagerDB db = new EliManagerDB();
            

//--------------------------------Add/Remove Parent-------------------------------------------------------
            if (add != null && add == "הוספת הורה")
            {
                tblParent parent = new tblParent();
                parent.ParentID = famlyForm.GetValues("ParentID")[0];
                bool flag = true;
                try
                {
                    db.checkParent(parent);
                
                    foreach (tblParent p in fam.Parents)
                    {
                        if (p.ParentID.Equals(parent.ParentID))
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        parent.ParentFirstName = famlyForm.GetValues("ParentFirstName")[0];
                        parent.ParentSurName = famlyForm.GetValues("ParentSurName")[0];
                        parent.ParentAddress = famlyForm.GetValues("ParentAddress")[0];
                        parent.ParentPhoneNumber = famlyForm.GetValues("ParentPhoneNumber")[0];
                        parent.Explain = famlyForm.GetValues("Explain")[0];
                        parent.IsWorking = famlyForm.GetValues("IsWorking")[0];
                        parent.ParentMail = famlyForm.GetValues("ParentMail")[0];
                        parent.ParentGender = famlyForm.GetValues("ParentGender")[0];
                        parent.ParentBirthDate =Convert.ToDateTime(famlyForm.GetValues("ParentBirthDate")[0]);
                        parent.Comment = famlyForm.GetValues("Comment")[0];

                        fam.Parents.Add(parent);
                        countP++;
                    }
                }catch (Exception e)
                {
                    ViewBag.operate = e.Message;
                    ViewBag.type = "danger";
                }
                return View("Step2",fam);
            }

            if (remove != null && remove == "RemoveParent")
            {
                fam.Parents.RemoveAt(fam.Parents.Count() - 1);
                countP--;
                return View("Step2", fam);
            }

//--------------------------------Add/Remove BrotherSister----------------------------------------------

            if (add != null && add == "הוספת אח/ות")
            {
                tblBrotherSister broSis = new tblBrotherSister();
                broSis.BrotherSisterID = famlyForm.GetValues("BrotherSisterID")[0];
                bool flag = true;
                try
                {
                    db.checkBrotherSister(broSis);

                    foreach (tblBrotherSister p in fam.BrotherSisters)
                    {
                        if (p.BrotherSisterID.Equals(broSis.BrotherSisterID))
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        broSis.BrotherSisterFirstName = famlyForm.GetValues("BrotherSisterFirstName")[0];
                        broSis.BrotherSisterSurName = famlyForm.GetValues("BrotherSisterSurName")[0];
                        broSis.BrotherSisterStudyFramework = famlyForm.GetValues("BrotherSisterStudyFramework")[0];
                        broSis.BrotherSisterGender = famlyForm.GetValues("BrotherSisterGender")[0];
                        broSis.BrotherSisterBirthDate = Convert.ToDateTime(famlyForm.GetValues("BrotherSisterBirthDate")[0]);
                        fam.BrotherSisters.Add(broSis);
                        countBS++;
                    }
                }catch (Exception e)
                {
                    ViewBag.operate = e.Message;
                    ViewBag.type = "danger";
                }
                return View("Step2", fam);
            }

            if (remove != null && remove == "RemoveBrotherSister")
            {
                fam.BrotherSisters.RemoveAt(fam.BrotherSisters.Count() - 1);
                countBS--;
                return View("Step2", fam);
            }

//--------------------------------next/prev BrotherSister----------------------------------------------
            if (prevBtn != null)
            {
                fam.Patient.ID = null;
                return View("Step1",fam.Patient );
            }
            if (nextBtn != null)
            {
                {
                    return View(new tblReference());
                }
            }
            return View("Step2", fam);
        }


        public ActionResult EndOfWizard(tblReference refe, string finBtn, string prevBtn)
        {
            EliManagerDB db = new EliManagerDB();
            Family fam = GetFamily();

            if (prevBtn != null)
            {
                return View("Step2", fam);
            }

            if (finBtn != null)
            {
                    tblTherapist ther = (tblTherapist)Session["Therapist"];
                    fam.Reference = refe;
                    fam.Therapist = ther;
                    try
                    {
                        db.addPatient(fam);
                    }
                    catch(Exception ex)
                    {
                        ViewBag.msg = ex.Message;
                    }
                    return View();
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