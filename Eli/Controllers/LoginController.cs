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
                    //WebSecurity.CreateUserAndAccount(user.UserName, user.Password);
                    //WebSecurity.Login(user.UserName, user.Password,false);
                    FormsAuthentication.SetAuthCookie(user.UserName, false);
                    //FormsAuthentication.SetAuthCookie(user.userId.ToString(), false);
                    return RedirectToAction("IndexPatients", "Patient");
                }
            }
            ViewBag.type = "danger";
            ViewBag.operate = "שם המשתמש או סיסמה אינם תקינים";
            return View(new User());
        }

        public ActionResult LogOut(){
            WebSecurity.Logout();
            return RedirectToAction("IndexLogin", "Login");
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

            tblTherapist therapist;

            if ((therapist=db.getTherapistByMail(mailToSend))!=null){
                
                    MailMessage Mail = new MailMessage();

                    Mail.To.Add(mailToSend);
                    // mail.From = new MailAddress(_objModelMail.From);  no need for this line!!!!
                    Mail.Subject = "מאת ארגון עוצמות:שליחת סיסמא";
                    string Body = therapist.UserName + ":שם משתמש " +"\n"+ therapist.Passcode + ": סיסמא";
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

                    ViewBag.operate = "הודעת אישור עם פרטי ההזדהות נשלחה למייל שלך";
                    ViewBag.type = "success";
                    return View();
            }
            ViewBag.type = "danger";
            ViewBag.operate = "מצטערים אבל המייל אינו קיים במערכת";
            return View();
        }
    }
}
