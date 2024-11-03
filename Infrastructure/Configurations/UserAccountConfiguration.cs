using Domain.Aggregates;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
public sealed class UserAccountConfiguration : IEntityTypeConfiguration<UserAccountEntity>
{
    public void Configure(EntityTypeBuilder<UserAccountEntity> builder)
    {
        builder
            .Property(x => x.Uuid)
            .IsRequired()
            .HasMaxLength(36);

        builder 
            .Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .Property(x => x.PhoneNumber)
            .HasMaxLength(20);

        builder
            .Property(x => x.Avatar)
            .HasMaxLength(255);

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .HasQueryFilter(x => !x.IsDeleted);

        builder
            .HasOne(x => x.UserLoginData)
            .WithOne(x => x.UserAccount)
            .HasForeignKey<UserLoginDataEntity>(x => x.UserAccountId);
    }
}