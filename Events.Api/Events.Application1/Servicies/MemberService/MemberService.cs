using AutoMapper;
using Events.Application.Interfaces;
using Events.Application.Models;
using Events.Application.Services.MemberService.DTOs;
using Events.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Events.Application.Services.MemberService;

public class MemberService : IMemberService
{
    private readonly IMemberRepo _memberRepo;
    private readonly IMapper _mapper;

    public MemberService(IMemberRepo memberRepo, IMapper mapper)
    {
        _memberRepo = memberRepo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetMemberDTO>> GetMembersOfEvent(Guid eventId)
    {
        var membersOfEvent = await _memberRepo.GetAllFromEvent(eventId);

        var membersDTOs = MapMembers(membersOfEvent);

        return membersDTOs;
    }

    public async Task<Result> DeleteMemberFromEvent(DeleteAndAddMemberRequestDTO requestDTO)
    {
        var result = await _memberRepo.RemoveFromEvent(requestDTO.MemberId, requestDTO.EventId);

        return result;
    }

    public async Task<Result> AddMemberToEvent(DeleteAndAddMemberRequestDTO requestDTO)
    {
        var result = await _memberRepo.AddToEvent(requestDTO.MemberId, requestDTO.EventId);
        return result;
    }

    private IEnumerable<GetMemberDTO> MapMembers (IEnumerable<User> users)
    {
        if (users.IsNullOrEmpty())
            return [];

        return users.Select(MapMember);
    }

    private GetMemberDTO MapMember(User user)
    {
        var userDTO = _mapper.Map<GetMemberDTO>(user);
        userDTO.ReqistrationCount = user.Registrations.Count;
        return userDTO;
    }
}
