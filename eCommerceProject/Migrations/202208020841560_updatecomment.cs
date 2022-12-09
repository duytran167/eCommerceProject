namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecomment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "BlogPostID", "dbo.BlogPosts");
            DropForeignKey("dbo.SubComments", "Comment_Id", "dbo.Comments");
            DropForeignKey("dbo.SubComments", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Comments", new[] { "BlogPostID" });
            DropIndex("dbo.Comments", new[] { "Users_Id" });
            DropIndex("dbo.SubComments", new[] { "Comment_Id" });
            DropIndex("dbo.SubComments", new[] { "User_Id" });
            DropColumn("dbo.Comments", "UserID");
            RenameColumn(table: "dbo.Comments", name: "Users_Id", newName: "UserID");
            AddColumn("dbo.Comments", "Text", c => c.String());
            AddColumn("dbo.Comments", "TimeStamp", c => c.DateTime(nullable: false));
            AddColumn("dbo.Comments", "RecordID", c => c.Int(nullable: false));
            AddColumn("dbo.Comments", "EntityID", c => c.Int(nullable: false));
            AlterColumn("dbo.Comments", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Comments", "UserID");
            DropColumn("dbo.Comments", "CommentMsg");
            DropColumn("dbo.Comments", "CommentedDate");
            DropColumn("dbo.Comments", "BlogPostID");
            DropTable("dbo.SubComments");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Comments", "BlogPostID", c => c.Int(nullable: false));
            AddColumn("dbo.Comments", "CommentedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Comments", "CommentMsg", c => c.String());
            DropIndex("dbo.Comments", new[] { "UserID" });
            AlterColumn("dbo.Comments", "UserID", c => c.String());
            DropColumn("dbo.Comments", "EntityID");
            DropColumn("dbo.Comments", "RecordID");
            DropColumn("dbo.Comments", "TimeStamp");
            DropColumn("dbo.Comments", "Text");
            RenameColumn(table: "dbo.Comments", name: "UserID", newName: "Users_Id");
            AddColumn("dbo.Comments", "UserID", c => c.String());
            CreateIndex("dbo.SubComments", "User_Id");
            CreateIndex("dbo.SubComments", "Comment_Id");
            CreateIndex("dbo.Comments", "Users_Id");
            CreateIndex("dbo.Comments", "BlogPostID");
            AddForeignKey("dbo.SubComments", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.SubComments", "Comment_Id", "dbo.Comments", "Id");
            AddForeignKey("dbo.Comments", "BlogPostID", "dbo.BlogPosts", "Id", cascadeDelete: true);
        }
    }
}
