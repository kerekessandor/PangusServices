namespace PangusServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class n : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Operators", "Main_MainID", "dbo.Mains");
            DropIndex("dbo.Operators", new[] { "Main_MainID" });
            DropColumn("dbo.Operators", "Main_MainID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Operators", "Main_MainID", c => c.Int());
            CreateIndex("dbo.Operators", "Main_MainID");
            AddForeignKey("dbo.Operators", "Main_MainID", "dbo.Mains", "MainID");
        }
    }
}
