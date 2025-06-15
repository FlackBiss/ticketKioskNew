using Refit;

namespace Lastik.Models.Token;

public interface ITokenHttpClient
{
    [Get("/api/widget/v1/token")]
    Task<ApiResponse<Token>> GetToken();
}