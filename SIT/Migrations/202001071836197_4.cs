namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vacations", "UsrId", "dbo.AspNetUsers");
            DropIndex("dbo.Vacations", new[] { "UsrId" });
            AlterColumn("dbo.Vacations", "UsrId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Vacations", "UsrId");
            AddForeignKey("dbo.Vacations", "UsrId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vacations", "UsrId", "dbo.AspNetUsers");
            DropIndex("dbo.Vacations", new[] { "UsrId" });
            AlterColumn("dbo.Vacations", "UsrId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Vacations", "UsrId");
            AddForeignKey("dbo.Vacations", "UsrId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
