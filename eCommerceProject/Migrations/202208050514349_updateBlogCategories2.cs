namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateBlogCategories2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlogVMs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EntityID = c.Int(nullable: false),
                        UserName = c.String(),
                        BlogPost_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BlogPosts", t => t.BlogPost_Id)
                .Index(t => t.BlogPost_Id);
            
            AddColumn("dbo.Comments", "BlogVM_Id", c => c.Int());
            CreateIndex("dbo.Comments", "BlogVM_Id");
            AddForeignKey("dbo.Comments", "BlogVM_Id", "dbo.BlogVMs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "BlogVM_Id", "dbo.BlogVMs");
            DropForeignKey("dbo.BlogVMs", "BlogPost_Id", "dbo.BlogPosts");
            DropIndex("dbo.Comments", new[] { "BlogVM_Id" });
            DropIndex("dbo.BlogVMs", new[] { "BlogPost_Id" });
            DropColumn("dbo.Comments", "BlogVM_Id");
            DropTable("dbo.BlogVMs");
        }
    }
}
