namespace Presentations.Extensions;

/// <summary>
/// Provides extension methods for configuring and setting up the application pipeline.
/// </summary>
public static class ApplicationExtensions
{
    /// <summary>
    /// Configures the HTTP request pipeline for the application, including middleware for Swagger, CORS, authentication, authorization, and controllers.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to configure.</param>
    /// <param name="appCors">The name of the CORS policy to apply.</param>
    public static void ApplicationSetup(this WebApplication app, string appCors)
    {
        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            // app.UseSwaggerDocumentation();
            app.MapGet("/", context => Task.Run(() =>
                context.Response.Redirect("/swagger/index.html")));
        }
        
        app.UseCors(appCors);
        
        app.UseAuthentication();

        app.UseAuthorization();

        // // app.MapHub<ChatHub>("/chat");

        app.MapControllers();

    }
}