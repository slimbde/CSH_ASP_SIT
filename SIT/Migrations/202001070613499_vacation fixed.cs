namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vacationfixed : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vacations", "Usr_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Vacations", new[] { "Usr_Id" });
            DropColumn("dbo.Vacations", "Usr_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vacations", "Usr_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Vacations", "Usr_Id");
            AddForeignKey("dbo.Vacations", "Usr_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
