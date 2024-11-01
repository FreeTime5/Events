using AutoMapper;
using Events.Application.Exceptions;
using Events.Application.Models.Event;
using Events.Application.Services.ImageService;
using Events.Domain.Entities;
using Events.Infrastructure.Services.EmailService;
using Events.Infrastructure.UnitOfWork;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Events.Application.Services.EventService.Implementations;

internal class EventService : Service, IEventService
{
    private const string PLACEFILTER = "place";
    private const string MAXMEMBERSFILTER = "maxmembers";
    private const string CATEGORYIDFILTER = "categoryid";
    private const string CREATORIDFILTER = "creatorid";
    private const int EVENTSONPAGE = 8;

    private readonly IImageService imageService;
    private readonly IEmailService emailSender;
    private readonly UserManager<Member> userManager;
    private readonly IMapper mapper;
    private readonly IValidator<CreateEventRequestDTO> createEventValidator;
    private readonly IValidator<UpdateEventRequestDTO> updateValidator;

    public EventService(IUnitOfWork unitOfWork,
        IImageService imageService,
        IEmailService emailSender,
        UserManager<Member> userManager,
        IMapper mapper,
        IValidator<CreateEventRequestDTO> createEventValidator,
        IValidator<UpdateEventRequestDTO> updateValidator)
        :base(unitOfWork)
    {
        this.imageService = imageService;
        this.emailSender = emailSender;
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

        var sameEvent = await unitOfWork.GetRepository<Event>()
            .FirstOrDefault(e => e.Title == eventRequestDTO.Title);

        if (sameEvent != null)
        {
            throw new ItemAlreadyAddedException("Event");
        }

        var imagePath = imageService.GetImagePath(eventRequestDTO.Image);

        var eventInstance = mapper.Map<Event>(eventRequestDTO);

        if (eventRequestDTO.CategoryName != null) 
        {
            var category = await unitOfWork.GetRepository<Category>()
                .FirstOrDefault(c => c.Name == eventRequestDTO.CategoryName)
                ?? throw new ItemNotFoundException("Category");
            eventInstance.CategoryId = category.Id;
        }

        eventInstance.Creator = user;
        eventInstance.ImageUrl = imagePath;
        eventInstance.ImageName = imageService.GetImageName(imagePath);

        unitOfWork.GetRepository<Event>().Add(eventInstance);
        await unitOfWork.SaveChangesAsync();
        await UploadImage(imagePath, eventRequestDTO.Image);
    }

