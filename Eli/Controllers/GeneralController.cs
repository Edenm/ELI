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
            ("otzmotnoreply@gmail.com", "shahareden");// Enter seders User name and password
            smtp.EnableSsl = true;
            smtp.Send(mail);
            
          if (_objModelMail.redirect=="therapist")
          {
            
              return RedirectToAction("IndexTherapist", "Therapist",new { operate = operate, type = type });
          }

          if (_objModelMail.redirect == "finance")
            {
                return RedirectToAction("IndexFinancingFactor", "FinanceFactor", new { operate = operate, type = type });
            }
            return RedirectToAction("IndexFinancingFactor", "FinanceFactor");

           

        }



       


        public ActionResult ParentsMail(MailModel _objModelMail)
        {

            EliManagerDB db = new EliManagerDB();
            List<tblParent> parents = db.Parent.ToList();

            if ((parents = db.getAllParentsByPatient(_objModelMail.patientId)) != null)
            {
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
                    ("otzmotnoreply@gmail.com", "shahareden");// Enter seders User name and password
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }

                

            }
            return RedirectToAction("IndexPatients", "Patient");
        }



        public ActionResult IndexAll(MailModel _objModelMail)
        {
            tblTherapist ther = (tblTherapist)Session["Therapist"];
            string email = ther.TherapistMail;
            string password = ther.Passcode;     
                                
            EliManagerDB db = new EliManagerDB();
            
            if (_objModelMail.redirect == "therapist")
            {
                List<tblTherapist> therapist = db.Therapist.ToList();
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
                    (email, password);// Enter seders User name and password
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }

                return RedirectToAction("IndexTherapist", "Therapist");
                
            }
             if (_objModelMail.redirect == "finance")
            {
                List<tblFinancingFactor> finance = db.FinancingFactor.ToList();
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
                    ("otzmotnoreply@gmail.com", "shahareden");// Enter seders User name and password
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }

                return RedirectToAction("IndexFinancingFactor", "FinanceFactor");

            }
            if (_objModelMail.redirect == "patient")
            {

                List<tblParent> parents = db.Parent.ToList();
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
                    ("otzmotnoreply@gmail.com", "shahareden");// Enter seders User name and password
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }

                return RedirectToAction("IndexPatients", "Patient");
            }
            
            
            return RedirectToAction("IndexFinancingFactor", "FinanceFactor");

        }
       
    }
}
