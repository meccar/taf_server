﻿using Microsoft.AspNetCore.Identity;
using System.Security;
using taf_server.Domain.SeedWork.Interfaces;

namespace taf_server.Domain.Aggregates;
public class RoleAggregate : IdentityRole, IDateTracking
{
    public bool IsDeleted { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
