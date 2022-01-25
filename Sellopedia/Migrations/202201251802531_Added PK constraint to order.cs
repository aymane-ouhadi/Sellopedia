namespace Sellopedia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPKconstrainttoorder : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Orders");
            AddPrimaryKey("dbo.Orders", new[] { "UserId", "ProductId", "Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Orders");
            AddPrimaryKey("dbo.Orders", new[] { "UserId", "ProductId" });
        }
    }
}
