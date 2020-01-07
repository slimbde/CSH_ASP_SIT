namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sss : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Vacations", "UserId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Vacations", "UserId", c => c.String());
        }
    }
}
