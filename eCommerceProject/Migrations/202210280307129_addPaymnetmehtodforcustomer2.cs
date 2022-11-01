namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPaymnetmehtodforcustomer2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "PaymentMethod");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "PaymentMethod", c => c.String());
        }
    }
}
