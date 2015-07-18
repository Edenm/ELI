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
    public class TherapistController : Controller
    {
        [HttpGet]
        public ActionResult IndexTherapist(string operate, string type)
        {
            EliManagerDB db = new EliManagerDB();

            List<tblTherapist> therapist = db.Therapist.ToList();

            therapist.Add(new tblTherapist());

            ViewBag.operate = operate;
            ViewBag.type = type;
            return View(therapist);
        }


        [HttpPost]
        public ActionResult IndexTherapist(tblTherapist tt, string submit)
        {
                EliManagerDB db = new EliManagerDB();

                ViewBag.type = "success";
                try{
                    if (submit.Equals("צור")){

                        
                        MailMessage mail = new MailMessage();
                        mail.To.Add(tt.TherapistMail);
                        // mail.From = new MailAddress(_objModelMail.From);  no need for this line!!!!
                        mail.Subject = "נוספת בהצלחה למערכת";
                        string Body = "שלום רב נוספת בהצלחה למערכת";
                        mail.Body = Body;
                        mail.IsBodyHtml = false;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential
                        (tt.TherapistMail, tt.Passcode);// Enter seders User name and password
                        smtp.EnableSsl = true;
                        smtp.Send(mail);

                   
                            db.addTherapist(tt);
                            ViewBag.operate = "מטפל התווסף בהצלחה";
                    }
                    else{
                            db.EditTherapist(tt);
                            ViewBag.operate = "פרטי מטפל התעדכנו בהצלחה";
                    }
                 }

                catch (SmtpException e)
                {
                    ViewBag.operate = "סיסמא לא תואמת לכתובת מייל.המטפל לא נוסף";
                    ViewBag.type = "danger";
                }
                catch (Exception e){
                        ViewBag.operate = e.Message;
                        ViewBag.type = "danger";
                 }

                return RedirectToAction("IndexTherapist", new { operate = ViewBag.operate, type = ViewBag.type });
        }

    }

}
