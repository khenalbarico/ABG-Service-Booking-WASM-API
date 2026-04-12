using System.Text.Json;

namespace LogicLib1.Services.ApiRelayer;

public sealed class RelayReq
{
    public string       ClassName  { get; set; } = string.Empty;
    public string       MethodName { get; set; } = string.Empty;
    public JsonElement? Payload    { get; set; }
}
