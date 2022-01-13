namespace Sellopedia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductdiscountPriceacceptNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "DiscountPrice", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "DiscountPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
