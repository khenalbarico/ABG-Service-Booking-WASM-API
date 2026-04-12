using LogicLib1.Helpers;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Reflection;
using System.Text.Json;

namespace LogicLib1.Services.ApiRelayer;

public sealed class ApiRelay(IRelayDispatcher dispatcher) : IApiRelay
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<HttpResponseData> GetResponse(HttpRequestData req, CancellationToken ct = default)
    {
        try
        {
            var relayReq = await JsonSerializer.DeserializeAsync<RelayReq>(req.Body, JsonOptions, ct)
                ?? throw new ArgumentException("Invalid relay request.");

            var result = await dispatcher.DispatchAsync(relayReq, ct);

            var resp = req.CreateResponse(HttpStatusCode.OK);
            await resp.WriteAsJsonAsync(result, ct);

            return resp;
        }
        catch (ArgumentException ex)
        {
            return await req.CreateTextResponse(HttpStatusCode.BadRequest, ex.Message);
        }
        catch (TargetInvocationException ex) when (ex.InnerException is not null)
        {
            return await req.CreateTextResponse(HttpStatusCode.InternalServerError, ex.InnerException.Message);
        }
        catch (Exception ex)
        {
            return await req.CreateTextResponse(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}