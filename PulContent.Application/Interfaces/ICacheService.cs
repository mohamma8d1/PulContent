namespace PulContent.Application.Interfaces;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : struct;
    Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : struct;
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
}
