namespace PangusServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class foreign : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Mains", "OperatorID", "dbo.Operators");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Mains", "OperatorID", "dbo.Operators");
        }
    }
}
