using System.Reflection;
using Domain.Aggregates;
using Domain.Entities;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

/// <summary>
/// Represents the application database context, inheriting from <see cref="IdentityDbContext"/>.
/// </summary>
/// <remarks>
/// This context is responsible for interacting with the database, managing the user account 
/// and login data aggregates, and configuring entity mappings. It also applies configurations 
/// from the executing assembly.
/// </remarks>
public class ApplicationDbContext 
    : IdentityDbContext<UserAccountAggregate, IdentityRole<int>, int>
{
    
    private void ConfigureUlidProperties(ModelBuilder builder)
    {
        builder.Entity<UserAccountAggregate>()
            .Property(u => u.Uuid)
            .HasConversion<UlidToStringConverter>()
            .IsRequired()
            .HasMaxLength(26);
    
        builder.Entity<UserLoginDataEntity>()
            .Property(u => u.Uuid)
            .HasConversion<UlidToStringConverter>()
            .IsRequired()
            .HasMaxLength(26);
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by the <see cref="DbContext"/>.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    #region UserAccountAggregate
    /// <summary>
    /// Gets or sets the <see cref="DbSet{UserAccountAggregate}"/> for user accounts.
    /// </summary>
    public DbSet<UserAccountAggregate> UserAccount { get; set; }
    /// <summary>
    /// Gets or sets the <see cref="DbSet{UserLoginDataEntity}"/> for user login data.
    /// </summary>
    public DbSet<UserLoginDataEntity> UserLoginData { get; set; }
    
    #endregion

    /// <summary>
    /// Configures the model and applies configurations from the current assembly.
    /// </summary>
    /// <param name="builder">The <see cref="ModelBuilder"/> used to configure the model.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        ConfigureUlidProperties(builder);
        
        // builder.Entity<UserAccountAggregate>().ToTable("Users");
        // builder.Entity<IdentityRole<int>>().ToTable("Roles");
        // builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
        // builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
        // builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
        // builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
        // builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
        
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
