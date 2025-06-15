using Lastik.Helpers.Logging;

namespace Lastik.Models.Token;

public class TokenStore(string token)
{
    private string _token = token;
    public string GetToken() => token;
}