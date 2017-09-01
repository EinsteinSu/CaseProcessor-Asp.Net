using System;
using System.Linq;
using System.Web.Mvc;
using CaseProcessor.Business;
using CaseProcessor.DataAccess.Models;
using Environment = CaseProcessor.DataAccess.Models.Environment;

namespace CaseProcessor.Website.DevExpress.Controllers
{
    public class HomeController : Controller
    {
        private readonly CasesMgr _mgr = new CasesMgr();

        public ActionResult Index()
        {
            return View();
        }


        #region Cases


        [ValidateInput(false)]
        public ActionResult GridViewPartial(int? query)
        {
            ViewData["query"] = query;
            return PartialView("_GridViewPartial", _mgr.GetAllCases(query ?? 1).ToList());
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GridViewPartialAddNew(Case item, int? query)
        {
            ViewData["query"] = query;
            if (ModelState.IsValid)
                try
                {
                    _mgr.Add(item);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_GridViewPartial", _mgr.GetAllCases(query ?? 1).ToList());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GridViewPartialUpdate(Case item, int? query)
        {
            ViewData["query"] = query;
            if (ModelState.IsValid)
                try
                {
                    _mgr.Update(item);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_GridViewPartial", _mgr.GetAllCases(query ?? 1).ToList());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GridViewPartialDelete(int id, int? query)
        {
            ViewData["query"] = query;
            if (id >= 0)
                try
                {
                    _mgr.Delete(id);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            return PartialView("_GridViewPartial", _mgr.GetAllCases(query ?? 1).ToList());
        }

        #endregion

        #region Todos

        [ValidateInput(false)]
        public ActionResult ToDoGridViewPartial(string caseId)
        {
            ViewData["CaseId"] = caseId;
            return PartialView("_ToDoGridViewPartial", _mgr.GetCaseToDos(int.Parse(caseId)));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ToDoGridViewPartialAddNew(ToDo item, int caseId)
        {
            ViewData["CaseId"] = caseId;
            item.CaseId = caseId;
            if (ModelState.IsValid)
                try
                {
                    _mgr.CreateToDo(item);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_ToDoGridViewPartial", _mgr.GetCaseToDos(caseId));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ToDoGridViewPartialUpdate(ToDo item, int caseId)
        {
            ViewData["CaseId"] = caseId;
            if (ModelState.IsValid)
                try
                {
                    _mgr.UpdateToDo(item);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_ToDoGridViewPartial", _mgr.GetCaseToDos(caseId));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ToDoGridViewPartialDelete(int toDoId, int caseId)
        {
            ViewData["CaseId"] = caseId;
            if (toDoId >= 0)
                try
                {
                    _mgr.DeleteToDo(toDoId);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            return PartialView("_ToDoGridViewPartial", _mgr.GetCaseToDos(caseId));
        }

        #endregion

        #region activities

        [ValidateInput(false)]
        public ActionResult ActivityGridViewPartial(int caseId)
        {
            ViewData["CaseId"] = caseId;
            return PartialView("_ActivityGridViewPartial", _mgr.GetCaseActivities(caseId));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ActivityGridViewPartialAddNew(Activity item, int caseId)
        {
            ViewData["CaseId"] = caseId;
            item.CaseId = caseId;
            item.Time = DateTime.Now;
            if (ModelState.IsValid)
                try
                {
                    _mgr.CreateActivity(item);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_ActivityGridViewPartial", _mgr.GetCaseActivities(caseId));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ActivityGridViewPartialUpdate(Activity item, int caseId)
        {
            ViewData["CaseId"] = caseId;
            if (ModelState.IsValid)
                try
                {
                    _mgr.UpdateActivity(item);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_ActivityGridViewPartial", _mgr.GetCaseActivities(caseId));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ActivityGridViewPartialDelete(int activityId, int caseId)
        {
            ViewData["CaseId"] = caseId;
            if (activityId >= 0)
                try
                {
                    _mgr.DeleteActivity(activityId);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            return PartialView("_ActivityGridViewPartial", _mgr.GetCaseActivities(caseId));
        }

        #endregion

        #region Environment

        [ValidateInput(false)]
        public ActionResult EnvironmentGridViewPartial(string caseId)
        {
            ViewData["CaseId"] = caseId;
            return PartialView("_EnvironmentGridViewPartial", _mgr.GetCaseEnvironments(int.Parse(caseId)));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EnvironmentGridViewPartialAddNew(Environment item, int caseId)
        {
            ViewData["CaseId"] = caseId;
            item.CaseId = caseId;
            if (ModelState.IsValid)
                try
                {
                    _mgr.CreateEnvironment(item);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_EnvironmentGridViewPartial", _mgr.GetCaseEnvironments(caseId));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EnvironmentGridViewPartialUpdate(Environment item, int caseId)
        {
            ViewData["CaseId"] = caseId;
            if (ModelState.IsValid)
                try
                {
                    _mgr.UpdateEnvironment(item);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_EnvironmentGridViewPartial", _mgr.GetCaseEnvironments(caseId));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EnvironmentGridViewPartialDelete(int environmentId, int caseId)
        {
            ViewData["CaseId"] = caseId;
            if (environmentId >= 0)
                try
                {
                    _mgr.DeleteEnvironment(environmentId);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            return PartialView("_EnvironmentGridViewPartial", _mgr.GetCaseEnvironments(caseId));
        }

        #endregion

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult BacklogEdit(Backlog item)
        {
            return null;
        }

        public ActionResult Backlog(int caseId,int query)
        {
            var backlog = _mgr.GetCaseBacklog(caseId);
            return View(backlog);
        }

        public ActionResult Close(int caseId, int query)
        {
            var closed = _mgr.GetClosed(caseId);
            return View(closed);
        }
    }
}