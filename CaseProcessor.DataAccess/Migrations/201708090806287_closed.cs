namespace CaseProcessor.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class closed : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Closeds", "CloseTime", c => c.DateTime());
            AlterColumn("dbo.Closeds", "RootCause", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Closeds", "RootCause", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.Closeds", "CloseTime", c => c.DateTime(nullable: false));
        }
    }
}
