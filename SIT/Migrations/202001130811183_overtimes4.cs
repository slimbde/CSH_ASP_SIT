namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class overtimes4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Overtimes", "PersonId", "dbo.AspNetUsers");
            DropIndex("dbo.Overtimes", new[] { "PersonId" });
            DropTable("dbo.Overtimes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Overtimes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PersonId = c.String(maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        Duration = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Overtimes", "PersonId");
            AddForeignKey("dbo.Overtimes", "PersonId", "dbo.AspNetUsers", "Id");
        }
    }
}
