// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

namespace IdentityServer.Pages.Ciba;

/// <summary>
/// Provides configuration options for the consent page in IdentityServer.
/// </summary>
public static class ConsentOptions
{
    /// <summary>
    /// Indicates whether offline access is enabled.
    /// If true, offline access will be presented as a scope during consent.
    /// </summary>
    public static readonly bool EnableOfflineAccess = true;

    /// <summary>
    /// The display name of the offline access scope.
    /// This is shown to the user on the consent page if offline access is enabled.
    /// </summary>
    public static readonly string OfflineAccessDisplayName = "Offline Access";

    /// <summary>
    /// The description of the offline access scope.
    /// This is shown to the user on the consent page if offline access is enabled.
    /// </summary>
    public static readonly string OfflineAccessDescription = "Access to your applications and resources, even when you are offline";

    /// <summary>
    /// The error message displayed when the user has not selected any permissions during consent.
    /// </summary>
    public static readonly string MustChooseOneErrorMessage = "You must pick at least one permission";

    /// <summary>
    /// The error message displayed when the user makes an invalid selection during consent.
    /// </summary>
    public static readonly string InvalidSelectionErrorMessage = "Invalid selection";
}