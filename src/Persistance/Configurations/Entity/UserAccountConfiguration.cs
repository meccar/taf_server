using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Enums;

namespace Persistance.Configurations.Entity;
public sealed class UserAccountConfiguration : IEntityTypeConfiguration<UserProfileAggregate>
{
    public void Configure(EntityTypeBuilder<UserProfileAggregate> builder)
    {
        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        
        builder
            .Property(x => x.EId)
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
            .Property(x => x.Status)
            .IsRequired()
            .HasDefaultValue(EUserAccountStatus.Inactive.ToString());
        
        builder
            .HasQueryFilter(x => !x.IsDeleted);
            // .HasQueryFilter(x => x.Status != UserAccountStatus.Inactive);
    }
}