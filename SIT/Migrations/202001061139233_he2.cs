namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class he2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vacations", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Vacations", new[] { "UserId" });
            AddColumn("dbo.Vacations", "Usr_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Vacations", "UserId", c => c.String());
            CreateIndex("dbo.Vacations", "Usr_Id");
            AddForeignKey("dbo.Vacations", "Usr_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vacations", "Usr_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Vacations", new[] { "Usr_Id" });
            AlterColumn("dbo.Vacations", "UserId", c => c.String(maxLength: 128));
            DropColumn("dbo.Vacations", "Usr_Id");
            CreateIndex("dbo.Vacations", "UserId");
            AddForeignKey("dbo.Vacations", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
