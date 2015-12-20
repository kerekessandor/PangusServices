namespace PangusServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pl : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Operators", "Location");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Operators", "Location", c => c.Int(nullable: false));
        }
    }
}
