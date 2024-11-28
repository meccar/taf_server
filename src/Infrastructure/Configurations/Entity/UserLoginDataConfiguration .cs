﻿using Domain.Entities;
using Domain.SeedWork.Enums.UserLoginData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Entity;
public class UserLoginDataConfiguration : IEntityTypeConfiguration<UserLoginDataEntity>
{
    public void Configure(EntityTypeBuilder<UserLoginDataEntity> builder)
    {
        builder
            .Property(x => x.EId)
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
        
        builder
            .Property(x => x.EmailStatus)
            .IsRequired(false)
            .HasDefaultValue(EEmailStatus.Pending.ToString());
    }
}
