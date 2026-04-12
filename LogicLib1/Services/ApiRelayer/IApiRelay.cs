using Microsoft.Azure.Functions.Worker.Http;

namespace LogicLib1.Services.ApiRelayer;

public interface IApiRelay
{
    Task<HttpResponseData> GetResponse(
         HttpRequestData   req,
         CancellationToken ct = default);
}
