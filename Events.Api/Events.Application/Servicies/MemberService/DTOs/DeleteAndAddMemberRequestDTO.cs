using Events.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Application.Servicies.MemberService.DTOs;

public class DeleteAndAddMemberRequestDTO
{
    public Guid MemberId { get; set; }

    public Guid EventId { get; set; }
}
