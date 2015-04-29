using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eli.Controllers
{
    [Authorize]
    public class ScheduleController : Controller
    {
        //
        // GET: /Schedule/

        public ActionResult IndexSchedule()
        {
            return View();
        }

    }
}
