using Application.Dtos.Authentication.Register;
using FluentValidation;
using Infrastructure.SeedWork.Constants;

namespace Application.Validators.Auth;

public class RegisterValidator : AbstractValidator<RegisterUserRequestDto>
{
    public RegisterValidator()
    {
        RuleFor(x => x.UserLoginData.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email format is not match")
            .When(x => x.UserLoginData != null); 

        RuleFor(x => x.UserLoginData.Password)
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
            .When(x => x.UserLoginData != null); 

        RuleFor(x => x.UserLoginData.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .MinimumLength(12).WithMessage("Phone number has to be at least 8 characters")
            .MaximumLength(13).WithMessage("Phone number cannot be more than 13 characters");

        RuleFor(x => x.UserAccount.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(255).WithMessage("First name cannot be more than 255 characters");

        RuleFor(x => x.UserAccount.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(255).WithMessage("Last name cannot be more than 255 characters");

        RuleFor(x => x.UserAccount.Gender)
            .NotNull().WithMessage("Gender is required")
            .Must(x => new string[] { "Male", "Female" }.Contains(x))
            .WithMessage("Gender can only be Male or Female");

        RuleFor(x => x.UserAccount.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required")
            .Matches(TimeConstants.DateFormatRegex)
            .WithMessage($"Date of birth must be in the {TimerConstants.FormatDate}");
    }
}