using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CaseProcessor.DataAccess.Models;

namespace CaseProcessor.Business
{
    public interface IDeveloperMgr
    {
        IEnumerable<Developer> GetDevelopers();
    }
}
