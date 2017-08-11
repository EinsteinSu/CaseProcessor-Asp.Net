using System.Data.Entity;
using CaseProcessor.DataAccess.Models;

namespace CaseProcessor.DataAccess
{
    public class CaseProcessorDataContext : DbContext
    {
        public CaseProcessorDataContext()
            : base("CaseProcessor")
        {
            Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }

        public IDbSet<Case> Cases { get; set; }

        public IDbSet<Developer> Developers { get; set; }

        public System.Data.Entity.DbSet<CaseProcessor.DataAccess.Models.Backlog> Backlogs { get; set; }

        public System.Data.Entity.DbSet<CaseProcessor.DataAccess.Models.Closed> Closeds { get; set; }

        public System.Data.Entity.DbSet<CaseProcessor.DataAccess.Models.KB> KBs { get; set; }

        public System.Data.Entity.DbSet<CaseProcessor.DataAccess.Models.ToDo> ToDoes { get; set; }

        public System.Data.Entity.DbSet<CaseProcessor.DataAccess.Models.Environment> Environments { get; set; }

        public System.Data.Entity.DbSet<CaseProcessor.DataAccess.Models.Activity> Activities { get; set; }
    }
}