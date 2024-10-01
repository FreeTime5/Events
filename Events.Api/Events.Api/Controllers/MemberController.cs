using Events.Application.Interfaces;
using Events.Application.Models;
using Events.Application.Servicies.MemberService.DTOs;
using Events.Application.Servicies.ServiciesErrors;
using Events.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MemberController : Controller
{
    private readonly IMemberService _memberService;
    private readonly IAccountService _accountService;

    public MemberController(IMemberService memberService, IAccountService accountService)
    {
        _memberService = memberService;
        _accountService = accountService;
    }

    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> RegisterOnEvent([FromBody] Guid eventId)
    {
        var currentUser = await _accountService.GetUser(User);
        if (currentUser == null)
            return BadRequest(Result.Failure([AccountErrors.UserNotSignedIn]));

        var requestDTO = new DeleteAndAddMemberRequestDTO()
        {
            EventId = eventId,
            MemberId = currentUser.Id
        };

        var result = await _memberService.AddMemberToEvent(requestDTO);

        if (!result.Secceeded)
            return BadRequest(result);

        return Ok(result);
    }

    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> LeaveTheEvent([FromBody] Guid eventId)
    {
        var currentUser = await _accountService.GetUser(User);
        if (currentUser == null)
            return BadRequest(Result.Failure([AccountErrors.UserNotSignedIn]));

        var requestDTO = new DeleteAndAddMemberRequestDTO()
        {
            EventId = eventId,
            MemberId = currentUser.Id
        };

        var result = await _memberService.DeleteMemberFromEvent(requestDTO);

        if (!result.Secceeded)
            return BadRequest(result);

        return Ok(result);
    }

    [Route("[action]")]
    [HttpGet]
    public async Task<IActionResult> GetAllMembers([FromQuery] Guid eventId)
    {
        var members = await _memberService.GetMembersOfEvent(eventId);

        return Ok(members);
    }
}
