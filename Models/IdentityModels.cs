﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Migrations.History;

namespace OnlineLibrary.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
	[DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
			
        }

		static ApplicationDbContext( ) {
			DbConfiguration.SetConfiguration(new MySql.Data.Entity.MySqlEFConfiguration( ));
		}

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

		protected override void OnModelCreating( DbModelBuilder modelBuilder ) {
			base.OnModelCreating(modelBuilder);
			
			modelBuilder.Entity<IdentityRole>( ).HasKey<string>(r => r.Id);
			modelBuilder.Entity<IdentityUserRole>( ).HasKey(r => new { r.RoleId, r.UserId });
			modelBuilder.Entity<ApplicationUser>( ).Property(u => u.UserName).HasMaxLength(128).IsRequired( );
			modelBuilder.Entity<IdentityRole>( ).Property(u => u.Name).HasMaxLength(128).IsRequired( );
			modelBuilder.Entity<ApplicationUser>( ).Property(u => u.Email).HasMaxLength(128).IsRequired( );



		}

	}

	public class ApplicationUserInitializer : CreateDatabaseIfNotExists<ApplicationDbContext> {
		protected override void Seed( ApplicationDbContext context ) {
			context.SaveChanges( );
		}
    }

}