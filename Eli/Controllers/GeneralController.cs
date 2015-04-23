﻿using Eli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Eli.Controllers
{
    public class GeneralController : Controller
    {
        


        [HttpPost]
        public ActionResult Index(MailModel _objModelMail)
        {
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
            ("margulis.shaharm@gmail.com", "smajrubh123");// Enter seders User name and password
            smtp.EnableSsl = true;
            smtp.Send(mail);

            return RedirectToAction("IndexFinancingFactor", "FinanceFactor");

        }
     

    }
}