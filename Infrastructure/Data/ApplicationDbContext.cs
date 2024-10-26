using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using taf_server.Domain.Aggregates;
using System.Reflection;

namespace taf_server.Infrastructure.Data;

/// <summary>
/// Represents the application database context, inheriting from <see cref="IdentityDbContext{TUser}"/>.
/// </summary>
/// <remarks>
/// This context is responsible for interacting with the database, managing the user account 
/// and login data aggregates, and configuring entity mappings. It also applies configurations 
/// from the executing assembly.
/// </remarks>
public class ApplicationDbContext : IdentityDbContext<UserAccountAggregate>
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
    public DbSet<UserAccountAggregate> UserAccount { set; get; }
    /// <summary>
    /// Gets or sets the <see cref="DbSet{UserLoginDataAggregate}"/> for user login data.
    /// </summary>
    public DbSet<UserLoginDataAggregate> UserLoginData { set; get; }

    #endregion
    
    /// <summary>
    /// Configures the model and applies configurations from the current assembly.
    /// </summary>
    /// <param name="builder">The <see cref="ModelBuilder"/> used to configure the model.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}
