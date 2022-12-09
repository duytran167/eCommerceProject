namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCommentView : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "Posts_Id", "dbo.BlogPosts");
            DropIndex("dbo.Comments", new[] { "Posts_Id" });
            RenameColumn(table: "dbo.Comments", name: "Posts_Id", newName: "BlogPostID");
            AddColumn("dbo.BlogPosts", "BlogPostView", c => c.Int(nullable: false));
            AlterColumn("dbo.Comments", "BlogPostID", c => c.Int(nullable: false));
            CreateIndex("dbo.Comments", "BlogPostID");
            AddForeignKey("dbo.Comments", "BlogPostID", "dbo.BlogPosts", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "BlogPostID", "dbo.BlogPosts");
            DropIndex("dbo.Comments", new[] { "BlogPostID" });
            AlterColumn("dbo.Comments", "BlogPostID", c => c.Int());
            DropColumn("dbo.BlogPosts", "BlogPostView");
            RenameColumn(table: "dbo.Comments", name: "BlogPostID", newName: "Posts_Id");
            CreateIndex("dbo.Comments", "Posts_Id");
            AddForeignKey("dbo.Comments", "Posts_Id", "dbo.BlogPosts", "Id");
        }
    }
}
