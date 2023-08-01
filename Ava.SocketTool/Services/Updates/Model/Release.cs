using System.Text.Json.Serialization;

namespace Ava.SocketTool.Services.Updates.Model;

public sealed class Release
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}