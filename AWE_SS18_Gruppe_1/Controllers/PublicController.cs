using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AWE_SS18_Gruppe_1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AWE_SS18_Gruppe_1.Controllers
{
    public class PublicController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Public
        public ActionResult Index()
        {
            //hier nur freie und reservierte Thesen übergeben
            List<Thesis> thesisliste = new List<Thesis>();
            var thesisDb = db.ThesisDb.Include(t => t.Programme).Include(t => t.User);
            foreach (Thesis thesis in thesisDb)
            {
                if (thesis.Status.Equals(Models.Status.frei) || thesis.Status.Equals(Models.Status.reserviert))
                {
                    thesisliste.Add(thesis);
                }
            }

            return View(thesisliste);


        }

        // GET: Public/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thesis thesis = db.ThesisDb.Find(id);
            if (thesis == null)
            {
                return HttpNotFound();
            }
            return View(thesis);
        }
        
        

        // POST: Public/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Thesis thesis = db.ThesisDb.Find(id);
            db.ThesisDb.Remove(thesis);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
