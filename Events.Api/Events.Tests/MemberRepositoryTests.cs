using Events.Application.Models.Account;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Events.Tests
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class MemberRepositoryTests
    {
        private readonly Services services;

        public MemberRepositoryTests()
        {
            services = new Services();
        }

        [Test]
        public async Task MemberRepo_TwiceAddToEvent_CatchInvalidOperationException()
        {
            var unitOfWork = services.Provider.GetService<IUnitOfWork>()!;
            var user = await unitOfWork.MemberRepository.GetById("7215638b-42d9-4ce8-af3c-628541e2d6be") ?? throw new Exception();
            var eventInstance = await unitOfWork.EventRepository.GetById("94782050-783f-494d-8c42-0cb935076e37") ?? throw new Exception();
            var registration = new Registration() { Member = user, Event = eventInstance };

            await unitOfWork.RegistrationRepository.Add(registration);

            var existedRegistration = await unitOfWork.RegistrationRepository.Find(user.Id, eventInstance.Id);

            Assert.That(existedRegistration, Is.Not.Null);

            await unitOfWork.RegistrationRepository.Remove(existedRegistration);

        }

        [Test]
        public async Task MemberRepo_GetUserById_GetExpectedUser()
        {
            var scope = services.Provider.CreateScope();
            var memberRepository = scope.ServiceProvider.GetService<IUnitOfWork>().MemberRepository;

            var scope2 = services.Provider.CreateScope();
            var accountService = scope2.ServiceProvider.GetService<IAccountService>();

            Claim[] claims = [new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "TestUser"), new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "User")];
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var userRequestDTO = new RegisterRequestDTO() { UserName = "TestUser", Email = "test@email.com", Password = "TestPassword3" };

            await accountService!.RegisterUserAndSignIn(userRequestDTO);

            var user = await accountService.GetUser(claimsPrincipal);

            var userFromRepository = await memberRepository.GetById(user.Id);

            Assert.That(user.Id, Is.EqualTo(userFromRepository.Id));

            await accountService.DeleteUser(user.UserName);
        }
    }
}
