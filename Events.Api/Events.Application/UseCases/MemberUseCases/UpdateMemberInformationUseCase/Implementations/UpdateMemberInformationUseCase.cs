using AutoMapper;
using Events.Application.Exceptions;
using Events.Application.Models.Member;
using Events.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.UseCases.MemberUseCases.UpdateMemberInformationUseCase.Implementations;

internal class UpdateMemberInformationUseCase : IUpdateMemberInformationUseCase
{
    private readonly UserManager<Member> userManager;
    private readonly IMapper mapper;

    public UpdateMemberInformationUseCase(UserManager<Member> userManager,
        IMapper mapper)
    {
        this.userManager = userManager;
        this.mapper = mapper;
    }

    public async Task Execute(UpdateMemberDTO requestDTO, string userName)
    {
        var user = await userManager.FindByNameAsync(userName) ?? throw new ItemNotFoundException("User");

        mapper.Map(requestDTO, user);

        await userManager.UpdateAsync(user);
    }
}
