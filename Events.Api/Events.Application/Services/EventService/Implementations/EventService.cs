using AutoMapper;
using Events.Application.Models.Event;
using Events.Application.Services.ImageService;
using Events.Domain.Entities;
using Events.Domain.Exceptions;
using Events.Infrastructure.UnitOfWorkPattern;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Events.Application.Services.EventService.Implementations;

internal class EventService : IEventService
{
    private const int EVENTSONPAGE = 8;
    private readonly IUnitOfWork unitOfWork;
    private readonly IImageService imageService;
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;
    private readonly IValidator<CreateEventRequestDTO> createEventValidator;
    private readonly IValidator<UpdateEventRequestDTO> updateValidator;

    public EventService(IUnitOfWork unitOfWork,
        IImageService imageService,
        UserManager<User> userManager,
        IMapper mapper,
        IValidator<CreateEventRequestDTO> createEventValidator,
        IValidator<UpdateEventRequestDTO> updateValidator)
    {
        this.unitOfWork = unitOfWork;
        this.imageService = imageService;
        this.userManager = userManager;
        this.mapper = mapper;
        this.createEventValidator = createEventValidator;
        this.updateValidator = updateValidator;
    }

    public async Task Create(CreateEventRequestDTO eventRequestDTO, User user)
    {
        var validationResult = await createEventValidator.ValidateAsync(eventRequestDTO);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var imagePath = imageService.GetImagePath(eventRequestDTO.Image);
        var eventInstance = mapper.Map<Event>(eventRequestDTO);

        eventInstance.Creator = user;
        eventInstance.ImageUrl = imagePath;

        var succesed = await unitOfWork.EventRepository.Add(eventInstance);
        if (succesed)
            await UploadImage(imagePath, eventRequestDTO.Image);
    }

    private async Task UploadImage(string imagePath, IFormFile? file)
    {
        if (!string.IsNullOrEmpty(imagePath))
            await imageService.UploadImage(imagePath, file);
    }

    public async Task DeleteEvent(string eventId, User user)
    {
        var role = (await userManager.GetRolesAsync(user)).First();
        var eventInstance = await unitOfWork.EventRepository.GetById(eventId);
        if (eventInstance == null)
        {
            throw new ItemNotFoundException("Event do not exist");
        }
        if (role == "Admin" || eventInstance.CreatorId == user.Id)
        {
            string imagePath = eventInstance!.ImageUrl!;
            await imageService.DeleteImage(imagePath);
            await unitOfWork.EventRepository.Delete(eventId);
            return;
        }
        throw new UserHaveNoPermissionException();
    }

    public IEnumerable<GetEventsResponseDTO> GetAllEvents()
    {
        var events = unitOfWork.EventRepository.GetAll();
        var eventsDTOs = MapEvents(events);
        return eventsDTOs;
    }

    public async Task<GetEventsResponseDTO> GetEventById(string id)
    {
        var ev = await unitOfWork.EventRepository.GetById(id);

        if (ev == null)
            throw new ItemNotFoundException("Event");

        var eventDTO = MapEvent(ev);

        return eventDTO;
    }

    public async Task<GetEventsResponseDTO> GetEventsByName(string name)
    {
        var ev = await unitOfWork.EventRepository.GetByName(name);

        if (ev == null)
        {
            throw new ItemNotFoundException("Event");
        }

        var eventDTO = MapEvent(ev);

        return eventDTO;
    }

    public IEnumerable<GetEventsResponseDTO> GetEventsWithPagination(int page)
    {
        var allEvents = GetAllEvents();

        var evetnsOnPage = Paginate(page, allEvents.AsQueryable());
        return evetnsOnPage;
    }

    public IEnumerable<GetEventsResponseDTO> GetFilteredEvents(int page, string filterItem, string filterValue)
    {
        var result = CheckFilterItem(filterItem);

        if (result)
        {
            var events = unitOfWork.EventRepository.GetAll();

            var filterBy = GetFilterItem(filterItem, filterValue);
            var filteredEvents = events.Where(filterBy).AsQueryable();
            var eventsOnPage = Paginate(page, filteredEvents);
            return MapEvents(eventsOnPage);
        }

        throw new InvalidDataException("Invalid filter item name");
    }

    private IEnumerable<T> Paginate<T>(int page, IQueryable<T> events)
    {
        return events.Skip((page - 1) * EVENTSONPAGE).Take(EVENTSONPAGE);
    }

    private bool CheckFilterItem(string filterItem)
    {
        switch (filterItem.ToLower())
        {
            case "maxmebmers":
            case "place":
            case "categoryid":
            case "creatorid":
                return true;
        }
        return false;
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
                return ev => ev.CategoryId == filterValue;
            case "creatorid":
                return ev => ev.CreatorId == filterValue;

        }
        return ev => ev.Place == filterValue;
    }

    public async Task UpdateEvent(UpdateEventRequestDTO requestDTO, ClaimsPrincipal claims)
    {
        var validationResult = await updateValidator.ValidateAsync(requestDTO);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        var previousEvent = await unitOfWork.EventRepository.GetById(requestDTO.Id);

        if (previousEvent == null)
        {
            throw new ItemNotFoundException("Event");
        }

        var user = await userManager.GetUserAsync(claims);

        if (!await userManager.IsInRoleAsync(user, "Admin") && user.Id != previousEvent.CreatorId)
        {
            throw new UserHaveNoPermissionException();
        }

        string previousImagePath = previousEvent.ImageUrl;

        var imagePath = imageService.GetImagePath(requestDTO.Image);
        var eventInstance = MapUpdateEventRequestDTO(requestDTO, previousEvent, imagePath);

        await unitOfWork.EventRepository.Update(eventInstance);
        await imageService.UpdateImage(previousImagePath, imagePath, requestDTO.Image);
    }


    private Event MapUpdateEventRequestDTO(UpdateEventRequestDTO requestDTO, Event previousEvent, string imagePath)
    {
        var newEvent = new Event()
        {
            Id = requestDTO.Id,
            Title = requestDTO.Title == null ? previousEvent.Title : requestDTO.Title,
            Describtion = requestDTO.Describtion == null ? previousEvent.Describtion : requestDTO.Describtion,
            Date = requestDTO.Date == null ? previousEvent.Date : requestDTO.Date.Value,
            Place = requestDTO.Place == null ? previousEvent.Place : requestDTO.Place,
            CategoryId = requestDTO.CategoryId == null ? previousEvent.CategoryId : requestDTO.CategoryId,
            ImageUrl = requestDTO.Image == null ? previousEvent.ImageUrl : imagePath
        };
        return newEvent;
    }

    private IEnumerable<GetEventsResponseDTO> MapEvents(IEnumerable<Event> events)
    {
        List<GetEventsResponseDTO> eventDTOs = [];
        foreach (var ev in events)
        {
            eventDTOs.Add(MapEvent(ev));
        }
        return eventDTOs;
    }

    private GetEventsResponseDTO MapEvent(Event ev)
    {
        GetEventsResponseDTO eventDTO = mapper.Map<GetEventsResponseDTO>(ev);

        eventDTO.EventImageUrl = ev.ImageUrl != null ? ev.ImageUrl : null;
        eventDTO.RegistratinCount = ev.Registrations.Count;
        eventDTO.CreatorName = ev.Creator != null ? ev.Creator.UserName : null;
        eventDTO.CategoryName = ev.Category != null ? ev.Category.Name : null;

        return eventDTO;
    }
}
