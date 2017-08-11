using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using CaseProcessor.DataAccess;
using CaseProcessor.DataAccess.Models;

namespace CaseProcessor.Website.Controllers
{
    public class ToDosController : Controller
    {
        private readonly CaseProcessorDataContext db = new CaseProcessorDataContext();

        // GET: ToDos
        public ActionResult Index()
        {
            return View();
        }

       
        // GET: ToDos/Create
        public ActionResult Create(int caseId)
        {
            var todo = new ToDo {CaseId = caseId};
            return View(todo);
        }

        // POST: ToDos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ToDoId,CaseId,Content,Done,Time")] ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                db.ToDoes.Add(toDo);
                db.SaveChanges();
                return RedirectToAction("EditCase", "Home", new {id = toDo.CaseId});
            }
            return RedirectToAction("EditCase", "Home", new {id = toDo.CaseId});
        }

        // GET: ToDos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var toDo = db.ToDoes.Find(id);
            if (toDo == null)
                return HttpNotFound();
            return View(toDo);
        }

        // POST: ToDos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ToDoId,CaseId,Content,Done,Time")] ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(toDo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("EditCase", "Home", new {id = toDo.CaseId});
            }
            return View(toDo);
        }

        // GET: ToDos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var toDo = db.ToDoes.Find(id);
            if (toDo == null)
                return HttpNotFound();
            return View(toDo);
        }

        // POST: ToDos/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var toDo = db.ToDoes.Find(id);
            db.ToDoes.Remove(toDo);
            db.SaveChanges();
            return RedirectToAction("EditCase", "Home", new {id = toDo.CaseId});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}