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
    public class FiledController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Filed
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            //hier nur abgegebene Thesen übergeben
            List<Thesis> thesisliste = new List<Thesis>();
            var thesisDb = db.ThesisDb.Include(t => t.Programme).Include(t => t.User);
            foreach (Thesis thesis in thesisDb)
            {
                if (thesis.Status.Equals(Models.Status.abgegeben))
                {
                    thesisliste.Add(thesis);
                }
            }

            return View(thesisliste);


        }


        // GET: Filed/Edit/5
        [Authorize(Roles = "Admin")]

        public ActionResult Edit(int? id)
        {
            Microsoft.AspNet.Identity.UserManager<ApplicationUser> user = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            string user1 = user.FindById(User.Identity.GetUserId()).UserName;

            var user2 = db.ThesisDb.Find(id).User.UserName;
            if (user1 != user2)
            {
                throw new Exception("Es dürfen nur selbst angelegte Thesen bearbeitet werden");
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

        // POST: Filed/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Bachelor,Master,Status,StudentName,StudentEmail,StudentID,Registration,Filing,Typ,Summary,Strenghts,Weaknesses,Evaluation,ContentVal,LayoutVal,StructureVal,StyleVal,LiteraturVal,DifficultyVal,NoveltyVal,RichnessVal,ContentWt,LayoutWt,StructureWt,StyleWt,LiteratureWt,DifficultyWt,NoveltyWt,RichnessWt,Grade,LastModified,ProgrammeID")] Thesis thesis)
        {
            thesis.LastModified = DateTime.Now;
            thesis.Status = Status.bewertet;
            if (ModelState.IsValid)
            {

               
                db.Entry(thesis).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProgrammeID = new SelectList(db.ProgrammeDB, "Id", "Name", thesis.ProgrammeID);
            return View(thesis);
        }
      

        // POST: Filed/Delete/5
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
