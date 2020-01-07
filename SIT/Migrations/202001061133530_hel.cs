namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hel : DbMigration
    {
        public override void Up()
        {

        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Vacations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        Duration = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
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
    }
}
