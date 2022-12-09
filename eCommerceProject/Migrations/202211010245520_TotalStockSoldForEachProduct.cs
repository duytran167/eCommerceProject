namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TotalStockSoldForEachProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "TotalStockSold", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "TotalStockSold");
        }
    }
}
