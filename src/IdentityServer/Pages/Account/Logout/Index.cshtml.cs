// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Domain.Aggregates;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Account.Logout;

/// <summary>
/// Handles the logout process for users, including showing the logout prompt and performing the actual logout.
/// </summary>
[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    private readonly SignInManager<UserAccountAggregate> _signInManager;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IEventService _events;

    /// <summary>
    /// The logout identifier, used to track the logout request context.
    /// </summary>
    [BindProperty] 
    public string? LogoutId { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Index"/> class with the required dependencies.
    /// </summary>
    /// <param name="signInManager">The <see cref="SignInManager{T}"/> responsible for handling user sign-ins and sign-outs.</param>
    /// <param name="interaction">The <see cref="IIdentityServerInteractionService"/> responsible for interacting with IdentityServer's interaction contexts.</param>
    /// <param name="events">The <see cref="IEventService"/> for raising events related to user authentication and logout.</param>
    public Index(SignInManager<UserAccountAggregate> signInManager, IIdentityServerInteractionService interaction, IEventService events)
    {
        _signInManager = signInManager;
        _interaction = interaction;
        _events = events;
    }

    /// <summary>
    /// Handles the HTTP GET request for the logout page. It determines whether to show the logout prompt or log out automatically.
    /// </summary>
    /// <param name="logoutId">The identifier for the logout request.</param>
    /// <returns>A <see cref="Task{IActionResult}"/> representing the asynchronous operation.</returns>
    public async Task<IActionResult> OnGet(string? logoutId)
    {
        LogoutId = logoutId;

        var showLogoutPrompt = LogoutOptions.ShowLogoutPrompt;

        if (User.Identity?.IsAuthenticated != true)
        {
            // if the user is not authenticated, then just show the logged out page
            showLogoutPrompt = false;
        }
        else
        {
            var context = await _interaction.GetLogoutContextAsync(LogoutId);
            if (context.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                showLogoutPrompt = false;
            }
        }
            
        if (showLogoutPrompt == false)
        {
            // if the request for logout was properly authenticated from IdentityServer, then
            // we don't need to show the prompt and can just log the user out directly.
            return await OnPost();
        }

        return Page();
    }

    /// <summary>
    /// Handles the HTTP POST request for logging the user out. This method performs the logout actions and raises necessary events.
    /// </summary>
    /// <returns>A <see cref="Task{IActionResult}"/> representing the asynchronous operation.</returns>
    public async Task<IActionResult> OnPost()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            // if there's no current logout context, create one
            // this captures necessary info from the current logged in user
            // this can still return null if there is no context needed
            LogoutId ??= await _interaction.CreateLogoutContextAsync();
                
            // delete the local authentication cookie
            await _signInManager.SignOutAsync();
            
            // delete the refresh token cookie
            HttpContext.Response.Cookies.Delete("__Secure_refresh_token");

            // check if we need to trigger federated logout
            var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

            // raise the logout event
            await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            Telemetry.Metrics.UserLogout(idp);

            // if it's a local login, we can ignore this workflow
            if (idp is not null && idp != Duende.IdentityServer.IdentityServerConstants.LocalIdentityProvider)
            {
                // check if the identity provider supports external logout
                if (await HttpContext.GetSchemeSupportsSignOutAsync(idp))
                {
                    // build the return URL so the external provider will redirect back here after logout
                    var url = Url.Page("/Account/Logout/Loggedout", new { logoutId = LogoutId });

                    // redirect to the external provider for sign-out
                    return SignOut(new AuthenticationProperties { RedirectUri = url }, idp);
                }
            }
        }

        return RedirectToPage("/Account/Logout/LoggedOut", new { logoutId = LogoutId });
    }
}

