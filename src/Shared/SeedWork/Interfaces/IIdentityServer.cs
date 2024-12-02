namespace Shared.SeedWork.Interfaces;

public interface IIdentityServer
{
    string GetIdentityServerAuthority();
    string GetIdentityServerClientId();
    string GetIdentityServerClientName();
    string GetIdentityServerClientSecret();
    string GetIdentityServerInteractiveClientId();
    string GetIdentityServerInteractiveClientName();
    string GetIdentityServerInteractiveClientSecret();
    string GetIdentityServerScopes();
}