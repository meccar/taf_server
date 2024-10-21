using taf_server.Infrastructure.SeedWork.Entities;

namespace taf_server.Infrastructure.Entities;
public class BlacklistTokenEntity : BaseEntity
{
    public int Id { get; set; }
    public int UserAccountId { get; set; }
    public required string Token { get; set; }
    public DateTime ExpiredAt { get; set; }
    public required UserAccountEntity User { get; set; }
    //public required BaseEntity baseEntity { get; set; }
}