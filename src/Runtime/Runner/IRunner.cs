namespace hayward.Runtime.Runner;

public interface IRunner
{
    int Run(string script, List<string> args);
}