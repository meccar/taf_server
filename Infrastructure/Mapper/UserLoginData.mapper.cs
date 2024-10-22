using AutoMapper;
using taf_server.Domain.Model;
using taf_server.Infrastructure.Entities;
using taf_server.Presentations.Dtos.UserAccount;
using taf_server.Presentations.Dtos.UserLoginData;

namespace taf_server.Infrastructure.Mapper;

public class UserLoginDataMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<UserLoginDataModel, UserLoginDataEntity>();
        config.CreateMap<UserLoginDataEntity, UserLoginDataModel>();
        config.CreateMap<CreateUserLoginDataDto, UserLoginDataModel>();
        config.CreateMap<CreateUserLoginDataDto, UserLoginDataEntity>();
        config.CreateMap<UserLoginDataEntity, UserLoginDataModel>();
    }
}