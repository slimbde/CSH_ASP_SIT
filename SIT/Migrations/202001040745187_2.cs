namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Sections", name: "UserId", newName: "ChiefId");
            RenameIndex(table: "dbo.Sections", name: "IX_UserId", newName: "IX_ChiefId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Sections", name: "IX_ChiefId", newName: "IX_UserId");
            RenameColumn(table: "dbo.Sections", name: "ChiefId", newName: "UserId");
        }
    }
}
