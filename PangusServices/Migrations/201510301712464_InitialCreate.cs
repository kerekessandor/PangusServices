namespace PangusServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnvelopeNois",
                c => new
                    {
                        AnvelopeNoiID = c.Int(nullable: false, identity: true),
                        NoiRulate = c.Boolean(nullable: false),
                        DotSerie = c.Single(),
                        Cantitate = c.Int(),
                        ProfilID = c.Int(),
                        ProfilName = c.String(),
                        AnvelopeName = c.String(),
                    })
                .PrimaryKey(t => t.AnvelopeNoiID)
                .ForeignKey("dbo.Profils", t => t.ProfilID)
                .Index(t => t.ProfilID);
            
            CreateTable(
                "dbo.Profils",
                c => new
                    {
                        ProfilID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AnvelopeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProfilID)
                .ForeignKey("dbo.Anvelopes", t => t.AnvelopeID, cascadeDelete: true)
                .Index(t => t.AnvelopeID);
            
            CreateTable(
                "dbo.Anvelopes",
                c => new
                    {
                        AnvelopeID = c.Int(nullable: false, identity: true),
                        Marca = c.String(),
                    })
                .PrimaryKey(t => t.AnvelopeID);
            
            CreateTable(
                "dbo.Depozitares",
                c => new
                    {
                        DepozitareID = c.Int(nullable: false, identity: true),
                        IsDepozitare = c.Boolean(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        NrInmatricuare = c.String(),
                        Dimensiune = c.String(),
                        Cantitate = c.Int(),
                        Descriere = c.String(),
                        Data = c.DateTime(nullable: false),
                        ProfilID = c.Int(),
                        Sfantu_Mciuc = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DepozitareID)
                .ForeignKey("dbo.Profils", t => t.ProfilID)
                .Index(t => t.ProfilID);
            
            CreateTable(
                "dbo.CarDetails",
                c => new
                    {
                        CarDetailID = c.Int(nullable: false, identity: true),
                        NrInmatricular = c.String(nullable: false),
                        MarcaAuto = c.String(),
                        KmRulati = c.Single(),
                        DimensiuneaA = c.String(),
                        Rim = c.String(),
                        AluminiuOtel = c.Boolean(nullable: false),
                        Descriere = c.String(),
                        CarTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CarDetailID)
                .ForeignKey("dbo.CarTypes", t => t.CarTypeID, cascadeDelete: true)
                .Index(t => t.CarTypeID);
            
            CreateTable(
                "dbo.CarTypes",
                c => new
                    {
                        CarTypeID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CarTypeID);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LastName = c.String(),
                        FirstName = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.Int(),
                        Date = c.DateTime(nullable: false),
                        pFizica_pJuridica = c.Boolean(nullable: false),
                        Client = c.String(),
                        Delegat = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Mains",
                c => new
                    {
                        MainID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        PaymentMethodID = c.Int(),
                        CarDetailID = c.Int(nullable: false),
                        AnvelopeNoiID = c.Int(),
                        DepozitareID = c.Int(),
                        isDone = c.Boolean(nullable: false),
                        Discount = c.Single(),
                        Sfantu_MCiuc = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MainID)
                .ForeignKey("dbo.AnvelopeNois", t => t.AnvelopeNoiID)
                .ForeignKey("dbo.CarDetails", t => t.CarDetailID, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.CustomerID, cascadeDelete: true)
                .ForeignKey("dbo.Depozitares", t => t.DepozitareID)
                .ForeignKey("dbo.PaymentMethods", t => t.PaymentMethodID)
                .Index(t => t.CustomerID)
                .Index(t => t.PaymentMethodID)
                .Index(t => t.CarDetailID)
                .Index(t => t.AnvelopeNoiID)
                .Index(t => t.DepozitareID);
            
            CreateTable(
                "dbo.CustomerServices",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Cantitate = c.Single(nullable: false),
                        NamedServ = c.String(),
                        Pret = c.Single(nullable: false),
                        Dimensiune = c.String(),
                        IsEditable = c.Boolean(nullable: false),
                        MainID = c.Int(nullable: false),
                        PangusServicii_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Mains", t => t.MainID, cascadeDelete: true)
                .ForeignKey("dbo.PangusServiciis", t => t.PangusServicii_ID)
                .Index(t => t.MainID)
                .Index(t => t.PangusServicii_ID);
            
            CreateTable(
                "dbo.PaymentMethods",
                c => new
                    {
                        PaymentMethodID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.PaymentMethodID);
            
            CreateTable(
                "dbo.dimTypes",
                c => new
                    {
                        dimTypeID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.dimTypeID);
            
            CreateTable(
                "dbo.PangusServiciis",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Single(nullable: false),
                        IsEditable = c.Boolean(nullable: false),
                        dimTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.dimTypes", t => t.dimTypeID, cascadeDelete: true)
                .Index(t => t.dimTypeID);
            
            CreateTable(
                "dbo.Firmes",
                c => new
                    {
                        FirmeID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Ro = c.String(),
                        CUI = c.String(),
                        Adresa = c.String(),
                    })
                .PrimaryKey(t => t.FirmeID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.PangusServiciis", "dimTypeID", "dbo.dimTypes");
            DropForeignKey("dbo.CustomerServices", "PangusServicii_ID", "dbo.PangusServiciis");
            DropForeignKey("dbo.Mains", "PaymentMethodID", "dbo.PaymentMethods");
            DropForeignKey("dbo.Mains", "DepozitareID", "dbo.Depozitares");
            DropForeignKey("dbo.CustomerServices", "MainID", "dbo.Mains");
            DropForeignKey("dbo.Mains", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.Mains", "CarDetailID", "dbo.CarDetails");
            DropForeignKey("dbo.Mains", "AnvelopeNoiID", "dbo.AnvelopeNois");
            DropForeignKey("dbo.CarDetails", "CarTypeID", "dbo.CarTypes");
            DropForeignKey("dbo.AnvelopeNois", "ProfilID", "dbo.Profils");
            DropForeignKey("dbo.Depozitares", "ProfilID", "dbo.Profils");
            DropForeignKey("dbo.Profils", "AnvelopeID", "dbo.Anvelopes");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.PangusServiciis", new[] { "dimTypeID" });
            DropIndex("dbo.CustomerServices", new[] { "PangusServicii_ID" });
            DropIndex("dbo.CustomerServices", new[] { "MainID" });
            DropIndex("dbo.Mains", new[] { "DepozitareID" });
            DropIndex("dbo.Mains", new[] { "AnvelopeNoiID" });
            DropIndex("dbo.Mains", new[] { "CarDetailID" });
            DropIndex("dbo.Mains", new[] { "PaymentMethodID" });
            DropIndex("dbo.Mains", new[] { "CustomerID" });
            DropIndex("dbo.CarDetails", new[] { "CarTypeID" });
            DropIndex("dbo.Depozitares", new[] { "ProfilID" });
            DropIndex("dbo.Profils", new[] { "AnvelopeID" });
            DropIndex("dbo.AnvelopeNois", new[] { "ProfilID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Firmes");
            DropTable("dbo.PangusServiciis");
            DropTable("dbo.dimTypes");
            DropTable("dbo.PaymentMethods");
            DropTable("dbo.CustomerServices");
            DropTable("dbo.Mains");
            DropTable("dbo.Customers");
            DropTable("dbo.CarTypes");
            DropTable("dbo.CarDetails");
            DropTable("dbo.Depozitares");
            DropTable("dbo.Anvelopes");
            DropTable("dbo.Profils");
            DropTable("dbo.AnvelopeNois");
        }
    }
}
