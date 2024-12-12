using Events.Application.Models.Member;

namespace Events.Application.UseCases.MemberUseCases.UpdateMemberInformationUseCase;

public interface IUpdateMemberInformationUseCase
{
    Task Execute(UpdateMemberDTO requestDTO, string userName);
}
