namespace Domain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddBrandColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Brand", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Brand");
        }
    }
}
