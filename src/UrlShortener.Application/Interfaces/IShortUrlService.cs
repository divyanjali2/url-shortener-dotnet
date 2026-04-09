using UrlShortener.Application.DTOs;

namespace UrlShortener.Application.Interfaces;

public interface IShortUrlService
{
    Task<CreateShortUrlResponse> CreateAsync(CreateShortUrlRequest request, string baseUrl, CancellationToken cancellationToken = default);
    Task<string?> GetOriginalUrlAsync(string shortCode, CancellationToken cancellationToken = default);
}