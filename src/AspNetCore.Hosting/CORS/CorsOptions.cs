namespace Light.AspNetCore.Hosting.CORS
{
    public class CorsOptions
    {
        public bool Enable { get; set; }
        public bool AllowAll { get; set; }
        public string ApiGw { get; set; } = string.Empty;
        public string Mvc { get; set; } = string.Empty;
        public string Blazor { get; set; } = string.Empty;
    }
}
