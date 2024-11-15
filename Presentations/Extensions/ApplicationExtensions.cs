namespace Presentations.Extensions;

public static class ApplicationExtensions
{
    public static void UseInfrastructure(this WebApplication app, string appCors)
    {
        
        // Configure the HTTP request pipeline.
        // if (app.Environment.IsDevelopment())
        // {
        //     app
        //         .UseSwagger(options =>
        //     {
        //         options.SerializeAsV2 = true;
        //     })
        //         .UseSwaggerUI(options =>
        //     {
        //         options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        //         options.RoutePrefix = string.Empty;
        //     });
        // }
        app.UseSwagger();
        app.UseSwaggerUI();
        // app.UseSwaggerDocumentation();
        
        // app.UseMiddleware<ExceptionHandlerMiddleware>();
        
        app.UseCors(appCors);
        
        app.UseAuthentication();
        
        app.UseAuthorization();
        
        // app.UseEndpoints(endpoints =>
        // {
        //     endpoints.MapControllers();
        // });
        
        app.MapGet("/", context => Task.Run(() =>
            context.Response.Redirect("/swagger/index.html")));
        // // app.MapHub<ChatHub>("/chat");
        
        app.MapControllers();
        
    }
}