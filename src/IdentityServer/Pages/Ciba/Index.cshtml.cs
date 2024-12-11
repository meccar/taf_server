// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Ciba;

/// <summary>
/// The page model for handling backchannel authentication requests.
/// </summary>
[AllowAnonymous]
[SecurityHeaders]
public class IndexModel : PageModel
{
    /// <summary>
    /// The backchannel user login request.
    /// This object holds the details of the login request being processed.
    /// </summary>
    public BackchannelUserLoginRequest LoginRequest { get; set; } = default!;

    private readonly IBackchannelAuthenticationInteractionService _backchannelAuthenticationInteraction;
    private readonly ILogger<IndexModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="IndexModel"/> class.
    /// </summary>
    /// <param name="backchannelAuthenticationInteractionService">The service for handling backchannel authentication interactions.</param>
    /// <param name="logger">The logger used for logging errors and information.</param>
    public IndexModel(
        IBackchannelAuthenticationInteractionService backchannelAuthenticationInteractionService,
        ILogger<IndexModel> logger)
    {
        _backchannelAuthenticationInteraction = backchannelAuthenticationInteractionService;
        _logger = logger;
    }

    /// <summary>
    /// Handles the GET request to retrieve the login request by its internal ID.
    /// If the request is not found, an error is logged and the user is redirected to the error page.
    /// </summary>
    /// <param name="id">The internal ID of the backchannel login request.</param>
    /// <returns>The page result, either displaying the login request or redirecting to an error page.</returns>
    public async Task<IActionResult> OnGet(string id)
    {
        var result = await _backchannelAuthenticationInteraction.GetLoginRequestByInternalIdAsync(id);
        if (result == null)
        {
            _logger.InvalidBackchannelLoginId(id);
            return RedirectToPage("/Home/Error/Index");
        }
        else
        {
            LoginRequest = result;
        }
        
        return Page();
    }
}