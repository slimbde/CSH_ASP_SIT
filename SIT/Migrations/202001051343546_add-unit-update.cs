namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addunitupdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Units", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Units", "Name", c => c.String());
        }
    }
}
