namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class floatscore : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Vacations", "Score", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Vacations", "Score", c => c.Single());
        }
    }
}
