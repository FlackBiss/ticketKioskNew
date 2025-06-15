using CommunityToolkit.Mvvm.Messaging;
using Lastik.Helpers.Logging;
using Lastik.Models;
using Lastik.Models.Loggining;
using Lastik.Models.Schedules.Entities;
using Lastik.Models.Schedules.Stores;
using Lastik.Utilities;
using Lastik.ViewModels;
using Lastik.ViewModels.Controllers;
using Lastik.ViewModels.Controls;
using Lastik.ViewModels.Pages;
using Lastik.ViewModels.Popups;
using Lastik.ViewModels.Windows;
using Lastik.Views.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Stores;

namespace Lastik.HostBuilders
{
    public static class BuildViewsExtension
    {
        public static IHostBuilder BuildViews(this IHostBuilder builder)
        {
            builder.ConfigureServices((context,services) =>
            {
                services.AddSingleton<LoaderController>(s =>
                    new LoaderController(
                        s.GetRequiredService<NavigationService<LoaderViewModel>>(),
                        s.GetRequiredService<IMessenger>()));

                services.AddSingleton<InactivityManager<ScheduleListViewModel>>(s=>
                    new InactivityManager<ScheduleListViewModel>(
                        s.GetRequiredService<NavigationStore>(),
                        s.GetRequiredService<ModalNavigationStore>(),
                        s.GetRequiredService<FiltersStore>(),
                        s.GetRequiredService<NavigationService<ScheduleListViewModel>>(),
                        s.GetRequiredService<CloseNavigationService<ModalNavigationStore>>(),
                        s.GetRequiredService<ParameterNavigationService<StandByPageViewModel, List<MainAd>>>(),
                        s.GetRequiredService<IApiHttpClient>(),
                        s.GetRequiredService<ILoggingService>(),
                        s.GetRequiredService<UserSessionStore>(),
                        context.Configuration.GetValue<int>("inactivityTime"),
                        context.Configuration.GetValue<int>("passwordInactivityTime")));

                services.AddSingleton(s =>
                {
                    var window = new MainWindow()
                    {
                        DataContext = s.GetRequiredService<MainWindowViewModel>()
                    };
                    return window;
                });

            });
            return builder;
        }
    }
}
