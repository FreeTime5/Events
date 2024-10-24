﻿using Events.Api.ApiServices.EmailService;
using Events.Api.Filters;
using Events.Application.Models.Event;
using Events.Application.Services.Account;
using Events.Application.Services.EventService;
using Events.Domain.Exceptions;
using Events.Domain.Shared;
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
        if (page < 1)
        {
            return BadRequest(new Error("Invalid page", 400));
        }
        var events = eventService.GetEventsWithPagination(page);

        if (events.Count() == 0 && page != 1)
        {
            return BadRequest(new Error("Invalid page", 400));
        }

        return Ok(events);
    }

    [HttpPost]
    [ServiceFilter(typeof(BindingFilter))]
    public async Task<IActionResult> CreateEvents([FromForm] CreateEventRequestDTO requestDTO)
    {
        var user = await accoutService.GetUser(User);

        if (user == null)
        {
            throw new ItemNotFoundException("User");
        }

        await eventService.Create(requestDTO, user);

        return Ok();
    }

    [HttpDelete]
    [ServiceFilter(typeof(BindingFilter))]
    public async Task<IActionResult> DeleteEvent([FromBody] string EventId)
    {
        var user = await accoutService.GetUser(User);
        if (user == null)
        {
            throw new ItemNotFoundException("User");
        }

        await eventService.DeleteEvent(EventId, user);

        return Ok();
    }

    [HttpPut]
    [ServiceFilter(typeof(BindingFilter))]
    public async Task<IActionResult> UpdateEvent([FromForm] UpdateEventRequestDTO requestDTO)
    {
        var user = await accoutService.GetUser(User);

        await eventService.UpdateEvent(requestDTO, user);

        var users = await eventService.GetAllUsersRegistredOnEvent(requestDTO.Id);

        foreach (var userDto in users)
        {
            if (!string.IsNullOrEmpty(user.Email))
            {
                await emailService.SendEmail(user.Email, "Events", "Event was updated");
            }
        }

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
