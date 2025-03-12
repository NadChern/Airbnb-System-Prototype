using System.Text.Json.Serialization;
namespace AirbnbREST.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BookingStatus
{
    Requested,
    Confirmed,
    Cancelled,
    Completed
}
