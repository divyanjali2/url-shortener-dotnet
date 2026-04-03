using UrlShortener.Application.DTOs;
using UrlShortener.Application.Interfaces;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Services;

public class ShortUrlService : IShortUrlService
{
    private readonly IShortUrlRepository _repository;
    private readonly IShortCodeGenerator _generator;

    public ShortUrlService(IShortUrlRepository repository, IShortCodeGenerator generator)
    {
        _repository = repository;
        _generator = generator;
    }

    public async Task<CreateShortUrlResponse> CreateAsync(CreateShortUrlRequest request, string baseUrl, CancellationToken cancellationToken = default)
    {
        string shortCode;

        if (!string.IsNullOrWhiteSpace(request.CustomAlias))
        {
            var exists = await _repository.ExistsByCodeAsync(request.CustomAlias, cancellationToken);
            if (exists)
                throw new InvalidOperationException("Custom alias already exists.");

            shortCode = request.CustomAlias;
        }
        else
        {
            do
            {
                shortCode = _generator.Generate();
            }
            while (await _repository.ExistsByCodeAsync(shortCode, cancellationToken));
        }

        var entity = new ShortUrl
        {
            Id = Guid.NewGuid(),
            OriginalUrl = request.OriginalUrl,
            ShortCode = shortCode,
            CreatedAtUtc = DateTime.UtcNow,
            ExpiresAtUtc = request.ExpiresAtUtc,
            IsActive = true
        };

        await _repository.AddAsync(entity, cancellationToken);

        return new CreateShortUrlResponse
        {
            OriginalUrl = entity.OriginalUrl,
            ShortCode = entity.ShortCode,
            ShortUrl = $"{baseUrl}/{entity.ShortCode}",
            CreatedAtUtc = entity.CreatedAtUtc
        };
    }

    public async Task<string?> GetOriginalUrlAsync(string shortCode, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByCodeAsync(shortCode, cancellationToken);

        if (entity is null || !entity.IsActive)
            return null;

        if (entity.ExpiresAtUtc.HasValue && entity.ExpiresAtUtc.Value <= DateTime.UtcNow)
            return null;

        return entity.OriginalUrl;
    }
}