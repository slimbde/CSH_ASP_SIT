namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addvacations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vacations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        Duration = c.Int(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vacations", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Vacations", new[] { "ApplicationUser_Id" });
            DropTable("dbo.Vacations");
        }
    }
}
