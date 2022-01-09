namespace Sellopedia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductUserNavigation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ProductId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ProductId");
        }
    }
}
