namespace eCommerceProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfkandformatkey2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.BlogPosts", name: "User_Id", newName: "UserId");
            RenameIndex(table: "dbo.BlogPosts", name: "IX_User_Id", newName: "IX_UserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.BlogPosts", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.BlogPosts", name: "UserId", newName: "User_Id");
        }
    }
}
