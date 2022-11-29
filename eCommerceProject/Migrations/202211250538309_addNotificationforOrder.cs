namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNotificationforOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "Noti", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "Noti", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Noti");
            DropColumn("dbo.Contacts", "Noti");
        }
    }
}
