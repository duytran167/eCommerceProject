namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatusForAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "StatusID", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Status");
            DropColumn("dbo.AspNetUsers", "StatusID");
        }
    }
}
