namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adduservacation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserVacations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        Days = c.Int(nullable: false),
                        Score = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.UserVacations");
        }
    }
}
