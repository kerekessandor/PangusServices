namespace PangusServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class b : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mains", "OperatorID", c => c.Int());
            CreateIndex("dbo.Mains", "OperatorID");
            AddForeignKey("dbo.Mains", "OperatorID", "dbo.Operators", "OperatorID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Mains", "OperatorID", "dbo.Operators");
            DropIndex("dbo.Mains", new[] { "OperatorID" });
            DropColumn("dbo.Mains", "OperatorID");
        }
    }
}
