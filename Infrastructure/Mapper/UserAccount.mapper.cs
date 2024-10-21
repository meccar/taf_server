using AutoMapper;
using taf_server.Domain.Model;
using taf_server.Infrastructure.Entities;
using taf_server.Presentations.Dtos.Authentication;
using taf_server.Presentations.Dtos.UserAccount;

namespace taf_server.Infrastructure.Mapper;
public class UserAccountMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<UserAccountModel, UserAccountEntity>();
        config.CreateMap<UserAccountEntity, UserAccountModel>();
        config.CreateMap<CreateUserAccountDto, UserAccountModel>();
        config.CreateMap<CreateUserAccountDto, UserAccountEntity>();
        config.CreateMap<UserAccountEntity, UserAccountModel>();
    }
}