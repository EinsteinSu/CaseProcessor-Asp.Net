using System;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using CaseProcessor.DataAccess;
using CaseProcessor.DataAccess.Models;

namespace CaseProcessor.Website.Controllers
{
    public class ClosedsController : Controller
    {
        private readonly CaseProcessorDataContext db = new CaseProcessorDataContext();


        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var closed = db.Closeds.Find(id);
            if (closed == null)
            {
                db.Closeds.Add(new Closed {CaseId = id.Value,CloseTime = DateTime.Now});
                db.SaveChanges();
                closed = db.Closeds.Find(id);
            }
            return View(closed);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CaseId,CloseTime,RootCause")] Closed closed)
        {
            if (string.IsNullOrEmpty(closed.RootCause))
            {
                return View(closed);
            }
            if (ModelState.IsValid)
            {
                var c = db.Cases.Find(closed.CaseId);
                c.Status = CaseStatus.Closed;
                c.InternalStatus = InternalStatus.Done;
                db.Entry(c).Property(p => p.Status).IsModified = true;
                db.Entry(c).Property(p => p.InternalStatus).IsModified = true;
                db.Entry(closed).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home", new {id = closed.CaseId});
            }
            return View(closed);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}