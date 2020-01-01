namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user_principal_fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
            AddColumn("dbo.AspNetUsers", "Surname", c => c.String());
            AddColumn("dbo.AspNetUsers", "Patronic", c => c.String());
            AddColumn("dbo.AspNetUsers", "TabNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "TabNo");
            DropColumn("dbo.AspNetUsers", "Patronic");
            DropColumn("dbo.AspNetUsers", "Surname");
            DropColumn("dbo.AspNetUsers", "Name");
        }
    }
}
