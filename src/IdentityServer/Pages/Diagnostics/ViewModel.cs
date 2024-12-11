// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using System.Text;
using System.Text.Json;

namespace IdentityServer.Pages.Diagnostics;

/// <summary>
/// Represents a view model containing the authentication result and associated client list.
/// </summary>
public class ViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ViewModel"/> class.
    /// </summary>
    /// <param name="result">The authentication result containing the authentication details.</param>
    public ViewModel(AuthenticateResult result)
    {
        AuthenticateResult = result;

        if (result.Properties?.Items.TryGetValue("client_list", out var encoded) == true)
        {
            if (encoded != null)
            {
                var bytes = Base64Url.Decode(encoded);
                var value = Encoding.UTF8.GetString(bytes);
                Clients = JsonSerializer.Deserialize<string[]>(value) ?? Enumerable.Empty<string>();
                return;
            }
        }
        Clients = Enumerable.Empty<string>();
    }

    /// <summary>
    /// Gets the authentication result.
    /// </summary>
    public AuthenticateResult AuthenticateResult { get; }
    
    /// <summary>
    /// Gets the list of clients associated with the authentication result.
    /// </summary>
    public IEnumerable<string> Clients { get; }
}