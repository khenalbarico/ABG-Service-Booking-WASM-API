namespace LogicLib1.Services.ApiRelayer;

public sealed class RelayServiceRegistry
{
    private readonly Dictionary<string, Type> _services = new(StringComparer.Ordinal);

    public void Add<TService>()       => _services[typeof(TService).Name] = typeof(TService);

    public Type Get(string className) => _services[className];
}