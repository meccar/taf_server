using Domain.SeedWork.Command;
using Shared.Dtos;

namespace Application.Commands.Auth.Delete;

public class DeleteUserCommand : ICommand<SuccessResponseDto>
{
    public string UserAccountEid { get; set; }
    public bool IsDeleted { get; set; } = true;
    public DateTime? DeletedAt { get; set; } = DateTime.Now;
    public DeleteUserCommand(
        string eid
    ) => UserAccountEid = eid;

}