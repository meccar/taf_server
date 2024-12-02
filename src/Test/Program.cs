using Duende.IdentityServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    // Configure ASP.NET Authentication
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        options.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    // Configure ASP.NET Cookie Authentication
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        // host prefixed cookie name
        options.Cookie.Name = "__Host-Standalone-bff";

        // strict SameSite handling
        options.Cookie.SameSite = SameSiteMode.Strict;

        // set session lifetime
        options.ExpireTimeSpan = TimeSpan.FromHours(8);

        // sliding or absolute
        options.SlidingExpiration = false;
    })
    // Configure OpenID Connect
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
    {
        options.Authority = "https://dev-8eufo1ac17o1ecwa.jp.auth0.com";
        
        options.ClientId = "RXbAuHJYhyO9vuDyZVcWgFM6D6VS9TSj";
        options.ClientSecret = "-5jYc5Cjan-W46LiypSKMZATcuV6dpmMjVa5sR6EafGZI8FNTSLGSG9PaDUYKIIc";
 
        options.ResponseType = OpenIdConnectResponseType.Code;
        options.ResponseMode = OpenIdConnectResponseMode.Query;
 
        options.CallbackPath = "/signin-oidc";
        options.SignedOutCallbackPath = "/signout-callback-oidc";
                
        options.UseTokenLifetime = true;
        options.MapInboundClaims = false;
        
        // Go to the user info endpoint to retrieve additional claims after creating an identity from the id_token
        options.GetClaimsFromUserInfoEndpoint = true;
        // Store access and refresh tokens in the authentication cookie
        options.SaveTokens = true;
 
        options.Scope.Clear();
        options.Scope.Add(IdentityServerConstants.StandardScopes.OpenId);
        options.Scope.Add(IdentityServerConstants.StandardScopes.Profile);
        options.Scope.Add(IdentityServerConstants.StandardScopes.Email);
        options.Scope.Add("TafServer");

    });
 
// Register BFF services and configure the BFF middleware
builder.Services.AddBff();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
// Use the BFF middleware (must be before UseAuthorization)
app.UseBff();
app.UseAuthorization();
 
// Adds the BFF management endpoints (/bff/login, /bff/logout, ...)
app.MapBffManagementEndpoints();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.AsBffApiEndpoint()
.RequireAuthorization()
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