    public async Task DeleteEvent(string eventId, ClaimsPrincipal claims)
    {
        var user = await userManager.FindByNameAsync(claims.Identity.Name) ?? throw new ItemNotFoundException("User");

        var role = (await userManager.GetRolesAsync(user)).First();

        var eventInstance = await unitOfWork.GetRepository<Event>()
            .FirstOrDefault(e => e.Id == eventId)
            ?? throw new ItemNotFoundException("Event");

        if (role != "Admin" && eventInstance.CreatorId != user.Id)
        {
            throw new UserHaveNoPermissionException();
        }

        string imagePath = eventInstance!.ImageUrl!;
        await imageService.DeleteImage(imagePath);

        unitOfWork.GetRepository<Event>().Delete(eventInstance);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateEvent(UpdateEventRequestDTO requestDTO, ClaimsPrincipal claims)
    {
        var validationResult = await updateValidator.ValidateAsync(requestDTO);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var user = await userManager.FindByNameAsync(claims.Identity.Name) ?? throw new ItemNotFoundException("User");

        var eventEntity = await unitOfWork.GetRepository<Event>()
            .FirstOrDefault(e => e.Id == requestDTO.Id)
            ?? throw new ItemNotFoundException("Event");

        if (!await userManager.IsInRoleAsync(user, "Admin") && user.Id != eventEntity.CreatorId)
        {
            throw new UserHaveNoPermissionException();
        }

        string previousImagePath = eventEntity.ImageUrl;

        var imagePath = imageService.GetImagePath(requestDTO.Image);
        eventEntity = MapUpdateEventRequestDTO(requestDTO, eventEntity, imagePath);

        if (requestDTO.CategoryName != null)
        {
            var category = await unitOfWork.GetRepository<Category>()
                .FirstOrDefault(c => c.Name == requestDTO.CategoryName)
                ?? throw new ItemNotFoundException("Category");
            eventEntity.CategoryId = category.Id;
        }

        unitOfWork.GetRepository<Event>().Update(eventEntity);
        await unitOfWork.SaveChangesAsync();

        await imageService.UpdateImage(previousImagePath, imagePath, requestDTO.Image);

        var users = await GetAllUsersRegistredOnEvent(eventEntity.Id);

        emailSender.SendEmail(users, "Events", $"The event: {eventEntity.Title} has been updated");
    }

    public IEnumerable<GetEventsResponseDTO> GetAllEvents()
    {
        var events = unitOfWork.GetRepository<Event>()
            .GetAllAsNoTracking();

        var eventsDTOs = MapEvents(events);

        return eventsDTOs;
    }

    public async Task<GetEventsResponseDTO> GetEventById(string id)
    {
        var eventInstance = await unitOfWork.GetRepository<Event>()
            .FirstOrDefaultAsNoTracking(e => e.Id == id)
            ?? throw new ItemNotFoundException("Event");

        return MapEvent(eventInstance);
    }

    public async Task<GetEventsResponseDTO> GetEventsByName(string title)
    {
        var ev = await unitOfWork.GetRepository<Event>()
            .FirstOrDefaultAsNoTracking(e => e.Title == title) 
            ?? throw new ItemNotFoundException("Event");

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
        if (page < 1)
        {
            throw new InvalidDataException("Page must be 1 or greater");
        }

        var result = CheckFilterItem(filterItem);

        if (!result)
        {
            throw new InvalidDataException("Invalid filter item name");
        }

        var events = unitOfWork.GetRepository<Event>().GetAllAsNoTracking();

        var filterBy = GetFilterItem(filterItem, filterValue);
        var filteredEvents = events.Where(filterBy);
        var eventsOnPage = Paginate(page, filteredEvents.AsQueryable());

        return MapEvents(eventsOnPage);
    }

    public async Task<IEnumerable<Member>> GetAllUsersRegistredOnEvent(string eventId)
    {
        var eventInstance = await unitOfWork.GetRepository<Event>()
            .FirstOrDefaultAsNoTracking(e => e.Id == eventId) 
            ?? throw new ItemNotFoundException("Event");

        return await unitOfWork.GetRepository<Registration>()
            .FindByAsNoTracking(r => r.EventId == eventId)
            .Select(r => r.Member)
            .ToListAsync();
    }

    private async Task UploadImage(string imagePath, IFormFile? file)
    {
        if (!string.IsNullOrEmpty(imagePath))
        {
            await imageService.UploadImage(imagePath, file);
        }
    }

    private GetEventsResponseDTO MapEvent(Event ev)
    {
        GetEventsResponseDTO eventDTO = mapper.Map<GetEventsResponseDTO>(ev);

        eventDTO.CreatorName = ev.Creator?.UserName;
        eventDTO.CategoryName = ev.Category?.Name;

        return eventDTO;
    }

    private IEnumerable<GetEventsResponseDTO> MapEvents(IQueryable<Event> events)
    {
        return events.Select(MapEvent);
    }

    private Event MapUpdateEventRequestDTO(UpdateEventRequestDTO requestDTO, Event previousEvent, string imagePath)
    {
        previousEvent.Title = requestDTO.Title ?? previousEvent.Title;
        previousEvent.Describtion = requestDTO.Describtion ?? previousEvent.Describtion;
        previousEvent.Date = requestDTO.Date ?? previousEvent.Date;
        previousEvent.Place = requestDTO.Place ?? previousEvent.Place;
        previousEvent.ImageUrl = requestDTO.Image == null ? previousEvent.ImageUrl : imagePath;

        return previousEvent;
    }

    private Func<Event, bool> GetFilterItem(string filter, string filterValue)
    {
        switch (filter.ToLower())
        {
            case PLACEFILTER:
                return ev => ev.Place == filterValue;
            case MAXMEMBERSFILTER:
                return ev => ev.MaxMembers == int.Parse(filterValue);
            case CATEGORYIDFILTER:
                return ev => ev.CategoryId == filterValue;
            case CREATORIDFILTER:
                return ev => ev.CreatorId == filterValue;

        }
        return ev => ev.Place == filterValue;
    }

    private bool CheckFilterItem(string filterItem)
    {
        switch (filterItem.ToLower())
        {
            case MAXMEMBERSFILTER:
            case PLACEFILTER:
            case CATEGORYIDFILTER:
            case CREATORIDFILTER:
                return true;
        }
        return false;
    }

    private IQueryable<T> Paginate<T>(int page, IQueryable<T> events)
    {
        return events.Skip((page - 1) * EVENTSONPAGE).Take(EVENTSONPAGE);
    }
}
