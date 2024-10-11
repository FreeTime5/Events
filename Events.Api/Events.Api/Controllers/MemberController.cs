using Events.Api.Filters;
using Events.Application.Models.Member;
using Events.Application.Services.Account;
using Events.Application.Services.MemberService;
using Events.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class MemberController : Controller
{
    private readonly IMemberService memberService;
    private readonly IAccountService accountService;

    public MemberController(IMemberService memberService, IAccountService accountService)
    {
        this.memberService = memberService;
        this.accountService = accountService;
    }

    [Route("Add")]
    [HttpPost]
    [ServiceFilter(typeof(BindingFilter))]
    public async Task<IActionResult> RegisterOnEvent([FromBody] string eventId)
    {
        var currentUser = await accountService.GetUser(User);
        if (currentUser == null)
        {
            throw new ItemNotFoundException("User");
        }

        var requestDTO = new DeleteAndAddMemberRequestDTO()
        {
            EventId = eventId,
            MemberId = currentUser.Id
        };

        await memberService.AddMemberToEvent(requestDTO);

        return Ok();
    }

    [Route("Remove")]
    [HttpDelete]
    [ServiceFilter(typeof(BindingFilter))]
    public async Task<IActionResult> LeaveTheEvent([FromBody] string eventId)
    {
        var currentUser = await accountService.GetUser(User);
        if (currentUser == null)
        {
            throw new ItemNotFoundException("User");
        }

        var requestDTO = new DeleteAndAddMemberRequestDTO()
        {
            EventId = eventId,
            MemberId = currentUser.Id
        };

        await memberService.DeleteMemberFromEvent(requestDTO);

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

    [HttpPut]
    [ServiceFilter(typeof(BindingFilter))]
    public async Task<IActionResult> UpdateMember([FromBody] UpdateMemberDTO requestDTO)
    {
        await memberService.UpdateMemberInformation(requestDTO);

        return Ok();
    }
}
