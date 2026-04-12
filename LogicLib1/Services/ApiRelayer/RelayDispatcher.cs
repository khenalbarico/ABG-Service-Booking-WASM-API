using LogicLib1.Services.AppDb;
using LogicLib1.Services.AppSmtp;
using System.Reflection;
using System.Text.Json;

namespace LogicLib1.Services.ApiRelayer;

public sealed class RelayDispatcher(IServiceProvider _services) : IRelayDispatcher
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    private static readonly Dictionary<string, Type> AllowedServices = new(StringComparer.Ordinal)
    {
        ["IAppDbOperator"] = typeof(IAppDbOperator),
        ["IAppEmailer"] = typeof(IAppEmailer)
    };

    public async Task<object?> DispatchAsync(RelayReq request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.ClassName))
            throw new ArgumentException("ClassName is required.");

        if (string.IsNullOrWhiteSpace(request.MethodName))
            throw new ArgumentException("MethodName is required.");

        if (!AllowedServices.TryGetValue(request.ClassName, out var serviceType))
            throw new ArgumentException($"Unsupported class: {request.ClassName}");

        var service = _services.GetService(serviceType)
            ?? throw new InvalidOperationException($"Service not registered: {request.ClassName}");

        var method = FindMethod(serviceType, request.MethodName)
            ?? throw new ArgumentException($"Method not found: {request.ClassName}.{request.MethodName}");

        var args = BuildArguments(method, request.Payload, ct);

        var invocationResult = method.Invoke(service, args);

        if (invocationResult is Task task)
        {
            await task;

            var taskType = task.GetType();
            if (taskType.IsGenericType)
            {
                return taskType.GetProperty("Result")?.GetValue(task);
            }

            return null;
        }

        return invocationResult;
    }

    private static MethodInfo? FindMethod(Type serviceType, string methodName)
    {
        return serviceType
            .GetMethods()
            .FirstOrDefault(m =>
                string.Equals(m.Name, methodName, StringComparison.Ordinal) &&
                IsSupportedSignature(m));
    }

    private static bool IsSupportedSignature(MethodInfo method)
    {
        var parameters = method.GetParameters();

        return parameters.Length switch
        {
            0 => true,
            1 => parameters[0].ParameterType != typeof(CancellationToken),
            2 => parameters[1].ParameterType == typeof(CancellationToken),
            _ => false
        };
    }

    private static object?[] BuildArguments(MethodInfo method, JsonElement? payload, CancellationToken ct)
    {
        var parameters = method.GetParameters();

        if (parameters.Length == 0)
            return [];

        if (parameters.Length == 1)
        {
            if (parameters[0].ParameterType == typeof(CancellationToken))
                return [ct];

            return [DeserializePayload(payload, parameters[0].ParameterType)];
        }

        return
        [
            DeserializePayload(payload, parameters[0].ParameterType),
            ct
        ];
    }

    private static object? DeserializePayload(JsonElement? payload, Type targetType)
    {
        if (payload is null)
        {
            if (targetType.IsValueType && Nullable.GetUnderlyingType(targetType) is null)
                throw new ArgumentException($"Payload is required for type {targetType.Name}.");

            return null;
        }

        return payload.Value.Deserialize(targetType, JsonOptions);
    }
}