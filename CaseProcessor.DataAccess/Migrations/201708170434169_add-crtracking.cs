namespace CaseProcessor.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcrtracking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cases", "CRTracking", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cases", "CRTracking");
        }
    }
}
