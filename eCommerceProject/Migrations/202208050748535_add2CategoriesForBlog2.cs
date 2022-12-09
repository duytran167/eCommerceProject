namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add2CategoriesForBlog2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogPosts", "BlogCategoriesID", c => c.Int(nullable: false));
            CreateIndex("dbo.BlogPosts", "BlogCategoriesID");
            AddForeignKey("dbo.BlogPosts", "BlogCategoriesID", "dbo.BlogCategories", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BlogPosts", "BlogCategoriesID", "dbo.BlogCategories");
            DropIndex("dbo.BlogPosts", new[] { "BlogCategoriesID" });
            DropColumn("dbo.BlogPosts", "BlogCategoriesID");
        }
    }
}
