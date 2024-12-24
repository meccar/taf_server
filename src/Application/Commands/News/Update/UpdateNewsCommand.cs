using Domain.SeedWork.Command;
using Shared.Dtos.News;

namespace Application.Commands.News.Update;

public class UpdateNewsCommand : ICommand<UpdateNewsResponseDto>
{
    public UpdateNewsCommand(
        UpdateNewsRequestDto updateNewsRequestDto, string eid
    ) => (UpdateNewsRequestDto, Eid) = (updateNewsRequestDto, eid);
    
    public UpdateNewsRequestDto UpdateNewsRequestDto { get; set; }
    public string Eid { get; set; }
}