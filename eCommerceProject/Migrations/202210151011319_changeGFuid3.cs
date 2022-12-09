namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeGFuid3 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ImageProducts");
            AlterColumn("dbo.ImageProducts", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.ImageProducts", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ImageProducts");
            AlterColumn("dbo.ImageProducts", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.ImageProducts", "Id");
        }
    }
}
