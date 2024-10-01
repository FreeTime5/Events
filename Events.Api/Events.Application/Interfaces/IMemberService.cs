using Events.Application.Models;
using Events.Application.Servicies.MemberService.DTOs;
using Events.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Application.Interfaces;

public interface IMemberService
{
    public Task<IEnumerable<GetMemberDTO>> GetMembersOfEvent(Guid eventId);

    public Task<Result> DeleteMemberFromEvent(DeleteAndAddMemberRequestDTO requestDTO);

    public Task<Result> AddMemberToEvent(DeleteAndAddMemberRequestDTO requestDTO);
}
