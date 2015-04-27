using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eli.ViewModel;
using Eli.Models;
using System.Web.Security;
using WebMatrix.WebData;
using System.Net.Mail;


namespace Eli.Controllers
{
    public class LoginController : Controller
    {

        [HttpGet]
        public ActionResult IndexLogin()
        {
            return View(new User());
        }

        [HttpPost]
        public ActionResult IndexLogin(User user )
        {
            EliManagerDB db= new EliManagerDB();
            if (ModelState.IsValid)
            {
                tblTherapist ther= db.isUserValid(user);

                if (ther != null)
                {
                    Session["Therapist"] = ther;
                    //WebSecurity.Login(user.UserName, user.Password);
                    //FormsAuthentication.SetAuthCookie(user.UserName, true);
                    return RedirectToAction("IndexPatients", "Patient");
                }
            }

            return View(new User());
        }

        [HttpGet]
        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPassword(FormCollection mail)
        {
            string mailToSend=mail.GetValues("mail")[0];

            EliManagerDB db = new EliManagerDB();

            List<tblTherapist> therapist = db.Therapist.ToList();
            for (int i = 0; i < therapist.Count();i++ )
            {
                MailMessage Mail = new MailMessage();
                if(therapist.ElementAt(i).TherapistMail==mailToSend)
                {


                    Mail.To.Add(mailToSend);
                    // mail.From = new MailAddress(_objModelMail.From);  no need for this line!!!!
                    Mail.Subject = "מאת ארגון עוצמות:שליחת סיסמא";
                    string Body = therapist.ElementAt(i).UserName + ":שם משתמש " +"\n"+ therapist.ElementAt(i).Passcode + ": סיסמא";
                    Mail.Body = Body;
                    Mail.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential
                    ("margulis.shaharm@gmail.com", "smajrubh123");// Enter seders User name and password
                    smtp.EnableSsl = true;
                    smtp.Send(Mail);



                    return RedirectToAction("IndexLogin", "Login");
                    
                }
            }
            return RedirectToAction("ForgetPassword", "Login");
        }
    }
}
