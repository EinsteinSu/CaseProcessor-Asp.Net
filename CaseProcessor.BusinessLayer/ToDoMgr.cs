using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaseProcessor.DataAccess;
using CaseProcessor.DataAccess.Models;

namespace CaseProcessor.Business
{
    public interface IToDoMgr
    {
        IEnumerable<ToDo> GetToDoList();
    }
    public class ToDoMgr : IToDoMgr, IDisposable
    {
        private readonly CaseProcessorDataContext _context = new CaseProcessorDataContext();
        public IEnumerable<ToDo> GetToDoList()
        {
            return _context.ToDoes.Include(i => i.Case).Where(w => !w.Done);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
