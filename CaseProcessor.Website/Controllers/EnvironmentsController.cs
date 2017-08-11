using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CaseProcessor.DataAccess;
using CaseProcessor.DataAccess.Models;

namespace CaseProcessor.Website.Controllers
{
    public class EnvironmentsController : Controller
    {
        private readonly CaseProcessorDataContext db = new CaseProcessorDataContext();

        // GET: Environments
        public ActionResult Index()
        {
            var environments = db.Environments.Include(e => e.Case);
            return View(environments.ToList());
        }

        // GET: Environments/Create
        public ActionResult Create(int caseId)
        {
            var environment = new Environment {CaseId = caseId, Type = EnvironmentType.Sql};
            return View(environment);
        }

        // POST: Environments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EnvironmentId,CaseId,Type,Value")] Environment environment)
        {
            if (ModelState.IsValid)
            {
                db.Environments.Add(environment);
                db.SaveChanges();
                return RedirectToAction("EditCase", "Home", new {id = environment.CaseId});
            }

            return RedirectToAction("EditCase", "Home", new {id = environment.CaseId});
        }

        // GET: Environments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var environment = db.Environments.Find(id);
            if (environment == null)
                return HttpNotFound();
            return View(environment);
        }

        // POST: Environments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EnvironmentId,CaseId,Type,Value")] Environment environment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(environment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("EditCase", "Home", new {id = environment.CaseId});
            }
            return RedirectToAction("EditCase", "Home", new {id = environment.CaseId});
        }

        // GET: Environments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var environment = db.Environments.Find(id);
            if (environment == null)
                return HttpNotFound();
            return View(environment);
        }

        // POST: Environments/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var environment = db.Environments.Find(id);
            db.Environments.Remove(environment);
            db.SaveChanges();
            return RedirectToAction("EditCase", "Home", new {id = environment.CaseId});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}