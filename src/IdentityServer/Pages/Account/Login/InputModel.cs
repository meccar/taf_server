// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Pages.Account.Login;

/// <summary>
/// Represents the input model for the login form on the login page.
/// This model is used to capture the user input for logging in.
/// </summary>
public class InputModel
{
    /// <summary>
    /// Gets or sets the username (email) of the user attempting to log in.
    /// </summary>
    [Required]
    public string? Username { get; set; }

    /// <summary>
    /// Gets or sets the password of the user attempting to log in.
    /// </summary>
    [Required]
    public string? Password { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user wants to be remembered after logging in.
    /// If true, a persistent cookie will be set to keep the user logged in across sessions.
    /// </summary>
    public bool RememberLogin { get; set; }

    /// <summary>
    /// Gets or sets the URL to which the user will be redirected after a successful login.
    /// </summary>
    public string? ReturnUrl { get; set; }

    /// <summary>
    /// Gets or sets the button that was clicked in the login form (e.g., "login", "cancel").
    /// This is used to determine the action taken by the user in the form.
    /// </summary>
    public string? Button { get; set; }
}