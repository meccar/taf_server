// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Home.Error;

/// <summary>
/// Represents the page model that handles displaying error information to the user.
/// This page retrieves and displays the error details if available.
/// </summary>
[AllowAnonymous]
[SecurityHeaders]
public class Index : PageModel
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IWebHostEnvironment _environment;

    /// <summary>
    /// Gets or sets the view model that contains the error details to display on the page.
    /// </summary>
    public ViewModel View { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Index"/> class.
    /// </summary>
    /// <param name="interaction">The interaction service for retrieving error context.</param>
    /// <param name="environment">The environment service to check if the app is in development mode.</param>
    public Index(IIdentityServerInteractionService interaction, IWebHostEnvironment environment)
    {
        _interaction = interaction;
        _environment = environment;
    }

    /// <summary>
    /// Handles the GET request for the error page.
    /// Retrieves error context from the identity server and sets it to the view model.
    /// In non-development environments, the error description is hidden for security reasons.
    /// </summary>
    /// <param name="errorId">The optional error ID to retrieve specific error details.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task OnGet(string? errorId)
    {
        // Retrieve error details from IdentityServer using the provided error ID
        var message = await _interaction.GetErrorContextAsync(errorId);
        if (message != null)
        {
            View.Error = message;

            // In non-development environments, hide the error description for security reasons
            if (!_environment.IsDevelopment())
            {
                message.ErrorDescription = null;
            }
        }
    }
}
