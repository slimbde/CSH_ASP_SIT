namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addunitchief : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Units", "ChiefId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Units", "ChiefId");
            AddForeignKey("dbo.Units", "ChiefId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Units", "ChiefId", "dbo.AspNetUsers");
            DropIndex("dbo.Units", new[] { "ChiefId" });
            DropColumn("dbo.Units", "ChiefId");
        }
    }
}
