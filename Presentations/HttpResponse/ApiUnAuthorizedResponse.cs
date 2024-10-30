using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Presentations.HttpResponse;

public class ApiUnAuthorizedResponse : ApiResponse
{
    public ApiUnAuthorizedResponse(ModelStateDictionary modelState)
        : base(401)
    {
        if (modelState.IsValid) throw new ArgumentException("ModelState must be invalid", nameof(modelState));

        Errors = Enumerable
            .ToArray<string>(modelState
                .SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage));
    }

    public ApiUnAuthorizedResponse(IdentityResult identityResult)
        : base(401)
    {
        Errors = Enumerable.ToArray<string>(identityResult.Errors
            .Select(x => x.Code + " - " + x.Description));
    }

    public ApiUnAuthorizedResponse(string message)
        : base(401, message)
    {
    }
}