using System.Reflection;

namespace Application;

/// <summary>
/// Provides a reference to the assembly containing the <see cref="AssemblyReference"/> class.
/// This class is typically used to retrieve metadata about the assembly, such as its version or name.
/// </summary>
public static class AssemblyReference
{
    /// <summary>
    /// Gets the <see cref="Assembly"/> object representing the assembly that contains the <see cref="AssemblyReference"/> class.
    /// </summary>
    /// <remarks>
    /// This static field is initialized once when the application starts and can be used to inspect 
    /// or access metadata about the current assembly, such as version, location, and other assembly attributes.
    /// </remarks>
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}