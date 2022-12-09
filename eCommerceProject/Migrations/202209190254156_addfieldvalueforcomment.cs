namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfieldvalueforcomment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StarRatingAndComments", "UserId", c => c.String(maxLength: 128));
            AddColumn("dbo.StarRatingAndComments", "isPublic", c => c.Boolean(nullable: false));
            CreateIndex("dbo.StarRatingAndComments", "UserId");
            AddForeignKey("dbo.StarRatingAndComments", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StarRatingAndComments", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.StarRatingAndComments", new[] { "UserId" });
            DropColumn("dbo.StarRatingAndComments", "isPublic");
            DropColumn("dbo.StarRatingAndComments", "UserId");
        }
    }
}
