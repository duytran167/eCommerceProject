namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCollection : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Collections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Description = c.String(),
                        ArticleDate = c.DateTime(),
                        ImagePath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.BannerSliders", "Link", c => c.String());
            AddColumn("dbo.Products", "CollectionID", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "CollectionID");
            AddForeignKey("dbo.Products", "CollectionID", "dbo.Collections", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "CollectionID", "dbo.Collections");
            DropIndex("dbo.Products", new[] { "CollectionID" });
            DropColumn("dbo.Products", "CollectionID");
            DropColumn("dbo.BannerSliders", "Link");
            DropTable("dbo.Collections");
        }
    }
}
