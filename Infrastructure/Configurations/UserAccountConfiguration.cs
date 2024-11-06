using Domain.Aggregates;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
public sealed class UserAccountConfiguration : IEntityTypeConfiguration<UserAccountAggregate>
{
    public void Configure(EntityTypeBuilder<UserAccountAggregate> builder)
    {
        builder
            .Property(u => u.Id)
            .ValueGeneratedOnAdd()
            .HasAnnotation("SqlServer:Identity", "1, 1");
        
        builder
            .Property(x => x.Uuid)
            // .HasConversion<UlidToStringConverter>()
            .IsRequired(false);

        builder 
            .Property(x => x.FirstName)
            .IsRequired();

        builder
            .Property(x => x.LastName)
            .IsRequired();

        builder
            .Property(x => x.Avatar);
        builder
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .HasQueryFilter(x => !x.IsDeleted);
    }
}