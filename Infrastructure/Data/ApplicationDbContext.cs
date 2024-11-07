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
    : IdentityDbContext<
        UserLoginDataEntity,
        IdentityRole<Guid>,
        Guid,
        IdentityUserClaim<Guid>,
        IdentityUserRole<Guid>,
        IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>,
        IdentityUserToken<Guid>>
{
    
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
        
        builder.Entity<UserLoginDataEntity>()
            .HasOne(u => u.UserAccount)
            .WithOne(u => u.UserLoginData)
            .HasForeignKey<UserLoginDataEntity>(u => u.UserAccountId)
            .HasPrincipalKey<UserAccountAggregate>(u => u.Id)
            .IsRequired();
        
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
