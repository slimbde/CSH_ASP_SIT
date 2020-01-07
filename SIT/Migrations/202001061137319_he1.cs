namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class he1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vacations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        Duration = c.Int(nullable: false),
                        Score = c.Single(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vacations", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Vacations", new[] { "UserId" });
            DropTable("dbo.Vacations");
        }
    }
}
