using Events.Application.Models.Account;

namespace Events.Api.ApiServices.CookieService;

public interface ICookieService
{
    void SetAuthorizationCookies(LogInResoponseDTO jwtToken);

    bool CheckAuthorizatoinCokies();

    void DeleteAuthorizationCookies();

    RefreshTokenRequestDTO? GetAuthorizatoinCookies();
}
