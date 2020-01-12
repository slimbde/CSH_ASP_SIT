namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lab1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Labours",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationUserLabours",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Labour_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Labour_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Labours", t => t.Labour_Id, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Labour_Id);
            
            AddColumn("dbo.AspNetUsers", "ParticipateInLabour", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationUserLabours", "Labour_Id", "dbo.Labours");
            DropForeignKey("dbo.ApplicationUserLabours", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserLabours", new[] { "Labour_Id" });
            DropIndex("dbo.ApplicationUserLabours", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.AspNetUsers", "ParticipateInLabour");
            DropTable("dbo.ApplicationUserLabours");
            DropTable("dbo.Labours");
        }
    }
}
