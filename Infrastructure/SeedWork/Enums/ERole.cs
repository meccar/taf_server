using System.Security.Claims;

namespace Infrastructure.SeedWork.Enums;

public static class ERole
{
    public const string Admin = "Admin";
    public const string CompanyManager = "CompanyManager";
    public const string CompanyUser = "CompanyUser";
    public const string User = "User";
    public const string Guest = "Guest";
}