namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class right3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Overtimes", "Duration", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Overtimes", "Duration");
        }
    }
}
