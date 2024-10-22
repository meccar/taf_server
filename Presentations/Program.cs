using FluentValidation;
using taf_server.Domain.Interfaces;
using taf_server.Domain.Repositories;
using taf_server.Infrastructure;
using taf_server.Infrastructure.Repositories;
using taf_server.Infrastructure.Repositories.Command;
using taf_server.Presentations.Dtos.Authentication;
using taf_server.Presentations.Validators.Auth;
using Serilog;
using taf_server.Presentations.Extensions;

var AppCors = "AppCors";

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

Log.Information("Starting EvenHub API up");

try
{
    // builder.Host.UseSerilog(LoggingConfiguration.Configure);
    builder.Host.AddAppConfigurations();
    
    // Add services to the container.
    builder.Logging.AddConsole();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddGrpc();

    // builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    // builder.Services.AddScoped<IUserAccountCommandRepository, UserAccountCommandRepository>();
    // builder.Services.AddScoped<IUserLoginDataCommandRepository, UserLoginDataCommandRepository>();

    // builder.Services.AddScoped<IValidator<RegisterUserRequestDto>, RegisterValidator>();

    builder.Services.ConfigureInfrastructureServices(builder.Configuration, AppCors);

        WebApplication? app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger(options => {
            options.SerializeAsV2 = true;
        });
        app.UseSwaggerUI(options => {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });
    }
    // app.MapGrpcService<GreeterService>();
    
    app.UseInfrastructure(AppCors);
    
    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");

    var type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;
}
finally
{
    Log.Information("Shut down API complete");
    Log.CloseAndFlush();
}