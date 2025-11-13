using System.Diagnostics.CodeAnalysis;

namespace hayward.Runtime.Plugins;

/// <summary>
/// Derive every plugin from this class. It forces the trimmer to keep the type.
/// </summary>
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
public abstract class BuiltinHandlerBase : IBuiltinHandler
{
    public abstract string Prefix { get; }
    public abstract void Register(KContext context);
}