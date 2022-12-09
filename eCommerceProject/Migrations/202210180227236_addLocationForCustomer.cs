namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addLocationForCustomer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Parent = c.Int(),
                        Levels = c.Int(),
                        Slug = c.String(),
                        NameWithType = c.String(),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.LocationId);
            
            AddColumn("dbo.AspNetUsers", "LocationId", c => c.Int());
            AddColumn("dbo.AspNetUsers", "District", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Ward", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Ward");
            DropColumn("dbo.AspNetUsers", "District");
            DropColumn("dbo.AspNetUsers", "LocationId");
            DropTable("dbo.Locations");
        }
    }
}
