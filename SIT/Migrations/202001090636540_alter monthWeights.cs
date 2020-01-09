namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class altermonthWeights : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MonthWeights", "MonthDays", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MonthWeights", "MonthDays");
        }
    }
}
