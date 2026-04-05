using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.DTOs;
using UrlShortener.Application.Interfaces;

namespace UrlShortener.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShortUrlsController : ControllerBase
{
    private readonly IShortUrlService _service;

    public ShortUrlsController(IShortUrlService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateShortUrlRequest request, CancellationToken cancellationToken)
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var result = await _service.CreateAsync(request, baseUrl, cancellationToken);
        return Ok(result);
    }

    [HttpGet("/{shortCode}")]
    public async Task<IActionResult> RedirectToOriginal(string shortCode, CancellationToken cancellationToken)
    {
        var originalUrl = await _service.GetOriginalUrlAsync(shortCode, cancellationToken);

        if (string.IsNullOrWhiteSpace(originalUrl))
            return NotFound(new { message = "Short URL not found or expired." });

        return Redirect(originalUrl);
    }
}