using AutoMapper;
using Events.Application.Interfaces;
using Events.Application.Models;
using Events.Application.Servicies.EventService.DTOs;
using Events.Domain.Entities;
using Events.Domain.Shared;
using FluentValidation;

namespace Events.Application.Servicies.EventService;

public class EventService
{
    private readonly IEventRepo _eventRepo;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateEventRequestDTO> _createEventValidator;
    private readonly IValidator<UpdateEventRequestDTO> _updateValidator;

    public EventService(IEventRepo eventRepo,
        IMapper mapper,
        IValidator<CreateEventRequestDTO> createEventValidator,
        IValidator<UpdateEventRequestDTO> updateValidator)
    {
        _eventRepo = eventRepo;
        _mapper = mapper;
        _createEventValidator = createEventValidator;
        _updateValidator = updateValidator;
    }

    public async Task<Result> Create(CreateEventRequestDTO eventRequestDTO)
    {
        var validationResult = await _createEventValidator.ValidateAsync(eventRequestDTO);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new Error(e.ErrorMessage, e.ErrorCode, e.PropertyName));
            return Result.Failure(errors);
        }

        var eventInstance = _mapper.Map<Event>(eventRequestDTO);
        var result = await _eventRepo.Add(eventInstance);
        return result;

    }

    public async Task<Result> DeleteEvent(Guid eventId)
    {
        return await _eventRepo.Delete(eventId);
    }

    public async Task<List<Event>> GetAllEvents()
    {
        var events = await _eventRepo.GetAll();

        return events;
    }

    public async Task<Event> GetEventById(Guid id)
    {
        var ev = await _eventRepo.GetById(id);

        return ev;
    }

    public async Task<Event> GetEventsByName(string name)
    {
        var ev = await _eventRepo.GetByName(name);

        return ev;
    }

    public async Task<List<Event>> GetSortedEvents(string sortItem)
    {
        var result = CheckSortItem(sortItem);

        if (result.Secceeded)
        {
            var events = await _eventRepo.GetAll();
            var sortBy = GetSortedItem(sortItem);
            var sortedEvents = events.OrderBy(sortBy).ToList();
            return sortedEvents;
        }
        return [];
    }

    private Result CheckSortItem(string sortItem)
    {
        switch (sortItem.ToLower())
        {
            case "maxmebmers":
            case "title":
            case "date":
                return Result.Success();

        }
        return Result.Failure([new Error("Incorrect sort item name", "", "sortItem")]);
    }

    private Func<Event, object> GetSortedItem(string sortItem)
    {
        switch (sortItem.ToLower())
        {
            case "maxmebmers":
                return ev => ev.MaxMembers;
            case "title":
                return ev => ev.Title;
            case "date":
                return ev => ev.Date;
        }
        return ev => ev.Date;
    }

    public async Task<Result> UpdateEvent(UpdateEventRequestDTO eventRequestDTO)
    {
        var validationResult = await _updateValidator.ValidateAsync(eventRequestDTO);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new Error(e.ErrorMessage, e.ErrorCode, e.PropertyName));
            return Result.Failure(errors);
        }

        var eventInstance = _mapper.Map<Event>(eventRequestDTO);
        var result = await _eventRepo.Update(eventInstance);
        return result;
    }
}
