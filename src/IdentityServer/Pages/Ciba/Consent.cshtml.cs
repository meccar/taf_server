// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Ciba;

/// <summary>
/// Represents the page model for handling user consent during backchannel login requests.
/// This page is accessible only to authenticated users.
/// </summary>
[Authorize]
[SecurityHeaders]
public class Consent : PageModel
{
    private readonly IBackchannelAuthenticationInteractionService _interaction;
    private readonly IEventService _events;
    private readonly ILogger<Consent> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="Consent"/> class.
    /// </summary>
    /// <param name="interaction">The service used to interact with backchannel authentication requests.</param>
    /// <param name="events">The event service for raising events.</param>
    /// <param name="logger">The logger service used to log messages.</param>
    public Consent(
        IBackchannelAuthenticationInteractionService interaction,
        IEventService events,
        ILogger<Consent> logger)
    {
        _interaction = interaction;
        _events = events;
        _logger = logger;
    }

    /// <summary>
    /// Gets or sets the view model that contains the consent information to be displayed on the page.
    /// </summary>
    public ViewModel View { get; set; } = default!;

    /// <summary>
    /// Gets or sets the input model that contains user consent choices.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; } = default!;

    /// <summary>
    /// Handles the GET request for the consent page. This method retrieves the login request by its ID and displays the consent UI.
    /// </summary>
    /// <param name="id">The ID of the login request.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains the IActionResult for the page.</returns>
    public async Task<IActionResult> OnGet(string? id)
    {
        if (!await SetViewModelAsync(id))
        {
            return RedirectToPage("/Home/Error/Index");
        }

        Input = new InputModel
        {
            Id = id
        };

        return Page();
    }

    /// <summary>
    /// Handles the POST request for submitting consent. The user either grants or denies consent to the requested scopes.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The result contains the IActionResult for the page.</returns>
    public async Task<IActionResult> OnPost()
    {
        // Validate the return URL and ensure the request is valid
        var request = await _interaction.GetLoginRequestByInternalIdAsync(Input.Id ?? throw new ArgumentNullException(nameof(Input.Id)));
        if (request == null || request.Subject.GetSubjectId() != User.GetSubjectId())
        {
            _logger.InvalidId(Input.Id);
            return RedirectToPage("/Home/Error/Index");
        }

        CompleteBackchannelLoginRequest? result = null;

        // User clicked 'no' - deny consent
        if (Input.Button == "no")
        {
            result = new CompleteBackchannelLoginRequest(Input.Id);

            // Emit consent denied event
            await _events.RaiseAsync(new ConsentDeniedEvent(User.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.RawScopeValues));
            Telemetry.Metrics.ConsentDenied(request.Client.ClientId, request.ValidatedResources.ParsedScopes.Select(s => s.ParsedName));
        }
        // User clicked 'yes' - grant consent and validate data
        else if (Input.Button == "yes")
        {
            // If user consented to some scope, build the response model
            if (Input.ScopesConsented.Any())
            {
                var scopes = Input.ScopesConsented;
                if (!ConsentOptions.EnableOfflineAccess)
                {
                    scopes = scopes.Where(x => x != Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess);
                }

                result = new CompleteBackchannelLoginRequest(Input.Id)
                {
                    ScopesValuesConsented = scopes.ToArray(),
                    Description = Input.Description
                };

                // Emit consent granted event
                await _events.RaiseAsync(new ConsentGrantedEvent(User.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.RawScopeValues, result.ScopesValuesConsented, false));
                Telemetry.Metrics.ConsentGranted(request.Client.ClientId, result.ScopesValuesConsented, false);
                var denied = request.ValidatedResources.ParsedScopes.Select(s => s.ParsedName).Except(result.ScopesValuesConsented);
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

        if (result is not null)
        {
            // Complete the login request and communicate outcome to IdentityServer
            await _interaction.CompleteLoginRequestAsync(result);

            return RedirectToPage("/Ciba/All");
        }

        // Redisplay the consent UI if validation fails
        if (!await SetViewModelAsync(Input.Id))
        {
            return RedirectToPage("/Home/Error/Index");
        }
        return Page();
    }

    /// <summary>
    /// Sets the consent view model by retrieving the login request for the specified ID.
    /// </summary>
    /// <param name="id">The ID of the login request.</param>
    /// <returns>A task that represents the asynchronous operation. The result indicates whether the view model was successfully set.</returns>
    private async Task<bool> SetViewModelAsync(string? id)
    {
        ArgumentNullException.ThrowIfNull(id);

        var request = await _interaction.GetLoginRequestByInternalIdAsync(id);
        if (request is not null && request.Subject.GetSubjectId() == User.GetSubjectId())
        {
            View = CreateConsentViewModel(request);
            return true;
        }
        else
        {
            _logger.NoMatchingBackchannelLoginRequest(id);
            return false;
        }
    }

    /// <summary>
    /// Creates the consent view model based on the provided login request.
    /// </summary>
    /// <param name="request">The login request that contains information for the consent page.</param>
    /// <returns>The created <see cref="ViewModel"/> for the consent page.</returns>
    private ViewModel CreateConsentViewModel(BackchannelUserLoginRequest request)
    {
        var vm = new ViewModel
        {
            ClientName = request.Client.ClientName ?? request.Client.ClientId,
            ClientUrl = request.Client.ClientUri,
            ClientLogoUrl = request.Client.LogoUri,
            BindingMessage = request.BindingMessage
        };

        vm.IdentityScopes = request.ValidatedResources.Resources.IdentityResources
            .Select(x => CreateScopeViewModel(x, Input == null || Input.ScopesConsented.Contains(x.Name)))
            .ToArray();

        var resourceIndicators = request.RequestedResourceIndicators ?? Enumerable.Empty<string>();
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
            apiScopes.Add(GetOfflineAccessScope(Input == null || Input.ScopesConsented.Contains(Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess)));
        }
        vm.ApiScopes = apiScopes;

        return vm;
    }

    /// <summary>
    /// Creates a scope view model from an identity resource.
    /// </summary>
    /// <param name="identity">The identity resource.</param>
    /// <param name="check">Indicates whether the scope should be checked.</param>
    /// <returns>The created <see cref="ScopeViewModel"/>.</returns>
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
    /// Creates a scope view model from a parsed scope and API scope.
    /// </summary>
    /// <param name="parsedScopeValue">The parsed scope value.</param>
    /// <param name="apiScope">The associated API scope.</param>
    /// <param name="check">Indicates whether the scope should be checked.</param>
    /// <returns>The created <see cref="ScopeViewModel"/>.</returns>
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
    /// <param name="check">Indicates whether the scope should be checked.</param>
    /// <returns>The created <see cref="ScopeViewModel"/> for offline access.</returns>
    private static ScopeViewModel GetOfflineAccessScope(bool check)
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