using Domain.SeedWork.Enums.Token;

namespace Domain.Model;

public class UserTokenModel
{
    public string UserAccountId { get; set; }
    public UserTokenType Name { get; set; }
    //public string AccessToken { get; set; }
    //public string AccessTokenExpires { get; set; }
    //public string RefreshToken { get; set; }
    //public string RefreshTokenExpires { get; set; }
    public string LoginProvider { get; set; }
    public string Value { get; set; }
    public UserAccountModel UserAccount { get; set; }

}
