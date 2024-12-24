// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Validation;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Consent;

/// <summary>
/// Handles the consent page logic for users to approve or deny access to requested resources.
/// </summary>
[Authorize]
[SecurityHeaders]
public class Index : PageModel
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IEventService _events;
    private readonly ILogger<Index> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="Index"/> class.
    /// </summary>
    /// <param name="interaction">The identity server interaction service.</param>
    /// <param name="events">The event service for consent-related events.</param>
    /// <param name="logger">The logger for the <see cref="Index"/> class.</param>
    public Index(
        IIdentityServerInteractionService interaction,
        IEventService events,
        ILogger<Index> logger)
    {
        _interaction = interaction;
        _events = events;
        _logger = logger;
    }

    /// <summary>
    /// Gets or sets the view model that will be used to render the consent page.
    /// </summary>
    public ViewModel View { get; set; } = default!;

    /// <summary>
    /// Gets or sets the input model that contains the data submitted by the user.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; } = default!;

    /// <summary>
    /// Handles the HTTP GET request to display the consent page.
    /// </summary>
    /// <param name="returnUrl">The URL to redirect to after consent is granted.</param>
    /// <returns>An action result representing the consent page.</returns>
    public async Task<IActionResult> OnGet(string? returnUrl)
    {
        if (!await SetViewModelAsync(returnUrl))
        {
            return RedirectToPage("/Home/Error/Index");
        }

        Input = new InputModel
        {
            ReturnUrl = returnUrl,
        };

        return Page();
    }

    /// <summary>
    /// Handles the HTTP POST request when the user submits their consent decision.
    /// </summary>
    /// <returns>An action result representing the outcome of the consent decision.</returns>
    public async Task<IActionResult> OnPost()
    {
        // Validate the return URL is still valid
        var request = await _interaction.GetAuthorizationContextAsync(Input.ReturnUrl);
        if (request == null) return RedirectToPage("/Home/Error/Index");

        ConsentResponse? grantedConsent = null;

        // User clicked 'no' - send back the standard 'access_denied' response
        if (Input.Button == "no")
        {
            grantedConsent = new ConsentResponse { Error = AuthorizationError.AccessDenied };

            // Emit event
            await _events.RaiseAsync(new ConsentDeniedEvent(User.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.RawScopeValues));
            Telemetry.Metrics.ConsentDenied(request.Client.ClientId, request.ValidatedResources.ParsedScopes.Select(s => s.ParsedName));
        }
        // User clicked 'yes' - validate the data
        else if (Input.Button == "yes")
        {
            // If the user consented to some scope, build the response model
            if (Input.ScopesConsented.Any())
            {
                var scopes = Input.ScopesConsented;
                if (ConsentOptions.EnableOfflineAccess == false)
                {
                    scopes = scopes.Where(x => x != Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess);
                }

                grantedConsent = new ConsentResponse
                {
                    RememberConsent = Input.RememberConsent,
                    ScopesValuesConsented = scopes.ToArray(),
                    Description = Input.Description
                };

                // Emit event
                await _events.RaiseAsync(new ConsentGrantedEvent(User.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.RawScopeValues, grantedConsent.ScopesValuesConsented, grantedConsent.RememberConsent));
                Telemetry.Metrics.ConsentGranted(request.Client.ClientId, grantedConsent.ScopesValuesConsented, grantedConsent.RememberConsent);
                var denied = request.ValidatedResources.ParsedScopes.Select(s => s.ParsedName).Except(grantedConsent.ScopesValuesConsented);
                Telemetry.Metrics.ConsentDenied(request.Client.ClientId, denied);
            }
            else
            {
                ModelState.AddModelError("", ConsentOptions.MustChooseOneErrorMessage);
            }
        }
        else
        {
            ModelState.AddModelError("", ConsentOptions.InvalidSelectionErrorMessage);
        }

        if (grantedConsent is not null)
        {
            ArgumentNullException.ThrowIfNull(Input.ReturnUrl, nameof(Input.ReturnUrl));

            // Communicate outcome of consent back to IdentityServer
            await _interaction.GrantConsentAsync(request, grantedConsent);

            // Redirect back to authorization endpoint
            if (request.IsNativeClient())
            {
                // The client is native, so this change in how to
                // return the response is for better UX for the end user.
                return this.LoadingPage(Input.ReturnUrl);
            }

            return Redirect(Input.ReturnUrl);
        }

        // Redisplay the consent UI if necessary
        if (!await SetViewModelAsync(Input.ReturnUrl))
        {
            return RedirectToPage("/Home/Error/Index");
        }
        return Page();
    }

    /// <summary>
    /// Sets the view model for the consent page based on the authorization request context.
    /// </summary>
    /// <param name="returnUrl">The URL to return to after consent is processed.</param>
    /// <returns>True if the view model was successfully set; otherwise, false.</returns>
    private async Task<bool> SetViewModelAsync(string? returnUrl)
    {
        ArgumentNullException.ThrowIfNull(returnUrl);

        var request = await _interaction.GetAuthorizationContextAsync(returnUrl);
        if (request is not null)
        {
            View = CreateConsentViewModel(request);
            return true;
        }
        else
        {
            _logger.NoConsentMatchingRequest(returnUrl);
            return false;
        }
    }

    /// <summary>
    /// Creates a view model to represent the consent UI based on the authorization request.
    /// </summary>
    /// <param name="request">The authorization request.</param>
    /// <returns>A view model representing the consent UI.</returns>
    private ViewModel CreateConsentViewModel(AuthorizationRequest request)
    {
        var vm = new ViewModel
        {
            ClientName = request.Client.ClientName ?? request.Client.ClientId,
            ClientUrl = request.Client.ClientUri,
            ClientLogoUrl = request.Client.LogoUri,
            AllowRememberConsent = request.Client.AllowRememberConsent
        };

        vm.IdentityScopes = request.ValidatedResources.Resources.IdentityResources
            .Select(x => CreateScopeViewModel(x, Input.ScopesConsented.Contains(x.Name)))
            .ToArray();

        var resourceIndicators = request.Parameters.GetValues(OidcConstants.AuthorizeRequest.Resource) ?? Enumerable.Empty<string>();
        var apiResources = request.ValidatedResources.Resources.ApiResources.Where(x => resourceIndicators.Contains(x.Name));

        var apiScopes = new List<ScopeViewModel>();
        foreach (var parsedScope in request.ValidatedResources.ParsedScopes)
        {
            var apiScope = request.ValidatedResources.Resources.FindApiScope(parsedScope.ParsedName);
            if (apiScope is not null)
            {
                var scopeVm = CreateScopeViewModel(parsedScope, apiScope, Input == null || Input.ScopesConsented.Contains(parsedScope.RawValue));
                scopeVm.Resources = apiResources.Where(x => x.Scopes.Contains(parsedScope.ParsedName))
                    .Select(x => new ResourceViewModel
                    {
                        Name = x.Name,
                        DisplayName = x.DisplayName ?? x.Name,
                    }).ToArray();
                apiScopes.Add(scopeVm);
            }
        }
        if (ConsentOptions.EnableOfflineAccess && request.ValidatedResources.Resources.OfflineAccess)
        {
            apiScopes.Add(CreateOfflineAccessScope(Input == null || Input.ScopesConsented.Contains(Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess)));
        }
        vm.ApiScopes = apiScopes;

        return vm;
    }

    /// <summary>
    /// Creates a scope view model for identity resources.
    /// </summary>
    /// <param name="identity">The identity resource to create a view model for.</param>
    /// <param name="check">Indicates whether the scope should be pre-selected.</param>
    /// <returns>A view model representing the identity scope.</returns>
    private static ScopeViewModel CreateScopeViewModel(IdentityResource identity, bool check)
    {
        return new ScopeViewModel
        {
            Name = identity.Name,
            Value = identity.Name,
            DisplayName = identity.DisplayName ?? identity.Name,
            Description = identity.Description,
            Emphasize = identity.Emphasize,
            Required = identity.Required,
            Checked = check || identity.Required
        };
    }

    /// <summary>
    /// Creates a scope view model for API scopes.
    /// </summary>
    /// <param name="parsedScopeValue">The parsed scope value from the authorization request.</param>
    /// <param name="apiScope">The API scope to create a view model for.</param>
    /// <param name="check">Indicates whether the scope should be pre-selected.</param>
    /// <returns>A view model representing the API scope.</returns>
    private static ScopeViewModel CreateScopeViewModel(ParsedScopeValue parsedScopeValue, ApiScope apiScope, bool check)
    {
        var displayName = apiScope.DisplayName ?? apiScope.Name;
        if (!String.IsNullOrWhiteSpace(parsedScopeValue.ParsedParameter))
        {
            displayName += ":" + parsedScopeValue.ParsedParameter;
        }

        return new ScopeViewModel
        {
            Name = parsedScopeValue.ParsedName,
            Value = parsedScopeValue.RawValue,
            DisplayName = displayName,
            Description = apiScope.Description,
            Emphasize = apiScope.Emphasize,
            Required = apiScope.Required,
            Checked = check || apiScope.Required
        };
    }

    /// <summary>
    /// Creates a scope view model for offline access.
    /// </summary>
    /// <param name="check">Indicates whether offline access should be pre-selected.</param>
    /// <returns>A view model representing the offline access scope.</returns>
    private static ScopeViewModel CreateOfflineAccessScope(bool check)
    {
        return new ScopeViewModel
        {
            Value = Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess,
            DisplayName = ConsentOptions.OfflineAccessDisplayName,
            Description = ConsentOptions.OfflineAccessDescription,
            Emphasize = true,
            Checked = check
        };
    }
}