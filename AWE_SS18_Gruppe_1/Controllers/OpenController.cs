﻿using System;
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
    public class OpenController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Open
        [Authorize(Roles = "Admin")]
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


        // GET: Open/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.ProgrammeID = new SelectList(db.ProgrammeDB, "Id", "Name");
            return View();
        }

        // POST: Open/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Bachelor,Master,Status,LastModified,ProgrammeID")] Thesis thesis)
        {
            Microsoft.AspNet.Identity.UserManager<ApplicationUser> user = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            thesis.User = user.FindById(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
                thesis.Status = Status.frei;
                thesis.LastModified = DateTime.Now;
              
                db.ThesisDb.Add(thesis);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProgrammeID = new SelectList(db.ProgrammeDB, "Id", "Name", thesis.ProgrammeID);
            return View(thesis);
        }

        // GET: Open/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            Microsoft.AspNet.Identity.UserManager<ApplicationUser> user = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            string user1 = user.FindById(User.Identity.GetUserId()).UserName;

            var user2 = db.ThesisDb.Find(id).User.UserName;
            if (user1 != user2)
            {
                throw new Exception("Es dürfen nur selbst angelegte Thesen geändert werden");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thesis thesis = db.ThesisDb.Find(id);
            if (thesis == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProgrammeID = new SelectList(db.ProgrammeDB, "Id", "Name", thesis.ProgrammeID);
            return View(thesis);
        }

        // POST: Open/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Bachelor,Master,Status,StudentName,StudentEmail,StudentID,Registration,LastModified,ProgrammeID")] Thesis thesis)
        {
            if (ModelState.IsValid)
            {
                thesis.LastModified = DateTime.Now;
                db.Entry(thesis).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProgrammeID = new SelectList(db.ProgrammeDB, "Id", "Name", thesis.ProgrammeID);
            return View(thesis);
        }

        // GET: Open/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            Microsoft.AspNet.Identity.UserManager<ApplicationUser> user = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            string user1 = user.FindById(User.Identity.GetUserId()).UserName;

            var user2 = db.ThesisDb.Find(id).User.UserName;
            if (user1 !=user2)
            {
                throw new Exception("Es dürfen nur selbst angelegte Thesen gelöscht werden");
            }
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

        // POST: Open/Delete/5
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
