// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer.Configuration;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace IdentityServer.Pages.Device;

/// <summary>
/// The page model for handling device flow consent.
/// </summary>
[SecurityHeaders]
[Authorize]
public class Index : PageModel
{
    private readonly IDeviceFlowInteractionService _interaction;
    private readonly IEventService _events;

    /// <summary>
    /// Initializes a new instance of the <see cref="Index"/> class.
    /// </summary>
    /// <param name="interaction">The device flow interaction service.</param>
    /// <param name="eventService">The event service to raise consent events.</param>
    /// <param name="options">The identity server options.</param>
    /// <param name="logger">The logger for logging information and errors.</param>
    public Index(
        IDeviceFlowInteractionService interaction,
        IEventService eventService,
        IOptions<IdentityServerOptions> options,
        ILogger<Index> logger)
    {
        _interaction = interaction;
        _events = eventService;
    }

    /// <summary>
    /// Gets or sets the view model for rendering consent page UI.
    /// </summary>
    public ViewModel View { get; set; } = default!;

    /// <summary>
    /// Gets or sets the input model, including the user's consent selection.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; } = default!;

    /// <summary>
    /// Handles GET requests to display the consent page.
    /// </summary>
    /// <param name="userCode">The user code to identify the device flow request.</param>
    /// <returns>The action result to render the consent page.</returns>
    public async Task<IActionResult> OnGet(string? userCode)
    {
        if (String.IsNullOrWhiteSpace(userCode))
        {
            return Page();
        }

        if (!await SetViewModelAsync(userCode))
        {
            ModelState.AddModelError("", DeviceOptions.InvalidUserCode);
            return Page();
        }

        Input = new InputModel { 
            UserCode = userCode,
        };

        return Page();
    }

    /// <summary>
    /// Handles POST requests to submit the consent form and grant or deny access.
    /// </summary>
    /// <returns>The action result to redirect the user based on their choice.</returns>
    public async Task<IActionResult> OnPost()
    {
        var request = await _interaction.GetAuthorizationContextAsync(Input.UserCode ?? throw new ArgumentNullException(nameof(Input.UserCode)));
        if (request == null) return RedirectToPage("/Home/Error/Index");

        ConsentResponse? grantedConsent = null;

        // user clicked 'no' - send back the standard 'access_denied' response
        if (Input.Button == "no")
        {
            grantedConsent = new ConsentResponse
            {
                Error = AuthorizationError.AccessDenied
            };

            // emit event
            await _events.RaiseAsync(new ConsentDeniedEvent(User.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.RawScopeValues));
            Telemetry.Metrics.ConsentDenied(request.Client.ClientId, request.ValidatedResources.ParsedScopes.Select(s => s.ParsedName));
        }
        // user clicked 'yes' - validate the data
        else if (Input.Button == "yes")
        {
            // if the user consented to some scope, build the response model
            if (Input.ScopesConsented.Any())
            {
                var scopes = Input.ScopesConsented;
                if (DeviceOptions.EnableOfflineAccess == false)
                {
                    scopes = scopes.Where(x => x != Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess);
                }

                grantedConsent = new ConsentResponse
                {
                    RememberConsent = Input.RememberConsent,
                    ScopesValuesConsented = scopes.ToArray(),
                    Description = Input.Description
                };

                // emit event
                await _events.RaiseAsync(new ConsentGrantedEvent(User.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.RawScopeValues, grantedConsent.ScopesValuesConsented, grantedConsent.RememberConsent));
                Telemetry.Metrics.ConsentGranted(request.Client.ClientId, grantedConsent.ScopesValuesConsented, grantedConsent.RememberConsent);
                var denied = request.ValidatedResources.ParsedScopes.Select(s => s.ParsedName).Except(grantedConsent.ScopesValuesConsented);
                Telemetry.Metrics.ConsentDenied(request.Client.ClientId, denied);
            }
            else
            {
                ModelState.AddModelError("", DeviceOptions.MustChooseOneErrorMessage);
            }
        }
        else
        {
            ModelState.AddModelError("", DeviceOptions.InvalidSelectionErrorMessage);
        }

        if (grantedConsent != null)
        {
            // communicate outcome of consent back to identityserver
            await _interaction.HandleRequestAsync(Input.UserCode, grantedConsent);

            // indicate that's it ok to redirect back to authorization endpoint
            return RedirectToPage("/Device/Success");
        }

        // we need to redisplay the consent UI
        if (!await SetViewModelAsync(Input.UserCode))
        {
            return RedirectToPage("/Home/Error/Index");
        }
        return Page();
    }

