namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class overtimes6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Overtimes", "EmployeeId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Overtimes", "EmployeeId");
            AddForeignKey("dbo.Overtimes", "EmployeeId", "dbo.AspNetUsers", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Overtimes", "EmployeeId", "dbo.AspNetUsers");
            DropIndex("dbo.Overtimes", new[] { "EmployeeId" });
            DropColumn("dbo.Overtimes", "EmployeeId");
        }
    }
}
