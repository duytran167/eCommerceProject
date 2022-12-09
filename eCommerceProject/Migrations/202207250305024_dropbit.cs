namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dropbit : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.BlogPosts", "Public");
            DropColumn("dbo.BlogPosts", "HotNews");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BlogPosts", "HotNews", c => c.Boolean(nullable: false));
            AddColumn("dbo.BlogPosts", "Public", c => c.Boolean(nullable: false));
        }
    }
}
