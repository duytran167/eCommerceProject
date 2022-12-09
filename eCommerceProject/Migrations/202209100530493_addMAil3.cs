namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMAil3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SendEmails", "subject", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SendEmails", "subject");
        }
    }
}
