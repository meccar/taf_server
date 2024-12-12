using Domain.SeedWork.Command;
using Shared.Dtos.News;

namespace Application.Commands.News;

public class CreateNewsCommand : ICommand<CreateNewsResponseDto>
{
    public CreateNewsCommand(
        CreateNewsRequestDto newsRequestDto
    ) =>
    NewsRequestModel = newsRequestDto;
    
    public CreateNewsRequestDto NewsRequestModel { get; set; }
}