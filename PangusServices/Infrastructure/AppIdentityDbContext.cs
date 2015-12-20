using Microsoft.AspNet.Identity.EntityFramework;
using PangusServices.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace PangusServices.Infrastructure
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext() : base("IdentityDb") { }

        static AppIdentityDbContext()
        {
            Database.SetInitializer<AppIdentityDbContext>(new IdentityDbInit());
        }

        public static AppIdentityDbContext Create()
        {
            return new AppIdentityDbContext();
        }

        public DbSet<Main> Mains { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<PangusServicii> PangusServiciis { get; set; }
        public DbSet<CustomerService> CustomerServices { get; set; }
        public DbSet<CarType> CarTypes { get; set; }

        public DbSet<Anvelope> Anvelopes { get; set; }
        public DbSet<AnvelopeNoi> AnvelopeNois { get; set; }
        public DbSet<Profil> Profils { get; set; }
        public DbSet<Depozitare> Depozitares { get; set; }
        public DbSet<CarDetail> CarDetails { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<dimType> DimTypes { get; set; }
        public DbSet<Firme> Firmes { get; set; }
        public DbSet<Operator> Operators { get; set; }

        public class IdentityDbInit : DropCreateDatabaseIfModelChanges<AppIdentityDbContext>
        {
            protected override void Seed(AppIdentityDbContext context)
            {
                PerformInitialSetup(context);
                base.Seed(context);
            }

            public void PerformInitialSetup(AppIdentityDbContext context)
            {
                AppUserManager useMgr = new AppUserManager(new UserStore<AppUser>(context));
                AppRoleManager roleMgr = new AppRoleManager(new RoleStore<AppRole>(context));

                string roleName = "Administrator";
                string userName = "Admin";
                string password = "adminadmin";
                string email = "admin@admin.com";

                if (!roleMgr.RoleExists(roleName))
                {
                    roleMgr.Create(new AppRole(roleName));
                }

                AppUser user = useMgr.FindByName(userName);
                if (user == null)
                {
                    useMgr.Create(new AppUser { UserName = userName, Email = email }, password);
                    user = useMgr.FindByName(userName);
                }
                if (!useMgr.IsInRole(user.Id, roleName))
                {
                    useMgr.AddToRole(user.Id, roleName);
                }
            }

        }



    }
}