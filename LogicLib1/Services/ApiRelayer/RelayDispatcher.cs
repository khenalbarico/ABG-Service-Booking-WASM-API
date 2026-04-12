using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;

namespace LogicLib1.Services.ApiRelayer;

public sealed class RelayDispatcher(
    IServiceProvider     services,
    RelayServiceRegistry registry) : IRelayDispatcher
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<object?> DispatchAsync(RelayReq request, CancellationToken ct = default)
    {
        var serviceType = registry.Get(request.ClassName);
        var service     = services.GetRequiredService(serviceType);

        var method = serviceType
            .GetMethods()
            .FirstOrDefault(m => m.Name == request.MethodName)
            ?? throw new InvalidOperationException(
                $"Method '{request.MethodName}' was not found on '{request.ClassName}'.");

        var result = method.Invoke(service, BuildArguments(method, request.Payload, ct));

        if (result is Task task)
        {
            await task;

            return task.GetType().IsGenericType
                ? task.GetType().GetProperty("Result")?.GetValue(task)
                : null;
        }

        return result;
    }

    private static object?[] BuildArguments(MethodInfo method, JsonElement? payload, CancellationToken ct)
    {
        var parameters = method.GetParameters();

        if (parameters.Length == 0)
            return [];

        var args = new object?[parameters.Length];
        var payloadProperties = GetPayloadProperties(payload);

        for (var i = 0; i < parameters.Length; i++)
        {
            var parameter = parameters[i];

            if (parameter.ParameterType == typeof(CancellationToken))
            {
                args[i] = ct;
                continue;
            }

            if (parameters.Length == 1)
            {
                args[i] = DeserializePayload(payload, parameter.ParameterType);
                continue;
            }

            if (!payloadProperties.TryGetValue(parameter.Name!, out var value))
                throw new InvalidOperationException(
                    $"Payload property '{parameter.Name}' was not found for method '{method.Name}'.");

            args[i] = value.Deserialize(parameter.ParameterType, JsonOptions);
        }

        return args;
    }

    private static Dictionary<string, JsonElement> GetPayloadProperties(JsonElement? payload)
    {
        if (payload is null || payload.Value.ValueKind != JsonValueKind.Object)
            return new Dictionary<string, JsonElement>(StringComparer.OrdinalIgnoreCase);

        return payload.Value
            .EnumerateObject()
            .ToDictionary(
                p => p.Name,
                p => p.Value,
                StringComparer.OrdinalIgnoreCase);
    }

    private static object? DeserializePayload(JsonElement? payload, Type targetType) =>
        payload?.Deserialize(targetType, JsonOptions);
}