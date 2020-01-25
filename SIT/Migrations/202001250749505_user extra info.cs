namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userextrainfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "MedExam", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "LabourSecurityExam", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "IndustrialSecurityExam", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "GotHelmet", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "GotSuit", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "GotBoots", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "GotCoat", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "GotCoat");
            DropColumn("dbo.AspNetUsers", "GotBoots");
            DropColumn("dbo.AspNetUsers", "GotSuit");
            DropColumn("dbo.AspNetUsers", "GotHelmet");
            DropColumn("dbo.AspNetUsers", "IndustrialSecurityExam");
            DropColumn("dbo.AspNetUsers", "LabourSecurityExam");
            DropColumn("dbo.AspNetUsers", "MedExam");
        }
    }
}
