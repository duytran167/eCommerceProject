using eCommerceProject.Enums;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace eCommerceProject.Models
{
	// You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
	public class ApplicationUser : IdentityUser
	{
		public string Address { get; set; }
		public string FullName { get; set; }
		public int StatusID { get; set; }
		[DisplayName("Upload File")]
		public string ImagePath { get; set; }

		[NotMapped]
		public HttpPostedFileBase ImageFile { get; set; }
		public AccountStatus Status { get; set; }

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
		public DbSet<Customer> Customers { get; set; }
		public DbSet<BannerSlider> BannerSliders { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Seller> Sellers { get; set; }
		public DbSet<BlogPost> BlogPosts { get; set; }
		public DbSet<BlogCategory> BlogCategories { get; set; }
		public DbSet<ImageProduct> ImageProducts { get; set; }
		public DbSet<Comments> Comments { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Size> Sizes { get; set; }

		public static ApplicationDbContext Create()
		{
			return new ApplicationDbContext();
		}


	}
}
