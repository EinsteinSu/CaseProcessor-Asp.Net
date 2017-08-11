namespace CaseProcessor.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tagchangeforeignKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tags", "Case_CaseId", "dbo.Cases");
            DropIndex("dbo.Tags", new[] { "Case_CaseId" });
            RenameColumn(table: "dbo.Tags", name: "Case_CaseId", newName: "CaseId");
            AlterColumn("dbo.Tags", "CaseId", c => c.Int(nullable: false));
            CreateIndex("dbo.Tags", "CaseId");
            AddForeignKey("dbo.Tags", "CaseId", "dbo.Cases", "CaseId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tags", "CaseId", "dbo.Cases");
            DropIndex("dbo.Tags", new[] { "CaseId" });
            AlterColumn("dbo.Tags", "CaseId", c => c.Int());
            RenameColumn(table: "dbo.Tags", name: "CaseId", newName: "Case_CaseId");
            CreateIndex("dbo.Tags", "Case_CaseId");
            AddForeignKey("dbo.Tags", "Case_CaseId", "dbo.Cases", "CaseId");
        }
    }
}
