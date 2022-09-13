namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removePriceDiscountUnit : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "Price");
            DropColumn("dbo.Products", "Discount");
            DropColumn("dbo.Products", "UnitsInStock");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "UnitsInStock", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "Discount", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "Price", c => c.Int(nullable: false));
        }
    }
}
