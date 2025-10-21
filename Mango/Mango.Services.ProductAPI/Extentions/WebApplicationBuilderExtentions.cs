using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Mango.Services.ProductAPI.Extentions;

public static class WebApplicationBuilderExtentions
{
    public static WebApplicationBuilder AddAppAuthentication(this WebApplicationBuilder builder)
    {
        var secret = builder.Configuration.GetValue<string>("ApiSettings:SecretKey");
        var issuer = builder.Configuration.GetValue<string>("ApiSettings:Issuer");
        var audience = builder.Configuration.GetValue<string>("ApiSettings:Audience");
        var key = Encoding.ASCII.GetBytes(secret);
        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience
            };
        });
        
        builder.Services.AddAuthorization();
        return builder;
    }
}