namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class right1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Overtimes", "Utilized", c => c.DateTime());
            AlterColumn("dbo.Overtimes", "UsrId", c => c.String(maxLength: 128));
            //AddColumn("dbo.Overtimes", "Duration", c => c.Int(nullable: true));
            //CreateIndex("dbo.Overtimes", "UsrId");
            AddForeignKey("dbo.Overtimes", "UsrId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Overtimes", "UsrId", "dbo.AspNetUsers");
            //DropIndex("dbo.Overtimes", new[] { "UsrId" });
            //DropColumn("dbo.Overtimes", "Duration");
            AlterColumn("dbo.Overtimes", "UsrId", c => c.String());
            DropColumn("dbo.Overtimes", "Utilized");
        }
    }
}
