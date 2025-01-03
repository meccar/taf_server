using System.Security.Claims;

namespace IdentityServer.Pipeline;

/// <summary>
/// Provides extension methods for configuring the application pipeline.
/// </summary>
public static class ApplicationExtensions
{
    /// <summary>
    /// Configures the application setup by adding middleware components to the HTTP request pipeline.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to configure.</param>
    /// <param name="appCors">The name of the CORS policy to apply.</param>
    public static void UseApplicationSetup(this WebApplication app, string appCors)
    {
        
        // Configure the HTTP request pipeline.

        
        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }
        
        // app.UseSwaggerDocumentation();
        
        // app.UseMiddleware<ExceptionHandlerMiddleware>();
        
        app.UseCors(appCors);
        
        app.UseHttpsRedirection();
        
        app.UseStaticFiles();
        app.UseRouting();
        
        app.UseIdentityServer();
        
        app.UseAuthentication();
        app.UseBff();
        app.UseAuthorization();

        app.MapBffManagementEndpoints();
        
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .AllowAnonymous();
        
        app.MapRazorPages()
            .RequireAuthorization();

        app.MapControllers()
            .AsBffApiEndpoint()
            .RequireAuthorization();
        
        app.MapGet("identity", (ClaimsPrincipal user) => user.Claims.Select(c => new { c.Type, c.Value }))
            .RequireAuthorization();
        
        // app.MapGet("/", context => Task.Run(() =>
        //     context.Response.Redirect("/swagger/index.html")));
        
        // // app.MapHub<ChatHub>("/chat");
    }
}