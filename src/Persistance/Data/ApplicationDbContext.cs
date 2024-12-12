using System.Reflection;
using Domain.Aggregates;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Data;

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
    public DbSet<UserProfileAggregate> UserProfile { get; set; }
    /// <summary>
    /// Gets or sets the <see cref="DbSet{UserAccountAggregate}"/> for user login data.
    /// </summary>
    public DbSet<UserAccountAggregate> UserAccount { get; set; }

    #endregion
    public DbSet<NewsAggregate> News { get; set; }

    /// <summary>
    /// Configures the model and applies configurations from the current assembly.
    /// </summary>
    /// <param name="builder">The <see cref="ModelBuilder"/> used to configure the model.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
