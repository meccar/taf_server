using Domain.SeedWork.Enums.UserLoginDataExternal;
using Infrastructure.SeedWork.Entities;

namespace Infrastructure.Entities;
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