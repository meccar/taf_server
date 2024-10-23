using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using taf_server.Presentations.HttpResponss;

namespace taf_server.Presentations.HttpResponse;
public class ApiBadRequestResponse : ApiResponse
{
    public ApiBadRequestResponse(ModelStateDictionary modelState)
        : base(400)
    {
        if (modelState.IsValid) throw new ArgumentException("ModelState must be invalid", nameof(modelState));

        Errors = Enumerable
            .ToArray<string>(modelState
                .SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage));
    }

    public ApiBadRequestResponse(IdentityResult identityResult)
        : base(400)
    {
        Errors = Enumerable.ToArray<string>(identityResult.Errors
                .Select(x => x.Code + " - " + x.Description));
    }

    public ApiBadRequestResponse(string message)
        : base(400, message)
    {
    }
}
