using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using taf_server.Domain.Aggregates;
using System.Reflection;

namespace taf_server.Infrastructure.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<UserAccountAggregate>(options)
{
    #region UserAccountAggregate

    public DbSet<UserAccountAggregate> UserAccount { set; get; }
    public DbSet<UserLoginDataAggregate> UserLoginData { set; get; }

    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
