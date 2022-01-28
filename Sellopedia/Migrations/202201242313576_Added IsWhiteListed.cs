namespace Sellopedia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIsWhiteListed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IsWhiteListed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsWhiteListed");
        }
    }
}
