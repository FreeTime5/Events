using Events.Api.Filters;
using Events.Application.Models.Member;
using Events.Application.Services.ClaimsService;
using Events.Application.UseCases.MemberUseCases.UpdateMemberInformationUseCase;
using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MemberController : Controller
    {
        private readonly IUpdateMemberInformationUseCase updateMemberInformationUseCase;
        private readonly IClaimsService claimsService;

        public MemberController(IUpdateMemberInformationUseCase updateMemberInformationUseCase,
            IClaimsService claimsService)
        {
            this.updateMemberInformationUseCase = updateMemberInformationUseCase;
            this.claimsService = claimsService;
        }

        [HttpPut]
        [ServiceFilter(typeof(BindingFilter))]
        public async Task<IActionResult> UpdateMember([FromBody] UpdateMemberDTO requestDTO)
        {
            var userName = claimsService.GetName(User);
            await updateMemberInformationUseCase.Execute(requestDTO, userName);

            return Ok();
        }
    }
}
