using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Model;
using Domain.SeedWork.Enums.Token;
using Domain.SeedWork.Enums.UserLoginDataExternal;
using Infrastructure.Configurations.Environment;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Management;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Infrastructure.Repositories.Service;

public class JwtService : IJwtService
{
    private readonly IMapper _mapper;
    private readonly EnvironmentConfiguration _environment;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<UserLoginDataEntity> _userManager;
    private readonly JwtSecurityTokenHandler _jwtHandler;
    private readonly byte[] _secret;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public JwtService(
        EnvironmentConfiguration environment,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        UserManager<UserLoginDataEntity> userManager
        )
    {
        _mapper = mapper;
        _environment = environment;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _jwtHandler = new JwtSecurityTokenHandler();
        _secret = Encoding.UTF8.GetBytes(environment.GetJwtSecret());
        _tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(_secret),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    }

    public async Task<TokenModel> GenerateAuthResponseWithRefreshTokenCookie(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        
        var token = await GenerateTokenPair(user);
        
        token.LoginProvider = EProvider.PASSWORD;
        
        var existingTokens = await _unitOfWork.UserTokenQueryRepository
            .TokenExistsAsync(user, token);
        
        if (!existingTokens)
        {
            await CreateNewTokens(user, token);
        }
        else
        {
            var result = await _unitOfWork.UserTokenCommandRepository.RemoveLoginAndAuthenticationTokenAsync(user, token);
            
            if(!result)
                throw new InvalidOperationException("Failed to create user tokens");

            await UpdateExistingTokens(user, token);
        }
        
        return new TokenModel(
            token.Token.TokenType,
            token.Token.AccessToken,
            token.Token.AccessTokenExpires,
            token.Token.RefreshToken,
            token.Token.RefreshTokenExpires
            );
    }

    private async Task<UserTokenModel> GenerateTokenPair(UserLoginDataEntity user)
    {
        var claims = await CreateUserClaims(user);
        var tokenType = _environment.GetJwtType();
        
        var (accessToken, accessExpiration) = await CreateAccessToken(claims);
        var (refreshToken, refreshExpiration) = await CreateRefreshToken(claims);

        return new UserTokenModel(null, null, null, null)
        {
            Token = new TokenModel(
                tokenType,
                accessToken,
                accessExpiration.ToString(),
                refreshToken,
                refreshExpiration.ToString()
            )
        };
    }
    
    private async Task<ClaimsIdentity> CreateUserClaims(UserLoginDataEntity user)
    {
        // var systemInfo = GetSystemInfo();
        // var localIpAddress = GetLocalIPAddress();

        return new ClaimsIdentity(new[]
        {
            new Claim("id", user.UserAccountId),
            new Claim(ClaimTypes.Email, user.Email),
            // new Claim("SystemInfo", systemInfo),
            // new Claim("LocalIPAddress", localIpAddress)
        });
    }
    
    private async Task CreateNewTokens(UserLoginDataEntity user, UserTokenModel token)
    {
        var accessToken = await _unitOfWork.UserTokenCommandRepository.CreateUserTokenAsync(
            user,
            new UserTokenModel(
                user.UserAccountId,
                ETokenName.ACCESS,
                token.LoginProvider,
                token.Token.AccessToken
            )
        );

        var refreshToken = await _unitOfWork.UserTokenCommandRepository.CreateUserTokenAsync(
            user,
            new UserTokenModel(
                user.UserAccountId,
                ETokenName.REFRESH,
                token.LoginProvider,
                token.Token.RefreshToken
            )
        );
                
        if (accessToken == null || refreshToken == null)
        {
            throw new InvalidOperationException("Failed to create user tokens");
        }
    }

    private async Task UpdateExistingTokens(UserLoginDataEntity user, UserTokenModel token)
    {
        // TODO: Implement token blacklisting logic here
        
        var accessToken = await _unitOfWork.UserTokenCommandRepository.UpdateUserTokenAsync(
            user,
            new UserTokenModel(
                user.UserAccountId,
                ETokenName.ACCESS,
                token.LoginProvider,
                token.Token.AccessToken
            )
        );

        var refreshToken = await _unitOfWork.UserTokenCommandRepository.UpdateUserTokenAsync(
            user,
            new UserTokenModel(
                user.UserAccountId,
                ETokenName.REFRESH,
                token.LoginProvider,
                token.Token.RefreshToken
            )
        );
        
        if (accessToken == null || refreshToken == null)
        {
            throw new InvalidOperationException("Failed to update user tokens");
        }
    }
    
    private async Task<(string token, TimeSpan expiration)> CreateAccessToken(ClaimsIdentity claims)
    {
        var expiration = TimeSpan.FromHours(_environment.GetJwtExpirationTime());
        
        var token = await GenerateToken(claims, expiration);
        return (token, expiration);
    }
    
    private async Task<(string token, TimeSpan expiration)> CreateRefreshToken(ClaimsIdentity claims)
    {
        var expiration = TimeSpan.FromHours(_environment.GetJwtRefreshExpirationTime());
        
        var token = await GenerateToken(claims, expiration);
        return (token, expiration);
    }
    
    private async Task<string> GenerateToken(ClaimsIdentity claims, TimeSpan expiration)
    {
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(_secret),
            SecurityAlgorithms.HmacSha256);

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.Add(expiration)
        };

        var token = _jwtHandler.CreateToken(descriptor);
        return _jwtHandler.WriteToken(token);
    }

    private static string GetSystemInfo()
    {
        try
        {
            var components = new[]
            {
                // OS info
                RuntimeInformation.OSDescription,
                RuntimeInformation.OSArchitecture.ToString(),
                
                // Machine name
                Environment.MachineName,
                
                // Network identity
                GetMacAddress(),
                
                // Process info
                Environment.ProcessorCount.ToString(),
                
                // Runtime info
                RuntimeInformation.FrameworkDescription
            };

            // Combine all components
            string deviceFingerprint = string.Join(":", components);

            // Hash the fingerprint
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(deviceFingerprint));
                return Convert.ToBase64String(hashBytes);
            }
        }
        catch (Exception)
        {
            // Return a fallback value if anything fails
            return "unknown";
        }
    }

    private static string GetLocalIPAddress()
    {
        string hostName = Dns.GetHostName();
        IPHostEntry hostEntry = Dns.GetHostEntry(hostName);
            
        foreach (IPAddress ip in hostEntry.AddressList)
        {
            // Only return IPv4 addresses
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return "unknown";
    }
    
    private static string GetMacAddress()
    {
        try
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider physical network adapters that are up
                if (nic.OperationalStatus == OperationalStatus.Up && 
                    nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    return BitConverter.ToString(nic.GetPhysicalAddress().GetAddressBytes())
                        .Replace("-", "");
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
        return string.Empty;
    }
    
    private string CreateRefreshTokenCookie(string refreshToken)
    {
        var cookieKey = _environment.GetJwtRefreshCookieKey();
        var maxAge = _environment.GetJwtRefreshTokenCookieMaxAge();
        
        return $"{cookieKey}={refreshToken}; HttpOnly; SameSite=None; Secure; Path=/; Max-Age={maxAge};";
    }
} 