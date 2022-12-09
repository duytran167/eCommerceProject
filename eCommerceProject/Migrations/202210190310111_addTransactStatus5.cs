namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTransactStatus5 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Orders", "TransactStatusId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "TransactStatusId", c => c.Int(nullable: false));
        }
    }
}
