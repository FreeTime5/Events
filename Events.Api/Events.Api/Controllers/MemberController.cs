using Events.Api.Filters;
using Events.Application.Models.Member;
using Events.Application.Services.MemberService;
using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MemberController : Controller
    {
        private readonly IMemberService memberService;

        public MemberController(IMemberService memberService)
        {
            this.memberService = memberService;
        }

        [HttpPut]
        [ServiceFilter(typeof(BindingFilter))]
        public async Task<IActionResult> UpdateMember([FromBody] UpdateMemberDTO requestDTO)
        {
            await memberService.UpdateMemberInformation(requestDTO, User.Identity.Name);

            return Ok();
        }
    }
}
