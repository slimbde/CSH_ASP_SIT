namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _123 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SectionUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SectionId = c.Int(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sections", t => t.SectionId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.SectionId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SectionUsers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SectionUsers", "SectionId", "dbo.Sections");
            DropIndex("dbo.SectionUsers", new[] { "UserId" });
            DropIndex("dbo.SectionUsers", new[] { "SectionId" });
            DropTable("dbo.SectionUsers");
        }
    }
}
