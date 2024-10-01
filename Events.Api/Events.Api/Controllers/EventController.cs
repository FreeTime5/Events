using Events.Application.Interfaces;
using Events.Application.Models;
using Events.Application.Servicies.EventService.DTOs;
using Events.Application.Servicies.ServiciesErrors;
using Events.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EventController : Controller
{
    private readonly IEventService _eventService;

    private readonly IAccountService _accoutService;

    public EventController(IEventService eventService, IAccountService accountService)
    {
        _eventService = eventService;
        _accoutService = accountService;
    }

    [Route("[action]")]
    [HttpGet]
    public async Task<IActionResult> Events([FromQuery] int page)
    {
        if (page < 1)
        {
            return BadRequest(Result.Failure([new Error("Incorrect page number", "", "PaginationPage")]));
        }
        var events = await _eventService.GetEventsWithPagination(page);

        if (events.Count() == 0 && page != 1)
        {
            return BadRequest(Result.Failure([new Error("Incorrect page number", "", "PaginationPage")]));
        }
        return Ok(events);
    }

    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> CreateEvents([FromForm] CreateEventRequestDTO requestDTO)
    {
        if (!_accoutService.IsSignIn(User))
        {
            return BadRequest();
        }
        var user = await _accoutService.GetUser(User);

        if (user == null)
        {
            return BadRequest(Result.Failure([AccountErrors.UserNotFound]));
        }

        var result = await _eventService.Create(requestDTO, user);

        if (!result.Secceeded)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [Route("[action]")]
    [HttpDelete]
    public async Task<IActionResult> DeleteEvent([FromBody] Guid EventId)
    {
        var user = await _accoutService.GetUser(User);
        if (user == null)
        {
            return BadRequest(Result.Failure([AccountErrors.UserNotFound]));
        }
        var result = await _eventService.DeleteEvent(EventId, user);

        if (!result.Secceeded)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> UpdateEvent([FromForm] UpdateEventRequestDTO requestDTO)
    {
        var result = await _eventService.UpdateEvent(requestDTO);

        if (!result.Secceeded)
            return BadRequest(result);

        return Ok(result);
    }

    

    [Route("Id")]
    [HttpGet]
    public async Task<IActionResult> Event([FromQuery] Guid eventId)
    {
        var ev = await _eventService.GetEventById(eventId);

        if (ev == null)
            return BadRequest(Result.Failure([EventErrors.EventNotFound]));

        return Ok(ev);
    }
    
    [Route("Title")]
    [HttpGet]
    public async Task<IActionResult> Event([FromQuery] string title)
    {
        var ev = await _eventService.GetEventsByName(title);

        if (ev == null)
            return BadRequest(Result.Failure([EventErrors.EventNotFound]));

        return Ok(ev);
    }

    [Route("Filter")]
    [HttpGet]
    public async Task<IActionResult> Events([FromQuery] string filterItem, [FromQuery] string filterValue)
    {
        var events = await _eventService.GetFilteredEvents(filterItem, filterValue);

        return Ok(events);
    }
}
