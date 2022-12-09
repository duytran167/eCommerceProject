namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addBi1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogPosts", "isPublic", c => c.Boolean(nullable: false));
            AddColumn("dbo.BlogPosts", "isHotNews", c => c.Boolean(nullable: false));
            DropColumn("dbo.BlogPosts", "Public");
            DropColumn("dbo.BlogPosts", "HotNews");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BlogPosts", "HotNews", c => c.Boolean(nullable: false));
            AddColumn("dbo.BlogPosts", "Public", c => c.Boolean(nullable: false));
            DropColumn("dbo.BlogPosts", "isHotNews");
            DropColumn("dbo.BlogPosts", "isPublic");
        }
    }
}
