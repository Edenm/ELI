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

        /** The method get the login screen */
        [HttpGet]
        public ActionResult IndexLogin()
        {
            return View(new User());
        }

        /** The method registred the user to system and to session */
        [HttpPost]
        public ActionResult IndexLogin(User user)
        {
            EliManagerDB db= new EliManagerDB();
            if (ModelState.IsValid)
            {
                tblTherapist ther= db.isUserValid(user);

                if (ther != null)
                {
                    Session["Therapist"] = ther;
                    FormsAuthentication.SetAuthCookie(user.UserName, false);
                  
                    return RedirectToAction("HomePage", "Login");
                }
            }
            ViewBag.type = "danger";
            ViewBag.operate = "שם המשתמש או סיסמה אינם תקינים";
            return View(new User());
        }

        /** The method transfer HomePage screen */
        [Authorize]
        public ActionResult HomePage()
        {
            return View();
        }

        /** The method log out from system */
        public ActionResult LogOut(){
            WebSecurity.Logout();
            return RedirectToAction("IndexLogin", "Login");
        }

        /** The method transfer ForgetPassword screen */
        [HttpGet]
        public ActionResult ForgetPassword()
        {
            return View();
        }

        /** The method get mail from ForgetPassword and send mail to user with his mail */
        [HttpPost]
        public ActionResult ForgetPassword(FormCollection mail)
        {
            string mailToSend=mail.GetValues("mail")[0];
            if (mailToSend.Length==0)
            {
                ViewBag.type = "danger";
                ViewBag.operate = "אנא הקלד מייל לשליחת סיסמא";
                return View();
            }
            
            try
            {
                MailAddress m = new MailAddress(mailToSend);

               
            }
            catch (FormatException)
            {
                ViewBag.type = "danger";
                ViewBag.operate = "אנא הקלד כתובת מייל חוקית";
                return View();
            }

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
                    ("otzmotnoreply@gmail.com", "shahareden");// Enter seders User name and password
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
