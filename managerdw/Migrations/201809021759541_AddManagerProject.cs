namespace managerdw.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddManagerProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "Manager_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Projects", "Manager_Id");
            AddForeignKey("dbo.Projects", "Manager_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "Manager_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Projects", new[] { "Manager_Id" });
            DropColumn("dbo.Projects", "Manager_Id");
        }
    }
}
