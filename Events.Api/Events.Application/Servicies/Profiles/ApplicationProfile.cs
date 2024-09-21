using AutoMapper;
using Events.Application.Servicies.EventService.DTOs;
using Events.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Application.Servicies.Profiles;

public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        CreateMap<CreateEventRequestDTO, Event>();
        CreateMap<UpdateEventRequestDTO, Event>();
    }
}
