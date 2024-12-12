using Events.Api.Filters;
using Events.Application.Models.Event;
using Events.Application.Services.ClaimsService;
using Events.Application.UseCases.EventUseCases.CreateEventUseCase;
using Events.Application.UseCases.EventUseCases.DeleteEventUseCase;
using Events.Application.UseCases.EventUseCases.GetEventByIdUseCase;
using Events.Application.UseCases.EventUseCases.GetEventByNameUseCase;
using Events.Application.UseCases.EventUseCases.GetEventsWithPaginationUseCase;
using Events.Application.UseCases.EventUseCases.GetFilteredEventsUseCase;
using Events.Application.UseCases.EventUseCases.UpdateEventUseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class EventController : Controller
{
    private readonly ICreateEventUseCase createEventUseCase;
    private readonly IDeleteEventUseCase deleteEventUseCase;
    private readonly IUpdateEventUseCase updateEventUseCase;
    private readonly IGetEventByIdUseCase getEventByIdUseCase;
    private readonly IGetEventByNameUseCase getEventByNameUseCase;
    private readonly IGetEventsWithPaginationUseCase getEventsWithPaginationUseCase;
    private readonly IGetFilteredEventsUseCase getFilteredEventsUseCase;
    private readonly IClaimsService claimsService;

    public EventController(ICreateEventUseCase createEventUseCase,
        IDeleteEventUseCase deleteEventUseCase,
        IUpdateEventUseCase updateEventUseCase,
        IGetEventByIdUseCase getEventByIdUseCase,
        IGetEventByNameUseCase getEventByNameUseCase,
        IGetEventsWithPaginationUseCase getEventsWithPaginationUseCase,
        IGetFilteredEventsUseCase getFilteredEventsUseCase,
        IClaimsService claimsService)
    {
        this.createEventUseCase = createEventUseCase;
        this.deleteEventUseCase = deleteEventUseCase;
        this.updateEventUseCase = updateEventUseCase;
        this.getEventByIdUseCase = getEventByIdUseCase;
        this.getEventByNameUseCase = getEventByNameUseCase;
        this.getEventsWithPaginationUseCase = getEventsWithPaginationUseCase;
        this.getFilteredEventsUseCase = getFilteredEventsUseCase;
        this.claimsService = claimsService;
    }

    [Route("List")]
    [HttpGet]
    [ServiceFilter(typeof(BindingFilter))]
    public IActionResult Events([FromQuery] int page)
    {
        var events = getEventsWithPaginationUseCase.Execute(page);

        return Ok(events);
    }

    [HttpPost]
    [ServiceFilter(typeof(BindingFilter))]
    public async Task<IActionResult> CreateEvents([FromForm] CreateEventRequestDTO requestDTO)
    {
        var userName = claimsService.GetName(User);

        await createEventUseCase.Execute(requestDTO, userName);

        return Ok();
    }

    [HttpDelete]
    [ServiceFilter(typeof(BindingFilter))]
    public async Task<IActionResult> DeleteEvent([FromBody] string EventId)
    {
        var userName = claimsService.GetName(User);

        await deleteEventUseCase.Execute(EventId, userName);

        return Ok();
    }

    [HttpPut]
    [ServiceFilter(typeof(BindingFilter))]
    public async Task<IActionResult> UpdateEvent([FromForm] UpdateEventRequestDTO requestDTO)
    {
        var userName = claimsService.GetName(User);

        await updateEventUseCase.Execute(requestDTO, userName);

        return Ok();
    }

    [Route("Id")]
    [HttpGet]
    [ServiceFilter(typeof(BindingFilter))]
    public async Task<IActionResult> EventById([FromQuery] string eventId)
    {
        var eventEntity = await getEventByIdUseCase.Execute(eventId);

        return Ok(eventEntity);
    }

    [Route("Title")]
    [HttpGet]
    [ServiceFilter(typeof(BindingFilter))]
    public async Task<IActionResult> EventByTitle([FromQuery] string name)
    {
        var eventEntity = await getEventByNameUseCase.Execute(name);

        return Ok(eventEntity);
    }

    [Route("Filter")]
    [HttpGet]
    [ServiceFilter(typeof(BindingFilter))]
    public IActionResult EventsWithFilter([FromQuery] string filterItem, [FromQuery] string filterValue, [FromQuery] int page)
    {
        var events = getFilteredEventsUseCase.Execute(page, filterItem, filterValue);

        return Ok(events);
    }
}
