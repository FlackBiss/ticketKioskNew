using Lastik.Helpers.Logging;
using Lastik.HostBuilders;
using Lastik.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvvmNavigationLib.Services;
using System.Windows;
using Lastik.Helpers;
using Lastik.Models;
using Lastik.ViewModels.Controls;
using Application = System.Windows.Application;
using Lastik.Models.ExceptionLog;


namespace Lastik
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _appHost = CreateHostBuilder().Build();

        public App()
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (DebugHelper.IsRunningInDebugMode) throw e.Exception;
            _appHost.Services.GetRequiredService<ILoggingService>().Log(e.Exception);
            _ = _appHost.Services.GetRequiredService<ExceptionLogStore>().SendAsync(e.Exception);
            e.Handled = true;
        }

        private static IHostBuilder CreateHostBuilder(string[]? args = null)
        {
            return Host.CreateDefaultBuilder(args)
                .BuildApiStores()
                .BuildNavigation()
                .BuildViewModels()
                .BuildViews();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            var initialNavigationService = _appHost.Services.GetRequiredService<NavigationService<ScheduleListViewModel>>();
            initialNavigationService.Navigate();
            
            MainWindow = _appHost.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            await _appHost.StartAsync();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _appHost.StopAsync();
            _appHost.Dispose();
            base.OnExit(e);
        }
    }

}
