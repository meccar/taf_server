using Shared.Model;

namespace Shared.Results;

public class UserAccountResult
{
    public bool Succeeded { get; }
    public UserAccountModel? UserAccount { get; }
    public IReadOnlyCollection<string> Errors { get; }

    private UserAccountResult(bool succeeded, UserAccountModel? userAccount = null, IEnumerable<string>? errors = null)
    {
        Succeeded = succeeded;
        UserAccount = userAccount;
        Errors = errors?.ToArray() ?? Array.Empty<string>();
    }

    public static UserAccountResult Success(UserAccountModel userAccount) =>
        new(true, userAccount);

    public static UserAccountResult Failure(params string[] errors) =>
        new(false, errors: errors);
}