using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
public class UserLoginDataConfiguration : IEntityTypeConfiguration<UserLoginDataAggregate>
{
    public void Configure(EntityTypeBuilder<UserLoginDataAggregate> builder)
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
