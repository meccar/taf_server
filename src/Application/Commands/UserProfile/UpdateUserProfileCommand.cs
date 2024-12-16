using Domain.SeedWork.Command;
using Shared.Dtos.UserProfile;

namespace Application.Commands.UserProfile;

public class UpdateUserProfileCommand : ICommand<UpdateUserProfileResponseDto>
{
    public UpdateUserProfileCommand(
        UpdateUserProfileRequestDto updateUserProfileRequestDto, string eid
    ) => (UpdateUserProfileRequestDto, Eid) = (updateUserProfileRequestDto, eid);

    public UpdateUserProfileRequestDto UpdateUserProfileRequestDto { get; set; }
    public string Eid { get; set; }
}
