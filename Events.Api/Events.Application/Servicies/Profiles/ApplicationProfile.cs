using AutoMapper;
using Events.Application.Servicies.EventService.DTOs;
using Events.Domain.Entities;

namespace Events.Application.Servicies.Profiles;

public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        CreateMap<CreateEventRequestDTO, Event>();
        CreateMap<UpdateEventRequestDTO, Event>();
    }
}
