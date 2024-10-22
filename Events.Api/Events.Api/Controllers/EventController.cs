using Events.Api.ApiServices.EmailService;
using Events.Api.Filters;
using Events.Application.Models.Event;
using Events.Application.Services.Account;
using Events.Application.Services.EventService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class EventController : Controller
{
    private readonly IEventService eventService;

    private readonly IAccountService accoutService;
    private readonly IEmailService emailService;

    public EventController(IEventService eventService,
        IAccountService accountService,
        IEmailService emailService)
    {
        this.eventService = eventService;
        accoutService = accountService;
        this.emailService = emailService;
    }

    [Route("List")]
    [HttpGet]
    [ServiceFilter(typeof(BindingFilter))]
    public IActionResult Events([FromQuery] int page)
    {
        var events = eventService.GetEventsWithPagination(page);

        return Ok(events);
    }

    [HttpPost]
    [ServiceFilter(typeof(BindingFilter))]
    public async Task<IActionResult> CreateEvents([FromForm] CreateEventRequestDTO requestDTO)
    {
        await eventService.Create(requestDTO, User);

        return Ok();
    }

    [HttpDelete]
    [ServiceFilter(typeof(BindingFilter))]
    public async Task<IActionResult> DeleteEvent([FromBody] string EventId)
    {
        await eventService.DeleteEvent(EventId, User);

        return Ok();
    }

    [HttpPut]
    [ServiceFilter(typeof(BindingFilter))]
    public async Task<IActionResult> UpdateEvent([FromForm] UpdateEventRequestDTO requestDTO)
    {
        var subsInfo = await eventService.UpdateEvent(requestDTO, User);
        await emailService.SendEmail(subsInfo, "Events", "Event was updated");

        return Ok();
    }

    [Route("Id")]
    [HttpGet]
    [ServiceFilter(typeof(BindingFilter))]
    public async Task<IActionResult> EventById([FromQuery] string eventId)
    {
        var ev = await eventService.GetEventById(eventId);

        return Ok(ev);
    }

    [Route("Title")]
    [HttpGet]
    [ServiceFilter(typeof(BindingFilter))]
    public async Task<IActionResult> EventByTitle([FromQuery] string title)
    {
        var ev = await eventService.GetEventsByName(title);

        return Ok(ev);
    }

    [Route("Filter")]
    [HttpGet]
    [ServiceFilter(typeof(BindingFilter))]
    public IActionResult EventsWithFilter([FromQuery] string filterItem, [FromQuery] string filterValue, [FromQuery] int page)
    {
        var events = eventService.GetFilteredEvents(page, filterItem, filterValue);

        return Ok(events);
    }
}
