namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sdfs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SectionUsers",
                c => new
                    {
                        SectionId = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        Section_Id = c.Int(),
                    })
                .PrimaryKey(t => t.SectionId)
                .ForeignKey("dbo.Sections", t => t.Section_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.Section_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SectionUsers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SectionUsers", "Section_Id", "dbo.Sections");
            DropIndex("dbo.SectionUsers", new[] { "Section_Id" });
            DropIndex("dbo.SectionUsers", new[] { "UserId" });
            DropTable("dbo.SectionUsers");
        }
    }
}
