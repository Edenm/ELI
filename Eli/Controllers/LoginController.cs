using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eli.ViewModel;
using Eli.Models;


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
                    Session["therapistName"] = ther.TherapistFirstName +" " +ther.TherapistSurName;
                    Session["therapistId"] = ther.ID;
                    return RedirectToAction("IndexPatients", "Patient");
                }
            }

            return View(new User());
        }

    }
}
