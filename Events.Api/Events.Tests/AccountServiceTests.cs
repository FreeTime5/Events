using Events.Application.Models.Account;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Events.Tests
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class AccountServiceTests
    {
        private readonly Services services;

        public AccountServiceTests()
        {
            services = new Services();
        }

        [Test]
        public async Task AccountService_Login_GetNotEqualTokensFromLogin()
        {
            using var scope = services.Provider.CreateScope();
            var accountService = scope.ServiceProvider.GetService<IAccountService>();

            var requestImitate = new LogInRequestDTO() { Password = "Igorigor5", UserName = "Freak" };

            var logInResponse1 = await accountService!.LogIn(requestImitate);
            var logInResponse2 = await accountService.LogIn(requestImitate);

            Assert.That(logInResponse2, Is.Not.EqualTo(logInResponse1));
        }

        [Test]
        public async Task AccountService_Logout_GetNullFromUserRefreshToken()
        {
            using var scope = services.Provider.CreateScope();
            var accountService = scope.ServiceProvider.GetService<IAccountService>();

            Claim[] claims = [new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "Freak"), new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "User")];
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var logInResponse1 = await accountService!.LogOut(claimsPrincipal);

            var user = await accountService.GetUser(claimsPrincipal);

            Assert.IsNull(user.RefreshToken);
        }

        [Test]
        public async Task AccountService_RegisterAndSignIn_GetSameUserFromDatabase()
        {
            using var scope = services.Provider.CreateScope();
            var accountService = scope.ServiceProvider.GetService<IAccountService>();

            var requestDTO = new RegisterRequestDTO() { UserName = "TestUser", Email = "test@email.com", Password = "TestPassword3" };

            Claim[] claims = [new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "TestUser"), new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "User")];
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var logInResponse = await accountService!.RegisterUserAndSignIn(requestDTO);

            var user = await accountService.GetUser(claimsPrincipal);

            Assert.That(requestDTO.UserName, Is.EqualTo(user.UserName));
            Assert.That(requestDTO.Email, Is.EqualTo(user.Email));
            Assert.That(requestDTO.Password, Is.Not.EqualTo(user.Email));

            await accountService.DeleteUser(user.UserName);
        }

        [Test]
        public async Task AccountService_RefreshTokens_GetUniqueTokens()
        {
            using var scope = services.Provider.CreateScope();
            var accountService = scope.ServiceProvider.GetService<IAccountService>();

            var requestDTO = new RegisterRequestDTO() { UserName = "TestUser", Email = "test@email.com", Password = "TestPassword3" };

            Claim[] claims = [new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "TestUser"), new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "User")];
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var logInResponse = await accountService!.RegisterUserAndSignIn(requestDTO);
            var user = await accountService.GetUser(claimsPrincipal);

            var refreshTokenResponse1 = await accountService.RefreshToken(new RefreshTokenRequestDTO() { JwtToken = logInResponse.JwtToken, RefreshToken = logInResponse.RefreshToken });

            Thread.Sleep(1000);

            var refreshTokenResponse2 = await accountService.RefreshToken(new RefreshTokenRequestDTO() { JwtToken = refreshTokenResponse1.JwtToken, RefreshToken = refreshTokenResponse1.RefreshToken });

            Assert.That(refreshTokenResponse1.JwtToken, Is.Not.EqualTo(refreshTokenResponse2.JwtToken));
            Assert.That(refreshTokenResponse1.RefreshToken, Is.Not.EqualTo(refreshTokenResponse2.RefreshToken));

            user = await accountService.GetUser(claimsPrincipal);

            await accountService.DeleteUser(user.UserName);
        }
    }
}
