namespace Domain.Interfaces;

public interface IIdentityServer
{
    string GetIdentityServerClientId();
    string GetIdentityServerClientSecret();
    string GetIdentityServerAuthority();
    string GetIdentityServerScopes();
}