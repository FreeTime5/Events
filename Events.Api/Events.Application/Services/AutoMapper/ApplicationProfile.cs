using AutoMapper;
using Events.Application.Models.Event;
using Events.Application.Models.Member;
using Events.Infrastructure.Entities;

namespace Events.Application.Services.AutoMapper;

internal class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        CreateMap<MemberDb, GetMemberDTO>();
        CreateMap<EventDb, GetEventsResponseDTO>();
        CreateMap<CreateEventRequestDTO, EventDb>();
    }
}
