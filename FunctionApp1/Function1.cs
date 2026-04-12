using LogicLib1.Services.ApiRelayer;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace FunctionApp1;

public class Function1(IApiRelay _relay)
{
    [Function("Relay")]
    public Task<HttpResponseData> Relay(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "relay")] HttpRequestData req,
        FunctionContext context)
        => _relay.GetResponse(req, context.CancellationToken);
}