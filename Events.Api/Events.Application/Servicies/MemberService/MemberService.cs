using Events.Application.Interfaces;
using Events.Application.Models;
using Events.Application.Servicies.MemberService.DTOs;
using Events.Domain.Entities;

namespace Events.Application.Servicies.MemberService;

public class MemberService
{
    private readonly IMemberRepo _memberRepo;

    public MemberService(IMemberRepo memberRepo)
    {
        _memberRepo = memberRepo;
    }

    public async Task<List<User>> GetMemberOfEvent(Event ev)
    {
        var membersOfEvent = await _memberRepo.GetAllFromEvent(ev);
        return membersOfEvent;
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
}
