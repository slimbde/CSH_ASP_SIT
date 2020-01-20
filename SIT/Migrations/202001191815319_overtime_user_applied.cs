namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class overtime_user_applied : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Overtimes", "UsrAppliedId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Overtimes", "UsrAppliedId");
        }
    }
}
