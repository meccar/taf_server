using Domain.SeedWork.Enums.Token;
using Domain.SeedWork.Enums.UserLoginDataExternal;

namespace Domain.Model;

public class UserTokenModel
{
    public string EId { get; set; }

    public string UserId { get; set; }
    public ETokenName? Name { get; set; }
    //public string AccessToken { get; set; }
    //public string AccessTokenExpires { get; set; }
    //public string RefreshToken { get; set; }
    //public string RefreshTokenExpires { get; set; }
    public EProvider? LoginProvider { get; set; }
    public string? Value { get; set; }
    public UserAccountModel? UserAccount { get; set; }
    public TokenModel Token { get; set; }
    public UserTokenModel(string userId, ETokenName? name, EProvider? loginProvider,string? value)
    {
        UserId = userId;
        Name = name;
        LoginProvider = loginProvider;
        Value = value;
    }
}
