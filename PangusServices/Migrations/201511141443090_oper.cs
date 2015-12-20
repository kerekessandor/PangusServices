namespace PangusServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class oper : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.Mains", "OperatorID", "dbo.Operators", "OperatorID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Mains", "OperatorID", "dbo.Operators");
        }
    }
}
