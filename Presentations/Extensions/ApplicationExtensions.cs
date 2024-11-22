namespace Presentations.Extensions;

public static class ApplicationExtensions
{
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