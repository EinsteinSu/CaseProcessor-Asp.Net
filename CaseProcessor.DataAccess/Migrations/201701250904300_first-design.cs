namespace CaseProcessor.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstdesign : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CaseRelatedBacklogs",
                c => new
                    {
                        CaseId = c.Int(nullable: false),
                        BacklogNumber = c.String(maxLength: 10),
                        Versions = c.String(maxLength: 100),
                        ETA = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.CaseId)
                .ForeignKey("dbo.Cases", t => t.CaseId)
                .Index(t => t.CaseId);
            
            CreateTable(
                "dbo.Cases",
                c => new
                    {
                        CaseId = c.Int(nullable: false, identity: true),
                        SrNumber = c.String(maxLength: 7),
                        Level = c.Int(nullable: false),
                        Version = c.String(maxLength: 10),
                        Customer = c.String(maxLength: 100),
                        Subject = c.String(maxLength: 200),
                        OpenDate = c.DateTime(),
                        Component = c.String(maxLength: 50),
                        Location = c.Int(nullable: false),
                        InternalStatus = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Owner = c.String(maxLength: 50),
                        DeveloperId = c.Int(),
                    })
                .PrimaryKey(t => t.CaseId)
                .ForeignKey("dbo.Developers", t => t.DeveloperId)
                .Index(t => t.DeveloperId);
            
            CreateTable(
                "dbo.CaseCloseds",
                c => new
                    {
                        CaseId = c.Int(nullable: false),
                        CloseTime = c.DateTime(nullable: false),
                        RootCause = c.String(nullable: false, maxLength: 500),
                    })
                .PrimaryKey(t => t.CaseId)
                .ForeignKey("dbo.Cases", t => t.CaseId)
                .Index(t => t.CaseId);
            
            CreateTable(
                "dbo.Developers",
                c => new
                    {
                        DeveloperId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.DeveloperId);
            
            CreateTable(
                "dbo.Environments",
                c => new
                    {
                        EnvironmentId = c.Int(nullable: false, identity: true),
                        CaseId = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.EnvironmentId)
                .ForeignKey("dbo.Cases", t => t.CaseId, cascadeDelete: true)
                .Index(t => t.CaseId);
            
            CreateTable(
                "dbo.CaseRelatedKBs",
                c => new
                    {
                        CaseId = c.Int(nullable: false),
                        Url = c.String(maxLength: 1024),
                    })
                .PrimaryKey(t => t.CaseId)
                .ForeignKey("dbo.Cases", t => t.CaseId)
                .Index(t => t.CaseId);
            
            CreateTable(
                "dbo.Remarks",
                c => new
                    {
                        RemarkId = c.Int(nullable: false, identity: true),
                        CaseId = c.Int(nullable: false),
                        Content = c.String(),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RemarkId)
                .ForeignKey("dbo.Cases", t => t.CaseId, cascadeDelete: true)
                .Index(t => t.CaseId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        TagId = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                        Case_CaseId = c.Int(),
                    })
                .PrimaryKey(t => t.TagId)
                .ForeignKey("dbo.Cases", t => t.Case_CaseId)
                .Index(t => t.Case_CaseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tags", "Case_CaseId", "dbo.Cases");
            DropForeignKey("dbo.Remarks", "CaseId", "dbo.Cases");
            DropForeignKey("dbo.CaseRelatedKBs", "CaseId", "dbo.Cases");
            DropForeignKey("dbo.Environments", "CaseId", "dbo.Cases");
            DropForeignKey("dbo.Cases", "DeveloperId", "dbo.Developers");
            DropForeignKey("dbo.CaseCloseds", "CaseId", "dbo.Cases");
            DropForeignKey("dbo.CaseRelatedBacklogs", "CaseId", "dbo.Cases");
            DropIndex("dbo.Tags", new[] { "Case_CaseId" });
            DropIndex("dbo.Remarks", new[] { "CaseId" });
            DropIndex("dbo.CaseRelatedKBs", new[] { "CaseId" });
            DropIndex("dbo.Environments", new[] { "CaseId" });
            DropIndex("dbo.CaseCloseds", new[] { "CaseId" });
            DropIndex("dbo.Cases", new[] { "DeveloperId" });
            DropIndex("dbo.CaseRelatedBacklogs", new[] { "CaseId" });
            DropTable("dbo.Tags");
            DropTable("dbo.Remarks");
            DropTable("dbo.CaseRelatedKBs");
            DropTable("dbo.Environments");
            DropTable("dbo.Developers");
            DropTable("dbo.CaseCloseds");
            DropTable("dbo.Cases");
            DropTable("dbo.CaseRelatedBacklogs");
        }
    }
}
