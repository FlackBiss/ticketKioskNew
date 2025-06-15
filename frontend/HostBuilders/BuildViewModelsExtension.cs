using CommunityToolkit.Mvvm.Messaging;
using Lastik.HostBuilders.ViewModels;
using Lastik.Models;
using Lastik.Models.Loggining;
using Lastik.ViewModels;
using Lastik.ViewModels.Controls;
using Lastik.ViewModels.Popups;
using Lastik.ViewModels.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Stores;

namespace Lastik.HostBuilders
{
    public static class BuildViewModelsExtension
    {
        public static IHostBuilder BuildViewModels(this IHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IMessenger>(_ => new WeakReferenceMessenger());
                services.AddSingleton<MainWindowViewModel>(s => new MainWindowViewModel(
                    s.GetRequiredService<IMessenger>(),
                    s.GetRequiredService<ServerErrorModalStore>(),
                    s.GetRequiredService<NavigationStore>(),
                    s.GetRequiredService<ModalNavigationStore>(),
                    s.GetRequiredService<InactivityManager<ScheduleListViewModel>>(),
                    s.GetRequiredService<IApiHttpClient>(),
                    s.GetRequiredService<ParameterNavigationService<PasswordPopupViewModel, string>>(),
                    s.GetRequiredService<UserSessionStore>(),
                    context.Configuration.GetValue<string>("defaultPassword") ?? string.Empty));
            });
            return builder.BuildBodyViewModels();
        }
    }
}
