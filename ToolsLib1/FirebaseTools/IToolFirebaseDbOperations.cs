using Firebase.Database;

namespace ToolsLib1.FirebaseTools;

public interface IToolFirebaseDbOperations
{
    Task<T> GetAsync<T>(
       params string[] childPaths);

    Task<List<T>> GetListAsync<T>(
        params string[] childPaths) where T : class, new();

    Task<FirebaseObject<T>> PostAsync<T>(
        T item,
        params string[] childPaths);

    Task PutAsync<T>(
        T item,
        params string[] childPaths);

    Task PatchNodeAsync<T>(T item, params string[] childPaths);

    Task PatchFieldsAsync(
        Dictionary<string, object?> updates,
        params     string[]         childPaths);

    Task DeleteAsync(params string[] childPaths);
}