namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class overtimes9 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Overtimes", "UsId", "dbo.AspNetUsers");
            DropIndex("dbo.Overtimes", new[] { "UsId" });
            DropTable("dbo.Overtimes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Overtimes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsId = c.String(maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        Duration = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Overtimes", "UsId");
            AddForeignKey("dbo.Overtimes", "UsId", "dbo.AspNetUsers", "Id");
        }
    }
}
