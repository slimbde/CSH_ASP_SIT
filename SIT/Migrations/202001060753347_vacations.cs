namespace SIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vacations : DbMigration
    {
        public override void Up()
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Vacations");
        }
    }
}
