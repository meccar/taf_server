using Shared.Model;

namespace Domain.SeedWork.Results;

public class UserLoginDataResult
{
    public bool Succeeded { get; }
    public UserLoginDataModel? UserData { get; }
    public IReadOnlyCollection<string> Errors { get; }

    private UserLoginDataResult(bool succeeded, UserLoginDataModel? userData = null, IEnumerable<string>? errors = null)
    {
        Succeeded = succeeded;
        UserData = userData;
        Errors = errors?.ToArray() ?? Array.Empty<string>();
    }

    public static UserLoginDataResult Success(UserLoginDataModel userData) =>
        new(true, userData);

    public static UserLoginDataResult Failure(params string[] errors) =>
        new(false, errors: errors);
}