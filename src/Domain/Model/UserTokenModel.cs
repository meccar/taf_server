// using System.Security.Claims;
// using Domain.SeedWork.Enums.Token;
// using Domain.SeedWork.Enums.UserLoginDataExternal;
//
// namespace Domain.Model;
//
// public class UserTokenModel
// {
//     public string EId { get; set; }
//
//     public int? UserId { get; set; }
//     public ETokenName? Name { get; set; }
//     public List<Claim> Claims { get; set; }
//     public EProvider? LoginProvider { get; set; }
//     public string? Value { get; set; }
//     public UserAccountModel? UserAccount { get; set; }
//     public TokenModel Token { get; set; }
//     public UserTokenModel(int? userId, ETokenName? name, EProvider? loginProvider,string? value, List<Claim>? claim)
//     {
//         UserId = userId;
//         Name = name;
//         LoginProvider = loginProvider;
//         Value = value;
//         Claims = claim;
//     }
// }
