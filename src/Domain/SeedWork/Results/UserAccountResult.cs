using Shared.Model;

namespace Domain.SeedWork.Results;

public class UserAccountResult
{
    public bool Succeeded { get; }
    public UserAccountModel? UserData { get; }
    public IReadOnlyCollection<string> Errors { get; }

    private UserAccountResult(bool succeeded, UserAccountModel? userData = null, IEnumerable<string>? errors = null)
    {
        Succeeded = succeeded;
        UserData = userData;
        Errors = errors?.ToArray() ?? Array.Empty<string>();
    }

    public static UserAccountResult Success(UserAccountModel userData) =>
        new(true, userData);

    public static UserAccountResult Failure(params string[] errors) =>
        new(false, errors: errors);
}