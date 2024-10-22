using Events.Api.Filters;
using Events.Application.Services.MemberService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class RegistrationController : Controller
{
    private readonly IMemberService memberService;

    public RegistrationController(IMemberService memberService)
    {
        this.memberService = memberService;
    }

    [HttpPost]
    [ServiceFilter(typeof(BindingFilter))]
    public async Task<IActionResult> RegisterOnEvent([FromBody] string eventId)
    {
        await memberService.AddMemberToEvent(eventId, User);

        return Ok();
    }

    [HttpDelete]
    [ServiceFilter(typeof(BindingFilter))]
    public async Task<IActionResult> LeaveTheEvent([FromBody] string eventId)
    {
        await memberService.DeleteMemberFromEvent(eventId, User);

        return Ok();
    }

    [Route("List")]
    [HttpGet]
    [ServiceFilter(typeof(BindingFilter))]
    public async Task<IActionResult> GetAllMembers([FromQuery] string eventId)
    {
        var members = await memberService.GetMembersOfEvent(eventId);

        return Ok(members);
    }
}
