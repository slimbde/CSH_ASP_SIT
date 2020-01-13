namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class overtimes8 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Overtimes", "EmployeeId", "dbo.AspNetUsers");
            DropIndex("dbo.Overtimes", new[] { "EmployeeId" });
            RenameColumn(table: "dbo.Overtimes", name: "EmployeeId", newName: "UsId");
            AlterColumn("dbo.Overtimes", "UsId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Overtimes", "UsId");
            AddForeignKey("dbo.Overtimes", "UsId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Overtimes", "UsId", "dbo.AspNetUsers");
            DropIndex("dbo.Overtimes", new[] { "UsId" });
            AlterColumn("dbo.Overtimes", "UsId", c => c.String(nullable: false, maxLength: 128));
            RenameColumn(table: "dbo.Overtimes", name: "UsId", newName: "EmployeeId");
            CreateIndex("dbo.Overtimes", "EmployeeId");
            AddForeignKey("dbo.Overtimes", "EmployeeId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
