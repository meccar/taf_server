// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using System.Reflection;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages;

/// <summary>
/// The page model for the Index page, which displays information about the IdentityServer version and license.
/// </summary>
[AllowAnonymous]
public class Index : PageModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Index"/> class.
    /// </summary>
    /// <param name="license">An optional <see cref="IdentityServerLicense"/> object that represents the license details.</param>
    public Index(IdentityServerLicense? license = null)
    {
        License = license;
    }

    /// <summary>
    /// Gets the version of the IdentityServer middleware, which is read from the assembly's informational version.
    /// </summary>
    /// <value>
    /// The version string of the IdentityServer middleware, or "unavailable" if the version cannot be determined.
    /// </value>
    public string Version
    {
        get => typeof(Duende.IdentityServer.Hosting.IdentityServerMiddleware).Assembly
                   .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                   ?.InformationalVersion.Split('+').First()
               ?? "unavailable";
    }

    /// <summary>
    /// Gets the license information for the IdentityServer.
    /// </summary>
    /// <value>
    /// An optional <see cref="IdentityServerLicense"/> object that holds the licensing information, or <c>null</c> if no license is provided.
    /// </value>
    public IdentityServerLicense? License { get; }
}
