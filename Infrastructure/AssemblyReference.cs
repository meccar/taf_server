using System.Reflection;

namespace Infrastructure;

/// <summary>
/// Provides a reference to the assembly containing the application.
/// </summary>
/// <remarks>
/// This static class holds a reference to the current assembly, which can be used
/// for reflection or retrieving assembly-level information, such as version or 
/// attributes.
/// </remarks>
public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(Infrastructure.AssemblyReference).Assembly;
}