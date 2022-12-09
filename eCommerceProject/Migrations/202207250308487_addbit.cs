namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addbit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogPosts", "Public", c => c.Boolean(nullable: false));
            AddColumn("dbo.BlogPosts", "HotNews", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BlogPosts", "HotNews");
            DropColumn("dbo.BlogPosts", "Public");
        }
    }
}
