namespace Sellopedia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedisvalid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IsValid", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsValid");
        }
    }
}
