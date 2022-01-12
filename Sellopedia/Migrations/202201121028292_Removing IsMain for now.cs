namespace Sellopedia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovingIsMainfornow : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ProductImages", "IsMainImage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductImages", "IsMainImage", c => c.Boolean(nullable: false));
        }
    }
}
