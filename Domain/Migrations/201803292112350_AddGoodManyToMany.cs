namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGoodManyToMany : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Comment_CommentId", "dbo.Comments");
            DropIndex("dbo.AspNetUsers", new[] { "Comment_CommentId" });
            CreateTable(
                "dbo.ApplicationUserComments",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Comment_CommentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Comment_CommentId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Comments", t => t.Comment_CommentId, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Comment_CommentId);
            
            DropColumn("dbo.AspNetUsers", "Comment_CommentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Comment_CommentId", c => c.Int());
            DropForeignKey("dbo.ApplicationUserComments", "Comment_CommentId", "dbo.Comments");
            DropForeignKey("dbo.ApplicationUserComments", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserComments", new[] { "Comment_CommentId" });
            DropIndex("dbo.ApplicationUserComments", new[] { "ApplicationUser_Id" });
            DropTable("dbo.ApplicationUserComments");
            CreateIndex("dbo.AspNetUsers", "Comment_CommentId");
            AddForeignKey("dbo.AspNetUsers", "Comment_CommentId", "dbo.Comments", "CommentId");
        }
    }
}
