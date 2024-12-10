using AutoMapper;
using Events.Application.Exceptions;
using Events.Application.Models.Event;
using Events.Application.Models.Member;
using Events.Application.Services.ImageService;
using Events.Domain.Entities;
using Events.Domain.UnitOfWorkInterface;
using System.Runtime.CompilerServices;

namespace Events.Application.Mapping;

internal class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        CreateMap<Member, GetMemberDTO>();

        CreateMap<Event, GetEventsResponseDTO>()
            .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.Creator.UserName))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

        CreateMap<CreateEventRequestDTO, Event>();

        CreateMap<UpdateEventRequestDTO, Event>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Title, opt => opt.Condition(src => src.Title is not null))
            .ForMember(dest => dest.Describtion, opt => opt.Condition(src => src.Describtion is not null))
            .ForMember(dest => dest.Date, opt => opt.Condition(src => src.Date is not null))
            .ForMember(dest => dest.Place, opt => opt.Condition(src => src.Place is not null))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom<EventImagePathResolver>())
            .ForMember(dest => dest.ImageName, opt => opt.MapFrom<EventImageNameResolver>())
            .ForMember(dest => dest.CategoryId, opt => opt.Condition(src => src.CategoryId is not null));

        CreateMap<UpdateMemberDTO, Member>()
            .ForMember(dest => dest.Birthday, opt => opt.Condition(src => src.Birthday != default))
            .ForMember(dest => dest.FirstName, opt => opt.Condition(src => src.FirstName is not null))
            .ForMember(dest => dest.LastName, opt => opt.Condition(src => src.LastName is not null));
    }

    private class EventImagePathResolver : IValueResolver<UpdateEventRequestDTO, Event, string>
    {
        private readonly IImageService imageService;

        public EventImagePathResolver(IImageService imageService)
        {
            this.imageService = imageService;
        }

        public string Resolve(UpdateEventRequestDTO source, Event destination, string member, ResolutionContext context)
        {
            if (source.Image is not null)
            {
                return imageService.GetImagePath(source.Image);
            }

            return destination.ImageUrl;
        }
    }

    private class EventImageNameResolver : IValueResolver<UpdateEventRequestDTO, Event, string>
    {
        public string Resolve(UpdateEventRequestDTO source, Event destination, string destMember, ResolutionContext context)
        {
            if (source.Image is not null)
            {
                return source.Image.FileName;
            }

            return destination.ImageName;
        }
    }
}

