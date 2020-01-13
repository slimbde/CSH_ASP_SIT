namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class overtimes2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Overtimes", name: "UsrId", newName: "PersonId");
            RenameIndex(table: "dbo.Overtimes", name: "IX_UsrId", newName: "IX_PersonId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Overtimes", name: "IX_PersonId", newName: "IX_UsrId");
            RenameColumn(table: "dbo.Overtimes", name: "PersonId", newName: "UsrId");
        }
    }
}
