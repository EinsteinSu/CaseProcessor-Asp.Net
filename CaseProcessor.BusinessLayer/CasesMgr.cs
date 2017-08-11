using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CaseProcessor.DataAccess;
using CaseProcessor.DataAccess.Models;
using log4net;

namespace CaseProcessor.Business
{
    public class CasesMgr : ICasesMgr
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
                        .Where(w => w.Status != CaseStatus.Closed);
                case 2:
                    return _context.Cases.Include(i => i.Developer)
                       .Include(i => i.Environments)
                       .Include(i => i.ToDoList)
                       .Include(i => i.Backlog)
                       .Include(i => i.Tags)
                       .Include(i => i.Closed)
                       .Include(i => i.Activities)
                        .Where(w => w.Status != CaseStatus.Closed).ToList()
                       .Where(w => !string.IsNullOrEmpty(w.CurrentToDo));
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
                return null;
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
                return false;
            }
        }

        public Closed GetClosed(int caseId)
        {
            Closed close;
            if (!_context.Closeds.Any(c => c.CaseId == caseId))
            {
                close = new Closed();
                _context.Closeds.Add(close);
                _context.SaveChanges();
            }
            else
            {
                close = _context.Closeds.Find(caseId);
            }
            return close;
        }
    }
}