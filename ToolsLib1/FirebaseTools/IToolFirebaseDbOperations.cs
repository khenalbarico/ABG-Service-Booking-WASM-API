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

    Task PatchAsync<T>(
        T item,
        params string[] childPaths);

    Task DeleteAsync(params string[] childPaths);
}