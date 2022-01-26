namespace Sellopedia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedorderreference : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Id", c => c.Guid(nullable: false, identity: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Id");
        }
    }
}
