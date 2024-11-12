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

    public UserTokenModel(string userAccountId, UserTokenType name, string loginProvider, string value)
    {
        UserAccountId = userAccountId;
        Name = name;
        LoginProvider = loginProvider;
        Value = value;
    }
}
