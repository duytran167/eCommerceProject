namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtimecreateforbanner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BannerSliders", "CreatedDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.AspNetUsers", "FullName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "FullName", c => c.String());
            DropColumn("dbo.BannerSliders", "CreatedDate");
        }
    }
}
