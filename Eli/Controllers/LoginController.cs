using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eli.ViewModel;
using Eli.Models;
using System.Web.Security;
using WebMatrix.WebData;


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
                    FormsAuthentication.SetAuthCookie(user.UserName, true);
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

            return View();
        }
    }
}
