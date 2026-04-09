using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Interfaces;
using UrlShortener.Domain.Entities;
using UrlShortener.Infrastructure.Persistence;

namespace UrlShortener.Infrastructure.Repositories;

public class ShortUrlRepository : IShortUrlRepository
{
    private readonly ApplicationDbContext _context;

    public ShortUrlRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ShortUrl shortUrl, CancellationToken cancellationToken = default)
    {
        await _context.ShortUrls.AddAsync(shortUrl, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> ExistsByCodeAsync(string shortCode, CancellationToken cancellationToken = default)
    {
        return _context.ShortUrls.AnyAsync(x => x.ShortCode == shortCode, cancellationToken);
    }

    public Task<ShortUrl?> GetByCodeAsync(string shortCode, CancellationToken cancellationToken = default)
    {
        return _context.ShortUrls.FirstOrDefaultAsync(x => x.ShortCode == shortCode, cancellationToken);
    }
}