using UrlShortener.Application.Interfaces;

namespace UrlShortener.Infrastructure.Services;

public class ShortCodeGenerator : IShortCodeGenerator
{
    private const string Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private readonly Random _random = new();

    public string Generate(int length = 7)
    {
        return new string(Enumerable.Range(0, length)
            .Select(_ => Characters[_random.Next(Characters.Length)])
            .ToArray());
    }
}