// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer.Models;

namespace IdentityServer.Pages.Home.Error;

/// <summary>
/// Represents the view model that holds error information for displaying on the error page.
/// </summary>
public class ViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ViewModel"/> class.
    /// </summary>
    public ViewModel()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ViewModel"/> class with an error message.
    /// </summary>
    /// <param name="error">The error message to be displayed.</param>
    public ViewModel(string error)
    {
        Error = new ErrorMessage { Error = error };
    }

    /// <summary>
    /// Represents an error message that can be displayed to the user.
    /// </summary>
    public ErrorMessage? Error { get; set; }
}