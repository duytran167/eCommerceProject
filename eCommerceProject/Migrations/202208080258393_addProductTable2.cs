namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addProductTable2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "CategoryID" });
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
            DropTable("dbo.Products");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Comments", "BlogVM_Id", "dbo.BlogVMs");
            DropForeignKey("dbo.BlogVMs", "BlogPost_Id", "dbo.BlogPosts");
            DropIndex("dbo.Comments", new[] { "BlogVM_Id" });
            DropIndex("dbo.BlogVMs", new[] { "BlogPost_Id" });
            DropColumn("dbo.Comments", "BlogVM_Id");
            DropTable("dbo.BlogVMs");
            CreateIndex("dbo.Products", "CategoryID");
            AddForeignKey("dbo.Products", "CategoryID", "dbo.Categories", "Id", cascadeDelete: true);
        }
    }
}
