using System.Reflection;
using DataBase.Configurations.Entity;
using Domain.Aggregates;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Data;

/// <summary>
/// Represents the application database context, inheriting from <see cref="IdentityDbContext"/>.
/// </summary>
/// <remarks>
/// This context is responsible for interacting with the database, managing the user account 
/// and login data aggregates, and configuring entity mappings. It also applies configurations 
/// from the executing assembly.
/// </remarks>
public class ApplicationDbContext 
    : IdentityDbContext<
        UserAccountAggregate,
        IdentityRole<int>,
        int,
        IdentityUserClaim<int>,
        IdentityUserRole<int>,
        IdentityUserLogin<int>,
        IdentityRoleClaim<int>,
        IdentityUserToken<int>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by the <see cref="DbContext"/>.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    #region UserProfileAggregate
    /// <summary>
    /// Gets or sets the <see cref="DbSet{UserProfileAggregate}"/> for user accounts.
    /// </summary>
    public DbSet<UserProfileAggregate> UserAccount { get; set; }
    /// <summary>
    /// Gets or sets the <see cref="DbSet{UserAccountAggregate}"/> for user login data.
    /// </summary>
    public DbSet<UserAccountAggregate> UserLoginData { get; set; }

    #endregion

    /// <summary>
    /// Configures the model and applies configurations from the current assembly.
    /// </summary>
    /// <param name="builder">The <see cref="ModelBuilder"/> used to configure the model.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // builder.Entity<UserProfileAggregate>()
        //     .HasQueryFilter(u => u.Status != UserAccountStatus.Inactive.ToString());
        //
        // builder.Entity<UserAccountAggregate>()
        //     .HasQueryFilter(u => u.UserAccount.Status != UserAccountStatus.Inactive.ToString());
        
        // builder.Entity<UserAccountAggregate>()
        //     .HasOne(u => u.UserAccount)
        //     .WithOne(u => u.UserLoginData)
        //     .HasForeignKey<UserAccountAggregate>(u => u.UserAccountId)
        //     .HasPrincipalKey<UserProfileAggregate>(u => u.Id)
        //     .IsRequired();

        // builder
        //     .Entity<UserAccountAggregate>()
        //     .HasMany(u => u.UserToken)
        //     .WithOne()
        //     .HasForeignKey(u => u.UserId);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
