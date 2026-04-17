using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;

namespace ToolsLib1.FirebaseTools;

public class FirebaseRealtimeDb1(IFirebaseCfg _cfg) : IToolFirebaseDbOperations
{
    readonly FirebaseClient _dbClient = _cfg.CreateDbClient();

    ChildQuery BuildQuery(params string[] childPaths)
    {
        ChildQuery query = _dbClient.Child(childPaths[0]);

        for (int i = 1; i < childPaths.Length; i++)
        {
            query = query.Child(childPaths[i]);
        }

        return query;
    }

    public async Task<T> GetAsync<T>(params string[] childPaths)
    {
        var query = BuildQuery(childPaths);

        return await query.OnceSingleAsync<T>();
    }

    public async Task<List<T>> GetListAsync<T>(params string[] childPaths) where T : class, new()
    {
        var query = BuildQuery(childPaths);

        var items = await query.OnceAsync<T>();

        return [.. items.Select(x =>
    {
        var value = x.Object;

        var uidProp = typeof(T).GetProperty("Uid");
        if (uidProp is not null && uidProp.CanWrite)
        {
            uidProp.SetValue(value, x.Key);
        }

        return value;
    })];
    }

    public async Task<FirebaseObject<T>> PostAsync<T>(T item, params string[] childPaths)
    {
        var query = BuildQuery(childPaths);

        return await query.PostAsync(item);
    }

    public async Task PutAsync<T>(T item, params string[] childPaths)
    {
        var query = BuildQuery(childPaths);

        await query.PutAsync(item);
    }

    public async Task PatchNodeAsync<T>(T item, params string[] childPaths)
    {
        var query = BuildQuery(childPaths);
        var json  = JsonConvert.SerializeObject(item);

        await query.PatchAsync(json);
    }

    public async Task PatchFieldsAsync(Dictionary<string, object?> updates, params string[] childPaths)
    {
        if (updates is null || updates.Count == 0)
            return;

        var query = BuildQuery(childPaths);
        var json  = JsonConvert.SerializeObject(updates);

        await query.PatchAsync(json);
    }

    public async Task DeleteAsync(params string[] childPaths)
    {
        var query = BuildQuery(childPaths);

        await query.DeleteAsync();
    }
}