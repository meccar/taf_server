using Domain.SeedWork.Command;
using Shared.Dtos.News;

namespace Application.Commands.News.Create;

public class CreateNewsCommand : ICommand<CreateNewsResponseDto>
{
    public CreateNewsCommand(
        CreateNewsRequestDto newsRequestDto
    ) => NewsRequestModel = newsRequestDto;
    
    public CreateNewsRequestDto NewsRequestModel { get; set; }
}