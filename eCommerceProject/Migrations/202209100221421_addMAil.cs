namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMAil : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SendEmails", "subject", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SendEmails", "subject");
        }
    }
}
