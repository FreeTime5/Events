using Events.Application.Models.Account;

namespace Events.Api.ApiServices.CookieService.Implementations;

internal class CookieService : ICookieService
{
    private readonly string authorizatoinCookieName = "Authorization";
    private readonly string refreshTokenCookieName = "RefreshToken";

    private readonly IResponseCookies responseCookies;
    private readonly IRequestCookieCollection requestCookies;

    public CookieService(IHttpContextAccessor httpContextAccessor)
    {
        this.responseCookies = httpContextAccessor.HttpContext.Response.Cookies;
        this.requestCookies = httpContextAccessor.HttpContext.Request.Cookies;
    }


    public bool CheckAuthorizatoinCokies()
    {
        return requestCookies.ContainsKey(authorizatoinCookieName) && requestCookies.ContainsKey(refreshTokenCookieName);
    }

    public void DeleteAuthorizationCookies()
    {
        responseCookies.Delete(authorizatoinCookieName);
        responseCookies.Delete(refreshTokenCookieName);
    }

    public void SetAuthorizationCookies(LogInResoponseDTO resoponseDTO)
    {
        responseCookies.Append(authorizatoinCookieName, resoponseDTO.JwtToken);
        responseCookies.Append(refreshTokenCookieName, resoponseDTO.RefreshToken);

    }

    public RefreshTokenRequestDTO? GetAuthorizatoinCookies()
    {
        if (!CheckAuthorizatoinCokies())
        {
            return null;
        }

        return new RefreshTokenRequestDTO()
        {
            JwtToken = requestCookies[authorizatoinCookieName],
            RefreshToken = requestCookies[refreshTokenCookieName]
        };
    }
}
