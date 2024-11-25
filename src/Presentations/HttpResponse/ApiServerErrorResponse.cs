using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Presentations.HttpResponse;

public class ApiServerErrorResponse : ApiResponse
{
    public ApiServerErrorResponse(ModelStateDictionary modelState)
        : base(500)
    {
        if (modelState.IsValid) throw new ArgumentException("ModelState must be invalid", nameof(modelState));

        Errors = Enumerable
            .ToArray<string>(modelState
                .SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage));
    }

    public ApiServerErrorResponse(IdentityResult identityResult)
        : base(500)
    {
        Errors = Enumerable.ToArray<string>(identityResult.Errors
            .Select(x => x.Code + " - " + x.Description));
    }

    public ApiServerErrorResponse(string message)
        : base(500, message)
    {
    }
}