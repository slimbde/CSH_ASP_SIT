namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addunit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Units",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Sections", "UnitId", c => c.Int());
            CreateIndex("dbo.Sections", "UnitId");
            AddForeignKey("dbo.Sections", "UnitId", "dbo.Units", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sections", "UnitId", "dbo.Units");
            DropIndex("dbo.Sections", new[] { "UnitId" });
            DropColumn("dbo.Sections", "UnitId");
            DropTable("dbo.Units");
        }
    }
}
