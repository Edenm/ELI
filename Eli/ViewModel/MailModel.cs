using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eli.Models
{
    public class MailModel
    {
         [Required(ErrorMessage = "שדה חובה")]
        public string From { get; set; }

         [Required(ErrorMessage = "שדה חובה")]

         [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "אנא הכנס כתובת מייל תקינה")]
        public string To { get; set; }

         [Required(ErrorMessage = "שדה חובה")]
        public string Subject { get; set; }

         [Required(ErrorMessage = "שדה חובה")]
        public string Body { get; set; }

         [Required(ErrorMessage = "שדה חובה")]
        public string id { get; set; }

         public string patientId { get; set; }
         public string redirect { get; set; }
         public string name { get; set; }
    }
}