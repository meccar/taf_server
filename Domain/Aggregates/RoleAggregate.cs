using Domain.SeedWork.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Domain.Aggregates;
public class RoleAggregate : IdentityRole<int>, IDateTracking
{
    public bool IsDeleted { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
