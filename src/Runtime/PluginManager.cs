using System.Reflection;
using hayward.Runtime.Plugins;

namespace hayward.Runtime;

public sealed class PluginManager
{
    private readonly List<IBuiltinHandler> _handlers = new();

    public void Load(string dllPath)
    {
        var asm = Assembly.LoadFrom(dllPath);

        var handlerTypes = asm.GetTypes()
            .Where(t => typeof(BuiltinHandlerBase).IsAssignableFrom(t) && !t.IsAbstract);

        foreach (var type in handlerTypes)
        {
            var handler = (IBuiltinHandler)Activator.CreateInstance(type)!;
            _handlers.Add(handler);
        }
    }

    public void RegisterAll(KContext context)
    {
        foreach (var h in _handlers)
        {
            h.Register(context);
        }
    }
}