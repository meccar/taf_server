using Domain.Aggregates;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Enums;

namespace DataBase.Configurations.Entity;
public class UserLoginDataConfiguration : IEntityTypeConfiguration<UserAccountAggregate>
{
    public void Configure(EntityTypeBuilder<UserAccountAggregate> builder)
    {
        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder
            .Property(x => x.EId)
            .IsRequired(false);
        
        builder
            .Property(x => x.UserAccountId)
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
        
        builder
            .Property(x => x.TwoFactorSecret)
            .IsRequired(false);
        
        builder
            .Property(x => x.EmailStatus)
            .IsRequired(false)
            .HasDefaultValue(EEmailStatus.Pending.ToString());
        
        builder
            .HasOne(x => x.UserAccount)
            .WithOne(x => x.UserLoginData)
            .HasForeignKey<UserAccountAggregate>(x => x.UserAccountId)
            .HasPrincipalKey<UserProfileAggregate>(x => x.Id)
            .IsRequired();

    }
}
