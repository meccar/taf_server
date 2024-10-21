using System.Reflection;

namespace taf_server.Infrastructure;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(Application.AssemblyReference).Assembly;
}