    /// <summary>
    /// Sets up the consent page's view model using the provided user code.
    /// </summary>
    /// <param name="userCode">The user code to identify the device flow request.</param>
    /// <returns>A task representing the asynchronous operation, with a result indicating whether the view model was set.</returns>
    private async Task<bool> SetViewModelAsync(string userCode)
    {
        var request = await _interaction.GetAuthorizationContextAsync(userCode);
        if (request != null)
        {
            View = CreateConsentViewModel(request);
            return true;
        }
        else
        {
            View = new ViewModel();
            return false;
        }
    }

    /// <summary>
    /// Creates a view model for consent based on the device flow authorization request.
    /// </summary>
    /// <param name="request">The device flow authorization request.</param>
    /// <returns>The view model for rendering the consent page.</returns>
    private ViewModel CreateConsentViewModel(DeviceFlowAuthorizationRequest request)
    {
        var vm = new ViewModel
        {
            ClientName = request.Client.ClientName ?? request.Client.ClientId,
            ClientUrl = request.Client.ClientUri,
            ClientLogoUrl = request.Client.LogoUri,
            AllowRememberConsent = request.Client.AllowRememberConsent
        };

        // Populate identity scope view models
        vm.IdentityScopes = request.ValidatedResources.Resources.IdentityResources.Select(x => CreateScopeViewModel(x, Input.ScopesConsented.Contains(x.Name))).ToArray();

        // Populate API scope view models
        var apiScopes = new List<ScopeViewModel>();
        foreach (var parsedScope in request.ValidatedResources.ParsedScopes)
        {
            var apiScope = request.ValidatedResources.Resources.FindApiScope(parsedScope.ParsedName);
            if (apiScope != null)
            {
                var scopeVm = CreateScopeViewModel(parsedScope, apiScope, Input == null || Input.ScopesConsented.Contains(parsedScope.RawValue));
                apiScopes.Add(scopeVm);
            }
        }

        // Add offline access scope if enabled
        if (DeviceOptions.EnableOfflineAccess && request.ValidatedResources.Resources.OfflineAccess)
        {
            apiScopes.Add(GetOfflineAccessScope(Input == null || Input.ScopesConsented.Contains(Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess)));
        }

        vm.ApiScopes = apiScopes;

        return vm;
    }

    /// <summary>
    /// Creates a scope view model for identity resources.
    /// </summary>
    /// <param name="identity">The identity resource.</param>
    /// <param name="check">Indicates whether the scope should be checked (selected).</param>
    /// <returns>The scope view model.</returns>
    private static ScopeViewModel CreateScopeViewModel(IdentityResource identity, bool check)
    {
        return new ScopeViewModel
        {
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
    /// <param name="parsedScopeValue">The parsed scope value.</param>
    /// <param name="apiScope">The API scope.</param>
    /// <param name="check">Indicates whether the scope should be checked (selected).</param>
    /// <returns>The scope view model.</returns>
    private static ScopeViewModel CreateScopeViewModel(ParsedScopeValue parsedScopeValue, ApiScope apiScope, bool check)
    {
        return new ScopeViewModel
        {
            Value = parsedScopeValue.RawValue,
            DisplayName = apiScope.DisplayName ?? apiScope.Name,
            Description = apiScope.Description,
            Emphasize = apiScope.Emphasize,
            Required = apiScope.Required,
            Checked = check || apiScope.Required
        };
    }

    /// <summary>
    /// Gets the offline access scope view model.
    /// </summary>
    /// <param name="check">Indicates whether the offline access scope should be checked (selected).</param>
    /// <returns>The offline access scope view model.</returns>
    private static ScopeViewModel GetOfflineAccessScope(bool check)
    {
        return new ScopeViewModel
        {
            Value = Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess,
            DisplayName = DeviceOptions.OfflineAccessDisplayName,
            Description = DeviceOptions.OfflineAccessDescription,
            Emphasize = true,
            Checked = check
        };
    }
}