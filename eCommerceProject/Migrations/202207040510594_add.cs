namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BannerSliders", "ArticleDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BannerSliders", "ArticleDate", c => c.DateTime(nullable: false));
        }
    }
}
