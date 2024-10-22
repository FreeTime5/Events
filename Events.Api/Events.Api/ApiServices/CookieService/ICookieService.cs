using Events.Application.Models.Account;

namespace Events.Api.ApiServices.CookieService;

public interface ICookieService
{
    void SetAuthorizationCookies(LogInResoponseDTO loginResponse);

    bool CheckAuthorizatoinCokies();

    void DeleteAuthorizationCookies();

    Tokens GetAuthorizatoinCookies();
}
