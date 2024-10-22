using AutoMapper;
using Events.Application.Models.Event;
using Events.Application.Models.Member;
using Events.Domain.Entities;

namespace Events.Application.Services.AutoMapper;

internal class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        CreateMap<User, GetMemberDTO>();
        CreateMap<Event, GetEventsResponseDTO>();
        CreateMap<CreateEventRequestDTO, Event>();
    }
}
