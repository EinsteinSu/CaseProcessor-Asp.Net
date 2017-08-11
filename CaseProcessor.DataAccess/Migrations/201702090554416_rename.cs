namespace CaseProcessor.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rename : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CaseRelatedBacklogs", newName: "Backlogs");
            RenameTable(name: "dbo.CaseCloseds", newName: "Closeds");
            RenameTable(name: "dbo.CaseRelatedKBs", newName: "KBs");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.KBs", newName: "CaseRelatedKBs");
            RenameTable(name: "dbo.Closeds", newName: "CaseCloseds");
            RenameTable(name: "dbo.Backlogs", newName: "CaseRelatedBacklogs");
        }
    }
}
