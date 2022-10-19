namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTransactStatus6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransactStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Orders", "TransactStatusId", c => c.Int(nullable: false));
            CreateIndex("dbo.Orders", "TransactStatusId");
            AddForeignKey("dbo.Orders", "TransactStatusId", "dbo.TransactStatus", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "TransactStatusId", "dbo.TransactStatus");
            DropIndex("dbo.Orders", new[] { "TransactStatusId" });
            DropColumn("dbo.Orders", "TransactStatusId");
            DropTable("dbo.TransactStatus");
        }
    }
}
