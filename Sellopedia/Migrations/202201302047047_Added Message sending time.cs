namespace Sellopedia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMessagesendingtime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "SendingTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "SendingTime");
        }
    }
}
