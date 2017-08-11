namespace CaseProcessor.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class todo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Remarks", "CaseId", "dbo.Cases");
            DropIndex("dbo.Remarks", new[] { "CaseId" });
            CreateTable(
                "dbo.ToDoes",
                c => new
                    {
                        ToDoId = c.Int(nullable: false, identity: true),
                        CaseId = c.Int(nullable: false),
                        Content = c.String(),
                        Done = c.Boolean(nullable: false),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ToDoId)
                .ForeignKey("dbo.Cases", t => t.CaseId, cascadeDelete: true)
                .Index(t => t.CaseId);
            
            DropTable("dbo.Remarks");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Remarks",
                c => new
                    {
                        RemarkId = c.Int(nullable: false, identity: true),
                        CaseId = c.Int(nullable: false),
                        Content = c.String(),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RemarkId);
            
            DropForeignKey("dbo.ToDoes", "CaseId", "dbo.Cases");
            DropIndex("dbo.ToDoes", new[] { "CaseId" });
            DropTable("dbo.ToDoes");
            CreateIndex("dbo.Remarks", "CaseId");
            AddForeignKey("dbo.Remarks", "CaseId", "dbo.Cases", "CaseId", cascadeDelete: true);
        }
    }
}
