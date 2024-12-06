using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Shared.HttpResponses;

/// <summary>
/// Represents a response with a 404 Not Found status code, commonly used when a requested resource cannot be found.
/// </summary>
public class ApiNotFoundResponse : ApiResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiNotFoundResponse"/> class with a 404 status code based on the provided ModelState.
    /// </summary>
    /// <param name="modelState">The model state containing errors to include in the response.</param>
    /// <exception cref="ArgumentException">Thrown when the ModelState is valid instead of invalid.</exception>
    public ApiNotFoundResponse(ModelStateDictionary modelState)
        : base(404)
    {
        if (modelState.IsValid)
            throw new ArgumentException("ModelState must be invalid", nameof(modelState));

        Errors = Enumerable
            .ToArray<string>(modelState
                .SelectMany(x => x.Value!.Errors)
                .Select(x => x.ErrorMessage));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiNotFoundResponse"/> class with a 404 status code based on the provided IdentityResult.
    /// </summary>
    /// <param name="identityResult">The IdentityResult containing errors to include in the response.</param>
    public ApiNotFoundResponse(IdentityResult identityResult)
        : base(404)
    {
        Errors = Enumerable.ToArray<string>(identityResult.Errors
            .Select(x => x.Code + " - " + x.Description));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiNotFoundResponse"/> class with a 404 status code and a custom message.
    /// </summary>
    /// <param name="message">The custom message to include in the response.</param>
    public ApiNotFoundResponse(string message)
        : base(404, message)
    {
    }
}