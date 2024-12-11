namespace Shared.Policies;

/// <summary>
/// Represents a policy key associated with a specific role.
/// </summary>
public class PolicyKey
{
    private readonly string _role;

    /// <summary>
    /// Initializes a new instance of the <see cref="PolicyKey"/> class with the specified role.
    /// </summary>
    /// <param name="role">The role associated with the policy key.</param>
    public PolicyKey(string role)
    {
        _role = role;
    }

    /// <summary>
    /// Returns the string representation of the policy key, which is the role.
    /// </summary>
    /// <returns>The role as a string.</returns>
    public override string ToString() => $"{_role}";
}