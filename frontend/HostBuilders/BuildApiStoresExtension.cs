using Lastik.Helpers;
using Lastik.Helpers.Logging;
using Lastik.Models;
using Lastik.Models.Cart.Stores;
using Lastik.Models.EventCalendar;
using Lastik.Models.ExceptionLog;
using Lastik.Models.Geometry.Stores;
using Lastik.Models.Kassa;
using Lastik.Models.Loggining;
using Lastik.Models.Order;
using Lastik.Models.Schedules.Stores;
using Lastik.Models.Terminal;
using Lastik.Models.Tickets.Stores;
using Lastik.Models.Token;
using Lastik.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;
using Application = System.Windows.Application;
using DefaultUrlParameterFormatter = Lastik.Utilities.DefaultUrlParameterFormatter;

namespace Lastik.HostBuilders
{
    public static class BuildApiStoresExtension
    {
        public static IHostBuilder BuildApiStores(this IHostBuilder builder)
        {
            builder.ConfigureServices((context,services) =>
            {
                var host = new Uri(context.Configuration.GetValue<string>("host")??string.Empty);
                
                services.AddRefitClient<ITokenHttpClient>(new RefitSettings(
                    contentSerializer: new NewtonsoftJsonContentSerializer()))
                    .ConfigureHttpClient(c => c.BaseAddress = host)
                    .AddHttpMessageHandler<LoggingHttpHandler>();

                services.AddSingleton<TokenStore>(s => new TokenStore(
                    context.Configuration.GetValue<string>("apiToken") ?? "1"));

                services.AddScoped<AuthHeaderHandler>();
                services.AddScoped<LoggingHttpHandler>();

                services.AddRefitClient<IApiHttpClient>(new RefitSettings(
                    contentSerializer:new NewtonsoftJsonContentSerializer(),
                    urlParameterFormatter:new DefaultUrlParameterFormatter()))
                    .ConfigureHttpClient(c =>
                    {
                        c.BaseAddress = host;
                        var timeout = context.Configuration.GetValue<int>("timeout");
                        if (timeout is not 0) c.Timeout = TimeSpan.FromSeconds(timeout);
                    })
                    .AddHttpMessageHandler<AuthHeaderHandler>()
                    .AddHttpMessageHandler<LoggingHttpHandler>();

                services.AddSingleton<FiltersStore>();
                services.AddSingleton<ScheduleStore>();
                services.AddSingleton<GeometryStore>();
                services.AddSingleton<EventCalendarStore>();
                services.AddSingleton<UserSessionStore>(s => new UserSessionStore(
                    s.GetRequiredService<IApiHttpClient>(),
                    s.GetRequiredService<ILoggingService>(),
                    context.Configuration.GetValue<int>("teirminalId"),
                    context.Configuration.GetValue<int>("inactivityTime")));
                services.AddSingleton<TerminalStore>(s => new TerminalStore(
                    s.GetRequiredService<IApiHttpClient>(), 
                    s.GetRequiredService<ILoggingService>(),
                    context.Configuration.GetValue<int>("teirminalId")));
                services.AddSingleton<TicketsStore>();
                services.AddSingleton<CartStore>(s => new CartStore(
                    s.GetRequiredService<IApiHttpClient>(),
                    s.GetRequiredService<ILoggingService>(),
                    context.Configuration.GetValue<int>("teirminalId")));
                services.AddSingleton<ExceptionLogStore>(s => new ExceptionLogStore(
                    s.GetRequiredService<IApiHttpClient>(),
                    s.GetRequiredService<ILoggingService>(),
                    context.Configuration.GetValue<int>("teirminalId")));
                services.AddSingleton<OrderStore>();
                services.AddSingleton<ILoggingService>(s => new FileLoggingService("Logs"));

                if (DebugHelper.IsRunningInDebugMode)
                    services.AddSingleton<IKassaService>(s => new FakeKassaService());
                else
                    services.AddSingleton<IKassaService>(s => new KassaHelper(s.GetRequiredService<ILoggingService>()));
            });
            
            return builder;
        }
    }
}
