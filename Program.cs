using taf_server.Domain.Interfaces;
using taf_server.Domain.Repositories;
using taf_server.Infrastructure.Repositories;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.AddConsole();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication();
builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();
builder.Services.AddScoped<IUserAccountCommandRepository, UserAccountCommandRepository>();
builder.Services.AddScoped<IUserLoginDataCommandRepository, UserLoginDataCommandRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
