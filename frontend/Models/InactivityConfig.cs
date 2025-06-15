using Newtonsoft.Json;

namespace Lastik.Models;

public record InactivityConfig(
    [property:JsonProperty("inactivityTime")] int InactivityTime,
    [property:JsonProperty("passwordInactivityTime")] int PasswordInactivityTime,
    [property:JsonProperty("password")] string Password);