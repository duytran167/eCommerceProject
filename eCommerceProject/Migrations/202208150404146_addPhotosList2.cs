namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPhotosList2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImageProducts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FileName = c.String(),
                        Extension = c.String(),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ImageProducts", "ProductId", "dbo.Products");
            DropIndex("dbo.ImageProducts", new[] { "ProductId" });
            DropTable("dbo.ImageProducts");
        }
    }
}
