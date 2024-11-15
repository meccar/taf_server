using Infrastructure.SeedWork.Enums;

namespace Infrastructure.SeedWork.Policies;

public class PolicyKey
{
    private readonly string _role;

    public PolicyKey(string role)
    {
        _role = role;
    }

    public override string ToString() => $"{_role}";
}