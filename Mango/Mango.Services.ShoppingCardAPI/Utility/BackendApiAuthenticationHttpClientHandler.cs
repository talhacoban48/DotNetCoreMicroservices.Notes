using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

namespace Mango.Services.ShoppingCardAPI.Utility;

public class BackendApiAuthenticationHttpClientHandler: DelegatingHandler
{
    
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BackendApiAuthenticationHttpClientHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = new HttpContextAccessor();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
        
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token); 
        
        return await base.SendAsync(request, cancellationToken);
    }
}