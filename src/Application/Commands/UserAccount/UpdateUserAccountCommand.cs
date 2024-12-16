using System.Windows.Input;
using Domain.SeedWork.Command;
using Shared.Dtos.UserAccount;

namespace Application.Commands.UserAccount;

public class UpdateUserAccountCommand : ICommand<UpdateUserAccountResponseDto>
{
    public UpdateUserAccountCommand(
        UpdateUserAccountRequestDto updateUserAccountRequestDto, string eid
    ) => (UpdateUserAccountRequestDto, Eid) = (updateUserAccountRequestDto, eid);
    
    public UpdateUserAccountRequestDto UpdateUserAccountRequestDto { get; set; }
    public string Eid { get; set; }
}