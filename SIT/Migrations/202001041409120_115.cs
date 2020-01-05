namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _115 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SectionUsers", "SectionId", "dbo.Sections");
            DropForeignKey("dbo.SectionUsers", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.SectionUsers", new[] { "SectionId" });
            DropIndex("dbo.SectionUsers", new[] { "UserId" });
            DropTable("dbo.SectionUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SectionUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SectionId = c.Int(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.SectionUsers", "UserId");
            CreateIndex("dbo.SectionUsers", "SectionId");
            AddForeignKey("dbo.SectionUsers", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.SectionUsers", "SectionId", "dbo.Sections", "Id");
        }
    }
}
