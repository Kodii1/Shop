using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Models;

namespace Shop.Api.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Item> Item { get; set; } = null!;
    public DbSet<ApplicationUser> User { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        var roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Id = "1",
                Name = "User",
                NormalizedName = "USER",
            },
            new IdentityRole
            {
                Id = "2",
                Name = "Admin",
                NormalizedName = "ADMIN",
            },
        };
        builder.Entity<IdentityRole>().HasData(roles);

        base.OnModelCreating(builder);
    }
}
