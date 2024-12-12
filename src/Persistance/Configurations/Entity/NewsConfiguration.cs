using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configurations.Entity;

public sealed class NewsConfiguration : IEntityTypeConfiguration<NewsAggregate>
{
    public void Configure(EntityTypeBuilder<NewsAggregate> builder)
    {
        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder
            .Property(x => x.Uuid)
            .ValueGeneratedOnAdd()
            // .IsRequired(false);
            // .HasDefaultValueSql(Ulid.NewUlid().ToString());
            .HasDefaultValueSql("(newid())");

        builder
            .HasQueryFilter(x => !x.IsDeleted);
        
        builder
            .HasOne<UserAccountAggregate>()
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserAccountId)
            // .HasPrincipalKey(u => u.Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<UserAccountAggregate>()
            .WithMany()
            .HasForeignKey(x => x.UpdatedByUserAccountId)
            // .HasPrincipalKey(u => u.Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}