using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using static Sellopedia.Models.EnumerationsClass;

namespace Sellopedia.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        //--------- User
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public AccountType AccountType { get; set; }
        public string ProfileImage { get; set; }

        //------- Navigation Properties
        //--- Product
        public virtual ICollection<Product> Products { get; set; }
        //--- Order
        public virtual ICollection<Order> Orders { get; set; }
        //--- Review
        public virtual ICollection<Review> Reviews { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        object placeHolderVariable;
        public System.Data.Entity.DbSet<Sellopedia.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<Sellopedia.Models.Order> Orders { get; set; }

        public System.Data.Entity.DbSet<Sellopedia.Models.Review> Reviews { get; set; }

        public System.Data.Entity.DbSet<Sellopedia.Models.ProductImage> ProductImages { get; set; }

        //public System.Data.Entity.DbSet<Sellopedia.Models.ApplicationUser> ApplicationUsers { get; set; }

        //public System.Data.Entity.DbSet<Sellopedia.Models.ApplicationUser> ApplicationUsers { get; set; }
        //public System.Data.Entity.DbSet<Sellopedia.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}