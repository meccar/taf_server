using Domain.SeedWork.Command;
using Shared.Dtos.Authentication;

namespace Application.Commands.Auth.Delete;

public class DeleteUserCommand : ICommand<DeleteUserResponseDto>
{
    public string Eid { get; set; }
    public bool IsDeleted { get; set; } = true;
    public DateTime? DeletedAt { get; set; } = DateTime.Now;
    public DeleteUserCommand(
        string eid
    ) => Eid = eid;

}