namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class overtime : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Overtimes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsrId = c.String(maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        Duration = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UsrId)
                .Index(t => t.UsrId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Overtimes", "UsrId", "dbo.AspNetUsers");
            DropIndex("dbo.Overtimes", new[] { "UsrId" });
            DropTable("dbo.Overtimes");
        }
    }
}
