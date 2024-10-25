using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using taf_server.Presentations.HttpResponss;

namespace taf_server.Presentations.HttpResponse;

public class ApiForbiddenResponse : ApiResponse
{
    public ApiForbiddenResponse(ModelStateDictionary modelState)
        : base(403)
    {
        if (modelState.IsValid) throw new ArgumentException("ModelState must be invalid", nameof(modelState));
        
        Errors = Enumerable
            .ToArray<string>(modelState
                .SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage));
    }

    public ApiForbiddenResponse(IdentityResult identityResult)
        : base(403)
    {
        Errors = Enumerable.ToArray<string>(identityResult.Errors
            .Select(x => x.Code + ": " + x.Description));
    }

    public ApiForbiddenResponse(string message)
        : base(403, message)
    {
    }
}