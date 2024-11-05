namespace WebApi.Models;

public record RefreshTokenRequest(string AccessToken, string RefreshToken);