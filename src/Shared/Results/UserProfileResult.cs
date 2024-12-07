// using Shared.Model;
//
// namespace Shared.Results;
//
// public class UserProfileResult
// {
//     public bool Succeeded { get; }
//     public UserProfileModel? UserProfile { get; }
//     public IReadOnlyCollection<string> Errors { get; }
//
//     private UserProfileResult(bool succeeded, UserProfileModel? userProfile = null, IEnumerable<string>? errors = null)
//     {
//         Succeeded = succeeded;
//         UserProfile = userProfile;
//         Errors = errors?.ToArray() ?? Array.Empty<string>();
//     }
//
//     public static UserProfileResult Success(UserProfileModel userProfile) =>
//         new(true, userProfile);
//
//     public static UserProfileResult Failure(params string[] errors) =>
//         new(false, errors: errors);
// }