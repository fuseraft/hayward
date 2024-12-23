namespace citrus.Runner;

public interface IRunner
{
    int Run(string script, List<string> args);
}