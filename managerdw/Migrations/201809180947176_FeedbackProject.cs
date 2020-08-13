namespace managerdw.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeedbackProject : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Price = c.String(),
                        Deadline = c.String(),
                        Edit = c.String(),
                        Comment = c.String(),
                        Projects_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.Projects_Id)
                .Index(t => t.Projects_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Feedbacks", "Projects_Id", "dbo.Projects");
            DropIndex("dbo.Feedbacks", new[] { "Projects_Id" });
            DropTable("dbo.Feedbacks");
        }
    }
}
