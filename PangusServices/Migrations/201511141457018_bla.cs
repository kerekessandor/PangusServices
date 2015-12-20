namespace PangusServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bla : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Mains", "OperatorID", "dbo.Operators");
            DropIndex("dbo.Mains", new[] { "OperatorID" });
            AddColumn("dbo.Operators", "Main_MainID", c => c.Int());
            CreateIndex("dbo.Operators", "Main_MainID");
            AddForeignKey("dbo.Operators", "Main_MainID", "dbo.Mains", "MainID");
            DropColumn("dbo.Mains", "OperatorID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Mains", "OperatorID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Operators", "Main_MainID", "dbo.Mains");
            DropIndex("dbo.Operators", new[] { "Main_MainID" });
            DropColumn("dbo.Operators", "Main_MainID");
            CreateIndex("dbo.Mains", "OperatorID");
            AddForeignKey("dbo.Mains", "OperatorID", "dbo.Operators", "OperatorID", cascadeDelete: true);
        }
    }
}
