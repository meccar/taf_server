using Domain.Aggregates;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Enums;

namespace Persistance.Configurations.Entity;
public class UserAccountConfiguration : IEntityTypeConfiguration<UserAccountAggregate>
{
    public void Configure(EntityTypeBuilder<UserAccountAggregate> builder)
    {
        builder
            .Property(x => x.EId)
            .IsRequired(false);
        
        builder
            .Property(x => x.UserProfileId)
            .IsRequired();
        
        builder
            .Property(x => x.Email)
            .IsRequired();
        
        builder
            .Property(x => x.PasswordRecoveryToken)
            .IsRequired(false);
        
        builder
            .Property(x => x.ConfirmationToken)
            .IsRequired(false);
        
        // builder
        //     .Property(x => x.TwoFactorSecret)
        //     .IsRequired(false);
        
        // builder
        //     .Property(x => x.EmailStatus)
        //     .IsRequired(false)
        //     .HasDefaultValue(EEmailStatus.Pending.ToString());
        
        builder
            .HasOne(x => x.UserProfile)
            .WithOne(x => x.UserAccount)
            .HasForeignKey<UserAccountAggregate>(x => x.UserProfileId)
            // .HasPrincipalKey<UserProfileAggregate>(x => x.Id)
            .IsRequired();

    }
}
