using Domain.Aggregates;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
public class UserLoginDataConfiguration : IEntityTypeConfiguration<UserLoginDataEntity>
{
    public void Configure(EntityTypeBuilder<UserLoginDataEntity> builder)
    {
        builder
            .Property(x => x.Uuid)
            .IsRequired()
            .HasMaxLength(36);

        builder
            .Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder
            .Property(x => x.PasswordRecoveryToken)
            .HasMaxLength(100);

        builder
            .Property(x => x.ConfirmationToken)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .HasIndex(x => x.Email).IsUnique();

        builder
            .HasIndex(x => x.ConfirmationToken);
    }
}
