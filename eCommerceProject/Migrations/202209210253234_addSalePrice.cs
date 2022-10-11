namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSalePrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "PriceSale", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "PriceSale");
        }
    }
}
