using Eli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eli.Controllers
{
    public class RazorController : Controller
    {
        //
        // GET: /Razor/

        public ActionResult Index()
        {
            EliManagerDB db = new EliManagerDB();

            List<tblTherapist> t = db.Therapist.ToList();
            return View(t);
        }

        public ActionResult pdf()
        {
            EliManagerDB db = new EliManagerDB();

            List<tblTherapist> t = db.Therapist.ToList();
            return new RazorPDF.PdfResult(t, "PDF");
        }

        //
        // GET: /Razor/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Razor/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Razor/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Razor/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Razor/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Razor/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Razor/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
