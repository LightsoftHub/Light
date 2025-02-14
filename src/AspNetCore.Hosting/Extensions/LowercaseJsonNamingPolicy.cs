using System.Text.Json;

namespace Light.AspNetCore.Hosting.Extensions
{
    public class LowercaseJsonNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return name.ConvertName();
        }
    }
}
