namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addvacationrating : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MonthWeights",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Score = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "VacationRating", c => c.Double());
            AddColumn("dbo.AspNetUsers", "CleaningRating", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "CleaningRating");
            DropColumn("dbo.AspNetUsers", "VacationRating");
            DropTable("dbo.MonthWeights");
        }
    }
}
