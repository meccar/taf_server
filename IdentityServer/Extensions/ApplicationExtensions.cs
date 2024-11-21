namespace IdentityServer.Extensions;

public static class ApplicationExtensions
{
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
        app.UseAuthorization();

        app.MapControllers();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .AllowAnonymous();
        
        app.MapRazorPages()
            .RequireAuthorization();
        
        app.MapControllers()
            .RequireAuthorization();
        
        // app.MapGet("identity", (ClaimsPrincipal user) => user.Claims.Select(c => new { c.Type, c.Value }))
        //     .RequireAuthorization();
        
        // app.MapGet("/", context => Task.Run(() =>
        //     context.Response.Redirect("/swagger/index.html")));
        
        // // app.MapHub<ChatHub>("/chat");
    }
}