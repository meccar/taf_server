using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Shared.HttpResponses;

    /// <summary>
    /// Represents a response with a 401 Unauthorized status code, indicating that the request lacks proper authentication or authorization.
    /// </summary>
    public class ApiUnauthorizedResponse : ApiResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiUnauthorizedResponse"/> class with a 401 status code and errors from the ModelState.
        /// </summary>
        /// <param name="modelState">The model state containing validation errors that led to the unauthorized response.</param>
        /// <exception cref="ArgumentException">Thrown when the modelState is valid.</exception>
        public ApiUnauthorizedResponse(ModelStateDictionary modelState)
            : base(401)
        {
            if (modelState.IsValid) throw new ArgumentException("ModelState must be invalid", nameof(modelState));

            Errors = Enumerable
                .ToArray<string>(modelState
                    .SelectMany(x => x.Value!.Errors)
                    .Select(x => x.ErrorMessage));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiUnauthorizedResponse"/> class with a 401 status code and errors from an IdentityResult.
        /// </summary>
        /// <param name="identityResult">The IdentityResult containing errors related to user authentication or authorization failure.</param>
        public ApiUnauthorizedResponse(IdentityResult identityResult)
            : base(401)
        {
            Errors = Enumerable.ToArray<string>(identityResult.Errors
                .Select(x => x.Code + " - " + x.Description));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiUnauthorizedResponse"/> class with a 401 status code and a custom error message.
        /// </summary>
        /// <param name="message">A custom message describing the unauthorized error.</param>
        public ApiUnauthorizedResponse(string message)
            : base(401, message)
        {
        }
    }