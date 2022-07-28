namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addBlogPost : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlogPosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 255),
                        ShortContents = c.String(),
                        Contents = c.String(),
                        ImagePath = c.String(),
                        Public = c.Boolean(nullable: false),
                        HotNews = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BlogPosts");
        }
    }
}
