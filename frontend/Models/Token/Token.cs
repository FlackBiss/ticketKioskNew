using Newtonsoft.Json;

namespace Lastik.Models.Token
{
    public record Token([property:JsonProperty("token")] string Value);
}
