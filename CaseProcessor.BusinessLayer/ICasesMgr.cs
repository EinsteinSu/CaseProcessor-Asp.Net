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
    }
}