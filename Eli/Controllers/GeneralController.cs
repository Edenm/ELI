using Eli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Eli.Controllers
{
    [Authorize]
    public class GeneralController : Controller
    {
        


        [HttpPost]
        public ActionResult Index(MailModel _objModelMail)
        {
            
            tblTherapist ther = (tblTherapist)Session["Therapist"];
            string email = ther.TherapistMail;
            
            string type = "success", operate = "מייל נשלח בהצלחה";
            _objModelMail.From = _objModelMail.From;
            MailMessage mail = new MailMessage();
            mail.To.Add(_objModelMail.To);
           // mail.From = new MailAddress(_objModelMail.From);  no need for this line!!!!
            mail.Subject = _objModelMail.Subject + " מאת :" + _objModelMail.From;
            string Body = _objModelMail.Body;
            mail.Body = Body;
            mail.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential
          (email, _objModelMail.MailPassword);// Enter seders User name and password
            smtp.EnableSsl = true;
            
          if (_objModelMail.redirect=="therapist")
          {
              try
              {
                  smtp.Send(mail);
                  return RedirectToAction("IndexTherapist", "Therapist", new { operate = operate, type = type });

              }
              catch (SmtpException e)
              {

                  return RedirectToAction("IndexTherapist", "Therapist", new { operate = "ההודעה לא נשלחה.אנא בדוק סיסמא או הגדרות אבטחה של המייל", type = "danger" });

              }
              catch (Exception e)
              {

                  return RedirectToAction("IndexTherapist", "Therapist", new { operate = "ההודעה לא נשלחה.אנא בדוק סיסמא או הגדרות אבטחה של המייל", type = "danger" });

              }
          }

          if (_objModelMail.redirect == "finance")
            {
                try
                {
                    smtp.Send(mail);
                    return RedirectToAction("IndexFinancingFactor", "FinanceFactor", new { operate = operate, type = type });

                }
                catch (SmtpException e)
                {

                    return RedirectToAction("IndexFinancingFactor", "FinanceFactor", new { operate = "ההודעה לא נשלחה.אנא בדוק סיסמא או הגדרות אבטחה של המייל", type = "danger" });

                }
                catch (Exception e)
                {

                    return RedirectToAction("IndexFinancingFactor", "FinanceFactor", new { operate = "ההודעה לא נשלחה.אנא בדוק סיסמא או הגדרות אבטחה של המייל", type = "danger" });

                }
            }
            return RedirectToAction("IndexFinancingFactor", "FinanceFactor");

           

        }



       


        public ActionResult ParentsMail(MailModel _objModelMail)
        {
            tblTherapist ther = (tblTherapist)Session["Therapist"];
            string email = ther.TherapistMail;
            EliManagerDB db = new EliManagerDB();
            List<tblParent> parents = db.Parent.ToList();
            try
            {
                if ((parents = db.getAllParentsByPatient(_objModelMail.patientId)) != null)
                {
                    if(parents.Count()==0)
                    {
                        return RedirectToAction("IndexPatients", "Patient", new { operate = "אין מטופלים במערכת-הודעה לא נשלחה", type = "danger" });

                    }
                    for (int i = 0; i < parents.Count(); i++)
                    {
                        _objModelMail.From = _objModelMail.From;
                        MailMessage mail = new MailMessage();
                        mail.To.Add(parents.ElementAt(i).ParentMail);
                        // mail.From = new MailAddress(_objModelMail.From);  no need for this line!!!!
                        mail.Subject = _objModelMail.Subject + " מאת :" + _objModelMail.From;
                        string Body = _objModelMail.Body;
                        mail.Body = Body;
                        mail.IsBodyHtml = false;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential
                          (email, _objModelMail.MailPassword);// Enter seders User name and password
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }



                }

                return RedirectToAction("IndexPatients", "Patient",new { operate = "ההודעה נשלחה בהצלחה", type = "success" });
            }
            
            catch (SmtpException e)
            {

                return RedirectToAction("IndexPatients", "Patient", new { operate = "ההודעה לא נשלחה.אנא בדוק סיסמא או הגדרות אבטחה של המייל", type = "danger" });

            }
            catch (Exception e)
            {

                return RedirectToAction("IndexPatients", "Patient", new { operate = "ההודעה לא נשלחה.אנא בדוק סיסמא או הגדרות אבטחה של המייל", type = "danger" });

            }
        }



        public ActionResult IndexAll(MailModel _objModelMail)
        {
            tblTherapist ther = (tblTherapist)Session["Therapist"];
            string email = ther.TherapistMail;
                                
            EliManagerDB db = new EliManagerDB();
            
            if (_objModelMail.redirect == "therapist")
            {
                try
                {
                    List<tblTherapist> therapist = db.Therapist.ToList();
                    List<tblTherapist> therapistCheck = db.getNotAdminTherepist();

                    if (therapistCheck.Count() == 0)
                    {
                        return RedirectToAction("IndexTherapist", "Patient", new { operate = "אין מטופלים במערכת-הודעה לא נשלחה", type = "danger" });

                    }
                    for (int i = 0; i < therapist.Count(); i++)
                    {
                        _objModelMail.From = _objModelMail.From;
                        MailMessage mail = new MailMessage();
                        mail.To.Add(therapist.ElementAt(i).TherapistMail);
                        // mail.From = new MailAddress(_objModelMail.From);  no need for this line!!!!
                        mail.Subject = _objModelMail.Subject + " מאת :" + _objModelMail.From;
                        string Body = _objModelMail.Body;
                        mail.Body = Body;
                        mail.IsBodyHtml = false;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential
                         (email, _objModelMail.MailPassword);// Enter seders User name and password
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }

                    return RedirectToAction("IndexTherapist", "Therapist",new { operate = "ההודעה נשלחה בהצלחה", type = "success" });
                }
                catch (SmtpException e)
                {
                    return RedirectToAction("IndexTherapist", "Therapist", new { operate = "ההודעה לא נשלחה.אנא בדוק סיסמא או הגדרות אבטחה של המייל", type = "danger" });

                }
                catch (Exception e)
                {
                    return RedirectToAction("IndexTherapist", "Therapist", new { operate = "ההודעה לא נשלחה.אנא בדוק סיסמא או הגדרות אבטחה של המייל", type = "danger" });

                }
                
            }
             if (_objModelMail.redirect == "finance")
            {
                List<tblFinancingFactor> finance = db.FinancingFactor.ToList();

                if (finance.Count() == 0)
                {
                    return RedirectToAction("IndexFinancingFactor", "Patient", new { operate = "אין מטופלים במערכת-הודעה לא נשלחה", type = "danger" });

                }
                try
                {
                    for (int i = 0; i < finance.Count(); i++)
                    {
                        _objModelMail.From = _objModelMail.From;
                        MailMessage mail = new MailMessage();
                        mail.To.Add(finance.ElementAt(i).FinancingFactorContactMail);
                        // mail.From = new MailAddress(_objModelMail.From);  no need for this line!!!!
                        mail.Subject = _objModelMail.Subject + " מאת :" + _objModelMail.From;
                        string Body = _objModelMail.Body;
                        mail.Body = Body;
                        mail.IsBodyHtml = false;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential
                         (email, _objModelMail.MailPassword);// Enter seders User name and password
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }

                    return RedirectToAction("IndexFinancingFactor", "FinanceFactor", new { operate = "ההודעה נשלחה בהצלחה", type = "success" });
                }
                catch (SmtpException e)
                {
                    return RedirectToAction("IndexFinancingFactor", "FinanceFactor", new { operate = "ההודעה לא נשלחה.אנא בדוק סיסמא או הגדרות אבטחה של המייל", type = "danger" });

                }
                catch (Exception e)
                {
                    return RedirectToAction("IndexFinancingFactor", "FinanceFactor", new { operate = "ההודעה לא נשלחה.אנא בדוק סיסמא או הגדרות אבטחה של המייל", type = "danger" });

                }

            }
            if (_objModelMail.redirect == "patient")
            {
                try
                {
                    List<tblParent> parents = db.Parent.ToList();
                    if (parents.Count() == 0)
                    {
                        return RedirectToAction("IndexPatients", "Patient", new { operate = "אין מטופלים במערכת-הודעה לא נשלחה", type = "danger" });

                    }
                    for (int i = 0; i < parents.Count(); i++)
                    {
                        _objModelMail.From = _objModelMail.From;
                        MailMessage mail = new MailMessage();
                        mail.To.Add(parents.ElementAt(i).ParentMail);
                        // mail.From = new MailAddress(_objModelMail.From);  no need for this line!!!!
                        mail.Subject = _objModelMail.Subject + " מאת :" + _objModelMail.From;
                        string Body = _objModelMail.Body;
                        mail.Body = Body;
                        mail.IsBodyHtml = false;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential
                         (email, _objModelMail.MailPassword);// Enter seders User name and password
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }

                    return RedirectToAction("IndexPatients", "Patient", new { operate = "ההודעה נשלחה בהצלחה", type = "success" });
                }
                catch (SmtpException e)
                {
                    return RedirectToAction("IndexPatients", "Patient", new { operate = "ההודעה לא נשלחה.אנא בדוק סיסמא או הגדרות אבטחה של המייל", type = "danger" });

                }
                catch (Exception e)
                {
                    return RedirectToAction("IndexPatients", "Patient", new { operate = "ההודעה לא נשלחה.אנא בדוק סיסמא או הגדרות אבטחה של המייל", type = "danger" });

                }

            }
            
            
            return RedirectToAction("IndexFinancingFactor", "FinanceFactor");

        }
       
    }
}
