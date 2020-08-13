namespace managerdw.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteManager : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Projects", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Projects", "Manager_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Project_Id", "dbo.Projects");
            DropIndex("dbo.Projects", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Projects", new[] { "Manager_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Project_Id" });
            CreateTable(
                "dbo.ApplicationUserProjects",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Project_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Project_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Project_Id);
            
            DropColumn("dbo.Projects", "ApplicationUser_Id");
            DropColumn("dbo.Projects", "Manager_Id");
            DropColumn("dbo.AspNetUsers", "Project_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Project_Id", c => c.Guid());
            AddColumn("dbo.Projects", "Manager_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Projects", "ApplicationUser_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.ApplicationUserProjects", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.ApplicationUserProjects", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserProjects", new[] { "Project_Id" });
            DropIndex("dbo.ApplicationUserProjects", new[] { "ApplicationUser_Id" });
            DropTable("dbo.ApplicationUserProjects");
            CreateIndex("dbo.AspNetUsers", "Project_Id");
            CreateIndex("dbo.Projects", "Manager_Id");
            CreateIndex("dbo.Projects", "ApplicationUser_Id");
            AddForeignKey("dbo.AspNetUsers", "Project_Id", "dbo.Projects", "Id");
            AddForeignKey("dbo.Projects", "Manager_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Projects", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
