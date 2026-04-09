using FluentValidation;
using UrlShortener.Application.DTOs;

namespace UrlShortener.Application.Validators;

public class CreateShortUrlRequestValidator : AbstractValidator<CreateShortUrlRequest>
{
    public CreateShortUrlRequestValidator()
    {
        RuleFor(x => x.OriginalUrl)
            .NotEmpty()
            .Must(BeValidUrl)
            .WithMessage("OriginalUrl must be a valid HTTP or HTTPS URL.");

        RuleFor(x => x.CustomAlias)
            .Matches("^[a-zA-Z0-9_-]*$")
            .When(x => !string.IsNullOrWhiteSpace(x.CustomAlias))
            .WithMessage("CustomAlias can only contain letters, numbers, underscores, and hyphens.");

        RuleFor(x => x.ExpiresAtUtc)
            .Must(date => date == null || date > DateTime.UtcNow)
            .WithMessage("ExpiresAtUtc must be in the future.");
    }

    private bool BeValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uri)
               && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }
}