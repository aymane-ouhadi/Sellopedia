namespace Sellopedia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fixingtheorder : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reviews", "Message", c => c.String(maxLength: 300));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reviews", "Message", c => c.String());
        }
    }
}
