namespace hayward.Runtime.Plugins;

public interface IBuiltinHandler
{
    /// <summary>
    /// Unique prefix (e.g., "http")
    /// </summary>
    string Prefix { get; }

    /// <summary>
    /// Register builtins into context
    /// </summary>
    void Register(KContext context);
}