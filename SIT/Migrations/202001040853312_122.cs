namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _122 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SectionUsers", "Section_Id", "dbo.Sections");
            DropForeignKey("dbo.SectionUsers", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.SectionUsers", new[] { "UserId" });
            DropIndex("dbo.SectionUsers", new[] { "Section_Id" });
            DropTable("dbo.SectionUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SectionUsers",
                c => new
                    {
                        SectionId = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        Section_Id = c.Int(),
                    })
                .PrimaryKey(t => t.SectionId);
            
            CreateIndex("dbo.SectionUsers", "Section_Id");
            CreateIndex("dbo.SectionUsers", "UserId");
            AddForeignKey("dbo.SectionUsers", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.SectionUsers", "Section_Id", "dbo.Sections", "Id");
        }
    }
}
