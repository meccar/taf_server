// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Ciba;

/// <summary>
/// Represents the page model for displaying all pending backchannel authentication login requests.
/// This page is accessible only to authenticated users.
/// </summary>
[SecurityHeaders]
[Authorize]
public class AllModel : PageModel
{
    /// <summary>
    /// Gets or sets the collection of pending backchannel user login requests for the current user.
    /// </summary>
    public IEnumerable<BackchannelUserLoginRequest> Logins { get; set; } = default!;

    private readonly IBackchannelAuthenticationInteractionService _backchannelAuthenticationInteraction;

    /// <summary>
    /// Initializes a new instance of the <see cref="AllModel"/> class.
    /// </summary>
    /// <param name="backchannelAuthenticationInteractionService">The service to interact with backchannel authentication.</param>
    public AllModel(IBackchannelAuthenticationInteractionService backchannelAuthenticationInteractionService)
    {
        _backchannelAuthenticationInteraction = backchannelAuthenticationInteractionService;
    }

    /// <summary>
    /// Handles the GET request for the All page. Retrieves all pending backchannel login requests for the current user.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task OnGet()
    {
        // Retrieve pending backchannel login requests for the current user
        Logins = await _backchannelAuthenticationInteraction.GetPendingLoginRequestsForCurrentUserAsync();
    }
}