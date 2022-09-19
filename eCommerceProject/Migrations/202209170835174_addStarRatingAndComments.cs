namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStarRatingAndComments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StarRatingAndComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Comments = c.String(nullable: false, maxLength: 255),
                        ThisDateTime = c.DateTime(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StarRatingAndComments", "ProductId", "dbo.Products");
            DropIndex("dbo.StarRatingAndComments", new[] { "ProductId" });
            DropTable("dbo.StarRatingAndComments");
        }
    }
}
