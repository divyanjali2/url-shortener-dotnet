using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Interfaces;

public interface IShortUrlRepository
{
    Task AddAsync(ShortUrl shortUrl, CancellationToken cancellationToken = default);
    Task<ShortUrl?> GetByCodeAsync(string shortCode, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string shortCode, CancellationToken cancellationToken = default);
}