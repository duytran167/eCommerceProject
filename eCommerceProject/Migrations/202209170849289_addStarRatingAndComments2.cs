namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStarRatingAndComments2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StarRatingAndComments", "Rating", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StarRatingAndComments", "Rating");
        }
    }
}
