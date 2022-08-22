namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addImageandRemoveCat : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Cats");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Cats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImagePath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
