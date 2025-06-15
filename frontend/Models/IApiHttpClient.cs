using Lastik.Helpers.Logging;
using Lastik.Models.Cart.Entities;
using Lastik.Models.Geometry.Entities;
using Lastik.Models.Schedules.Entities;
using Newtonsoft.Json;
using Refit;
using System.IO;
using System.Net.Http;
using Lastik.Models.Loggining;
using Lastik.Models.Terminal;
using Lastik.Utilities;

namespace Lastik.Models;

public class QueryConditions(string searchText)
{
    [JsonProperty("_lookup")] public string Lookup = searchText;
    [JsonProperty("tags")] public List<string> Tags = [];
}

public interface IImageHttpClient
{
    [Get("/{imageId}")]
    Task<ApiResponse<HttpResponseMessage>> LoadImage(string imageId);
}

public interface IApiHttpClient:IImageHttpClient
{

    [Get("/api/schemes")]
    Task<ApiResponse<List<Hall>>> GetFilters();

    [Get("/api/events/{hallGeometryId}")]
    Task<ApiResponse<Schedule>> GetHallGeometry(int hallGeometryId);

    [Get("/api/events")]
    Task<ApiResponse<List<Schedule>>> GetEvents([Query] EventQueryParameters parameters);

    [Get("/api/events_dates")]
    Task<ApiResponse<EventCalendar.EventCalendar>> GetEventCalendar();

    [Get("/api/widget/v1/schedule/{scheduleId}/tickets")]
    Task<ApiResponse<Tickets.Entities.Tickets>> GetTickets(string scheduleId);

    [Get("/api/news?page=1")]
    Task<ApiResponse<List<Schedule>>> GetNews();

    [Get("/api/information")]
    Task<ApiResponse<Schedule>> GetHelp();

    [Get("/api/terminals/{id}")]
    Task<ApiResponse<Terminal.Terminal>> GetTerminal(int id);

    [Post("/api/tickets")]
    Task<ApiResponse<string>> SendCart([Body] CartItem cartPreview);

    [Post("/api/terminals/edit")]
    Task<ApiResponse<string>> SendKktState([Body] TerminalEdit cartPreview);

    [Post("/api/sessions")]
    Task<ApiResponse<UserSession>> SendUserSession([Body] UserSession cartPreview);

    [Post("/api/exception_logs")]
    [Headers("Content-Type: application/json")]
    Task<ApiResponse<ExceptionLog.ExceptionLogResponse>> SendExceptionLogs([Body] ExceptionLog.ExceptionLog exceptionLog);

    [Get("/api/stand_bies?page=1&view=true")]
    Task<ApiResponse<List<MainAd>>> GetStandByModels();
}

public static class ApiResponseExtensions
{
    public static T GetContent<T>(this ApiResponse<T> response, ILoggingService logger,T fallbackValue)
    {
        logger.Log(response.Error, response.ReasonPhrase ?? string.Empty);
        if (response.IsSuccessStatusCode) return response.Content ?? fallbackValue;
        return fallbackValue;
    }

    public static T GetContent<T>(this ApiResponse<T> response, ILoggingService logger)
        where T : new() => response.GetContent(logger, new T());

    public static List<T> GetFilteredContent<T>(this ApiResponse<List<T>> response, ILoggingService logger,
        IEnumerable<Func<T, bool>> filters)
        where T : new() => response.GetContent(logger).ApplyFilters(filters).ToList();
}

public static class ApiHttpClientExtensions
{

    public static async Task<string> LoadImageAndGetPath(this IImageHttpClient client, ILoggingService logger, string url, string localPath = "AllImages")
    {
        
        var filename = url.Replace('/', '_');
        if (string.IsNullOrEmpty(filename)) return string.Empty;

        var imageFile = Path.Combine(localPath, filename);

        if (!Directory.Exists(localPath)) Directory.CreateDirectory(localPath);
        if (File.Exists(imageFile)) return Path.GetFullPath(imageFile);
        var response = (await client.LoadImage(url)).GetContent(logger);
        await using var fs = new FileStream(
            Path.GetFullPath(imageFile),
            FileMode.CreateNew);
        await response.Content.CopyToAsync(fs);
        return Path.GetFullPath(imageFile);
    }
}
