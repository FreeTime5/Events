using AutoMapper;
using Events.Application.Exceptions;
using Events.Application.Models.Event;
using Events.Application.Services.ImageService;
using Events.Infrastructure.Entities;
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
    private readonly UserManager<MemberDb> userManager;
    private readonly IMapper mapper;
    private readonly IValidator<CreateEventRequestDTO> createEventValidator;
    private readonly IValidator<UpdateEventRequestDTO> updateValidator;

    public EventService(IUnitOfWork unitOfWork,
        IImageService imageService,
        UserManager<MemberDb> userManager,
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

    public async Task Create(CreateEventRequestDTO eventRequestDTO, ClaimsPrincipal claims)
    {
        var validationResult = await createEventValidator.ValidateAsync(eventRequestDTO);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var user = await userManager.FindByNameAsync(claims.Identity.Name) ?? throw new ItemNotFoundException("User");

        var sameEvent = await unitOfWork.EventRepository.GetByTitle(eventRequestDTO.Title);
        if (sameEvent != null)
        {
            throw new ItemAlreadyAddedException("Event");
        }

        var imagePath = imageService.GetImagePath(eventRequestDTO.Image);

        var eventInstance = mapper.Map<EventDb>(eventRequestDTO);
        eventInstance.Creator = user;
        eventInstance.ImageUrl = imagePath;
        eventInstance.ImageName = imageService.GetImageName(imagePath);

        await unitOfWork.EventRepository.Add(eventInstance);
        await UploadImage(imagePath, eventRequestDTO.Image);
    }

    private async Task UploadImage(string imagePath, IFormFile? file)
    {
        if (!string.IsNullOrEmpty(imagePath))
        {
            await imageService.UploadImage(imagePath, file);
        }
    }

    public async Task DeleteEvent(string eventId, ClaimsPrincipal claims)
    {
        var user = await userManager.FindByNameAsync(claims.Identity.Name) ?? throw new ItemNotFoundException("User");

        var role = (await userManager.GetRolesAsync(user)).First();

        var eventInstance = await unitOfWork.EventRepository.GetById(eventId) ?? throw new ItemNotFoundException("Event do not exist");

        if (role != "Admin" && eventInstance.CreatorId != user.Id)
        {
            throw new UserHaveNoPermissionException();
        }

        string imagePath = eventInstance!.ImageUrl!;
        await imageService.DeleteImage(imagePath);
        await unitOfWork.EventRepository.Delete(eventInstance);
    }

    public IEnumerable<GetEventsResponseDTO> GetAllEvents()
    {
        var events = unitOfWork.EventRepository.GetAll();
        var eventsDTOs = MapEvents(events);
        return eventsDTOs;
    }

    public async Task<GetEventsResponseDTO> GetEventById(string id)
    {
        var eventInstance = await unitOfWork.EventRepository.GetById(id) ?? throw new ItemNotFoundException("Event");

        return MapEvent(eventInstance);
    }

    public async Task<GetEventsResponseDTO> GetEventsByName(string name)
    {
        var ev = await unitOfWork.EventRepository.GetByTitle(name) ?? throw new ItemNotFoundException("Event");

        return MapEvent(ev);
    }

    public IEnumerable<GetEventsResponseDTO> GetEventsWithPagination(int page)
    {
        if (page < 1)
        {
            throw new InvalidDataException("Page must be 1 or greater");
        }
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
            var filteredEvents = events.Where(filterBy);
            var eventsOnPage = Paginate(page, filteredEvents.AsQueryable());
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

    private Func<EventDb, bool> GetFilterItem(string filter, string filterValue)
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

    public async Task<IEnumerable<GetAllUsersResponseDTO>> UpdateEvent(UpdateEventRequestDTO requestDTO, ClaimsPrincipal claims)
    {
        var validationResult = await updateValidator.ValidateAsync(requestDTO);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var user = await userManager.FindByNameAsync(claims.Identity.Name) ?? throw new ItemNotFoundException("User");

        var eventEntity = await unitOfWork.EventRepository.GetById(requestDTO.Id) ?? throw new ItemNotFoundException("Event");

        if (!await userManager.IsInRoleAsync(user, "Admin") && user.Id != eventEntity.CreatorId)
        {
            throw new UserHaveNoPermissionException();
        }

        string previousImagePath = eventEntity.ImageUrl;

        var imagePath = imageService.GetImagePath(requestDTO.Image);
        eventEntity = MapUpdateEventRequestDTO(requestDTO, eventEntity, imagePath);

        await unitOfWork.EventRepository.Update(eventEntity);
        await imageService.UpdateImage(previousImagePath, imagePath, requestDTO.Image);

        return await GetAllUsersRegistredOnEvent(requestDTO.Id);
    }

    private EventDb MapUpdateEventRequestDTO(UpdateEventRequestDTO requestDTO, EventDb previousEvent, string imagePath)
    {
        previousEvent.Title = requestDTO.Title ?? previousEvent.Title;
        previousEvent.Describtion = requestDTO.Describtion ?? previousEvent.Describtion;
        previousEvent.Date = requestDTO.Date ?? previousEvent.Date;
        previousEvent.Place = requestDTO.Place ?? previousEvent.Place;
        previousEvent.CategoryId = requestDTO.CategoryId ?? previousEvent.CategoryId;
        previousEvent.ImageUrl = requestDTO.Image == null ? previousEvent.ImageUrl : imagePath;

        return previousEvent;
    }

    private IEnumerable<GetEventsResponseDTO> MapEvents(IEnumerable<EventDb> events)
    {
        List<GetEventsResponseDTO> eventDTOs = [];
        foreach (var ev in events)
        {
            eventDTOs.Add(MapEvent(ev));
        }
        return eventDTOs;
    }

    private GetEventsResponseDTO MapEvent(EventDb ev)
    {
        GetEventsResponseDTO eventDTO = mapper.Map<GetEventsResponseDTO>(ev);

        eventDTO.CreatorName = ev.Creator != null ? ev.Creator.UserName : null;
        eventDTO.CategoryName = ev.Category != null ? ev.Category.Name : null;

        return eventDTO;
    }

    public async Task<IEnumerable<GetAllUsersResponseDTO>> GetAllUsersRegistredOnEvent(string eventId)
    {
        var eventInstance = await unitOfWork.EventRepository.GetByIdWithRegistrations(eventId) ?? throw new ItemNotFoundException("Event");

        return eventInstance.Registrations.Select(r => r.Member).Select(u => new GetAllUsersResponseDTO() { Email = u.Email, UserName = u.UserName });
    }
}
