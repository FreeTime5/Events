using Events.Api.Filters;
using Events.Application.Services.ClaimsService;
using Events.Application.UseCases.EventUseCases.GetAllUsersRegistredOnEventUseCase;
using Events.Application.UseCases.MemberUseCases.AddMemberToEventUseCase;
using Events.Application.UseCases.MemberUseCases.RemoveMemberFromEventUseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Controllers;

[Authorize(Policy = "RolePolicy")]
[ApiController]
[Route("[controller]")]
public class RegistrationController : Controller
{
    private readonly IAddMemberToEventUseCase addMemberToEventUseCase;
    private readonly IRemoveMemberFromEventUseCase removeMemberFromEventUseCase;
    private readonly IGetAllUsersRegistredOnEventUseCase getAllUsersRegistredOnEventUseCase;
    private readonly IClaimsService claimsService;

    public RegistrationController(IAddMemberToEventUseCase addMemberToEventUseCase,
        IRemoveMemberFromEventUseCase removeMemberFromEventUseCase,
        IGetAllUsersRegistredOnEventUseCase getAllUsersRegistredOnEventUseCase,
        IClaimsService claimsService)
    {
        this.addMemberToEventUseCase = addMemberToEventUseCase;
        this.removeMemberFromEventUseCase = removeMemberFromEventUseCase;
        this.getAllUsersRegistredOnEventUseCase = getAllUsersRegistredOnEventUseCase;
        this.claimsService = claimsService;
    }

    [HttpPost]
    [ServiceFilter(typeof(BindingFilter))]
    public async Task<IActionResult> RegisterOnEvent([FromBody] string eventId)
    {
        var userName = claimsService.GetName(User);
        await addMemberToEventUseCase.Execute(eventId, userName);

        return Ok();
    }

    [HttpDelete]
    [ServiceFilter(typeof(BindingFilter))]
    public async Task<IActionResult> LeaveTheEvent([FromBody] string eventId)
    {
        var userName = claimsService.GetName(User);
        await removeMemberFromEventUseCase.Execute(eventId, userName);

        return Ok();
    }

    [Route("List")]
    [HttpGet]
    [ServiceFilter(typeof(BindingFilter))]
    public async Task<IActionResult> GetAllMembers([FromQuery] string eventId)
    {
        var members = await getAllUsersRegistredOnEventUseCase.Execute(eventId);

        return Ok(members.Select(m => new { m.Id, m.UserName }));
    }
}
