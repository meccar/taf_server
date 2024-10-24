using taf_server.Domain.SeedWork.Enums.UserLoginDataExternal;
using taf_server.Infrastructure.SeedWork.Entities;

namespace taf_server.Infrastructure.Entities;
public class UserLoginDataExternalEntity : BaseEntity
{
    public int Id { get; set; }
    public string Uuid { get; set; } = "";
    public string SsocialId { get; set; } = "";
    public Provider Provider { get; set; }
    public string Token { get; set; } = "";
    public DateTime EpiredAt { get; set; }
    public UserAccountEntity? UserAccount { get; set; }
}