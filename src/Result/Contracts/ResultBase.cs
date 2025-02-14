using System;
using System.Text.Json.Serialization;

namespace Light.Contracts
{
    public class ResultBase : IResult
    {
        [JsonPropertyOrder(-1)]
        public string RequestId { get; set; } = Guid.NewGuid().ToString();

        [JsonPropertyOrder(-1)]
        public string Code { get; set; }

        [JsonPropertyOrder(-1)]
        public bool Succeeded { get; set; }

        [JsonPropertyOrder(-1)]
        public string Message { get; set; } = "";
    }
}
