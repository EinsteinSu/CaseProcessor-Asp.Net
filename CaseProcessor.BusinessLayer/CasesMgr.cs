using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CaseProcessor.DataAccess;
using CaseProcessor.DataAccess.Models;
using log4net;
using Environment = CaseProcessor.DataAccess.Models.Environment;

namespace CaseProcessor.Business
{
    public class CasesMgr : ICasesMgr, IDisposable
    {
        private static readonly ILog Logger = LogManager.GetLogger("CaseMgr");
        private readonly CaseProcessorDataContext _context = new CaseProcessorDataContext();

        public IEnumerable<Case> GetAllCases(int query)
        {
            switch (query)
            {
                case 1:
                    return _context.Cases.Include(i => i.Developer)
                        .Include(i => i.Environments)
                        .Include(i => i.ToDoList)
                        .Include(i => i.Backlog)
                        .Include(i => i.Tags)
                        .Include(i => i.Closed)
                        .Include(i => i.Activities)
                        .Where(w => w.Status != CaseStatus.Closed && w.Status != CaseStatus.EnhancementRequestCreated && w.Status != CaseStatus.DefectConfirmed && !w.CRTracking);
                case 2:
                    return _context.Cases.Include(i => i.Developer)
                       .Include(i => i.Environments)
                       .Include(i => i.ToDoList)
                       .Include(i => i.Backlog)
                       .Include(i => i.Tags)
                       .Include(i => i.Closed)
                       .Include(i => i.Activities)
                        .Where(w => w.Status != CaseStatus.Closed && !w.CRTracking).ToList()
                       .Where(w => !string.IsNullOrEmpty(w.CurrentToDo));
                case 3:
                    return _context.Cases.Include(i => i.Developer)
                        .Include(i => i.Environments)
                        .Include(i => i.ToDoList)
                        .Include(i => i.Backlog)
                        .Include(i => i.Tags)
                        .Include(i => i.Closed)
                        .Include(i => i.Activities)
                        .Where(w => w.Status == CaseStatus.FromSupport && !w.CRTracking);
                case 4:
                    return _context.Cases.Include(i => i.Developer)
                        .Include(i => i.Environments)
                        .Include(i => i.ToDoList)
                        .Include(i => i.Backlog)
                        .Include(i => i.Tags)
                        .Include(i => i.Closed)
                        .Include(i => i.Activities)
                        .Where(w => w.CRTracking);
                default:
                    return _context.Cases.Include(i => i.Developer)
                        .Include(i => i.Environments)
                        .Include(i => i.ToDoList)
                        .Include(i => i.Backlog)
                        .Include(i => i.Tags)
                        .Include(i => i.Closed)
                        .Include(i => i.Activities);
            }
        }

        public Case GetCaseById(int id)
        {
            return _context.Cases.Include(i => i.Environments)
                    .Include(i => i.ToDoList)
                    .Include(i => i.Backlog)
                    .Include(i => i.Tags)
                    .Include(i => i.Closed)
                    .Include(i => i.Activities).FirstOrDefault(f => f.CaseId == id);
        }

        public Case Add(Case c)
        {
            try
            {
                _context.Cases.Add(c);
                _context.Backlogs.Add(new Backlog { CaseId = c.CaseId });
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Logger.Error("Insert case failed.", e);
                return null;
            }

            return GetCaseById(c.CaseId);
        }

        public Case Update(Case c)
        {
            try
            {
                _context.Entry(c).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return GetCaseById(c.CaseId);
        }

        public bool Delete(int id)
        {
            var c = GetCaseById(id);
            if (c == null)
            {
                throw new KeyNotFoundException();
            }
            try
            {
                _context.Cases.Remove(c);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Logger.Error("Delete case fialed.", e);
                throw;
            }
        }

        public IEnumerable<ToDo> GetCaseToDos(int caseId)
        {
            var item = GetCaseById(caseId);
            return item?.ToDoList.OrderBy(o => o.Time);
        }

        public void CreateToDo(ToDo todo)
        {
            try
            {
                _context.ToDoes.Add(todo);
                _context.SaveChanges();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        public void UpdateToDo(ToDo todo)
        {
            try
            {
                _context.Entry(todo).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        public void DeleteToDo(int id)
        {
            try
            {
                var todo = _context.ToDoes.Find(id);
                if (todo != null)
                {
                    _context.ToDoes.Remove(todo);
                    _context.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        public IEnumerable<Environment> GetCaseEnvironments(int caseId)
        {
            var item = GetCaseById(caseId);
            return item?.Environments;
        }

        public void CreateEnvironment(Environment environment)
        {
            try
            {
                _context.Environments.Add(environment);
                _context.SaveChanges();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        public void UpdateEnvironment(Environment environment)
        {
            try
            {
                _context.Entry(environment).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        public void DeleteEnvironment(int id)
        {
            try
            {
                var environment = _context.Environments.Find(id);
                if (environment != null)
                {
                    _context.Environments.Remove(environment);
                    _context.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        public IEnumerable<Activity> GetCaseActivities(int caseId)
        {
            var item = GetCaseById(caseId);
            return item?.Activities.OrderBy(o => o.Time);
        }

        public void CreateActivity(Activity activity)
        {
           
            try
            {
                _context.Activities.Add(activity);
                _context.SaveChanges();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        public void UpdateActivity(Activity activity)
        {
            try
            {
                _context.Entry(activity).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        public void DeleteActivity(int id)
        {
            try
            {
                var activity = _context.Activities.Find(id);
                if (activity != null)
                {
                    _context.Activities.Remove(activity);
                    _context.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        public Backlog GetCaseBacklog(int caseId)
        {
            var item = GetCaseById(caseId);
            return item?.Backlog;
        }

        public void UpdateBacklog(Backlog backlog)
        {
            try
            {
                _context.Entry(backlog).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        public Closed GetCaseClosed(int caseId)
        {
            var item = GetCaseById(caseId);
            return item?.Closed;
        }

        public void UpdateClosed(Closed closed)
        {
            try
            {
                _context.Entry(closed).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        public Closed GetClosed(int caseId)
        {
            Closed close;
            if (!_context.Closeds.Any(c => c.CaseId == caseId))
            {
                close = new Closed {CaseId = caseId};
                _context.Closeds.Add(close);
                _context.SaveChanges();
            }
            else
            {
                close = _context.Closeds.Find(caseId);
            }
            return close;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}