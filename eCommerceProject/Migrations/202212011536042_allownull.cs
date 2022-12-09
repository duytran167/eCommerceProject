namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class allownull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Discount", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Discount", c => c.Int(nullable: false));
        }
    }
}
