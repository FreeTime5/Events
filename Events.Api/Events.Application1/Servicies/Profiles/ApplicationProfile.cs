using AutoMapper;
using Events.Application.Services.EventService.DTOs;
using Events.Application.Services.MemberService.DTOs;
using Events.Domain.Entities;

namespace Events.Application.Services.Profiles;

public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        CreateMap<User, GetMemberDTO>();
        CreateMap<Event, GetEventsResponseDTO>();
        CreateMap<CreateEventRequestDTO, Event>();
    }
}
