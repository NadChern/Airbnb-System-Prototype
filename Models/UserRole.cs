using System.Text.Json.Serialization;

namespace AirbnbREST.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserRole
{
    Host,
    Guest
}
