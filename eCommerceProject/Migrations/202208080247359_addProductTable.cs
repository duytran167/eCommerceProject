namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addProductTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BlogVMs", "BlogPost_Id", "dbo.BlogPosts");
            DropForeignKey("dbo.Comments", "BlogVM_Id", "dbo.BlogVMs");
            DropIndex("dbo.BlogVMs", new[] { "BlogPost_Id" });
            DropIndex("dbo.Comments", new[] { "BlogVM_Id" });
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductName = c.String(nullable: false, maxLength: 255),
                        ShortDesc = c.String(),
                        Description = c.String(),
                        ImagePath = c.String(),
                        CategoryID = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        Discount = c.Int(nullable: false),
                        UnitsInStock = c.Int(nullable: false),
                        BestSellers = c.Boolean(nullable: false),
                        Active = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID);
            
            DropColumn("dbo.Comments", "BlogVM_Id");
            DropTable("dbo.BlogVMs");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Comments", "BlogVM_Id", c => c.Int());
            DropForeignKey("dbo.Products", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "CategoryID" });
            DropTable("dbo.Products");
            CreateIndex("dbo.Comments", "BlogVM_Id");
            CreateIndex("dbo.BlogVMs", "BlogPost_Id");
            AddForeignKey("dbo.Comments", "BlogVM_Id", "dbo.BlogVMs", "Id");
            AddForeignKey("dbo.BlogVMs", "BlogPost_Id", "dbo.BlogPosts", "Id");
        }
    }
}
