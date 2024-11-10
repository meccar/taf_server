using Application.Mapper;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Configurations;

public static class MapperConfiguration
{
    public static IServiceCollection ConfigureMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(config =>
        {
            UserAccountMapper.CreateMap(config);
            UserLoginDataMapper.CreateMap(config);
            UserTokenMapper.CreateMap(config);
        });
        
        return services;
    }

}