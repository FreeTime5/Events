using AutoMapper;
using Events.Application.Servicies.EventService.DTOs;
using Events.Application.Servicies.MemberService.DTOs;
using Events.Domain.Entities;

namespace Events.Application.Servicies.Profiles;

public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        CreateMap<User, GetMemberDTO>();
        CreateMap<Event, GetEventsResponseDTO>();
        CreateMap<CreateEventRequestDTO, Event>();
    }
}
