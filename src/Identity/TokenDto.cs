namespace Light.Identity;

public record TokenDto(string AccessToken, int ExpiresIn, string? RefreshToken);