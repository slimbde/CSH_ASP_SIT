namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class right : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Sections", "ChiefId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Units", "ChiefId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Overtimes", "UsId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "Section_Id" });
            DropIndex("dbo.Sections", new[] { "ChiefId" });
            DropIndex("dbo.Units", new[] { "ChiefId" });
            DropIndex("dbo.Overtimes", new[] { "UsId" });
            //DropColumn("dbo.AspNetUsers", "SectionId");
            //RenameColumn(table: "dbo.AspNetUsers", name: "Section_Id", newName: "SectionId");
            AddColumn("dbo.Overtimes", "UsrId", c => c.String(maxLength: 128));
            //AlterColumn("dbo.Sections", "ChiefId", c => c.String());
            //AlterColumn("dbo.Units", "ChiefId", c => c.String());
            DropColumn("dbo.Overtimes", "UsId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Overtimes", "UsId", c => c.String(maxLength: 128));
            //AlterColumn("dbo.Units", "ChiefId", c => c.String(maxLength: 128));
            //AlterColumn("dbo.Sections", "ChiefId", c => c.String(maxLength: 128));
            DropColumn("dbo.Overtimes", "UsrId");
            //(table: "dbo.AspNetUsers", name: "SectionId", newName: "Section_Id");
            //AddColumn("dbo.AspNetUsers", "SectionId", c => c.Int());
            CreateIndex("dbo.Overtimes", "UsId");
            CreateIndex("dbo.Units", "ChiefId");
            CreateIndex("dbo.Sections", "ChiefId");
            CreateIndex("dbo.AspNetUsers", "Section_Id");
            AddForeignKey("dbo.Overtimes", "UsId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Units", "ChiefId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Sections", "ChiefId", "dbo.AspNetUsers", "Id");
        }
    }
}
