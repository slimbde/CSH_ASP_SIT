namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class overtimes12 : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UsId)
                .Index(t => t.UsId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Overtimes", "UsId", "dbo.AspNetUsers");
            DropIndex("dbo.Overtimes", new[] { "UsId" });
            DropTable("dbo.Overtimes");
        }
    }
}
