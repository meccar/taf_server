// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

namespace IdentityServer.Pages.Consent;

/// <summary>
/// Provides configuration options related to user consent during the backchannel authentication process.
/// These options help control the behavior and messages displayed during the consent process.
/// </summary>
public static class ConsentOptions
{
    /// <summary>
    /// Gets a value indicating whether offline access is enabled for consent requests.
    /// If set to <c>true</c>, users will be asked for consent to allow offline access to their resources.
    /// Default is <c>true</c>.
    /// </summary>
    public static readonly bool EnableOfflineAccess = true;

    /// <summary>
    /// Gets the display name for offline access consent.
    /// This is the name that will be shown to the user when asking for offline access permissions.
    /// Default is "Offline Access".
    /// </summary>
    public static readonly string OfflineAccessDisplayName = "Offline Access";

    /// <summary>
    /// Gets the description for offline access consent.
    /// This description explains to the user what "Offline Access" means in the context of their consent.
    /// Default is "Access to your applications and resources, even when you are offline".
    /// </summary>
    public static readonly string OfflineAccessDescription = "Access to your applications and resources, even when you are offline";

    /// <summary>
    /// Gets the error message displayed when the user has not selected at least one permission during the consent process.
    /// Default is "You must pick at least one permission".
    /// </summary>
    public static readonly string MustChooseOneErrorMessage = "You must pick at least one permission";

    /// <summary>
    /// Gets the error message displayed when the user makes an invalid selection during the consent process.
    /// Default is "Invalid selection".
    /// </summary>
    public static readonly string InvalidSelectionErrorMessage = "Invalid selection";
}