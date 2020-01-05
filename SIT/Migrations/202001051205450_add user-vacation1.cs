namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adduservacation1 : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Vacations", "ApplicationUser_Id", "dbo.AspNetUsers");
            //DropIndex("dbo.Vacations", new[] { "ApplicationUser_Id" });
            //DropTable("dbo.Vacations");
        }
        
        public override void Down()
        {
            //CreateTable(
            //    "dbo.Vacations",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            StartDate = c.DateTime(),
            //            EndDate = c.DateTime(),
            //            Duration = c.Int(),
            //            ApplicationUser_Id = c.String(maxLength: 128),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateIndex("dbo.Vacations", "ApplicationUser_Id");
            //AddForeignKey("dbo.Vacations", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
