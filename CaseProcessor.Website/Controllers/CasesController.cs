using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CaseProcessor.DataAccess;
using CaseProcessor.DataAccess.Models;

namespace CaseProcessor.Website.Controllers
{
    public class CasesController : Controller
    {
        private CaseProcessorDataContext db = new CaseProcessorDataContext();

        // GET: Cases
        public ActionResult Index()
        {
            var cases = db.Cases;//.Include(@ => @.Backlog).Include(@ => @.Closed).Include(@ => @.Developer).Include(@ => @.KB);
            return View(cases.ToList());
        }

        // GET: Cases/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Case @case = db.Cases.Find(id);
            if (@case == null)
            {
                return HttpNotFound();
            }
            return View(@case);
        }

        // GET: Cases/Create
        public ActionResult Create()
        {
            ViewBag.CaseId = new SelectList(db.Backlogs, "CaseId", "BacklogNumber");
            ViewBag.CaseId = new SelectList(db.Closeds, "CaseId", "RootCause");
            ViewBag.DeveloperId = new SelectList(db.Developers, "DeveloperId", "Name");
            ViewBag.CaseId = new SelectList(db.KBs, "CaseId", "Url");
            return View();
        }

        // POST: Cases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CaseId,SrNumber,Level,Version,Customer,Subject,OpenDate,Component,Location,InternalStatus,Status,Owner,DeveloperId")] Case @case)
        {
            if (ModelState.IsValid)
            {
                db.Cases.Add(@case);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CaseId = new SelectList(db.Backlogs, "CaseId", "BacklogNumber", @case.CaseId);
            ViewBag.CaseId = new SelectList(db.Closeds, "CaseId", "RootCause", @case.CaseId);
            ViewBag.DeveloperId = new SelectList(db.Developers, "DeveloperId", "Name", @case.DeveloperId);
            ViewBag.CaseId = new SelectList(db.KBs, "CaseId", "Url", @case.CaseId);
            return View(@case);
        }

        // GET: Cases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Case @case = db.Cases.Find(id);
            if (@case == null)
            {
                return HttpNotFound();
            }
            ViewBag.CaseId = new SelectList(db.Backlogs, "CaseId", "BacklogNumber", @case.CaseId);
            ViewBag.CaseId = new SelectList(db.Closeds, "CaseId", "RootCause", @case.CaseId);
            ViewBag.DeveloperId = new SelectList(db.Developers, "DeveloperId", "Name", @case.DeveloperId);
            ViewBag.CaseId = new SelectList(db.KBs, "CaseId", "Url", @case.CaseId);
            return View(@case);
        }

        // POST: Cases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CaseId,SrNumber,Level,Version,Customer,Subject,OpenDate,Component,Location,InternalStatus,Status,Owner,DeveloperId")] Case @case)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@case).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CaseId = new SelectList(db.Backlogs, "CaseId", "BacklogNumber", @case.CaseId);
            ViewBag.CaseId = new SelectList(db.Closeds, "CaseId", "RootCause", @case.CaseId);
            ViewBag.DeveloperId = new SelectList(db.Developers, "DeveloperId", "Name", @case.DeveloperId);
            ViewBag.CaseId = new SelectList(db.KBs, "CaseId", "Url", @case.CaseId);
            return View(@case);
        }

        // GET: Cases/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Case @case = db.Cases.Find(id);
            if (@case == null)
            {
                return HttpNotFound();
            }
            return View(@case);
        }

        // POST: Cases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Case @case = db.Cases.Find(id);
            db.Cases.Remove(@case);
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
