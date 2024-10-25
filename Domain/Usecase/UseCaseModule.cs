// using Infrastructure.UsecasesProxy;
// using taf_server.Application.Commands.Auth.Register;
// using taf_server.Domain.Model;
// using taf_server.Infrastructure.SeedWork.Enums;
//
// namespace taf_server.Domain.Usecase;
//
// public static class UseCaseModule
// {
//     public static IServiceCollection AddUseCases(this IServiceCollection services)
//     {
//         services.AddKeyedScoped<IUseCase<UserAccountModel>, RegisterCommand>(UsecasesProxyProvide.RegisterUseCase);
//         
//         services.AddKeyedScoped(UsecasesProxyProvide.RegisterUseCase, 
//             (sp, key) => new UseCaseProxy<IUseCase<UserAccountModel>>(
//                 sp.GetRequiredKeyedService<IUseCase<UserAccountModel>>(key)));
//         
//         return services;
//     }
// }