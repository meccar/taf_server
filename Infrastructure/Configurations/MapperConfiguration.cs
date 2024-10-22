using taf_server.Infrastructure.Mapper;

namespace taf_server.Infrastructure.Configurations;

public static class MapperConfiguration
{
    public static IServiceCollection ConfigureMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(config =>
        {
            UserAccountMapper.CreateMap(config);
            UserLoginDataMapper.CreateMap(config);
        });
        
        return services;
    }

}