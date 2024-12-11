using System.Security.Claims;
using Shared.Enums;

namespace Shared.Model;

/// <summary>
/// Represents a user token model that stores authentication-related information, including the user ID,
/// token name, claims, login provider, token value, and associated token details.
/// This class is used to store information related to a user's authentication token.
/// </summary>
public class UserTokenModel
{
    /// <summary>
    /// Gets or sets the employee ID (EId) associated with the user.
    /// This field represents a unique identifier for the user, such as an employee ID or other system identifier.
    /// </summary>
    public string? EId { get; set; }

    /// <summary>
    /// Gets or sets the user ID associated with the token.
    /// This field is nullable and can be used to store the unique identifier for the user in the system.
    /// </summary>
    public int? UserId { get; set; }

    /// <summary>
    /// Gets or sets the name of the token.
    /// This field is nullable and can be used to represent the token's type, for example, access or refresh token.
    /// </summary>
    public ETokenName? Name { get; set; }

    /// <summary>
    /// Gets or sets the list of claims associated with the token.
    /// Claims represent pieces of information related to the user (e.g., roles, permissions, etc.) embedded in the token.
    /// </summary>
    public List<Claim> Claims { get; set; } 

    /// <summary>
    /// Gets or sets the login provider associated with the token.
    /// This field is nullable and can be used to identify the provider (e.g., Google, Facebook, custom provider).
    /// </summary>
    public EProvider? LoginProvider { get; set; }

    /// <summary>
    /// Gets or sets the value of the token.
    /// This field is nullable and typically holds the actual string value of the token.
    /// </summary>
    public string? Value { get; set; }
    
    /// <summary>
    /// Gets or sets the associated token model.
    /// This field is required and stores details about the token, such as creation time, expiration time, etc.
    /// </summary>
    public TokenModel? Token { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserTokenModel"/> class.
    /// </summary>
    /// <param name="userId">The unique user identifier associated with the token.</param>
    /// <param name="name">The name of the token (e.g., access or refresh token).</param>
    /// <param name="loginProvider">The login provider used to authenticate the user (e.g., Google, Facebook, etc.).</param>
    /// <param name="value">The value of the token.</param>
    /// <param name="claim">A list of claims associated with the token.</param>
    public UserTokenModel(int? userId, ETokenName? name, EProvider? loginProvider,string? value, List<Claim>? claim)
    {
        UserId = userId;
        Name = name;
        LoginProvider = loginProvider;
        Value = value;
        Claims = claim ?? new List<Claim>();
    }
}
