namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDateTimeforSlide : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BannerSliders", "ArticleDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BannerSliders", "ArticleDate");
        }
    }
}
