using System.Collections.Generic;
using System.Linq;
using CaseProcessor.DataAccess;
using CaseProcessor.DataAccess.Models;
using log4net;

namespace CaseProcessor.Business
{
    public class DeveloperMgr : IDeveloperMgr
    {
        private static readonly ILog Logger = LogManager.GetLogger("DeveloperMgr");
        private readonly CaseProcessorDataContext _context = new CaseProcessorDataContext();
        public IEnumerable<Developer> GetDevelopers()
        {
            return _context.Developers.ToList();
        }
    }
}