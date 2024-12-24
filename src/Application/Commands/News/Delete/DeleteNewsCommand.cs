using Domain.SeedWork.Command;
using Shared.Dtos;

namespace Application.Commands.News.Delete;

public class DeleteNewsCommand : ICommand<SuccessResponseDto>
{
    public string NewsEid { get; set; }
    public bool IsDeleted { get; set; } = true;
    public DateTime DeletedAt { get; set; } = DateTime.Now;
    public DeleteNewsCommand(
        string eid
    ) => NewsEid = eid;
}