using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Shared.HttpResponses;

/// <summary>
/// Represents a response with a 403 Forbidden status code, commonly used when access to a resource is forbidden.
/// </summary>
public class ApiForbiddenResponse : ApiResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiForbiddenResponse"/> class with a 403 status code based on the provided ModelState.
    /// </summary>
    /// <param name="modelState">The model state containing errors to include in the response.</param>
    /// <exception cref="ArgumentException">Thrown when the ModelState is valid instead of invalid.</exception>
    public ApiForbiddenResponse(ModelStateDictionary modelState)
        : base(403)
    {
        if (modelState.IsValid)
            throw new ArgumentException("ModelState must be invalid", nameof(modelState));

        Errors = Enumerable
            .ToArray<string>(modelState
                .SelectMany(x => x.Value!.Errors)
                .Select(x => x.ErrorMessage));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiForbiddenResponse"/> class with a 403 status code based on the provided IdentityResult.
    /// </summary>
    /// <param name="identityResult">The IdentityResult containing errors to include in the response.</param>
    public ApiForbiddenResponse(IdentityResult identityResult)
        : base(403)
    {
        Errors = Enumerable.ToArray<string>(identityResult.Errors
            .Select(x => x.Code + ": " + x.Description));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiForbiddenResponse"/> class with a 403 status code and a custom message.
    /// </summary>
    /// <param name="message">The custom message to include in the response.</param>
    public ApiForbiddenResponse(string message)
        : base(403, message)
    {
    }
}