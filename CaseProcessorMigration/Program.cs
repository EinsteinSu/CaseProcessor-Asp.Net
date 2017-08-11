using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaseProcesser.Common;
using CaseProcesser.Models;
using CaseProcessor.DataAccess;
using CaseProcessor.DataAccess.Models;
using Case = CaseProcessor.DataAccess.Models.Case;
using CaseStatus = CaseProcessor.DataAccess.Models.CaseStatus;
using InternalStatus = CaseProcessor.DataAccess.Models.InternalStatus;

namespace CaseProcessorMigration
{
    class Program
    {
        static readonly CaseProcesser.Common.CaseContext original = new CaseContext();
        static readonly CaseProcessor.DataAccess.CaseProcessorDataContext current = new CaseProcessorDataContext();
        static void Main(string[] args)
        {
            //current.Database.Log = Console.Write;
            InserDevelopers();
            foreach (var c in original.GetCases())
            {
                CaseProcessor.DataAccess.Models.Case cc = new Case();
                cc.SrNumber = c.CRNumber;
                cc.Level = ConvertCaseLevel(c.Level);
                cc.Version = c.AMVersion;
                cc.Subject = c.Subject;
                cc.Environments = ConvertEnvironment(c.Environment);
                cc.Component = c.Component;
                cc.Status = (CaseStatus)c.Status;
                cc.Owner = c.Owner;
                cc.Customer = c.Customer;
                cc.Backlog = ConvertBacklog(c);
                cc.InternalStatus = (InternalStatus)c.InternalStatus;
                cc.OpenDate = c.OpenDate;
                cc.DeveloperId = c.DeveloperId;
                cc.Closed = ConvertCaseClosed(c.CaseId);
                cc.Activities = ConvertActivities(c);
                cc.ToDoList = ConvertToDoes(c);
                cc.Location = (CaseProcessor.DataAccess.Models.Location) c.Location;
                current.Cases.Add(cc);
            }
            try
            {
                current.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

        }

        static IList<CaseProcessor.DataAccess.Models.ToDo> ConvertToDoes(CaseProcesser.Models.Case c)
        {
            var list = new List<CaseProcessor.DataAccess.Models.ToDo>();
            foreach (var toDo in c.ToDoList)
            {
                var td = new CaseProcessor.DataAccess.Models.ToDo();
                td.Time = toDo.Time;
                td.Content = toDo.Content;
                td.Done = toDo.Done;
                list.Add(td);
            }
            return list;
        } 

        static IList<CaseProcessor.DataAccess.Models.Activity> ConvertActivities(CaseProcesser.Models.Case c)
        {
            var list = new List<CaseProcessor.DataAccess.Models.Activity>();
            foreach (var a1 in c.Activities)
            {
                Console.WriteLine(c.Activities.Count.ToString());
                CaseProcessor.DataAccess.Models.Activity a = new CaseProcessor.DataAccess.Models.Activity
                {
                    Time = a1.ActiveDate,
                    Content = a1.Description
                };
                list.Add(a);
            }
            return list;

        }

        static void InserDevelopers()
        {
            foreach (var developer in original.Developers)
            {
                current.Developers.Add(new CaseProcessor.DataAccess.Models.Developer { Name = developer.Name });
            }
            current.SaveChanges();
        }

        static CaseProcessor.DataAccess.Models.Closed ConvertCaseClosed(int caseId)
        {
            var c = original.CaseCloseds.FirstOrDefault(f => f.SubcaseId == caseId);
            if (c != null)
            {
                var c1 = new CaseProcessor.DataAccess.Models.Closed();
                c1.CloseTime = c.CloseTime;
                c1.RootCause = "Migrated, Fix it at next time";
                return c1;
            }
            return null;
        }

        static Backlog ConvertBacklog(CaseProcesser.Models.Case c)
        {
            Backlog backlog = new Backlog();
            backlog.BacklogNumber = c.Hotfix.BugId;
            backlog.ETA = c.Hotfix.EtaTime;
            backlog.Versions = c.Hotfix.Versions;
            return backlog;
        }

        static int ConvertCaseLevel(CaseLevel level)
        {
            switch (level)
            {
                case CaseLevel.Level1:
                    return 1;
                case CaseLevel.Level2:
                    return 2;
                case CaseLevel.Level3:
                    return 3;
                case CaseLevel.Level4:
                    return 4;
                default:
                    return 0;
            }
        }

        static List<CaseProcessor.DataAccess.Models.Environment> ConvertEnvironment(CaseProcesser.Models.Environment environment)
        {
            var en = new List<CaseProcessor.DataAccess.Models.Environment>();
            if (environment.ExchangeVersion != null)
            {
                en.Add(new CaseProcessor.DataAccess.Models.Environment { Type = EnvironmentType.Exchange, Value = environment.ExchangeVersion });
            }
            if (environment.SqlServerVersion != null)
            {
                en.Add(new CaseProcessor.DataAccess.Models.Environment { Type = EnvironmentType.Sql, Value = environment.SqlServerVersion });
            }
            if (environment.MApi != null)
            {
                en.Add(new CaseProcessor.DataAccess.Models.Environment { Type = EnvironmentType.MapiCdo, Value = environment.MApi });
            }
            if (environment.OSVersion != null)
            {
                en.Add(new CaseProcessor.DataAccess.Models.Environment { Type = EnvironmentType.OS, Value = environment.OSVersion });
            }
            return en;
        }
    }
}
