namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addShipdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "PackageDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "PackageDate");
        }
    }
}
