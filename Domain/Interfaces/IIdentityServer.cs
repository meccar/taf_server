namespace Domain.Interfaces;

public interface IIdentityServer
{
    string GetIdentityServerAuthority();
    string GetIdentityServerClientId();
    string GetIdentityServerClientName();
    string GetIdentityServerClientSecret();
    string GetIdentityServerMvcClientId();
    string GetIdentityServerMvcClientName();
    string GetIdentityServerMvcClientSecret();
    string GetIdentityServerScopes();
}