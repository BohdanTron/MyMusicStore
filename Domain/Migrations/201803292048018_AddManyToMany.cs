namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddManyToMany : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Comment_CommentId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Comment_CommentId");
            AddForeignKey("dbo.AspNetUsers", "Comment_CommentId", "dbo.Comments", "CommentId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Comment_CommentId", "dbo.Comments");
            DropIndex("dbo.AspNetUsers", new[] { "Comment_CommentId" });
            DropColumn("dbo.AspNetUsers", "Comment_CommentId");
        }
    }
}
