namespace LogicLib1.Services.ApiRelayer;

public interface IRelayDispatcher
{
    Task<object?> DispatchAsync(RelayReq req, CancellationToken ct = default);
}
