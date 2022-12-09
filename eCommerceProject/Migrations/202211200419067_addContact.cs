namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addContact : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false, maxLength: 100),
                        PhoneNumber = c.String(nullable: false, maxLength: 10),
                        Email = c.String(nullable: false, maxLength: 50),
                        Content = c.String(nullable: false, maxLength: 500),
                        ArticleDate = c.DateTime(),
                        ImagePath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.Locations");
        }
        
        public override void Down()
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
            
            DropTable("dbo.Contacts");
        }
    }
}
