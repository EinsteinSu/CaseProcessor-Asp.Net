namespace CaseProcessor.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changebacklognumbermaxlength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CaseRelatedBacklogs", "BacklogNumber", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CaseRelatedBacklogs", "BacklogNumber", c => c.String(maxLength: 10));
        }
    }
}
