namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removePriceDiscountUnit23 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "CategoryID", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "Price", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "Discount", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "UnitsInStock", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "CategoryID");
            AddForeignKey("dbo.Products", "CategoryID", "dbo.Categories", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "CategoryID" });
            DropColumn("dbo.Products", "UnitsInStock");
            DropColumn("dbo.Products", "Discount");
            DropColumn("dbo.Products", "Price");
            DropColumn("dbo.Products", "CategoryID");
        }
    }
}
