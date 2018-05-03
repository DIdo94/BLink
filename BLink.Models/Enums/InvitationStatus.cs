using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BLink.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum InvitationStatus
    {
        NotReplied = 0,
        Accepted = 1,
        Refused = 2
    }
}
