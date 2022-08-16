namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSizeTable2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sizes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SizeName = c.String(nullable: false, maxLength: 255),
                        Stock = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Products", "SizeID", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "ProductCode", c => c.String());
            CreateIndex("dbo.Products", "SizeID");
            AddForeignKey("dbo.Products", "SizeID", "dbo.Sizes", "Id", cascadeDelete: true);
            DropColumn("dbo.Products", "UnitsInStock");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "UnitsInStock", c => c.Int(nullable: false));
            DropForeignKey("dbo.Products", "SizeID", "dbo.Sizes");
            DropIndex("dbo.Products", new[] { "SizeID" });
            DropColumn("dbo.Products", "ProductCode");
            DropColumn("dbo.Products", "SizeID");
            DropTable("dbo.Sizes");
        }
    }
}
