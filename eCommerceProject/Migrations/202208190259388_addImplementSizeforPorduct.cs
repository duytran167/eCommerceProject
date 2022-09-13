namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addImplementSizeforPorduct : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "SizeID", "dbo.Sizes");
            DropIndex("dbo.Products", new[] { "SizeID" });
            AddColumn("dbo.Sizes", "ProductId", c => c.Int(nullable: false));
            CreateIndex("dbo.Sizes", "ProductId");
            AddForeignKey("dbo.Sizes", "ProductId", "dbo.Products", "Id", cascadeDelete: true);
            DropColumn("dbo.Products", "SizeID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "SizeID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Sizes", "ProductId", "dbo.Products");
            DropIndex("dbo.Sizes", new[] { "ProductId" });
            DropColumn("dbo.Sizes", "ProductId");
            CreateIndex("dbo.Products", "SizeID");
            AddForeignKey("dbo.Products", "SizeID", "dbo.Sizes", "Id", cascadeDelete: true);
        }
    }
}
