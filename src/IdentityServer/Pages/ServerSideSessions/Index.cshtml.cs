// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.ServerSideSessions;

/// <summary>
/// Handles the server-side session management page, allowing users to filter and remove sessions.
/// </summary>
public class IndexModel : PageModel
{
    private readonly ISessionManagementService? _sessionManagementService;

    /// <summary>
    /// Initializes a new instance of the <see cref="IndexModel"/> class.
    /// </summary>
    /// <param name="sessionManagementService">The session management service used to query and manage user sessions.</param>
    public IndexModel(ISessionManagementService? sessionManagementService = null)
    {
        _sessionManagementService = sessionManagementService;
    }

    /// <summary>
    /// Gets or sets the user sessions queried by the session management service.
    /// </summary>
    public QueryResult<UserSession>? UserSessions { get; set; }

    /// <summary>
    /// Gets or sets the display name filter used for querying sessions.
    /// </summary>
    [BindProperty(SupportsGet = true)]
    public string? DisplayNameFilter { get; set; }

    /// <summary>
    /// Gets or sets the session ID filter used for querying sessions.
    /// </summary>
    [BindProperty(SupportsGet = true)]
    public string? SessionIdFilter { get; set; }

    /// <summary>
    /// Gets or sets the subject ID filter used for querying sessions.
    /// </summary>
    [BindProperty(SupportsGet = true)]
    public string? SubjectIdFilter { get; set; }

    /// <summary>
    /// Gets or sets the token used for pagination or querying results.
    /// </summary>
    [BindProperty(SupportsGet = true)]
    public string? Token { get; set; }

    /// <summary>
    /// Gets or sets the previous page flag used for pagination.
    /// </summary>
    [BindProperty(SupportsGet = true)]
    public string? Prev { get; set; }

    /// <summary>
    /// Handles the GET request for querying user sessions with the specified filters.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> representing the result of the GET request.</returns>
    public async Task OnGet()
    {
        if (_sessionManagementService != null)
        {
            UserSessions = await _sessionManagementService.QuerySessionsAsync(new SessionQuery
            {
                ResultsToken = Token,
                RequestPriorResults = Prev == "true",
                DisplayName = DisplayNameFilter,
                SessionId = SessionIdFilter,
                SubjectId = SubjectIdFilter
            });
        }
    }

    /// <summary>
    /// Gets or sets the session ID for removal of sessions.
    /// </summary>
    [BindProperty]
    public string? SessionId { get; set; }

    /// <summary>
    /// Handles the POST request for removing the selected user session.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> representing the result of the POST request, which redirects to the server-side sessions page.</returns>
    public async Task<IActionResult> OnPost()
    {
        ArgumentNullException.ThrowIfNull(_sessionManagementService);

        await _sessionManagementService.RemoveSessionsAsync(new RemoveSessionsContext
        {
            SessionId = SessionId,
        });

        // Redirect back to the server-side sessions page with the appropriate query parameters.
        return RedirectToPage("/ServerSideSessions/Index", new
        {
            Token,
            DisplayNameFilter,
            SessionIdFilter,
            SubjectIdFilter,
            Prev
        });
    }
}

