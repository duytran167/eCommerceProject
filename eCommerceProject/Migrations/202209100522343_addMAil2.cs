namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMAil2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SendEmails", "subject");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SendEmails", "subject", c => c.String(nullable: false));
        }
    }
}
