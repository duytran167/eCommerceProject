namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfkandformatkey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogPosts", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.BlogPosts", "User_Id");
            AddForeignKey("dbo.BlogPosts", "User_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BlogPosts", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.BlogPosts", new[] { "User_Id" });
            DropColumn("dbo.BlogPosts", "User_Id");
        }
    }
}
