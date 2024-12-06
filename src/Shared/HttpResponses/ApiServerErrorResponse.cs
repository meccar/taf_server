using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Shared.HttpResponses;

/// <summary>
/// Represents a response with a 500 Internal Server Error status code, indicating a server-side error.
/// </summary>
public class ApiServerErrorResponse : ApiResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiServerErrorResponse"/> class with a 500 status code and errors from the ModelState.
    /// </summary>
    /// <param name="modelState">The model state containing validation errors that led to the server error.</param>
    /// <exception cref="ArgumentException">Thrown when the modelState is valid.</exception>
    public ApiServerErrorResponse(ModelStateDictionary modelState)
        : base(500)
    {
        if (modelState.IsValid) throw new ArgumentException("ModelState must be invalid", nameof(modelState));

        Errors = Enumerable
            .ToArray<string>(modelState
                .SelectMany(x => x.Value!.Errors)
                .Select(x => x.ErrorMessage));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiServerErrorResponse"/> class with a 500 status code and errors from an IdentityResult.
    /// </summary>
    /// <param name="identityResult">The IdentityResult containing errors related to the user creation or validation process.</param>
    public ApiServerErrorResponse(IdentityResult identityResult)
        : base(500)
    {
        Errors = Enumerable.ToArray<string>(identityResult.Errors
            .Select(x => x.Code + " - " + x.Description));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiServerErrorResponse"/> class with a 500 status code and a custom error message.
    /// </summary>
    /// <param name="message">A custom message describing the error.</param>
    public ApiServerErrorResponse(string message)
        : base(500, message)
    {
    }
}