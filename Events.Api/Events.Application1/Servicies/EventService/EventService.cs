using AutoMapper;
using Events.Application.Interfaces;
using Events.Application.Models;
using Events.Application.Services.EventService.DTOs;
using Events.Application.Services.ServiciesErrors;
using Events.Domain.Entities;
using Events.Domain.Shared;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Events.Application.Services.EventService;

public class EventService : IEventService
{
    private const int EVENTSONPAGE = 8;

    private readonly IEventRepo _eventRepo;
    private readonly IImageService _imageService;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateEventRequestDTO> _createEventValidator;
    private readonly IValidator<UpdateEventRequestDTO> _updateValidator;

    public EventService(IEventRepo eventRepo,
        IImageService imageService,
        UserManager<User> userManager,
        IMapper mapper,
        IValidator<CreateEventRequestDTO> createEventValidator,
        IValidator<UpdateEventRequestDTO> updateValidator)
    {
        _eventRepo = eventRepo;
        _imageService = imageService;
        _userManager = userManager;
        _mapper = mapper;
        _createEventValidator = createEventValidator;
        _updateValidator = updateValidator;
    }

    public async Task<Result> Create(CreateEventRequestDTO eventRequestDTO, User user)
    {
        var validationResult = await _createEventValidator.ValidateAsync(eventRequestDTO);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new Error(e.ErrorMessage, e.ErrorCode, e.PropertyName));
            return Result.Failure(errors);
        }


        var imagePath = _imageService.GetImagePath(eventRequestDTO.Image);
        var eventInstance = _mapper.Map<Event>(eventRequestDTO);
        eventInstance.Creator = user;
        eventInstance.ImageUrl = imagePath;
        var result = await _eventRepo.Add(eventInstance);
        if (result.Secceeded)
            await UploadImage(imagePath, eventRequestDTO.Image);
        return result;

    }

    private async Task UploadImage(string imagePath, IFormFile? file)
    {
        if (imagePath != null)
           await _imageService.UploadImage(imagePath, file);
    }

    public async Task<Result> DeleteEvent(Guid eventId, User user)
    {
        var role = (await _userManager.GetRolesAsync(user)).First();
        var eventInstance = await _eventRepo.GetById(eventId);
        if (eventInstance == null)
        {
            return Result.Failure([EventErrors.EventNotFound]);
        }
        if (role == "Admin" || eventInstance.CreatorId == user.Id)
        {
            string imagePath = eventInstance!.ImageUrl!;
            await _imageService.DeleteImage(imagePath);
            return await _eventRepo.Delete(eventId);
        }
        return Result.Failure([AccountErrors.UserHaveNotPermision]);
    }

    public async Task<IEnumerable<GetEventsResponseDTO>> GetAllEvents()
    {
        var events = await _eventRepo.GetAll();
        var eventsDTOs = MapEvents(events);
        return eventsDTOs;
    }

    public async Task<GetEventsResponseDTO?> GetEventById(Guid id)
    {
        var ev = await _eventRepo.GetById(id);

        if (ev == null)
            return null;

        var eventDTO = MapEvent(ev);

        return eventDTO;
    }

    public async Task<GetEventsResponseDTO?> GetEventsByName(string name)
    {
        var ev = await _eventRepo.GetByName(name);

        if (ev == null)
            return null;

        var eventDTO = MapEvent(ev);

        return eventDTO;
    }

    public async Task<IEnumerable<GetEventsResponseDTO>> GetEventsWithPagination(int page)
    {
        var allEvents = (await GetAllEvents()).ToList();
        int startIndex = EVENTSONPAGE * (page - 1);
        if (startIndex >= allEvents.Count)
        {
            return [];
        }
        if (startIndex >= allEvents.Count - EVENTSONPAGE)
        {
            return allEvents.GetRange(startIndex, allEvents.Count - startIndex);
        }
        return allEvents.GetRange(startIndex, EVENTSONPAGE);
    }

    public async Task<IEnumerable<GetEventsResponseDTO>> GetFilteredEvents(string filterItem, string filterValue)
    {
        var result = CheckFilterItem(filterItem);

        if (result.Secceeded)
        {
            var events = await _eventRepo.GetAll();
            try
            {
                var sortBy = GetFilterItem(filterItem, filterValue);
                var sortedEvents = events.Where(sortBy).ToList();
                return MapEvents(sortedEvents);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex.Message, "", ex.Source ?? "");
                return [];
            }
        }
        return [];
    }

    private Result CheckFilterItem(string sortItem)
    {
        switch (sortItem.ToLower())
        {
            case "maxmebmers":
            case "place":
            case "categoryid":
            case "creatorid":
                return Result.Success();

        }
        return Result.Failure([new Error("Incorrect sort item name", "", "sortItem")]);
    }

    

    private Func<Event, bool> GetFilterItem(string filter, string filterValue)
    {
        switch (filter.ToLower())
        {
            case "place":
                return ev => ev.Place == filterValue;
            case "maxmembers":
                return ev => ev.MaxMembers == int.Parse(filterValue);
            case "categoryid":
                return ev => ev.CategoryId == int.Parse(filterValue);
            case "creatorid":
                return ev => ev.CreatorId == filterValue;

        }
        return ev => ev.Place == filterValue;
    }

    public async Task<Result> UpdateEvent(UpdateEventRequestDTO requestDTO)
    {
        var validationResult = await _updateValidator.ValidateAsync(requestDTO);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new Error(e.ErrorMessage, e.ErrorCode, e.PropertyName));
            return Result.Failure(errors);
        }
        var previousEvent = await _eventRepo.GetById(requestDTO.Id);
        string previousImagePath = previousEvent!.ImageUrl!;
        if (previousEvent == null)
        {
            return Result.Failure([EventErrors.EventNotFound]);
        }
        var imagePath = _imageService.GetImagePath(requestDTO.Image);
        var eventInstance = MapUpdateEventRequestDTO(requestDTO, previousEvent, imagePath);
        var result = await _eventRepo.Update(eventInstance);

        if (result.Secceeded && imagePath != _imageService.GetDefaultImagePath())
            await _imageService.UpdateImage(previousImagePath, imagePath, requestDTO.Image);
        return result;
    }

    
    private Event MapUpdateEventRequestDTO (UpdateEventRequestDTO requestDTO, Event previousEvent, string imagePath)
    {
        var newEvent = new Event()
        {
            Id = requestDTO.Id,
            Title = requestDTO.Title == null ? previousEvent.Title : requestDTO.Title,
            Describtion = requestDTO.Describtion == null ? previousEvent.Describtion : requestDTO.Describtion,
            Date = requestDTO.Date == null ? previousEvent.Date : requestDTO.Date.Value,
            Place = requestDTO.Place == null ? previousEvent.Place : requestDTO.Place,
            CategoryId = requestDTO.CategoryId == null ? previousEvent.CategoryId : requestDTO.CategoryId.Value,
            ImageUrl = requestDTO.Image == null ? previousEvent.ImageUrl : imagePath
        };
        return newEvent;
    }

    private IEnumerable<GetEventsResponseDTO> MapEvents (IEnumerable<Event> events)
    {
        List<GetEventsResponseDTO> eventDTOs = [];
        foreach (var ev in events)
        {
            eventDTOs.Add(MapEvent(ev));
        }
        return eventDTOs;
    }

    private GetEventsResponseDTO MapEvent (Event ev)
    {
        GetEventsResponseDTO eventDTO = _mapper.Map<GetEventsResponseDTO>(ev);

        eventDTO.EventImageUrl = ev.ImageUrl != null ? ev.ImageUrl : null;
        eventDTO.RegistratinCount = ev.Registrations.Count;
        eventDTO.CreatorName = ev.Creator != null ? ev.Creator.UserName : null;
        eventDTO.CategoryName = ev.Category != null ? ev.Category.Name : null;

        return eventDTO; 
    }
}
