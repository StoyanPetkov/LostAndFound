using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections;
using System.Collections.Generic;

namespace LF.Models
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

        [Required]
        public string CityId { get; set; }

        public City City { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }

    public class City
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid CityId { get; set; }

        public bool IsCity { get; set; }

        [Required]
        [MaxLength(255)]
        public string CityName { get; set; }

        [Required]
        public Guid RegionId { get; set; }

        [ForeignKey("RegionId")]
        public Region Region { get; set; }
    }

    public class Region
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid RegionId { get; set; }

        [Required]
        [MaxLength(255)]
        public string RegionName { get; set; }

        [Required]
        public Guid CountryId { get; set; }

        public Country Country { get; set; }
    }

    public class Country
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid CountryId { get; set; }

        [Required]
        [MaxLength(255)]
        public string CountryName { get; set; }
    }

    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string CategoryName { get; set; }

        public Guid? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public Category ParentCategory { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }

    public class Item
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public string UserId { get; set; }

        public Guid? CommentId { get; set; }

        [Required]
        public Guid CityId { get; set; }

        [ForeignKey("UserId")]
        [Required]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("CommentId")]
        public virtual Comment Comments{ get; set; }

        [ForeignKey("CategoryId")]
        [Required]
        public virtual Category Category { get; set; }

        [ForeignKey("CityId")]
        [Required]
        public virtual City City { get; set; }

        [Required]
        [MaxLength(255)]
        public string ItemName { get; set; }

        [Required]
        public bool IsLost { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public byte? Size { get; set; }

        public float? RewardValue { get; set; }

        public string ImagesLocation { get; set; }

    }

    public class Comment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public Guid ItemId { get; set; }

        public virtual Comment ParentComment { get; set; }

        public virtual ApplicationUser User { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("LostAndFound", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}