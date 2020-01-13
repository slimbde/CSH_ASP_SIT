namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class right2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Overtimes", "Duration");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Overtimes", "Duration", c => c.Int(nullable: false));
        }
    }
}
