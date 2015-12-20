namespace PangusServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _operator : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mains", "OperatorName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Mains", "OperatorName");
        }
    }
}
