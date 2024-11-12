using Domain.Aggregates;
using Domain.SeedWork.Enums.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class UserTokenEntity : IdentityUserToken<Guid>
{
    [Required]
    public required string UserAccountId { get; set; }
    //public UserTokenType Type { get; set; }
    // public string IpAddress { get; set; } = "";
    // public string UserAgent { get; set; } = "";
    //public string Token { get; set; } = "";
    //public DateTime ExpiredAt { get; set; }

    [ForeignKey("UserAccountId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual UserAccountAggregate UserAccount { get; set; } = null!;

}
