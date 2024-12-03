using FluentValidation;
using Shared.Constants;
using Shared.Dtos.Authentication.Register;

namespace Shared.Validators.Auth;

public class RegisterValidator : AbstractValidator<RegisterUserRequestDto>
{
    public RegisterValidator()
    {
        RuleFor(x => x.UserAccount.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email format is not match")
            .When(x => x.UserAccount != null); 

        RuleFor(x => x.UserAccount.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password need to have at least 8 characters")
            .Matches(PasswordConstants.Uppercase)
            .WithMessage("Password must contain at least one uppercase letter")
            .Matches(PasswordConstants.Lowercase)
            .WithMessage("Password must contain at least one lowercase letter")
            .Matches(PasswordConstants.Digit)
            .WithMessage("Password must contain at least one digit")
            .Matches(PasswordConstants.SpecialCharacter)
            .WithMessage("Password must contain at least one special character")
            .When(x => x.UserAccount != null); 

        RuleFor(x => x.UserAccount.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .MinimumLength(12).WithMessage("Phone number has to be at least 8 characters")
            .MaximumLength(13).WithMessage("Phone number cannot be more than 13 characters")
            .When(x => x.UserAccount != null);
        
        RuleFor(x => x.UserProfile.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(255).WithMessage("First name cannot be more than 255 characters")
            .When(x => x.UserProfile != null);
        
        RuleFor(x => x.UserProfile.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(255).WithMessage("Last name cannot be more than 255 characters")
            .When(x => x.UserProfile != null);
        
        RuleFor(x => x.UserProfile.Gender)
            .NotNull().WithMessage("Gender is required")
            .Must(x => new string[] { "Male", "Female" }.Contains(x))
            .WithMessage("Gender can only be Male or Female")
            .When(x => x.UserProfile != null);
        
        RuleFor(x => x.UserProfile.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required")
            .Matches(TimeConstants.DateFormatRegex)
            .WithMessage($"Date of birth must be in the {TimerConstants.FormatDate}")
            .When(x => x.UserProfile != null);
    }
}