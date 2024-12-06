// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

namespace IdentityServer.Pages.Device;

/// <summary>
/// Provides configuration options for device flow consent, including settings for offline access and error messages.
/// </summary>
public static class DeviceOptions
{
    /// <summary>
    /// Gets a value indicating whether offline access is enabled.
    /// </summary>
    public static readonly bool EnableOfflineAccess = true;

    /// <summary>
    /// Gets the display name for the offline access scope.
    /// </summary>
    public static readonly string OfflineAccessDisplayName = "Offline Access";

    /// <summary>
    /// Gets the description for the offline access scope.
    /// </summary>
    public static readonly string OfflineAccessDescription = "Access to your applications and resources, even when you are offline";

    /// <summary>
    /// Gets the error message displayed when an invalid user code is provided.
    /// </summary>
    public static readonly string InvalidUserCode = "Invalid user code";

    /// <summary>
    /// Gets the error message displayed when no permissions are selected.
    /// </summary>
    public static readonly string MustChooseOneErrorMessage = "You must pick at least one permission";

    /// <summary>
    /// Gets the error message displayed when an invalid selection is made.
    /// </summary>
    public static readonly string InvalidSelectionErrorMessage = "Invalid selection";
}