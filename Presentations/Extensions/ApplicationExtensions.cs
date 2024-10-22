using Microsoft.AspNetCore.Diagnostics;

namespace taf_server.Presentations.Extensions;

public static class ApplicationExtensions
{
    public static void UseInfrastructure(this WebApplication app, string appCors)
    {
        // app.UseSwaggerDocumentation();
        
        // app.UseMiddleware<ExceptionHandlerMiddleware>();
        
        app.UseCors(appCors);
        
        app.UseAuthentication();
        
        app.UseAuthorization();
        
        app.MapGet("/", context => Task.Run(() =>
            context.Response.Redirect("/swagger/index.html")));
        // // app.MapHub<ChatHub>("/chat");
        
        app.MapControllers();
        
    }
}