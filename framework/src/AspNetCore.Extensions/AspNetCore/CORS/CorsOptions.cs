﻿namespace Light.AspNetCore.CORS
{
    public class CorsOptions
    {
        public bool Enable { get; set; }

        public bool AllowAll { get; set; }

        public string[]? Origins { get; set; }
    }
}
