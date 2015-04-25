using Eli.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eli.ViewModel
{
    public class User
    {

        private string userName;

        private string password;

        [Required(ErrorMessage = "שדה חובה")]
        public string UserName
        {
            
            get { return userName; }
            set { userName = value; }
        }

        [Required(ErrorMessage = "שדה חובה")]
        public string Password
        {

            get { return password; }
            set { password = value; }
        }


    }
}
