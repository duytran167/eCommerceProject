namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateBlogCategories : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogPosts", "BlogCategories_Id", c => c.Int());
            CreateIndex("dbo.BlogPosts", "BlogCategories_Id");
            AddForeignKey("dbo.BlogPosts", "BlogCategories_Id", "dbo.BlogCategories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BlogPosts", "BlogCategories_Id", "dbo.BlogCategories");
            DropIndex("dbo.BlogPosts", new[] { "BlogCategories_Id" });
            DropColumn("dbo.BlogPosts", "BlogCategories_Id");
        }
    }
}
