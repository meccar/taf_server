using System.Security.Claims;

namespace IdentityServer.Areas.Extensions;

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
        
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
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
        app.UseAuthorization();

        // app.UseEndpoints(endpoints =>
        // {
        //     endpoints.MapControllers();
        // });
        
        app.MapRazorPages()
            .RequireAuthorization();
        app.MapControllers()
            .RequireAuthorization();
        
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        
        app.MapGet("identity", (ClaimsPrincipal user) => user.Claims.Select(c => new { c.Type, c.Value }))
            .RequireAuthorization();
        
        // app.MapGet("/", context => Task.Run(() =>
        //     context.Response.Redirect("/swagger/index.html")));
        
        // // app.MapHub<ChatHub>("/chat");
    }
}