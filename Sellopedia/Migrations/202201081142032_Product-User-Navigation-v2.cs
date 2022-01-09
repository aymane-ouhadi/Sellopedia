namespace Sellopedia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductUserNavigationv2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "ProductId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "ProductId", c => c.Int(nullable: false));
        }
    }
}
