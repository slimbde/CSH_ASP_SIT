namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class voting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Votings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsrId = c.String(maxLength: 128),
                        VacationRating = c.Double(nullable: false),
                        Voted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UsrId)
                .Index(t => t.UsrId);
            
            DropColumn("dbo.AspNetUsers", "VacationRating");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "VacationRating", c => c.Double());
            DropForeignKey("dbo.Votings", "UsrId", "dbo.AspNetUsers");
            DropIndex("dbo.Votings", new[] { "UsrId" });
            DropTable("dbo.Votings");
        }
    }
}
