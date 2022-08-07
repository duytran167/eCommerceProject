namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCommentAndSubvComment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommentMsg = c.String(),
                        CommentedDate = c.DateTime(nullable: false),
                        Posts_Id = c.Int(),
                        Users_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BlogPosts", t => t.Posts_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Users_Id)
                .Index(t => t.Posts_Id)
                .Index(t => t.Users_Id);
            
            CreateTable(
                "dbo.SubComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommentMsg = c.String(),
                        CommentedDate = c.DateTime(nullable: false),
                        Comment_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Comments", t => t.Comment_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Comment_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubComments", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SubComments", "Comment_Id", "dbo.Comments");
            DropForeignKey("dbo.Comments", "Users_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "Posts_Id", "dbo.BlogPosts");
            DropIndex("dbo.SubComments", new[] { "User_Id" });
            DropIndex("dbo.SubComments", new[] { "Comment_Id" });
            DropIndex("dbo.Comments", new[] { "Users_Id" });
            DropIndex("dbo.Comments", new[] { "Posts_Id" });
            DropTable("dbo.SubComments");
            DropTable("dbo.Comments");
        }
    }
}
