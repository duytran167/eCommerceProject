namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addpricefororder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "SizeId", c => c.Int());
            CreateIndex("dbo.OrderDetails", "SizeId");
            AddForeignKey("dbo.OrderDetails", "SizeId", "dbo.Sizes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDetails", "SizeId", "dbo.Sizes");
            DropIndex("dbo.OrderDetails", new[] { "SizeId" });
            DropColumn("dbo.OrderDetails", "SizeId");
        }
    }
}
