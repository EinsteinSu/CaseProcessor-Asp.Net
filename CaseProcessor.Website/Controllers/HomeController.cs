using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CaseProcessor.Business;
using CaseProcessor.DataAccess.Models;

namespace CaseProcessor.Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICasesMgr _casesMgr;

        public HomeController(ICasesMgr casesMgr)
        {
            _casesMgr = casesMgr;
        }

        public HomeController()
        {
            _casesMgr = new CasesMgr();
        }

        public ActionResult Index(int? query, string sortOrder)
        {
            if (query == null)
            {
                query = 1;
            }
            ViewBag.Query = query;
            SetSortOrder(sortOrder);
            ViewBag.CurrentSort = sortOrder;
            var list = _casesMgr.GetAllCases(query.Value);
            switch (sortOrder)
            {
                case "SrNumber":
                    list = list.OrderBy(s => s.SrNumber);
                    break;
                case "SrNumber_desc":
                    list = list.OrderByDescending(s => s.SrNumber);
                    break;
                case "Level":
                    list = list.OrderBy(s => s.Level);
                    break;
                case "Level_desc":
                    list = list.OrderByDescending(s => s.Level);
                    break;
                case "Duration":
                    list = list.OrderBy(s => s.Duration);
                    break;
                case "Duration_desc":
                    list = list.OrderByDescending(s => s.Duration);
                    break;
                case "InternalStatus":
                    list = list.OrderBy(s => s.InternalStatus);
                    break;
                case "InternalStatus_desc":
                    list = list.OrderByDescending(s => s.InternalStatus);
                    break;
                case "CurrentToDo":
                    list = list.OrderBy(s => s.CurrentToDo);
                    break;
                case "CurrentToDo_desc":
                    list = list.OrderByDescending(s => s.CurrentToDo);
                    break;
                case "Status":
                    list = list.OrderBy(s => s.Status);
                    break;
                case "Status_desc":
                    list = list.OrderByDescending(s => s.Status);
                    break;
                case "Version":
                    list = list.OrderBy(s => s.Version);
                    break;
                case "Version_desc":
                    list = list.OrderByDescending(s => s.Version);
                    break;
                case "Component":
                    list = list.OrderBy(s => s.Component);
                    break;
                case "Component_desc":
                    list = list.OrderByDescending(s => s.Component);
                    break;
                case "Location":
                    list = list.OrderBy(s => s.Location);
                    break;
                case "Location_desc":
                    list = list.OrderByDescending(s => s.Location);
                    break;
                case "Customer":
                    list = list.OrderBy(s => s.Customer);
                    break;
                case "Customer_desc":
                    list = list.OrderByDescending(s => s.Customer);
                    break;
                case "Owner":
                    list = list.OrderBy(s => s.Owner);
                    break;
                case "Owner_desc":
                    list = list.OrderByDescending(s => s.Owner);
                    break;
                case "Developer":
                    list = list.OrderBy(s => s.Developer.Name);
                    break;
                case "Developer_desc":
                    list = list.OrderByDescending(s => s.Developer.Name);
                    break;
                default:
                    break;
            }
            return View(list);
        }

        private void SetSortOrder(string sortOrder)
        {
            ViewBag.SrNumberSortParam = sortOrder == "SrNumber" ? "SrNumber_desc" : "SrNumber";
            ViewBag.LevelSortParam = sortOrder == "Level" ? "Level_desc" : "Level";
            ViewBag.DurationSortParam = sortOrder == "Duration" ? "Duration_desc" : "Duration";
            ViewBag.InternalStatusSortParam = sortOrder == "InternalStatus" ? "InternalStatus_desc" : "InternalStatus";
            ViewBag.CurrentToDoSortParam = sortOrder == "CurrentToDo" ? "CurrentToDo_desc" : "CurrentToDo";
            ViewBag.StatusSortParam = sortOrder == "Status" ? "Status_desc" : "Status";
            ViewBag.VersionSortParam = sortOrder == "Version" ? "Version_desc" : "Version";
            ViewBag.ComponentSortParam = sortOrder == "Component" ? "Component_desc" : "Component";
            ViewBag.LocationSortParam = sortOrder == "Location" ? "Location_desc" : "Location";
            ViewBag.CustomerSortParam = sortOrder == "Customer" ? "Customer_desc" : "Customer";
            ViewBag.OwnerSortParam = sortOrder == "Owner" ? "Owner_desc" : "Owner";
            ViewBag.DeveloperSortParam = sortOrder == "Developer" ? "Developer_desc" : "Developer";
        }

       

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult EditCase(int id)
        {
            var model = _casesMgr.GetCaseById(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCase([Bind(Include = "CaseId,SrNumber,Level,Version,Customer,Subject,OpenDate,Component,Location,InternalStatus,Status,Owner,DeveloperId")] Case c)
        {
            if (ModelState.IsValid)
            {
                var updated = _casesMgr.Update(c);
                if (updated == null)
                {
                    ViewBag.Error = "Update error";
                    return View(c);
                }
                return RedirectToAction("Index");
            }
            return View(c);
        }

        public ActionResult CaseDetails(int id)
        {
            var model = _casesMgr.GetCaseById(id);
            var content = new ContentResult();
            content.Content = model.Subject;
            return content;
        }

        public ActionResult Create()
        {
            Case c = new Case { Backlog = new Backlog() };
            return View(c);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CaseId,SrNumber,Level,Version,Customer,Subject,OpenDate,Component,Location,InternalStatus,Status,Owner,DeveloperId")] Case @case)
        {
            if (ModelState.IsValid)
            {
                _casesMgr.Add(@case);
                return RedirectToAction("Index");
            }
            return View(@case);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Case @case = _casesMgr.GetCaseById(id.Value);
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
            _casesMgr.Delete(id);
            return RedirectToAction("Index");
        }
    }


}