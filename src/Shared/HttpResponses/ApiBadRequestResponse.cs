using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Shared.HttpResponses;

/// <summary>
/// Represents a standardized response for HTTP 400 Bad Request errors.
/// </summary>
public class ApiBadRequestResponse : ApiResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiBadRequestResponse"/> class using a <see cref="ModelStateDictionary"/>.
    /// </summary>
    /// <param name="modelState">The model state containing validation errors.</param>
    /// <exception cref="ArgumentException">Thrown if the provided <paramref name="modelState"/> is valid.</exception>
    public ApiBadRequestResponse(ModelStateDictionary modelState)
        : base(400)
    {
        if (modelState.IsValid) throw new ArgumentException("ModelState must be invalid", nameof(modelState));

        Errors = Enumerable
            .ToArray(modelState
                .SelectMany(x => x.Value!.Errors)
                .Select(x => x.ErrorMessage));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiBadRequestResponse"/> class using an <see cref="IdentityResult"/>.
    /// </summary>
    /// <param name="identityResult">The result of an identity operation containing errors.</param>
    public ApiBadRequestResponse(IdentityResult identityResult)
        : base(400)
    {
        Errors = Enumerable.ToArray(identityResult.Errors
                .Select(x => x.Code + " - " + x.Description));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiBadRequestResponse"/> class with a custom error message.
    /// </summary>
    /// <param name="message">A message describing the error.</param>
    public ApiBadRequestResponse(string message)
        : base(400, message)
    {
    }
}
