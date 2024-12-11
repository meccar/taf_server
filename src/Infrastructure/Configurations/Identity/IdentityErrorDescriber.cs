// using Microsoft.AspNetCore.Identity;
//
// namespace Infrastructure.Configurations.Identity;
// public class CustomIdentityErrorDescriber : IdentityErrorDescriber
// {
//     // Password errors
//     public override IdentityError PasswordTooShort(int length)
//     {
//         return new IdentityError
//         {
//             Code = nameof(PasswordTooShort),
//             Description = $"Password must be at least {length} characters long."
//         };
//     }
//
//     public override IdentityError PasswordRequiresNonAlphanumeric()
//     {
//         return new IdentityError
//         {
//             Code = nameof(PasswordRequiresNonAlphanumeric),
//             Description = "Password must contain at least one non-alphanumeric character (e.g., @, #, $)."
//         };
//     }
//
//     public override IdentityError PasswordRequiresDigit()
//     {
//         return new IdentityError
//         {
//             Code = nameof(PasswordRequiresDigit),
//             Description = "Password must contain at least one numeric digit (0-9)."
//         };
//     }
//
//     public override IdentityError PasswordRequiresLower()
//     {
//         return new IdentityError
//         {
//             Code = nameof(PasswordRequiresLower),
//             Description = "Password must contain at least one lowercase letter (a-z)."
//         };
//     }
//
//     public override IdentityError PasswordRequiresUpper()
//     {
//         return new IdentityError
//         {
//             Code = nameof(PasswordRequiresUpper),
//             Description = "Password must contain at least one uppercase letter (A-Z)."
//         };
//     }
//
//     public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
//     {
//         return new IdentityError
//         {
//             Code = nameof(PasswordRequiresUniqueChars),
//             Description = $"Password must contain at least {uniqueChars} unique character(s)."
//         };
//     }
//
//     // User uniqueness errors
//     public override IdentityError DuplicateEmail(string email)
//     {
//         return new IdentityError
//         {
//             Code = nameof(DuplicateEmail),
//             Description = $"The email '{email}' is already in use."
//         };
//     }
//
//     public override IdentityError DuplicateUserName(string userName)
//     {
//         return new IdentityError
//         {
//             Code = nameof(DuplicateUserName),
//             Description = $"The username '{userName}' is already taken."
//         };
//     }
//
//     public override IdentityError InvalidEmail(string email)
//     {
//         return new IdentityError
//         {
//             Code = nameof(InvalidEmail),
//             Description = $"The email '{email}' is not a valid email address."
//         };
//     }
//
//     // Token errors
//     public override IdentityError InvalidToken()
//     {
//         return new IdentityError
//         {
//             Code = nameof(InvalidToken),
//             Description = "The token provided is invalid or has expired."
//         };
//     }
//
//     public override IdentityError RecoveryCodeRedemptionFailed()
//     {
//         return new IdentityError
//         {
//             Code = nameof(RecoveryCodeRedemptionFailed),
//             Description = "The recovery code is invalid or has already been used."
//         };
//     }
//
//     public override IdentityError InvalidUserName(string userName)
//     {
//         return new IdentityError
//         {
//             Code = nameof(InvalidUserName),
//             Description = $"The username '{userName}' is invalid. It may only contain alphanumeric characters and special symbols."
//         };
//     }
//
//     // Default errors for other cases
//     public override IdentityError DefaultError()
//     {
//         return new IdentityError
//         {
//             Code = nameof(DefaultError),
//             Description = "An unknown error has occurred. Please try again."
//         };
//     }
// }