namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Vacations", "UsrId", c => c.String(nullable: false, maxLength: 128));
            //CreateIndex("dbo.Vacations", "UsrId");
            //AddForeignKey("dbo.Vacations", "UsrId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.Vacations", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vacations", "UserId", c => c.String(nullable: false));
            //DropForeignKey("dbo.Vacations", "UsrId", "dbo.AspNetUsers");
            //DropIndex("dbo.Vacations", new[] { "UsrId" });
            //DropColumn("dbo.Vacations", "UsrId");
        }
    }
}
