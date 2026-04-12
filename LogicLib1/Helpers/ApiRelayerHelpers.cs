using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace LogicLib1.Helpers;

public static class ApiRelayerHelpers
{
    public static async Task<HttpResponseData> CreateTextResponse(
        this HttpRequestData req,
             HttpStatusCode  statusCode,
             string          message)
    {
        var response = req.CreateResponse(statusCode);

        await response.WriteStringAsync(message);

        return response;
    }
}
