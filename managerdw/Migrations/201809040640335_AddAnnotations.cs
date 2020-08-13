namespace managerdw.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAnnotations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskProjects", "Stages_Id", c => c.Guid());
            AlterColumn("dbo.Priorities", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Projects", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Stages", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Status", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.TaskProjects", "Name", c => c.String(nullable: false));
            CreateIndex("dbo.TaskProjects", "Stages_Id");
            AddForeignKey("dbo.TaskProjects", "Stages_Id", "dbo.Stages", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskProjects", "Stages_Id", "dbo.Stages");
            DropIndex("dbo.TaskProjects", new[] { "Stages_Id" });
            AlterColumn("dbo.TaskProjects", "Name", c => c.String());
            AlterColumn("dbo.Status", "Name", c => c.String());
            AlterColumn("dbo.Stages", "Name", c => c.String());
            AlterColumn("dbo.Projects", "Name", c => c.String());
            AlterColumn("dbo.Priorities", "Name", c => c.String());
            DropColumn("dbo.TaskProjects", "Stages_Id");
        }
    }
}
