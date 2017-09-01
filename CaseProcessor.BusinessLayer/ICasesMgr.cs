using System.Collections.Generic;
using CaseProcessor.DataAccess.Models;

namespace CaseProcessor.Business
{
    public interface ICasesMgr
    {
        IEnumerable<Case> GetAllCases(int query);

        Case GetCaseById(int id);

        Case Add(Case c);

        Case Update(Case c);

        bool Delete(int id);

        IEnumerable<ToDo> GetCaseToDos(int caseId);

        void CreateToDo(ToDo todo);

        void UpdateToDo(ToDo todo);

        void DeleteToDo(int id);

        IEnumerable<Environment> GetCaseEnvironments(int caseId);

        void CreateEnvironment(Environment environment);

        void UpdateEnvironment(Environment environment);

        void DeleteEnvironment(int id);

        IEnumerable<Activity> GetCaseActivities(int caseId);

        void CreateActivity(Activity activity);

        void UpdateActivity(Activity activity);

        void DeleteActivity(int id);

        Backlog GetCaseBacklog(int caseId);

        void UpdateBacklog(Backlog backlog);

        Closed GetCaseClosed(int caseId);

        void UpdateClosed(Closed closed);
    }
}