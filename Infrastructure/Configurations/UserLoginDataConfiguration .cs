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
            // .HasConversion<UlidToStringConverter>()
            .IsRequired(false);
        
        builder
            .Property(x => x.Email)
            .IsRequired();
        
        builder
            .Property(x => x.PasswordRecoveryToken)
            .IsRequired(false);
        
        builder
            .Property(x => x.ConfirmationToken)
            .IsRequired(false);
        
        builder
            .Property(x => x.TwoFactorSecret)
            .IsRequired(false);
        
        //
        // builder
        //     .HasIndex(x => x.ConfirmationToken);

        // builder
        //     .HasOne(x => x.UserAccount)
        //     .WithOne(x => x.UserLoginData)
        //     .HasPrincipalKey<UserLoginDataEntity>(x => x.UserAccountId)
        //     .HasForeignKey<UserAccountAggregate>(x => x.Id);
    }
}
