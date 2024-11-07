// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using Domain.Interfaces;
// using Domain.Model;
// using Microsoft.IdentityModel.Tokens;
//
// namespace Infrastructure.Services.JwtToken;
//
// public class JwtService : IJwtService
// {
//     public string GenerateToken(
//         UserLoginDataModel userLoginDataModel,
//         bool hasVerify2FA)
//     {
//         var handler = new JwtSecurityTokenHandler();
//         var key = Encoding.ASCII.GetBytes(AuthSettings.PrivateKey);
//         var credentials = new SigningCredentials(
//             new SymmetricSecurityKey(key),
//             SecurityAlgorithms.HmacSha256Signature);
//
//         var tokenDescriptor = new SecurityTokenDescriptor
//         {
//             Subject = GenerateClaims(userLoginDataModel),
//             Expires = DateTime.UtcNow.AddMinutes(15),
//             SigningCredentials = credentials,
//         };
//
//         var token = handler.CreateToken(tokenDescriptor);
//         return handler.WriteToken(token);
//     }
//     
//     private static ClaimsIdentity GenerateClaims(UserLoginDataModel userLoginDataModel)
//     {
//         var claims = new ClaimsIdentity();
//         claims.AddClaim(new Claim(ClaimTypes.Name, userLoginDataModel.Email));
//
//         foreach (var role in userLoginDataModel.Roles)
//             claims.AddClaim(new Claim(ClaimTypes.Role, role));
//
//         return claims;
//     }
//     
//     public async Task<string> ResponseAuthWithAccessTokenAndRefreshTokenCookie(
//         UserAccountModel userAccountModel,
//         bool hasVerify2FA = false)
//     {
//         
//     }
// }