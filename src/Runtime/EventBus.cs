using hayward.Parsing;
using hayward.Tracing.Error;
using hayward.Typing;

namespace hayward.Runtime;

public sealed class EventBus
{
    // string to list of (callback: Value(Function), once: bool)
    private readonly Dictionary<string, List<(Value Callback, bool Once)>> _handlers = [];

    public void On(string name, Value callback, bool once = false)
    {
        if (!_handlers.TryGetValue(name, out var list))
        {
            list = [];
            _handlers[name] = list;
        }

        list.Add((callback, once));
    }

    public void Once(string name, Value callback) => On(name, callback, once: true);

    public void Off(string name, Value? callback = null)
    {
        if (!_handlers.TryGetValue(name, out var list))
        {
            return;
        }

        if (callback == null)
        {
            list.Clear();
        }
        else
        {
            list.RemoveAll(h => h.Callback == callback);
        }
    }

    public void Emit(Token token, string name, List<Value> data)
    {
        if (!_handlers.TryGetValue(name, out var list))
        {
            return;
        }

        List<(Value, bool)> toRemove = [];

        foreach (var (cb, once) in list)
        {
            if (once) 
            {
                toRemove.Add((cb, once));
            }

            try
            {
                if (cb.IsString())
                {
                    Interpreter.Current?.InvokeEvent(token, cb.GetString(), data);
                }
            }
            catch (Exception ex)
            {
                throw new EventError(token, $"Event handler error ({name}): {ex.Message}");
            }
        }

        foreach (var item in toRemove)
        {
            list.Remove(item);
        }
    }
}