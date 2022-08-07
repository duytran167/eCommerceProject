namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCommentView2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "UserID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "UserID");
        }
    }
}
