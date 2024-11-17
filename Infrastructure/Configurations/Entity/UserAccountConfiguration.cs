using Domain.Aggregates;
using Domain.SeedWork.Enums.UserAccount;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Entity;
public sealed class UserAccountConfiguration : IEntityTypeConfiguration<UserAccountAggregate>
{
    public void Configure(EntityTypeBuilder<UserAccountAggregate> builder)
    {
        builder
            .Property(u => u.Id)
            .ValueGeneratedOnAdd()
            .HasAnnotation("SqlServer:Identity", "1, 1");
        
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
            .HasDefaultValue(UserAccountStatus.Inactive.ToString());
        
        builder
            .HasQueryFilter(x => !x.IsDeleted);
    }
}