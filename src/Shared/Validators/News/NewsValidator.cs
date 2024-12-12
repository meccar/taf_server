using FluentValidation;
using Shared.Dtos.News;

namespace Shared.Validators.News;

public class NewsValidator : AbstractValidator<CreateNewsRequestDto>
{
    public NewsValidator()
    {
        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("ImageUrl is required");
        
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required");
        
        RuleFor(x => x.SubTitle);
        
        RuleFor(x => x.Summary);
        
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required");
        
        RuleFor(x => x.IsPublished)
            .NotEmpty().WithMessage("IsPublished is required");
    }
}