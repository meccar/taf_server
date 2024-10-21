using taf_server.Domain.SeedWork.Enums.UserLoginDataExternal;
using taf_server.Infrastructure.SeedWork.Entities;

namespace taf_server.Infrastructure.Entities;
public class UserLoginDataExternalEntity : BaseEntity
{
    public int Id { get; set; }
    public required string Uuid { get; set; }
    public required string SsocialId { get; set; }
    public Provider Provider { get; set; }
    public required string Token { get; set; }
    public DateTime EpiredAt { get; set; }
    //public required BaseEntity BaseEntity { get; set; }
    public required UserAccountEntity UserAccount { get; set; }
}